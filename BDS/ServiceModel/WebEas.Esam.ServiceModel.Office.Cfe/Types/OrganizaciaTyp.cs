﻿using ServiceStack.DataAnnotations;
using System;
using System.Runtime.Serialization;
using WebEas.ServiceModel;

namespace WebEas.Esam.ServiceModel.Office.Cfe.Types
{
    [DataContract]
    [Schema("cfe")]
    [Alias("C_OrganizaciaTyp")]
    public class OrganizaciaTyp : BaseEntity
    {
        [DataMember]
        [AutoIncrement]
        [PrimaryKey]
        [PfeColumn(Text = "_C_OrganizaciaTyp_Id")]
        public int C_OrganizaciaTyp_Id { get; set; }

        [DataMember]
        [PfeColumn(Text = "Názov typu organizácie")]
        public string Nazov { get; set; }

        [DataMember]
        [PfeColumn(Text = "Platnosť od")]
        public DateTime PlatnostOd { get; set; }

        [DataMember]
        [PfeColumn(Text = "Platnosť do")]
        public DateTime? PlatnostDo { get; set; }

    }
}
