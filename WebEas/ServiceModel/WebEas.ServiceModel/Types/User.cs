using System;
using System.Runtime.Serialization;
using ServiceStack.DataAnnotations;

namespace WebEas.ServiceModel.Types
{
	[Schema("cfe")]
	[Alias("D_User")]
    [DataContract]
    public class User : BaseEntity, IValidateConstraint
    {
        [PrimaryKey]
        [DataMember]
        public Guid D_User_Id { get; set; }

        [DataMember]
        [PfeColumn(Text = "Prihlasovacie meno", Mandatory = true)]
        public string LoginName { get; set; }

        [IgnoreDataMember]
        [PfeColumn(Text = "_Heslo")]
        public string LoginPswd { get; set; }

        [DataMember]
        [PfeColumn(Text = "Meno", Mandatory = true)]
        public string FirstName { get; set; }

        [DataMember]
        [PfeColumn(Text = "Priezvisko", Mandatory = true)]
        public string LastName { get; set; }

        [DataMember]
        [PfeColumn(Text = "Celé meno", ReadOnly = true)]
        [IgnoreInsertOrUpdate]
        [Compute]
        public string FullName { get; set; }

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
        [PfeColumn(Text = "Telefón")]
        [StringLength(100)]
        public string Telefon { get; set; }

        [DataMember]
        [PfeColumn(Text = "Fax")]
        [StringLength(100)]
        public string Fax { get; set; }

        [DataMember]
        [PfeColumn(Text = "Web")]
        [StringLength(100)]
        public string Web { get; set; }

        [DataMember]
        [PfeColumn(Text = "Štatutár")]
        [StringLength(450)]
        public string Statutar { get; set; }

        [DataMember]
        [PfeColumn(Text = "Doménové meno")]
        public string DomainName { get; set; }

        [DataMember]
        [PfeColumn(Text = "Doménový")]
        public bool AD { get; set; }

        [DataMember]
        [PfeColumn(Text = "Evidenčné číslo", Mandatory = true)]
        public string EC { get; set; }

        [DataMember]
        [PfeColumn(Text = "_Nadriadený")]
        public Guid? D_User_Id_Parent { get; set; }

        [DataMember]
        [PfeColumn(Text = "Externý identif.")]
        public Guid? D_User_Id_Externe { get; set; }

        [DataMember]
        [PfeColumn(Text = "Dátum začiatku", Type = PfeDataType.Date, Mandatory = true)]
        public DateTime PlatnostOd { get; set; }

        [DataMember]
        [PfeColumn(Text = "Dátum ukončenia", Type = PfeDataType.Date)]
        public DateTime? PlatnostDo { get; set; }

        [DataMember]
        [PfeColumn(Text = "_Krajina")]  // bude sa riesit neskor, nie ja na to cas, defualtne 1 = SVK
        public short? Country { get; set; }

        [DataMember]
        [PfeColumn(Text = "Posledné prihlásenie", Type = PfeDataType.DateTime, ReadOnly = true)]
        public DateTime? LastLogin { get; set; }

        [DataMember]
        [PfeColumn(Text = "_C_UserType_Id")]
        public short? C_UserType_Id { get; set; }

        public string ChangeConstraintMessage(string constraintName, int errorCode, WebEasSqlKnownErrorType errorType)
        {
            if (constraintName == "UQ_D_User_LoginName")
            {
                return "Zadané prihlasovacie meno už existuje!";
            }

            return null;
        }

    }
}
