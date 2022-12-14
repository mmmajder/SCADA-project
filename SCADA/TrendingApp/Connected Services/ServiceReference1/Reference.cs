//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace TrendingApp.ServiceReference1 {
    using System.Runtime.Serialization;
    using System;
    
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="TagInvoked", Namespace="http://schemas.datacontract.org/2004/07/SCADA.Model")]
    [System.SerializableAttribute()]
    public partial class TagInvoked : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged {
        
        [System.NonSerializedAttribute()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private int IdField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string TagNameField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private TrendingApp.ServiceReference1.TagType TagTypeField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private System.DateTime TimeStampField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private double ValueField;
        
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
        public int Id {
            get {
                return this.IdField;
            }
            set {
                if ((this.IdField.Equals(value) != true)) {
                    this.IdField = value;
                    this.RaisePropertyChanged("Id");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string TagName {
            get {
                return this.TagNameField;
            }
            set {
                if ((object.ReferenceEquals(this.TagNameField, value) != true)) {
                    this.TagNameField = value;
                    this.RaisePropertyChanged("TagName");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public TrendingApp.ServiceReference1.TagType TagType {
            get {
                return this.TagTypeField;
            }
            set {
                if ((this.TagTypeField.Equals(value) != true)) {
                    this.TagTypeField = value;
                    this.RaisePropertyChanged("TagType");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public System.DateTime TimeStamp {
            get {
                return this.TimeStampField;
            }
            set {
                if ((this.TimeStampField.Equals(value) != true)) {
                    this.TimeStampField = value;
                    this.RaisePropertyChanged("TimeStamp");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public double Value {
            get {
                return this.ValueField;
            }
            set {
                if ((this.ValueField.Equals(value) != true)) {
                    this.ValueField = value;
                    this.RaisePropertyChanged("Value");
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
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="TagType", Namespace="http://schemas.datacontract.org/2004/07/SCADA.Model.Tags")]
    public enum TagType : int {
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        AI = 0,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        AO = 1,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        DI = 2,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        DO = 3,
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="ServiceReference1.ITrendingAppService", CallbackContract=typeof(TrendingApp.ServiceReference1.ITrendingAppServiceCallback))]
    public interface ITrendingAppService {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ITrendingAppService/Init", ReplyAction="http://tempuri.org/ITrendingAppService/InitResponse")]
        void Init();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ITrendingAppService/Init", ReplyAction="http://tempuri.org/ITrendingAppService/InitResponse")]
        System.Threading.Tasks.Task InitAsync();
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface ITrendingAppServiceCallback {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ITrendingAppService/ValueChanged", ReplyAction="http://tempuri.org/ITrendingAppService/ValueChangedResponse")]
        void ValueChanged(TrendingApp.ServiceReference1.TagInvoked[] inputTags);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface ITrendingAppServiceChannel : TrendingApp.ServiceReference1.ITrendingAppService, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class TrendingAppServiceClient : System.ServiceModel.DuplexClientBase<TrendingApp.ServiceReference1.ITrendingAppService>, TrendingApp.ServiceReference1.ITrendingAppService {
        
        public TrendingAppServiceClient(System.ServiceModel.InstanceContext callbackInstance) : 
                base(callbackInstance) {
        }
        
        public TrendingAppServiceClient(System.ServiceModel.InstanceContext callbackInstance, string endpointConfigurationName) : 
                base(callbackInstance, endpointConfigurationName) {
        }
        
        public TrendingAppServiceClient(System.ServiceModel.InstanceContext callbackInstance, string endpointConfigurationName, string remoteAddress) : 
                base(callbackInstance, endpointConfigurationName, remoteAddress) {
        }
        
        public TrendingAppServiceClient(System.ServiceModel.InstanceContext callbackInstance, string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(callbackInstance, endpointConfigurationName, remoteAddress) {
        }
        
        public TrendingAppServiceClient(System.ServiceModel.InstanceContext callbackInstance, System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(callbackInstance, binding, remoteAddress) {
        }
        
        public void Init() {
            base.Channel.Init();
        }
        
        public System.Threading.Tasks.Task InitAsync() {
            return base.Channel.InitAsync();
        }
    }
}
