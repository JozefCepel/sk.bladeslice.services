using ServiceStack;
using ServiceStack.DataAnnotations;
using System;
using System.Runtime.Serialization;
using System.Xml;
using WebEas.ServiceModel;
using WebEas.ServiceModel.Types;

namespace WebEas.Esam.ServiceModel.Office.Cfe.Dto
{
    // Create
    [Route("/CreateUser", "POST")]
    [Api("User")]
    [DataContract]
    public class CreateUser : UserDto, IReturn<UserView> { }

    // Update
    [Route("/UpdateUser", "PUT")]
    [Api("User")]
    [DataContract]
    public class UpdateUser : UserDto, IReturn<UserView>
    {
        [PrimaryKey]
        [DataMember(IsRequired = true)]
        public Guid D_User_Id { get; set; }
    }

    // BlockUser
    [Route("/ChangePassword", "POST")]
    [Api("User")]
    [DataContract]
    public class ChangePassword
    {
        [DataMember(IsRequired = true)]
        public Guid D_User_Id { get; set; }

        [DataMember(IsRequired = true)]
        public string newPassword { get; set; }

        [DataMember(IsRequired = false)]
        public string oldPassword { get; set; }
    }

    // BlockUser
    [Route("/BlockUser", "POST")]
    [Api("User")]
    [DataContract]
    public class BlockUser
    {
        [DataMember(IsRequired = true)]
        public Guid[] userId { get; set; }

        [DataMember(IsRequired = true)]
        public DateTime date { get; set; }
    }

    // CopykUserPermissions
    [Route("/CopyUserPermissions", "POST")]
    [Api("User")]
    [DataContract]
    public class CopyUserPermissions
    {
        [DataMember(IsRequired = true)]
        public Guid sourceUserId { get; set; }

        [DataMember(IsRequired = true)]
        public Guid destUserId { get; set; }

        [DataMember(IsRequired = true)]
        public bool Roles { get; set; }

        [DataMember(IsRequired = true)]
        public bool Rights { get; set; }

        [DataMember(IsRequired = true)]
        public bool TreePermissions { get; set; }

        [DataMember(IsRequired = true)]
        public bool ORSPermissions { get; set; }
    }

    // Update
    [Route("/DeleteUser", "DELETE")]
    [Api("User")]
    [DataContract]
    public class DeleteUser : UserDto
    {
        [PrimaryKey]
        [DataMember(IsRequired = true)]
        public Guid[] D_User_Id { get; set; }
    }

    #region DTO
    [DataContract]
    public class UserDto : BaseDto<User>
    {
        [DataMember]
        [NotEmptyOrDefault]
        public string LoginName { get; set; }

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
        public string EC { get; set; }

        [DataMember]
        public Guid? D_User_Id_Parent { get; set; }

        [DataMember]
        public Guid? D_User_Id_Externe { get; set; }

        [DataMember]
        [NotEmptyOrDefault]
        public DateTime PlatnostOd { get; set; }

        [DataMember]
        public DateTime? PlatnostDo { get; set; }

        [DataMember]
        public short? Country { get; set; }

        [DataMember]
        public short? C_UserType_Id { get; set; }

        /// <summary>
        /// Binds to entity.
        /// </summary>
        /// <param name="data"></param>
        protected override void BindToEntity(User data)
        {
            //data.D_User_Id = D_User_Id;
            data.LoginName = LoginName;
            data.FirstName = FirstName;
            data.LastName = LastName;
            data.TitulPred = TitulPred;
            data.TitulZa = TitulZa;
            data.Email = Email;
            data.DomainName = DomainName;
            data.EC = EC;
            data.D_User_Id_Parent = D_User_Id_Parent;
            data.D_User_Id_Externe = D_User_Id_Externe;
            data.PlatnostOd = PlatnostOd;
            data.PlatnostDo = PlatnostDo;
            data.Country = Country;
            data.C_UserType_Id = C_UserType_Id;
        }
    }
    #endregion
}