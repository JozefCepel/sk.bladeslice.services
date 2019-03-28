using ServiceStack;
using ServiceStack.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using WebEas.Esam.ServiceModel.Office.Bds.Types;
using WebEas.ServiceModel;

namespace WebEas.Esam.ServiceModel.Office.Bds.Dto
{
    [WebEasRequiresAnyRole(RolesDefinition.Bds.Roles.BdsWriter)]
    [Route("/GetVybavDoklady", "POST")]
    [Api("Doklady")]
    [DataContract]
    public class GetVybavDokladyReq : IReturn<List<long>>
    {
        [DataMember]
        public long[] IDs { get; set; }

        [DataMember]
        public string IdField { get; set; }
    }

    [WebEasRequiresAnyRole(RolesDefinition.Bds.Roles.BdsWriter)]
    [Route("/GetOdvybavDoklady", "POST")]
    [Api("Doklady")]
    [DataContract]
    public class GetOdvybavDokladyReq : IReturn<List<long>>
    {
        [DataMember]
        public long[] IDs { get; set; }

        [DataMember]
        public string IdField { get; set; }
    }
}