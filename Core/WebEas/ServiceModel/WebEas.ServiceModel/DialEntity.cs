using System;
using System.Linq;
using System.Runtime.Serialization;

namespace WebEas.ServiceModel
{
    [DataContract]
    public class DialEntity<T> : ITenantEntityNullable, IDialEntity<T>
    {
        [DataMember]
        public T ItemCode { get; set; }

        [DataMember]
        public string ItemName { get; set; }

        [IgnoreDataMember]
        public Guid? D_Tenant_Id { get; set; }
    }
}