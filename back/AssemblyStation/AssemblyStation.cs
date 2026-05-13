using Common.Contracts;
using Common.Models;
using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Protocol;
using System.Text;
using System.Text.Json;

namespace AssemblyStation;

public class AssemblyStationComponent : MachineComponentBase
{
    private const string OperationTopic = "emulator/operation";
    private const string StatusTopic = "emulator/status";
    private const string CheckHealthTopic = "emulator/checkhealth";

    private readonly IMqttClient _client;

    public AssemblyStationComponent(string guid, string name, string connectionString)
        : base(guid, name, connectionString)
    {
        _client = new MqttFactory().CreateMqttClient();
    }

    public override async Task<Tray?> Receive(Tray tray)
    {
        await ExecuteAssemblyAsync(12345);
    
        return tray; //I am unsure if this is correct, but i have made the other logic for the MQTT connection and execution, so it should not be difficult to work with going forward.
    }

    public override Task<Tray?> Provide(Tray tray)
    {
        return Task.FromResult<Tray?>(tray);
    }

    private async Task ExecuteAssemblyAsync(int processId)
    {
        await EnsureConnectedAsync();

        var finished = new TaskCompletionSource<bool>();

        async Task Handler(MqttApplicationMessageReceivedEventArgs e)
{
    var topic = e.ApplicationMessage.Topic;
    var payload = Encoding.UTF8.GetString(e.ApplicationMessage.PayloadSegment);

    Console.WriteLine($"[{Name}] {topic}: {payload}");

    try
    {
        if (topic == StatusTopic)
        {
            var status = JsonSerializer.Deserialize<AssemblyStatusMessage>(payload);

            if (status == null)
            {
                finished.TrySetException(new Exception("Invalid status response."));
                return;
            }

            if (status.State == 0)
            {
                finished.TrySetResult(true);
                return;
            }

            if (status.State == 2)
            {
                finished.TrySetException(new Exception("Assembly station entered error state."));
                return;
            }
        }

        if (topic == CheckHealthTopic)
        {
            var health = JsonSerializer.Deserialize<HealthMessage>(
                payload,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
            );

            if (health is null)
            {
                finished.TrySetException(new Exception("Invalid health response."));
                return;
            }

            if (!health.Healthy || health.StatusCode == 9999)
            {
                finished.TrySetException(
                    new Exception($"Assembly station health check failed: {health.Message}")
                );
                return;
            }

            finished.TrySetResult(true);
        }
    }
    catch (JsonException ex)
    {
        finished.TrySetException(
            new Exception($"Could not parse MQTT message from topic {topic}. Payload: {payload}", ex)
        );
    }

    await Task.CompletedTask;
}

        _client.ApplicationMessageReceivedAsync += Handler;

        try
        {
            await _client.SubscribeAsync(StatusTopic);
            await _client.SubscribeAsync(CheckHealthTopic);

            var json = JsonSerializer.Serialize(new AssemblyOperationMessage
            {
                ProcessID = processId
            });

            var message = new MqttApplicationMessageBuilder()
                .WithTopic(OperationTopic)
                .WithPayload(json)
                .WithQualityOfServiceLevel(MqttQualityOfServiceLevel.AtLeastOnce)
                .Build();

            await _client.PublishAsync(message);

            var timeout = Task.Delay(TimeSpan.FromSeconds(30));
            var completed = await Task.WhenAny(finished.Task, timeout);

            if (completed == timeout)
                throw new TimeoutException("Assembly station timed out.");

            await finished.Task;
        }
        finally
        {
            _client.ApplicationMessageReceivedAsync -= Handler;
        }
    }

    private async Task EnsureConnectedAsync()
    {
        if (_client.IsConnected)
            return;

        var uri = new Uri(ConnectionString);

        var options = new MqttClientOptionsBuilder()
            .WithTcpServer(uri.Host, uri.Port)
            .Build();
        try {
            await _client.ConnectAsync(options);
        }
        catch (Exception ex)
        {
            throw new Exception("Failed to connect to assembly station MQTT broker.", ex);
        }
    }
}

public class AssemblyOperationMessage
{
    public int ProcessID { get; set; }
}

public class AssemblyStatusMessage
{
    public int LastOperation { get; set; }
    public int CurrentOperation { get; set; }
    public int State { get; set; }
    public string TimeStamp { get; set; } = "";
}

public class HealthMessage
{
    public bool Healthy { get; set; }
    public string? Message { get; set; }
    public int StatusCode { get; set; }
}