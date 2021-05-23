using System;
using System.Runtime.Serialization;
using ServiceStack.DataAnnotations;
using WebEas.ServiceModel;

namespace WebEas.Esam.ServiceModel.Office.Cfe.Types
{
    [Schema("cfe")]
    [Alias("D_Tenant")]
    [DataContract]
    public class Tenant : BaseEntity
    {
        [PrimaryKey]
        [DataMember]
        public Guid D_Tenant_Id { get; set; }

        [DataMember]
        [PfeColumn(Text = "_Typ", DefaultValue = 1)] //1 - 'Zákazník (obec, mesto, zriadená organiz.)'
        public byte C_TenantType_Id { get; set; }

        [DataMember]
        [PfeColumn(Text = "_OrganizaciaTypDetail", DefaultValue = 3)] //3 - 'Obec'
        public byte C_OrganizaciaTypDetail_Id { get; set; }

        [DataMember]
        [PfeColumn(Text = "Názov", Mandatory = true)]
        public string Nazov { get; set; }

        [DataMember]
        [PfeColumn(Text = "Server")] //, Mandatory = true
        public string Server { get; set; }

        [DataMember]
        [PfeColumn(Text = "Databáza", Mandatory = true, DefaultValue = "esam01")]
        public string Databaza { get; set; }

        [DataMember]
        [PfeColumn(Text = "Externý identif.")]
        [StringLength(36)]
        public Guid? D_Tenant_Id_Externe { get; set; }

        [DataMember]
        [PfeColumn(Text = "Identifikátor ISO")]
        public Guid? IsoId { get; set; }

        [DataMember]
        [PfeColumn(Text = "_D_PO_Osoba_Id")]
        public long? D_PO_Osoba_Id { get; set; }

        [DataMember]
        [PfeColumn(Text = "Telefón")]
        [StringLength(100)]
        public string Telefon { get; set; }

        [DataMember]
        [PfeColumn(Text = "Fax")]
        [StringLength(100)]
        public string Fax { get; set; }

        [DataMember]
        [PfeColumn(Text = "E-mail")]
        [StringLength(100)]
        public string Email { get; set; }

        [DataMember]
        [PfeColumn(Text = "Web")]
        [StringLength(100)]
        public string Web { get; set; }

        [DataMember]
        [PfeColumn(Text = "Štatutár")]
        [StringLength(450)]
        public string Statutar { get; set; }
    }
}
