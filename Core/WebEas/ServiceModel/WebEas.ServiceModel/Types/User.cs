using System;
using System.Runtime.Serialization;
using ServiceStack.DataAnnotations;

namespace WebEas.ServiceModel.Office.Egov.Types
{
	[Schema("cfe")]
	[Alias("V_Users")]
    [DataContract]
    public class User : BaseEntity
    {
        [DataMember]
        public Guid D_User_Id { get; set; }

        [DataMember]
        [PfeColumn(Text = "Login meno")]
        public string LoginName { get; set; }

        [DataMember]
        [PfeColumn(Text = "Login heslo")]
        public string LoginPswd { get; set; }

        [DataMember]
        [PfeColumn(Text = "Meno")]
        public string FirstName { get; set; }

        [DataMember]
        [PfeColumn(Text = "Priezvisko")]
        public string LastName { get; set; }

        [DataMember]
        [PfeColumn(Text = "Celé meno")]
        public string FullName { get; set; }

        [DataMember]
        [PfeColumn(Text = "Titul pred")]
        public string TitulPred { get; set; }

        [DataMember]
        [PfeColumn(Text = "Titul za")]
        public string TitulZa { get; set; }

        [DataMember]
        [PfeColumn(Text = "E-mailová adresa")]
        public string Email { get; set; }

        [DataMember]
        [PfeColumn(Text = "Doménové meno")]
        public string DomainName { get; set; }

        [DataMember]
        [PfeColumn(Text = "Evidenčné číslo")]
        public string EC { get; set; }

        [DataMember]
        [PfeColumn(Text = "_Nadradený")]
        public Guid D_User_Id_Parent { get; set; }

        [DataMember]
        [PfeColumn(Text = "Nadradený")]
        [PfeCombo(typeof(User), NameColumn = "D_User_Id_Parent")]
        public string UserParent { get; set; }

        [DataMember]
        [PfeColumn(Text = "Dátum začiatku")]
        public DateTime DateStart { get; set; }

        [DataMember]
        [PfeColumn(Text = "Dátum ukončenia")]
        public DateTime DateEnd { get; set; }

        [DataMember]
        [PfeColumn(Text = "Krajina")]
        public short Country { get; set; }
    }
}
