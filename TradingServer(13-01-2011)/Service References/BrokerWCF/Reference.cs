﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.17626
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace TradingServer.BrokerWCF {
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="BrokerWCF.IBrokerWCF")]
    public interface IBrokerWCF {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IBrokerWCF/NotifyBroker", ReplyAction="http://tempuri.org/IBrokerWCF/NotifyBrokerResponse")]
        TradingServer.BrokerWCF.NotifyBrokerResponse NotifyBroker(TradingServer.BrokerWCF.NotifyBrokerRequest request);
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.MessageContractAttribute(WrapperName="NotifyBroker", WrapperNamespace="http://tempuri.org/", IsWrapped=true)]
    public partial class NotifyBrokerRequest {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://tempuri.org/", Order=0)]
        public string value;
        
        public NotifyBrokerRequest() {
        }
        
        public NotifyBrokerRequest(string value) {
            this.value = value;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.MessageContractAttribute(WrapperName="NotifyBrokerResponse", WrapperNamespace="http://tempuri.org/", IsWrapped=true)]
    public partial class NotifyBrokerResponse {
        
        public NotifyBrokerResponse() {
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface IBrokerWCFChannel : TradingServer.BrokerWCF.IBrokerWCF, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class BrokerWCFClient : System.ServiceModel.ClientBase<TradingServer.BrokerWCF.IBrokerWCF>, TradingServer.BrokerWCF.IBrokerWCF {
        
        public BrokerWCFClient() {
        }
        
        public BrokerWCFClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public BrokerWCFClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public BrokerWCFClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public BrokerWCFClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public TradingServer.BrokerWCF.NotifyBrokerResponse NotifyBroker(TradingServer.BrokerWCF.NotifyBrokerRequest request) {
            return base.Channel.NotifyBroker(request);
        }
    }
}