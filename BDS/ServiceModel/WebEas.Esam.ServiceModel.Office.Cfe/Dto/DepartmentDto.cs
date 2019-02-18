﻿using ServiceStack;
using ServiceStack.DataAnnotations;
using System;
using System.Runtime.Serialization;
using WebEas.Esam.ServiceModel.Office.Cfe.Types;
using WebEas.ServiceModel;

namespace WebEas.Esam.ServiceModel.Office.Cfe.Dto
{
    // Create
    [WebEasRequiresAnyRole(RolesDefinition.Cfe.Roles.CfeMember)]
    [Route("/CreateDepartment", "POST")]
    [Api("Department")]
    [DataContract]
    public class CreateDepartment : DepartmentDto { }

    // Update
    [WebEasRequiredRole(Roles.Admin)]
    [Route("/UpdateDepartment", "PUT")]
    [Api("Department")]
    [DataContract]
    public class UpdateDepartment : DepartmentDto
    {
        [PrimaryKey]
        [DataMember(IsRequired = true)]
        public long D_Department_Id { get; set; }
    }

    // Delete
    [WebEasRequiresAnyRole(RolesDefinition.Cfe.Roles.CfeMember)]
    [Route("/DeleteDepartment", "DELETE")]
    [Api("Department")]
    [DataContract]
    public class DeleteDepartment
    {
        [DataMember(IsRequired = true)]
        public long D_Department_Id { get; set; }
    }

    #region DTO
    [DataContract]
    public class DepartmentDto : BaseDto<DepartmentView>
    {

        [DataMember]
        [Required]
        public int Typ { get; set; }

        [DataMember]
        [NotEmptyOrDefault]
        public string Kod { get; set; }

        [DataMember]
        [NotEmptyOrDefault]
        public string Nazov { get; set; }

        [DataMember]
        public int? C_Department_Id_Parent { get; set; }

        [DataMember]
        public int? Zodpovedny_id { get; set; }

        [DataMember]
        [NotEmptyOrDefault]
        public DateTime PlatnostOd { get; set; }

        [DataMember]
        public DateTime? PlatnostDo { get; set; }

        /// <summary>
        /// Binds to entity.
        /// </summary>
        /// <param name="data"></param>
        protected override void BindToEntity(DepartmentView data)
        {
            data.Typ = Typ;
            data.Kod = Kod;
            data.Nazov = Nazov;
            data.C_Department_Id_Parent = C_Department_Id_Parent;
            data.Zodpovedny_id = Zodpovedny_id;
            data.PlatnostOd = PlatnostOd;
            data.PlatnostDo = PlatnostDo;
        }
    }
    #endregion
}