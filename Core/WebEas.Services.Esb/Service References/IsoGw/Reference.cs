﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace WebEas.Services.Esb.IsoGw {
    
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.6.1064.2")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://schemas.dcom.sk/integration/riso/v2")]
    public partial class RegisterISOFault : object, System.ComponentModel.INotifyPropertyChanged {
        
        private string errorCodeField;
        
        private string errorMessageField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=0)]
        public string errorCode {
            get {
                return this.errorCodeField;
            }
            set {
                this.errorCodeField = value;
                this.RaisePropertyChanged("errorCode");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=1)]
        public string errorMessage {
            get {
                return this.errorMessageField;
            }
            set {
                this.errorMessageField = value;
                this.RaisePropertyChanged("errorMessage");
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
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.6.1064.2")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://schemas.dcom.sk/integration/riso/v2")]
    public partial class ResHasService : object, System.ComponentModel.INotifyPropertyChanged {
        
        private bool resultField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=0)]
        public bool result {
            get {
                return this.resultField;
            }
            set {
                this.resultField = value;
                this.RaisePropertyChanged("result");
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
    
    /// <remarks/>
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(ReqGetInfo))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(ReqHasService))]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.6.1064.2")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://schemas.dcom.sk/integration/riso/v2")]
    public partial class ReqBase : object, System.ComponentModel.INotifyPropertyChanged {
        
        private string municipalOfficeIdField;
        
        private string serviceNamespaceField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=0)]
        public string municipalOfficeId {
            get {
                return this.municipalOfficeIdField;
            }
            set {
                this.municipalOfficeIdField = value;
                this.RaisePropertyChanged("municipalOfficeId");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=1)]
        public string serviceNamespace {
            get {
                return this.serviceNamespaceField;
            }
            set {
                this.serviceNamespaceField = value;
                this.RaisePropertyChanged("serviceNamespace");
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
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.6.1064.2")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://schemas.dcom.sk/integration/riso/v2")]
    public partial class ReqGetInfo : ReqBase {
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.6.1064.2")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://schemas.dcom.sk/integration/riso/v2")]
    public partial class ReqHasService : ReqBase {
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(Namespace="http://schemas.dcom.sk/integration/riso/v2", ConfigurationName="IsoGw.registerISO")]
    public interface registerISO {
        
        // CODEGEN: Generating message contract since the operation hasService is neither RPC nor document wrapped.
        [System.ServiceModel.OperationContractAttribute(Action="http://schemas.dcom.sk/integration/riso/hasService", ReplyAction="*")]
        [System.ServiceModel.FaultContractAttribute(typeof(WebEas.Services.Esb.IsoGw.RegisterISOFault), Action="*", Name="RegisterISOFault")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(ReqBase))]
        WebEas.Services.Esb.IsoGw.hasServiceRes hasService(WebEas.Services.Esb.IsoGw.hasServiceReq request);
        
        // CODEGEN: Generating message contract since the operation getInfo is neither RPC nor document wrapped.
        [System.ServiceModel.OperationContractAttribute(Action="http://schemas.dcom.sk/integration/riso/getInfo", ReplyAction="*")]
        [System.ServiceModel.FaultContractAttribute(typeof(WebEas.Services.Esb.IsoGw.RegisterISOFault), Action="*", Name="RegisterISOFault")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(ReqBase))]
        WebEas.Services.Esb.IsoGw.getInfoRes getInfo(WebEas.Services.Esb.IsoGw.getInfoReq request);
        
        // CODEGEN: Generating message contract since the operation getConfig is neither RPC nor document wrapped.
        [System.ServiceModel.OperationContractAttribute(Action="http://schemas.dcom.sk/integration/riso/getConfig", ReplyAction="*")]
        [System.ServiceModel.FaultContractAttribute(typeof(WebEas.Services.Esb.IsoGw.RegisterISOFault), Action="*", Name="RegisterISOFault")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(ReqBase))]
        WebEas.Services.Esb.IsoGw.getConfigRes getConfig(WebEas.Services.Esb.IsoGw.getConfigReq request);
        
        // CODEGEN: Generating message contract since the operation getOperationType is neither RPC nor document wrapped.
        [System.ServiceModel.OperationContractAttribute(Action="http://schemas.dcom.sk/integration/riso/getOperationType", ReplyAction="*")]
        [System.ServiceModel.FaultContractAttribute(typeof(WebEas.Services.Esb.IsoGw.RegisterISOFault), Action="*", Name="RegisterISOFault")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(ReqBase))]
        WebEas.Services.Esb.IsoGw.getOperationTypeRes getOperationType(WebEas.Services.Esb.IsoGw.getOperationTypeReq request);
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class hasServiceReq {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://schemas.dcom.sk/integration/riso/v2", Order=0)]
        public WebEas.Services.Esb.IsoGw.ReqHasService ReqHasService;
        
        public hasServiceReq() {
        }
        
        public hasServiceReq(WebEas.Services.Esb.IsoGw.ReqHasService ReqHasService) {
            this.ReqHasService = ReqHasService;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class hasServiceRes {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://schemas.dcom.sk/integration/riso/v2", Order=0)]
        public WebEas.Services.Esb.IsoGw.ResHasService ResHasService;
        
        public hasServiceRes() {
        }
        
        public hasServiceRes(WebEas.Services.Esb.IsoGw.ResHasService ResHasService) {
            this.ResHasService = ResHasService;
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.6.1064.2")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://schemas.dcom.sk/integration/riso/v2")]
    public partial class ServiceDetail : object, System.ComponentModel.INotifyPropertyChanged {
        
        private string nameField;
        
        private IntegrationType integrationTypeField;
        
        private string serviceNamespaceField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=0)]
        public string name {
            get {
                return this.nameField;
            }
            set {
                this.nameField = value;
                this.RaisePropertyChanged("name");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=1)]
        public IntegrationType integrationType {
            get {
                return this.integrationTypeField;
            }
            set {
                this.integrationTypeField = value;
                this.RaisePropertyChanged("integrationType");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=2)]
        public string serviceNamespace {
            get {
                return this.serviceNamespaceField;
            }
            set {
                this.serviceNamespaceField = value;
                this.RaisePropertyChanged("serviceNamespace");
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
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.6.1064.2")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://schemas.dcom.sk/integration/riso/v2")]
    public enum IntegrationType {
        
        /// <remarks/>
        LEVEL0,
        
        /// <remarks/>
        LEVEL0A,
        
        /// <remarks/>
        LEVEL0M,
        
        /// <remarks/>
        LEVEL1,
        
        /// <remarks/>
        LEVEL2,
        
        /// <remarks/>
        LEVEL3,
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class getInfoReq {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://schemas.dcom.sk/integration/riso/v2", Order=0)]
        public WebEas.Services.Esb.IsoGw.ReqGetInfo ReqGetInfo;
        
        public getInfoReq() {
        }
        
        public getInfoReq(WebEas.Services.Esb.IsoGw.ReqGetInfo ReqGetInfo) {
            this.ReqGetInfo = ReqGetInfo;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class getInfoRes {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://schemas.dcom.sk/integration/riso/v2", Order=0)]
        [System.Xml.Serialization.XmlArrayItemAttribute("serviceDetail", IsNullable=false)]
        public WebEas.Services.Esb.IsoGw.ServiceDetail[] ResGetInfo;
        
        public getInfoRes() {
        }
        
        public getInfoRes(WebEas.Services.Esb.IsoGw.ServiceDetail[] ResGetInfo) {
            this.ResGetInfo = ResGetInfo;
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.6.1064.2")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://schemas.dcom.sk/integration/riso/v2")]
    public partial class ReqGetConfig : GetConfigBase {
        
        private string municipalOfficeIdField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=0)]
        public string municipalOfficeId {
            get {
                return this.municipalOfficeIdField;
            }
            set {
                this.municipalOfficeIdField = value;
                this.RaisePropertyChanged("municipalOfficeId");
            }
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(ResGetConfig))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(ReqGetConfig))]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.6.1064.2")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://schemas.dcom.sk/integration/riso/v2")]
    public abstract partial class GetConfigBase : object, System.ComponentModel.INotifyPropertyChanged {
        
        private ConfigParameter[] configParameterField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("configParameter", Order=0)]
        public ConfigParameter[] configParameter {
            get {
                return this.configParameterField;
            }
            set {
                this.configParameterField = value;
                this.RaisePropertyChanged("configParameter");
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
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.6.1064.2")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://schemas.dcom.sk/integration/riso/v2")]
    public partial class ConfigParameter : object, System.ComponentModel.INotifyPropertyChanged {
        
        private string nameField;
        
        private string valueField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=0)]
        public string name {
            get {
                return this.nameField;
            }
            set {
                this.nameField = value;
                this.RaisePropertyChanged("name");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=1)]
        public string value {
            get {
                return this.valueField;
            }
            set {
                this.valueField = value;
                this.RaisePropertyChanged("value");
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
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.6.1064.2")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://schemas.dcom.sk/integration/riso/v2")]
    public partial class ResGetConfig : GetConfigBase {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class getConfigReq {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://schemas.dcom.sk/integration/riso/v2", Order=0)]
        public WebEas.Services.Esb.IsoGw.ReqGetConfig ReqGetConfig;
        
        public getConfigReq() {
        }
        
        public getConfigReq(WebEas.Services.Esb.IsoGw.ReqGetConfig ReqGetConfig) {
            this.ReqGetConfig = ReqGetConfig;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class getConfigRes {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://schemas.dcom.sk/integration/riso/v2", Order=0)]
        public WebEas.Services.Esb.IsoGw.ResGetConfig ResGetConfig;
        
        public getConfigRes() {
        }
        
        public getConfigRes(WebEas.Services.Esb.IsoGw.ResGetConfig ResGetConfig) {
            this.ResGetConfig = ResGetConfig;
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.6.1064.2")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://schemas.dcom.sk/integration/riso/v2")]
    public partial class ReqGetOperationType : object, System.ComponentModel.INotifyPropertyChanged {
        
        private string municipalOfficeIdField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=0)]
        public string municipalOfficeId {
            get {
                return this.municipalOfficeIdField;
            }
            set {
                this.municipalOfficeIdField = value;
                this.RaisePropertyChanged("municipalOfficeId");
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
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.6.1064.2")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://schemas.dcom.sk/integration/riso/v2")]
    public partial class ResGetOperationType : object, System.ComponentModel.INotifyPropertyChanged {
        
        private OperationType operationTypeField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=0)]
        public OperationType operationType {
            get {
                return this.operationTypeField;
            }
            set {
                this.operationTypeField = value;
                this.RaisePropertyChanged("operationType");
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
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.6.1064.2")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://schemas.dcom.sk/integration/riso/v2")]
    public enum OperationType {
        
        /// <remarks/>
        DCOM_ISO,
        
        /// <remarks/>
        DCOM,
        
        /// <remarks/>
        ISO,
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class getOperationTypeReq {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://schemas.dcom.sk/integration/riso/v2", Order=0)]
        public WebEas.Services.Esb.IsoGw.ReqGetOperationType ReqGetOperationType;
        
        public getOperationTypeReq() {
        }
        
        public getOperationTypeReq(WebEas.Services.Esb.IsoGw.ReqGetOperationType ReqGetOperationType) {
            this.ReqGetOperationType = ReqGetOperationType;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class getOperationTypeRes {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://schemas.dcom.sk/integration/riso/v2", Order=0)]
        public WebEas.Services.Esb.IsoGw.ResGetOperationType ResGetOperationType;
        
        public getOperationTypeRes() {
        }
        
        public getOperationTypeRes(WebEas.Services.Esb.IsoGw.ResGetOperationType ResGetOperationType) {
            this.ResGetOperationType = ResGetOperationType;
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface registerISOChannel : WebEas.Services.Esb.IsoGw.registerISO, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class registerISOClient : System.ServiceModel.ClientBase<WebEas.Services.Esb.IsoGw.registerISO>, WebEas.Services.Esb.IsoGw.registerISO {
        
        public registerISOClient() {
        }
        
        public registerISOClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public registerISOClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public registerISOClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public registerISOClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        WebEas.Services.Esb.IsoGw.hasServiceRes WebEas.Services.Esb.IsoGw.registerISO.hasService(WebEas.Services.Esb.IsoGw.hasServiceReq request) {
            return base.Channel.hasService(request);
        }
        
        public WebEas.Services.Esb.IsoGw.ResHasService hasService(WebEas.Services.Esb.IsoGw.ReqHasService ReqHasService) {
            WebEas.Services.Esb.IsoGw.hasServiceReq inValue = new WebEas.Services.Esb.IsoGw.hasServiceReq();
            inValue.ReqHasService = ReqHasService;
            WebEas.Services.Esb.IsoGw.hasServiceRes retVal = ((WebEas.Services.Esb.IsoGw.registerISO)(this)).hasService(inValue);
            return retVal.ResHasService;
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        WebEas.Services.Esb.IsoGw.getInfoRes WebEas.Services.Esb.IsoGw.registerISO.getInfo(WebEas.Services.Esb.IsoGw.getInfoReq request) {
            return base.Channel.getInfo(request);
        }
        
        public WebEas.Services.Esb.IsoGw.ServiceDetail[] getInfo(WebEas.Services.Esb.IsoGw.ReqGetInfo ReqGetInfo) {
            WebEas.Services.Esb.IsoGw.getInfoReq inValue = new WebEas.Services.Esb.IsoGw.getInfoReq();
            inValue.ReqGetInfo = ReqGetInfo;
            WebEas.Services.Esb.IsoGw.getInfoRes retVal = ((WebEas.Services.Esb.IsoGw.registerISO)(this)).getInfo(inValue);
            return retVal.ResGetInfo;
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        WebEas.Services.Esb.IsoGw.getConfigRes WebEas.Services.Esb.IsoGw.registerISO.getConfig(WebEas.Services.Esb.IsoGw.getConfigReq request) {
            return base.Channel.getConfig(request);
        }
        
        public WebEas.Services.Esb.IsoGw.ResGetConfig getConfig(WebEas.Services.Esb.IsoGw.ReqGetConfig ReqGetConfig) {
            WebEas.Services.Esb.IsoGw.getConfigReq inValue = new WebEas.Services.Esb.IsoGw.getConfigReq();
            inValue.ReqGetConfig = ReqGetConfig;
            WebEas.Services.Esb.IsoGw.getConfigRes retVal = ((WebEas.Services.Esb.IsoGw.registerISO)(this)).getConfig(inValue);
            return retVal.ResGetConfig;
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        WebEas.Services.Esb.IsoGw.getOperationTypeRes WebEas.Services.Esb.IsoGw.registerISO.getOperationType(WebEas.Services.Esb.IsoGw.getOperationTypeReq request) {
            return base.Channel.getOperationType(request);
        }
        
        public WebEas.Services.Esb.IsoGw.ResGetOperationType getOperationType(WebEas.Services.Esb.IsoGw.ReqGetOperationType ReqGetOperationType) {
            WebEas.Services.Esb.IsoGw.getOperationTypeReq inValue = new WebEas.Services.Esb.IsoGw.getOperationTypeReq();
            inValue.ReqGetOperationType = ReqGetOperationType;
            WebEas.Services.Esb.IsoGw.getOperationTypeRes retVal = ((WebEas.Services.Esb.IsoGw.registerISO)(this)).getOperationType(inValue);
            return retVal.ResGetOperationType;
        }
    }
}