using ServiceStack;
using ServiceStack.DataAnnotations;
using System;
using System.Runtime.Serialization;
using WebEas.Esam.ServiceModel.Office.Cfe.Types;
using WebEas.ServiceModel;

namespace WebEas.Esam.ServiceModel.Office.Cfe.Dto
{
    // Create
    [Route("/CreateTenant", "POST")]
    [Api("Tenant")]
    [DataContract]
    public class CreateTenant : TenantDto, IReturn<TenantView> { }

    // Update
    [Route("/UpdateTenant", "PUT")]
    [Api("Tenant")]
    [DataContract]
    public class UpdateTenant : TenantDto, IReturn<TenantView>
    {
        [PrimaryKey]
        [DataMember(IsRequired = true)]
        public Guid D_Tenant_Id { get; set; }
    }

    // Delete - nie je
    [Route("/DeleteTenant", "DELETE")]
    [Api("Tenant")]
    [DataContract]
    public class DeleteTenant
    {
        [PrimaryKey]
        [DataMember(IsRequired = true)]
        public Guid[] D_Tenant_Id { get; set; }
    }

    #region DTO
    [DataContract]
    public class TenantDto : BaseDto<Tenant>
    {
        [DataMember]
        public byte C_TenantType_Id { get; set; }

        [DataMember]
        public byte C_OrganizaciaTypDetail_Id { get; set; }

        [DataMember]
        [NotEmptyOrDefault]
        public string Nazov { get; set; }

        [DataMember]
        public string Server { get; set; }

        [DataMember]
        public string Databaza { get; set; }

        [DataMember]
        public Guid? D_Tenant_Id_Externe { get; set; }

        [DataMember]
        public long? D_PO_Osoba_Id { get; set; }

        //[DataMember]
        //public Guid? IsoId { get; set; }

        [DataMember]
        public string Telefon { get; set; }

        [DataMember]
        public string Fax { get; set; }

        [DataMember]
        public string Email { get; set; }

        [DataMember]
        public string Web { get; set; }

        [DataMember]
        public string Statutar { get; set; }

        /// <summary>
        /// Binds to entity.
        /// </summary>
        /// <param name="data"></param>
        protected override void BindToEntity(Tenant data)
        {
            data.C_TenantType_Id  = C_TenantType_Id;
            data.C_OrganizaciaTypDetail_Id = C_OrganizaciaTypDetail_Id;
            data.Nazov = Nazov;
            data.Server = Server;
            data.Databaza = Databaza;
            data.D_Tenant_Id_Externe = D_Tenant_Id_Externe;
            data.D_PO_Osoba_Id = D_PO_Osoba_Id;
            //data.IsoId = IsoId;
            data.Telefon = Telefon;
            data.Fax = Fax;
            data.Email = Email;
            data.Web = Web;
            data.Statutar = Statutar;
        }
    }
    #endregion
}