﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.18331
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SampleWeb.SampleService {
    using System.Runtime.Serialization;
    using System;
    
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="RouteHit", Namespace="http://schemas.datacontract.org/2004/07/Sample.Wcf")]
    [System.SerializableAttribute()]
    public partial class RouteHit : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged {
        
        [System.NonSerializedAttribute()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private long HitCountField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string RouteNameField;
        
        [global::System.ComponentModel.BrowsableAttribute(false)]
        public System.Runtime.Serialization.ExtensionDataObject ExtensionData {
            get {
                return this.extensionDataField;
            }
            set {
                this.extensionDataField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public long HitCount {
            get {
                return this.HitCountField;
            }
            set {
                if ((this.HitCountField.Equals(value) != true)) {
                    this.HitCountField = value;
                    this.RaisePropertyChanged("HitCount");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string RouteName {
            get {
                return this.RouteNameField;
            }
            set {
                if ((object.ReferenceEquals(this.RouteNameField, value) != true)) {
                    this.RouteNameField = value;
                    this.RaisePropertyChanged("RouteName");
                }
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="SampleService.ISampleService")]
    public interface ISampleService {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ISampleService/FetchRouteHits", ReplyAction="http://tempuri.org/ISampleService/FetchRouteHitsResponse")]
        SampleWeb.SampleService.RouteHit[] FetchRouteHits();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ISampleService/ServiceMethodThatIsNotProfiled", ReplyAction="http://tempuri.org/ISampleService/ServiceMethodThatIsNotProfiledResponse")]
        string ServiceMethodThatIsNotProfiled();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ISampleService/MassiveNesting", ReplyAction="http://tempuri.org/ISampleService/MassiveNestingResponse")]
        string MassiveNesting();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ISampleService/MassiveNesting2", ReplyAction="http://tempuri.org/ISampleService/MassiveNesting2Response")]
        string MassiveNesting2();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ISampleService/Duplicated", ReplyAction="http://tempuri.org/ISampleService/DuplicatedResponse")]
        string Duplicated();
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface ISampleServiceChannel : SampleWeb.SampleService.ISampleService, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class SampleServiceClient : System.ServiceModel.ClientBase<SampleWeb.SampleService.ISampleService>, SampleWeb.SampleService.ISampleService {
        
        public SampleServiceClient() {
        }
        
        public SampleServiceClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public SampleServiceClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public SampleServiceClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public SampleServiceClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public SampleWeb.SampleService.RouteHit[] FetchRouteHits() {
            return base.Channel.FetchRouteHits();
        }
        
        public string ServiceMethodThatIsNotProfiled() {
            return base.Channel.ServiceMethodThatIsNotProfiled();
        }
        
        public string MassiveNesting() {
            return base.Channel.MassiveNesting();
        }
        
        public string MassiveNesting2() {
            return base.Channel.MassiveNesting2();
        }
        
        public string Duplicated() {
            return base.Channel.Duplicated();
        }
    }
}
