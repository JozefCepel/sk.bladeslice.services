using System.Runtime.Serialization;
using ServiceStack;

namespace WebEas.ServiceModel
{
    [DataContract]
    public abstract class ListParovanieOsobRequestBase
    {
        [DataMember]
        public string ID_KN { get; set; }

        [DataMember]
        public string ID_DCOM { get; set; }

        [DataMember]
        public string Sparoval { get; set; }
    }
}