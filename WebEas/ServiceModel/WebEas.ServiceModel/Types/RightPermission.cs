﻿using ServiceStack.DataAnnotations;
using System;
using System.Runtime.Serialization;

namespace WebEas.ServiceModel.Types
{
    [DataContract]
    [Schema("cfe")]
    [Alias("D_RightPermission")]
    public class RightPermission : BaseTenantEntity
    {
        [DataMember]
        [AutoIncrement]
        [PrimaryKey]
        public int D_RightPermission_Id { get; set; }
        
        [DataMember]
        [PfeColumn(Text = "Právo")]
        public int C_Right_Id { get; set; }

        [DataMember]
        [PfeColumn(Text = "Rola")]
        public int? C_Role_Id { get; set; }

        [DataMember]
        [PfeColumn(Text = "Používateľ")]
        public Guid? D_User_Id { get; set; }
    }
}
