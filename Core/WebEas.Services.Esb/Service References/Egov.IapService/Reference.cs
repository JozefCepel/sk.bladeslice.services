﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace WebEas.Services.Esb.Egov.IapService {
    using System.Runtime.Serialization;
    using System;
    
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="BaseEntity", Namespace="http://schemas.datacontract.org/2004/07/WebEas.ServiceModel")]
    [System.SerializableAttribute()]
    [System.Runtime.Serialization.KnownTypeAttribute(typeof(WebEas.Services.Esb.Egov.IapService.BaseTenantEntity))]
    [System.Runtime.Serialization.KnownTypeAttribute(typeof(WebEas.Services.Esb.Egov.IapService.PotvrdeniePublikovania))]
    [System.Runtime.Serialization.KnownTypeAttribute(typeof(WebEas.Services.Esb.Egov.IapService.Zasobnik))]
    public partial class BaseEntity : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged {
        
        [System.NonSerializedAttribute()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private long AccessFlagField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private System.DateTime DatumVytvoreniaField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private System.DateTime DatumZmenyField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string PoznamkaField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private long ciField;
        
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
        public long AccessFlag {
            get {
                return this.AccessFlagField;
            }
            set {
                if ((this.AccessFlagField.Equals(value) != true)) {
                    this.AccessFlagField = value;
                    this.RaisePropertyChanged("AccessFlag");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public System.DateTime DatumVytvorenia {
            get {
                return this.DatumVytvoreniaField;
            }
            set {
                if ((this.DatumVytvoreniaField.Equals(value) != true)) {
                    this.DatumVytvoreniaField = value;
                    this.RaisePropertyChanged("DatumVytvorenia");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public System.DateTime DatumZmeny {
            get {
                return this.DatumZmenyField;
            }
            set {
                if ((this.DatumZmenyField.Equals(value) != true)) {
                    this.DatumZmenyField = value;
                    this.RaisePropertyChanged("DatumZmeny");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Poznamka {
            get {
                return this.PoznamkaField;
            }
            set {
                if ((object.ReferenceEquals(this.PoznamkaField, value) != true)) {
                    this.PoznamkaField = value;
                    this.RaisePropertyChanged("Poznamka");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public long ci {
            get {
                return this.ciField;
            }
            set {
                if ((this.ciField.Equals(value) != true)) {
                    this.ciField = value;
                    this.RaisePropertyChanged("ci");
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
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="BaseTenantEntity", Namespace="http://schemas.datacontract.org/2004/07/WebEas.ServiceModel")]
    [System.SerializableAttribute()]
    [System.Runtime.Serialization.KnownTypeAttribute(typeof(WebEas.Services.Esb.Egov.IapService.PotvrdeniePublikovania))]
    [System.Runtime.Serialization.KnownTypeAttribute(typeof(WebEas.Services.Esb.Egov.IapService.Zasobnik))]
    public partial class BaseTenantEntity : WebEas.Services.Esb.Egov.IapService.BaseEntity {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="PotvrdeniePublikovania", Namespace="http://schemas.datacontract.org/2004/07/WebEas.ServiceModel.Office.Egov.Iap.Types" +
        "")]
    [System.SerializableAttribute()]
    public partial class PotvrdeniePublikovania : WebEas.Services.Esb.Egov.IapService.BaseTenantEntity {
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private int C_InformacnyKanal_IdField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private System.Nullable<int> D_DataBuffer_IdField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private int D_PotvrdeniePublikovania_IdField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string D_Pouzivatel_IdField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private System.Nullable<long> D_Priloha_IdField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private System.DateTime DatumPublikovaniaField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string PredmetOznamuField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string TextOznamuField;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public int C_InformacnyKanal_Id {
            get {
                return this.C_InformacnyKanal_IdField;
            }
            set {
                if ((this.C_InformacnyKanal_IdField.Equals(value) != true)) {
                    this.C_InformacnyKanal_IdField = value;
                    this.RaisePropertyChanged("C_InformacnyKanal_Id");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public System.Nullable<int> D_DataBuffer_Id {
            get {
                return this.D_DataBuffer_IdField;
            }
            set {
                if ((this.D_DataBuffer_IdField.Equals(value) != true)) {
                    this.D_DataBuffer_IdField = value;
                    this.RaisePropertyChanged("D_DataBuffer_Id");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public int D_PotvrdeniePublikovania_Id {
            get {
                return this.D_PotvrdeniePublikovania_IdField;
            }
            set {
                if ((this.D_PotvrdeniePublikovania_IdField.Equals(value) != true)) {
                    this.D_PotvrdeniePublikovania_IdField = value;
                    this.RaisePropertyChanged("D_PotvrdeniePublikovania_Id");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string D_Pouzivatel_Id {
            get {
                return this.D_Pouzivatel_IdField;
            }
            set {
                if ((object.ReferenceEquals(this.D_Pouzivatel_IdField, value) != true)) {
                    this.D_Pouzivatel_IdField = value;
                    this.RaisePropertyChanged("D_Pouzivatel_Id");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public System.Nullable<long> D_Priloha_Id {
            get {
                return this.D_Priloha_IdField;
            }
            set {
                if ((this.D_Priloha_IdField.Equals(value) != true)) {
                    this.D_Priloha_IdField = value;
                    this.RaisePropertyChanged("D_Priloha_Id");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public System.DateTime DatumPublikovania {
            get {
                return this.DatumPublikovaniaField;
            }
            set {
                if ((this.DatumPublikovaniaField.Equals(value) != true)) {
                    this.DatumPublikovaniaField = value;
                    this.RaisePropertyChanged("DatumPublikovania");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string PredmetOznamu {
            get {
                return this.PredmetOznamuField;
            }
            set {
                if ((object.ReferenceEquals(this.PredmetOznamuField, value) != true)) {
                    this.PredmetOznamuField = value;
                    this.RaisePropertyChanged("PredmetOznamu");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string TextOznamu {
            get {
                return this.TextOznamuField;
            }
            set {
                if ((object.ReferenceEquals(this.TextOznamuField, value) != true)) {
                    this.TextOznamuField = value;
                    this.RaisePropertyChanged("TextOznamu");
                }
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="Zasobnik", Namespace="http://schemas.datacontract.org/2004/07/WebEas.ServiceModel.Office.Egov.Iap.Types" +
        "")]
    [System.SerializableAttribute()]
    public partial class Zasobnik : WebEas.Services.Esb.Egov.IapService.BaseTenantEntity {
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private System.Nullable<short> C_Modul_IdField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private System.Nullable<int> C_Operacie_IdField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private System.Nullable<int> C_SledovanaPolozka_IdField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private System.Nullable<int> C_Sluzba_IdField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private int C_StavEntity_IdField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private short C_StavZverejnenia_Id_FacebookField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private short C_StavZverejnenia_Id_PortalField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private short C_StavZverejnenia_Id_RozhlasField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private short C_StavZverejnenia_Id_TlacField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private short C_StavZverejnenia_Id_UrTabulaField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private int C_StavovyPriestor_IdField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private int D_DataBuffer_IdField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private System.Nullable<int> D_Pohlad_IdField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string PopisField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private System.Nullable<long> Zaznam_IdField;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public System.Nullable<short> C_Modul_Id {
            get {
                return this.C_Modul_IdField;
            }
            set {
                if ((this.C_Modul_IdField.Equals(value) != true)) {
                    this.C_Modul_IdField = value;
                    this.RaisePropertyChanged("C_Modul_Id");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public System.Nullable<int> C_Operacie_Id {
            get {
                return this.C_Operacie_IdField;
            }
            set {
                if ((this.C_Operacie_IdField.Equals(value) != true)) {
                    this.C_Operacie_IdField = value;
                    this.RaisePropertyChanged("C_Operacie_Id");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public System.Nullable<int> C_SledovanaPolozka_Id {
            get {
                return this.C_SledovanaPolozka_IdField;
            }
            set {
                if ((this.C_SledovanaPolozka_IdField.Equals(value) != true)) {
                    this.C_SledovanaPolozka_IdField = value;
                    this.RaisePropertyChanged("C_SledovanaPolozka_Id");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public System.Nullable<int> C_Sluzba_Id {
            get {
                return this.C_Sluzba_IdField;
            }
            set {
                if ((this.C_Sluzba_IdField.Equals(value) != true)) {
                    this.C_Sluzba_IdField = value;
                    this.RaisePropertyChanged("C_Sluzba_Id");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public int C_StavEntity_Id {
            get {
                return this.C_StavEntity_IdField;
            }
            set {
                if ((this.C_StavEntity_IdField.Equals(value) != true)) {
                    this.C_StavEntity_IdField = value;
                    this.RaisePropertyChanged("C_StavEntity_Id");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public short C_StavZverejnenia_Id_Facebook {
            get {
                return this.C_StavZverejnenia_Id_FacebookField;
            }
            set {
                if ((this.C_StavZverejnenia_Id_FacebookField.Equals(value) != true)) {
                    this.C_StavZverejnenia_Id_FacebookField = value;
                    this.RaisePropertyChanged("C_StavZverejnenia_Id_Facebook");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public short C_StavZverejnenia_Id_Portal {
            get {
                return this.C_StavZverejnenia_Id_PortalField;
            }
            set {
                if ((this.C_StavZverejnenia_Id_PortalField.Equals(value) != true)) {
                    this.C_StavZverejnenia_Id_PortalField = value;
                    this.RaisePropertyChanged("C_StavZverejnenia_Id_Portal");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public short C_StavZverejnenia_Id_Rozhlas {
            get {
                return this.C_StavZverejnenia_Id_RozhlasField;
            }
            set {
                if ((this.C_StavZverejnenia_Id_RozhlasField.Equals(value) != true)) {
                    this.C_StavZverejnenia_Id_RozhlasField = value;
                    this.RaisePropertyChanged("C_StavZverejnenia_Id_Rozhlas");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public short C_StavZverejnenia_Id_Tlac {
            get {
                return this.C_StavZverejnenia_Id_TlacField;
            }
            set {
                if ((this.C_StavZverejnenia_Id_TlacField.Equals(value) != true)) {
                    this.C_StavZverejnenia_Id_TlacField = value;
                    this.RaisePropertyChanged("C_StavZverejnenia_Id_Tlac");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public short C_StavZverejnenia_Id_UrTabula {
            get {
                return this.C_StavZverejnenia_Id_UrTabulaField;
            }
            set {
                if ((this.C_StavZverejnenia_Id_UrTabulaField.Equals(value) != true)) {
                    this.C_StavZverejnenia_Id_UrTabulaField = value;
                    this.RaisePropertyChanged("C_StavZverejnenia_Id_UrTabula");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public int C_StavovyPriestor_Id {
            get {
                return this.C_StavovyPriestor_IdField;
            }
            set {
                if ((this.C_StavovyPriestor_IdField.Equals(value) != true)) {
                    this.C_StavovyPriestor_IdField = value;
                    this.RaisePropertyChanged("C_StavovyPriestor_Id");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public int D_DataBuffer_Id {
            get {
                return this.D_DataBuffer_IdField;
            }
            set {
                if ((this.D_DataBuffer_IdField.Equals(value) != true)) {
                    this.D_DataBuffer_IdField = value;
                    this.RaisePropertyChanged("D_DataBuffer_Id");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public System.Nullable<int> D_Pohlad_Id {
            get {
                return this.D_Pohlad_IdField;
            }
            set {
                if ((this.D_Pohlad_IdField.Equals(value) != true)) {
                    this.D_Pohlad_IdField = value;
                    this.RaisePropertyChanged("D_Pohlad_Id");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Popis {
            get {
                return this.PopisField;
            }
            set {
                if ((object.ReferenceEquals(this.PopisField, value) != true)) {
                    this.PopisField = value;
                    this.RaisePropertyChanged("Popis");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public System.Nullable<long> Zaznam_Id {
            get {
                return this.Zaznam_IdField;
            }
            set {
                if ((this.Zaznam_IdField.Equals(value) != true)) {
                    this.Zaznam_IdField = value;
                    this.RaisePropertyChanged("Zaznam_Id");
                }
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="dcomFault", Namespace="http://schemas.dcom.sk/fault/1.0")]
    [System.SerializableAttribute()]
    public partial class dcomFault : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged {
        
        [System.NonSerializedAttribute()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string faultCauseField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private int faultCodeField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string faultMessageField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private System.Collections.Generic.List<WebEas.Services.Esb.Egov.IapService.InvalidParameterType> invalidParameterField;
        
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
        public string faultCause {
            get {
                return this.faultCauseField;
            }
            set {
                if ((object.ReferenceEquals(this.faultCauseField, value) != true)) {
                    this.faultCauseField = value;
                    this.RaisePropertyChanged("faultCause");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public int faultCode {
            get {
                return this.faultCodeField;
            }
            set {
                if ((this.faultCodeField.Equals(value) != true)) {
                    this.faultCodeField = value;
                    this.RaisePropertyChanged("faultCode");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string faultMessage {
            get {
                return this.faultMessageField;
            }
            set {
                if ((object.ReferenceEquals(this.faultMessageField, value) != true)) {
                    this.faultMessageField = value;
                    this.RaisePropertyChanged("faultMessage");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public System.Collections.Generic.List<WebEas.Services.Esb.Egov.IapService.InvalidParameterType> invalidParameter {
            get {
                return this.invalidParameterField;
            }
            set {
                if ((object.ReferenceEquals(this.invalidParameterField, value) != true)) {
                    this.invalidParameterField = value;
                    this.RaisePropertyChanged("invalidParameter");
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
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="InvalidParameterType", Namespace="http://schemas.dcom.sk/fault/1.0")]
    [System.SerializableAttribute()]
    public partial class InvalidParameterType : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged {
        
        [System.NonSerializedAttribute()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string errorMessageField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string parameterNameField;
        
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
        public string errorMessage {
            get {
                return this.errorMessageField;
            }
            set {
                if ((object.ReferenceEquals(this.errorMessageField, value) != true)) {
                    this.errorMessageField = value;
                    this.RaisePropertyChanged("errorMessage");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string parameterName {
            get {
                return this.parameterNameField;
            }
            set {
                if ((object.ReferenceEquals(this.parameterNameField, value) != true)) {
                    this.parameterNameField = value;
                    this.RaisePropertyChanged("parameterName");
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
    [System.ServiceModel.ServiceContractAttribute(Namespace="http://schemas.dcom.sk/public/egov/wcf/1.0", ConfigurationName="Egov.IapService.IIapService")]
    public interface IIapService {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://schemas.dcom.sk/public/egov/wcf/1.0/IIapService/CreateAutomatichPublish", ReplyAction="http://schemas.dcom.sk/public/egov/wcf/1.0/IIapService/CreateAutomatichPublishRes" +
            "ponse")]
        [System.ServiceModel.FaultContractAttribute(typeof(WebEas.Services.Esb.Egov.IapService.dcomFault), Action="http://schemas.dcom.sk/public/egov/wcf/1.0/IIapService/CreateAutomatichPublish/Fa" +
            "ult/SoapFault", Name="dcomFault", Namespace="http://schemas.dcom.sk/fault/1.0")]
        [System.ServiceModel.TransactionFlowAttribute(System.ServiceModel.TransactionFlowOption.Allowed)]
        void CreateAutomatichPublish(WebEas.Services.Esb.Egov.IapService.Zasobnik buffer, WebEas.Services.Esb.Egov.IapService.PotvrdeniePublikovania confirmation);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://schemas.dcom.sk/public/egov/wcf/1.0/IIapService/CreateManualtPublish", ReplyAction="http://schemas.dcom.sk/public/egov/wcf/1.0/IIapService/CreateManualtPublishRespon" +
            "se")]
        [System.ServiceModel.FaultContractAttribute(typeof(WebEas.Services.Esb.Egov.IapService.dcomFault), Action="http://schemas.dcom.sk/public/egov/wcf/1.0/IIapService/CreateManualtPublish/Fault" +
            "/SoapFault", Name="dcomFault", Namespace="http://schemas.dcom.sk/fault/1.0")]
        [System.ServiceModel.TransactionFlowAttribute(System.ServiceModel.TransactionFlowOption.Allowed)]
        void CreateManualtPublish(System.Collections.Generic.List<WebEas.Services.Esb.Egov.IapService.Zasobnik> insertData, System.Collections.Generic.List<WebEas.Services.Esb.Egov.IapService.Zasobnik> updateData);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://schemas.dcom.sk/public/egov/wcf/1.0/IIapService/Test", ReplyAction="http://schemas.dcom.sk/public/egov/wcf/1.0/IIapService/TestResponse")]
        [System.ServiceModel.FaultContractAttribute(typeof(WebEas.Services.Esb.Egov.IapService.dcomFault), Action="http://schemas.dcom.sk/public/egov/wcf/1.0/IIapService/Test/Fault/SoapFault", Name="dcomFault", Namespace="http://schemas.dcom.sk/fault/1.0")]
        [System.ServiceModel.TransactionFlowAttribute(System.ServiceModel.TransactionFlowOption.Mandatory)]
        void Test(string text);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface IIapServiceChannel : WebEas.Services.Esb.Egov.IapService.IIapService, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class IapServiceClient : System.ServiceModel.ClientBase<WebEas.Services.Esb.Egov.IapService.IIapService>, WebEas.Services.Esb.Egov.IapService.IIapService {
        
        public IapServiceClient() {
        }
        
        public IapServiceClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public IapServiceClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public IapServiceClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public IapServiceClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public void CreateAutomatichPublish(WebEas.Services.Esb.Egov.IapService.Zasobnik buffer, WebEas.Services.Esb.Egov.IapService.PotvrdeniePublikovania confirmation) {
            base.Channel.CreateAutomatichPublish(buffer, confirmation);
        }
        
        public void CreateManualtPublish(System.Collections.Generic.List<WebEas.Services.Esb.Egov.IapService.Zasobnik> insertData, System.Collections.Generic.List<WebEas.Services.Esb.Egov.IapService.Zasobnik> updateData) {
            base.Channel.CreateManualtPublish(insertData, updateData);
        }
        
        public void Test(string text) {
            base.Channel.Test(text);
        }
    }
}
