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
        _client = new MqttFactory().CreateMqttClient();}

        private static bool ParseHealthPayload(string payload)
{
    payload = payload.Trim();

    // If the whole payload is sent as a JSON string, unwrap it.
    // Example: "{'IsHealthy': true}"
    if (payload.StartsWith("\"") && payload.EndsWith("\""))
    {
        try
        {
            var unwrapped = JsonSerializer.Deserialize<string>(payload);
            if (!string.IsNullOrWhiteSpace(unwrapped))
            {
                payload = unwrapped.Trim();
            }
        }
        catch
        {
            payload = payload.Trim('"');
        }
    }

    // If someone sends ("IsHealthy": true), convert it to JSON object style.
    if (payload.StartsWith("(") && payload.EndsWith(")"))
    {
        payload = "{" + payload[1..^1] + "}";
    }

    // If someone sends {'IsHealthy': true}, convert single quotes to double quotes.
    // This is only meant as a fallback for this simple health payload.
    if (payload.Contains('\''))
    {
        payload = payload.Replace('\'', '"');
    }

    using var document = JsonDocument.Parse(payload);
    var root = document.RootElement;

    if (root.TryGetProperty("IsHealthy", out var value))
        return value.GetBoolean();

    if (root.TryGetProperty("isHealthy", out value))
        return value.GetBoolean();

    if (root.TryGetProperty("ishealthy", out value))
        return value.GetBoolean();

    if (root.TryGetProperty("Healthy", out value))
        return value.GetBoolean();

    if (root.TryGetProperty("healthy", out value))
        return value.GetBoolean();

    throw new JsonException($"Health payload did not contain IsHealthy/Healthy. Payload: {payload}");
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

private static readonly TimeSpan MaxOperationTime = TimeSpan.FromSeconds(30);
private static readonly TimeSpan MaxHealthTime = TimeSpan.FromSeconds(10);

private async Task ExecuteAssemblyAsync(int processId)
{
    await EnsureConnectedAsync();

    var operationFinished = new TaskCompletionSource<bool>();
    var healthChecked = new TaskCompletionSource<bool>();

    async Task Handler(MqttApplicationMessageReceivedEventArgs e)
    {
        var topic = e.ApplicationMessage.Topic;
        var payload = Encoding.UTF8.GetString(e.ApplicationMessage.PayloadSegment);

        Console.WriteLine($"[{Name}] {topic}: {payload}");

        try
        {
            if (topic == StatusTopic)
            {
                var status = JsonSerializer.Deserialize<AssemblyStatusMessage>(payload, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                if (status == null)
                {
                    operationFinished.TrySetException(
                        new Exception("Invalid assembly status response.")
                    );
                    return;
                }

                // State 0 = Idle / operation finished
                if (status.State == 0)
                {
                    operationFinished.TrySetResult(true);
                    return;
                }

                // State 1 = Executing / still running
                if (status.State == 1)
                {
                    Console.WriteLine("Assembly station is still executing...");
                    return;
                }

                // State 2 = Error
                if (status.State == 2)
                {
                    operationFinished.TrySetException(
                        new Exception("Assembly station entered error state.")
                    );
                    return;
                }
            }

            if (topic == CheckHealthTopic)
{
    try
    {
        bool isHealthy = ParseHealthPayload(payload);

        if (!isHealthy)
        {
            healthChecked.TrySetException(
                new Exception("Assembly station health check failed.")
            );
            return;
        }

        healthChecked.TrySetResult(true);
        return;
    }
    catch (JsonException ex)
    {
        healthChecked.TrySetException(
            new Exception($"Could not parse MQTT message from topic {topic}. Payload: {payload}", ex)
        );
        return;
    }
}
        }
        catch (JsonException ex)
        {
            if (topic == StatusTopic)
            {
                operationFinished.TrySetException(
                    new Exception($"Could not parse assembly status payload: {payload}", ex)
                );
            }

            if (topic == CheckHealthTopic)
            {
                healthChecked.TrySetException(
                    new Exception($"Could not parse assembly health payload: {payload}", ex)
                );
            }
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

        // UPPAAL: Executing state invariant t <= MAX_OPERATION_TIME
        await operationFinished.Task.WaitAsync(MaxOperationTime);

        // UPPAAL: WaitHealth state invariant t <= MAX_HEALTH_TIME
        await healthChecked.Task.WaitAsync(MaxHealthTime);
    }
    catch (TimeoutException)
    {
        throw new TimeoutException(
            $"Assembly station timed out. Operation timeout: {MaxOperationTime.TotalSeconds}s, health timeout: {MaxHealthTime.TotalSeconds}s."
        );
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