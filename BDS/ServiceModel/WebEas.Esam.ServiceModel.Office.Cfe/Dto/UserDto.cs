using ServiceStack;
using ServiceStack.DataAnnotations;
using System;
using System.Runtime.Serialization;
using WebEas.ServiceModel;
using WebEas.ServiceModel.Types;

namespace WebEas.Esam.ServiceModel.Office.Cfe.Dto
{
    // Create
    [WebEasRequiresAnyRole(RolesDefinition.Cfe.Roles.CfeWriter)]
    [Route("/CreateUser", "POST")]
    [Api("User")]
    [DataContract]
    public class CreateUser : UserDto { }

    // Update
    [WebEasRequiresAnyRole(RolesDefinition.Cfe.Roles.CfeWriter)]
    [Route("/UpdateUser", "PUT")]
    [Api("User")]
    [DataContract]
    public class UpdateUser : UserDto
    {
        [PrimaryKey]
        [DataMember(IsRequired = true)]
        public Guid D_User_Id { get; set; }
    }

    // Delete
    [WebEasRequiresAnyRole(RolesDefinition.Cfe.Roles.CfeWriter)]
    [Route("/DeleteUser", "DELETE")]
    [Api("User")]
    [DataContract]
    public class DeleteUser
    {
        [DataMember(IsRequired = true)]
        public Guid D_User_Id { get; set; }
    }

    #region DTO
    [DataContract]
    public class UserDto : BaseDto<User>
    {
        [DataMember]
        [NotEmptyOrDefault]
        public string LoginName { get; set; }

        [DataMember]
        public string LoginPswd { get; set; }

        [DataMember]
        public string FirstName { get; set; }

        [DataMember]
        public string LastName { get; set; }

        [DataMember]
        public string TitulPred { get; set; }

        [DataMember]
        public string TitulZa { get; set; }

        [DataMember]
        [NotEmptyOrDefault]
        public string Email { get; set; }

        [DataMember]
        public string DomainName { get; set; }

        [DataMember]
        [PfeColumn(Text = "Evidenčné číslo")]
        public string EC { get; set; }

        [DataMember]
        public Guid? D_User_Id_Parent { get; set; }

        [DataMember]
        [NotEmptyOrDefault]
        public DateTime PlatnostOd { get; set; }

        [DataMember]
        public DateTime? PlatnostDo { get; set; }

        [DataMember]
        public short? Country { get; set; }

        /// <summary>
        /// Binds to entity.
        /// </summary>
        /// <param name="data"></param>
        protected override void BindToEntity(User data)
        {
            //data.D_User_Id = D_User_Id;
            data.LoginName = LoginName;
            data.LoginPswd = LoginPswd;
            data.FirstName = FirstName;
            data.LastName = LastName;
            data.TitulPred = TitulPred;
            data.TitulZa = TitulZa;
            data.Email = Email;
            data.DomainName = DomainName;
            data.EC = EC;
            data.D_User_Id_Parent = D_User_Id_Parent;
            data.PlatnostOd = PlatnostOd;
            data.PlatnostDo = PlatnostDo;
            data.Country = Country;
        }
    }
    #endregion
}