using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace WebEas.Esam.ServiceModel.Office.Dto
{
    [DataContract]
    public class ContextUser
    {
        [DataMember]
        public string TenantId { get; set; }

        [DataMember]
        public string FormattedName { get; set; }

        [DataMember]
        public string DomenaName { get; set; }

        [DataMember]
        public string ActorId { get; set; }

        [DataMember]
        public bool ModuleAdmin { get; set; }

        [DataMember]
        public bool DcomAdmin { get; set; }

        [DataMember]
        public bool IsWriter { get; set; }

        [DataMember]
        public string Version { get; set; }

        [DataMember]
        public string Released { get; set; }

        [DataMember]
        public string Environment { get; set; }

        [DataMember]
        public string DbReleased { get; set; }

        [DataMember]
        public string VillageName { get; set; }

        [DataMember]
        public bool HasMultipleTenants { get; set; }

        [DataMember]
        public string OperationType { get; set; }

        [DataMember]
        public long FilterRok { get; set; }
    }
}
