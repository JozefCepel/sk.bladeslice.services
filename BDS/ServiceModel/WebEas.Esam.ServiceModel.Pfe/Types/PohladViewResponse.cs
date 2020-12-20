using System.Runtime.Serialization;

namespace WebEas.ServiceModel.Pfe.Types
{
    [DataContract]
    public class PohladViewResponse : PohladView
    {

        [DataMember(Name = "acts")]
        public string Actions { get; set; }
    }
}