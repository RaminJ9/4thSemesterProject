namespace WarehouseService;
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.3")]
    public partial class EmulatorServiceClient : System.ServiceModel.ClientBase<WarehouseService.IEmulatorService>, WarehouseService.IEmulatorService
    {
        
        /// <summary>
        /// Implement this partial method to configure the service endpoint.
        /// </summary>
        /// <param name="serviceEndpoint">The endpoint to configure</param>
        /// <param name="clientCredentials">The client credentials</param>
        static partial void ConfigureEndpoint(System.ServiceModel.Description.ServiceEndpoint serviceEndpoint, System.ServiceModel.Description.ClientCredentials clientCredentials);
        

        // CONSTRUCTORS ---------------------------------------------------------------------------------------------------------


        public EmulatorServiceClient() : 
                base(EmulatorServiceClient.GetDefaultBinding(), EmulatorServiceClient.GetDefaultEndpointAddress())
        {
            this.Endpoint.Name = EndpointConfiguration.BasicHttpBinding_IEmulatorService.ToString();
            ConfigureEndpoint(this.Endpoint, this.ClientCredentials);
        }
        
        public EmulatorServiceClient(EndpointConfiguration endpointConfiguration) : 
                base(EmulatorServiceClient.GetBindingForEndpoint(endpointConfiguration), EmulatorServiceClient.GetEndpointAddress(endpointConfiguration))
        {
            this.Endpoint.Name = endpointConfiguration.ToString();
            ConfigureEndpoint(this.Endpoint, this.ClientCredentials);
        }

        public EmulatorServiceClient(string remoteAddress) : //USING THIS ONE
                base(EmulatorServiceClient.GetDefaultBinding(), new System.ServiceModel.EndpointAddress(remoteAddress))
        {
            this.Endpoint.Name = EndpointConfiguration.BasicHttpBinding_IEmulatorService.ToString();
            ConfigureEndpoint(this.Endpoint, this.ClientCredentials);
        }
        
        public EmulatorServiceClient(EndpointConfiguration endpointConfiguration, string remoteAddress) : 
                base(EmulatorServiceClient.GetBindingForEndpoint(endpointConfiguration), new System.ServiceModel.EndpointAddress(remoteAddress))
        {
            this.Endpoint.Name = endpointConfiguration.ToString();
            ConfigureEndpoint(this.Endpoint, this.ClientCredentials);
        }
        
        public EmulatorServiceClient(EndpointConfiguration endpointConfiguration, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(EmulatorServiceClient.GetBindingForEndpoint(endpointConfiguration), remoteAddress)
        {
            this.Endpoint.Name = endpointConfiguration.ToString();
            ConfigureEndpoint(this.Endpoint, this.ClientCredentials);
        }
        
        public EmulatorServiceClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress)
        {
        }


        // FUNCTIONALITY ----------------------------------------------------------------------
        
        //The actual methods we will be calling
        public System.Threading.Tasks.Task<string> PickItemAsync(int trayId)
        {
            return base.Channel.PickItemAsync(trayId);
        }
        
        public System.Threading.Tasks.Task<string> InsertItemAsync(int trayId, string name)
        {
            return base.Channel.InsertItemAsync(trayId, name);
        }
        
        public System.Threading.Tasks.Task<string> GetInventoryAsync()
        {
            return base.Channel.GetInventoryAsync();
        }



        //CONNECTION -------------------------------------------------------------------------------

        
        public virtual System.Threading.Tasks.Task OpenAsync() //Start connection
        {
            return System.Threading.Tasks.Task.Factory.FromAsync(((System.ServiceModel.ICommunicationObject)(this)).BeginOpen(null, null), new System.Action<System.IAsyncResult>(((System.ServiceModel.ICommunicationObject)(this)).EndOpen));
        }
        
        public virtual System.Threading.Tasks.Task CloseAsync() //Close connection
        {
            return System.Threading.Tasks.Task.Factory.FromAsync(((System.ServiceModel.ICommunicationObject)(this)).BeginClose(null, null), new System.Action<System.IAsyncResult>(((System.ServiceModel.ICommunicationObject)(this)).EndClose));
        }



        //IGNORE FOR NOW / used by constructors
        
        private static System.ServiceModel.Channels.Binding GetBindingForEndpoint(EndpointConfiguration endpointConfiguration)
        {
            if ((endpointConfiguration == EndpointConfiguration.BasicHttpBinding_IEmulatorService))
            {
                System.ServiceModel.BasicHttpBinding result = new System.ServiceModel.BasicHttpBinding();
                result.MaxBufferSize = int.MaxValue;
                result.ReaderQuotas = System.Xml.XmlDictionaryReaderQuotas.Max;
                result.MaxReceivedMessageSize = int.MaxValue;
                result.AllowCookies = true;
                return result;
            }
            throw new System.InvalidOperationException(string.Format("Could not find endpoint with name \'{0}\'.", endpointConfiguration));
        }
        
        private static System.ServiceModel.EndpointAddress GetEndpointAddress(EndpointConfiguration endpointConfiguration)
        {
            if ((endpointConfiguration == EndpointConfiguration.BasicHttpBinding_IEmulatorService))
            {
                return new System.ServiceModel.EndpointAddress("http://localhost:8081/Service.asmx");
            }
            throw new System.InvalidOperationException(string.Format("Could not find endpoint with name \'{0}\'.", endpointConfiguration));
        }
        
        private static System.ServiceModel.Channels.Binding GetDefaultBinding()
        {
            return EmulatorServiceClient.GetBindingForEndpoint(EndpointConfiguration.BasicHttpBinding_IEmulatorService);
        }
        
        private static System.ServiceModel.EndpointAddress GetDefaultEndpointAddress()
        {
            return EmulatorServiceClient.GetEndpointAddress(EndpointConfiguration.BasicHttpBinding_IEmulatorService);
        }
        
        public enum EndpointConfiguration
        {
            
            BasicHttpBinding_IEmulatorService,
        }
    }