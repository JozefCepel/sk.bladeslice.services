using System;
using System.Runtime.Serialization;
using ServiceStack.DataAnnotations;

namespace WebEas.ServiceModel.Types
{
	[Schema("cfe")]
	[Alias("D_Users")]
    [DataContract]
    public class User : BaseEntity
    {
        [PrimaryKey]
        [DataMember]
        [AutoIncrement]
        public Guid D_User_Id { get; set; }

        [DataMember]
        [PfeColumn(Text = "Login meno", Mandatory = true)]
        public string LoginName { get; set; }

        [DataMember]
        [PfeColumn(Text = "Login heslo")]
        public string LoginPswd { get; set; }

        [DataMember]
        [PfeColumn(Text = "Meno", Mandatory = true)]
        public string FirstName { get; set; }

        [DataMember]
        [PfeColumn(Text = "Priezvisko", Mandatory = true)]
        public string LastName { get; set; }

        [DataMember]
        [PfeColumn(Text = "Titul pred")]
        public string TitulPred { get; set; }

        [DataMember]
        [PfeColumn(Text = "Titul za")]
        public string TitulZa { get; set; }

        [DataMember]
        [PfeColumn(Text = "E-mailová adresa", Mandatory = true)]
        public string Email { get; set; }

        [DataMember]
        [PfeColumn(Text = "Doménové meno")]
        public string DomainName { get; set; }

        [DataMember]
        [PfeColumn(Text = "Evidenčné číslo")]
        public string EC { get; set; }

        [DataMember]
        [PfeColumn(Text = "_Nadradený")]
        public Guid? D_User_Id_Parent { get; set; }

        [DataMember]
        [PfeColumn(Text = "Dátum začiatku", Type = PfeDataType.Date, Mandatory = true)]
        public DateTime PlatnostOd { get; set; }

        [DataMember]
        [PfeColumn(Text = "Dátum ukončenia", Type = PfeDataType.Date)]
        public DateTime? PlatnostDo { get; set; }

        [DataMember]
        [PfeColumn(Text = "_Krajina")]  // bude sa riesit neskor, nie ja na to cas, defualtne 1 = SVK
        public short? Country { get; set; }
    }
}
