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
        [PfeColumn(Text = "Login name", Mandatory = true)]
        public string LoginName { get; set; }

        [IgnoreDataMember]
        [PfeColumn(Text = "_Heslo")]
        public string LoginPswd { get; set; }

        [DataMember]
        [PfeColumn(Text = "First name", Mandatory = true)]
        public string FirstName { get; set; }

        [DataMember]
        [PfeColumn(Text = "Surname", Mandatory = true)]
        public string LastName { get; set; }

        [DataMember]
        [PfeColumn(Text = "Full name", ReadOnly = true)]
        [IgnoreInsertOrUpdate]
        [Compute]
        public string FullName { get; set; }

        [DataMember]
        [PfeColumn(Text = "Title before")]
        public string TitulPred { get; set; }

        [DataMember]
        [PfeColumn(Text = "Title after")]
        public string TitulZa { get; set; }

        [DataMember]
        [PfeColumn(Text = "E-mail address", Mandatory = true)]
        public string Email { get; set; }

        [DataMember]
        [PfeColumn(Text = "Telephone")]
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
        [PfeColumn(Text = "_Štatutár")]
        [StringLength(450)]
        public string Statutar { get; set; }

        [DataMember]
        [PfeColumn(Text = "_Doménové meno")]
        public string DomainName { get; set; }

        [DataMember]
        [PfeColumn(Text = "_Doménový")]
        public bool AD { get; set; }

        [DataMember]
        [PfeColumn(Text = "Evidence No.", Mandatory = true)]
        public string EC { get; set; }

        [DataMember]
        [PfeColumn(Text = "_D_User_Id_Parent")]
        public Guid? D_User_Id_Parent { get; set; }

        [DataMember]
        [PfeColumn(Text = "_Externý identif.")]
        public Guid? D_User_Id_Externe { get; set; }

        [DataMember]
        [PfeColumn(Text = "Start date", Type = PfeDataType.Date, Mandatory = true)]
        public DateTime PlatnostOd { get; set; }

        [DataMember]
        [PfeColumn(Text = "End date", Type = PfeDataType.Date)]
        public DateTime? PlatnostDo { get; set; }

        [DataMember]
        [PfeColumn(Text = "_Krajina")]  // bude sa riesit neskor, nie ja na to cas, defualtne 1 = SVK
        public short? Country { get; set; }

        [DataMember]
        [PfeColumn(Text = "Last login", Type = PfeDataType.DateTime, ReadOnly = true)]
        public DateTime? LastLogin { get; set; }

        [DataMember]
        [PfeColumn(Text = "_C_UserType_Id")]
        public short? C_UserType_Id { get; set; }

        public string ChangeConstraintMessage(string constraintName, int errorCode, WebEasSqlKnownErrorType errorType)
        {
            if (constraintName == "UQ_D_User_LoginName")
            {
                return "Login name already exists!";
            }

            return null;
        }

    }
}
