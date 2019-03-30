using ServiceStack;
using ServiceStack.DataAnnotations;
using System;
using System.Runtime.Serialization;
using WebEas.Esam.ServiceModel.Office.Cfe.Types;
using WebEas.ServiceModel;

namespace WebEas.Esam.ServiceModel.Office.Cfe.Dto
{
    // Create - nie je

    // Update
    [WebEasRequiresAnyRole(RolesDefinition.Cfe.Roles.CfeWriter)]
    [Route("/UpdateTenant", "PUT")]
    [Api("Tenant")]
    [DataContract]
    public class UpdateTenant : TenantDto
    {
        [PrimaryKey]
        [DataMember(IsRequired = true)]
        public Guid D_Tenant_Id { get; set; }
    }

    // Delete - nie je

    #region DTO
    [DataContract]
    public class TenantDto : BaseDto<Tenant>
    {
        [DataMember]
        public short C_TenantType_Id { get; set; }

        [DataMember]
        [NotEmptyOrDefault]
        public string Nazov { get; set; }

        [DataMember]
        public string Server { get; set; }

        [DataMember]
        public string Databaza { get; set; }

        /// <summary>
        /// Binds to entity.
        /// </summary>
        /// <param name="data"></param>
        protected override void BindToEntity(Tenant data)
        {
            data.C_TenantType_Id  = C_TenantType_Id;
            data.Nazov = Nazov;
            data.Server = Server;
            data.Databaza = Databaza;
        }
    }
    #endregion
}