namespace WarehouseService;

[System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.3")]
[System.ServiceModel.ServiceContractAttribute(ConfigurationName="WarehouseService.IEmulatorService")]
public interface IEmulatorService
    {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IEmulatorService/PickItem", ReplyAction="http://tempuri.org/IEmulatorService/PickItemResponse")]
        System.Threading.Tasks.Task<string> PickItemAsync(int trayId);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IEmulatorService/InsertItem", ReplyAction="http://tempuri.org/IEmulatorService/InsertItemResponse")]
        System.Threading.Tasks.Task<string> InsertItemAsync(int trayId, string name);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IEmulatorService/GetInventory", ReplyAction="http://tempuri.org/IEmulatorService/GetInventoryResponse")]
        System.Threading.Tasks.Task<string> GetInventoryAsync();
    }