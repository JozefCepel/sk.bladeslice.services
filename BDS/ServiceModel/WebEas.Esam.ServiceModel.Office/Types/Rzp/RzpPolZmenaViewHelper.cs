using ServiceStack.DataAnnotations;
using System.Runtime.Serialization;
using WebEas.ServiceModel;

namespace WebEas.Esam.ServiceModel.Office.Types.Rzp
{
    [Schema("rzp")]
    [Alias("V_RzpPolZmena")]
    [DataContract]
    public class RzpPolZmenaViewHelper
    {
        [PrimaryKey]
        [AutoIncrement]
        [DataMember]
        public long D_RzpPolZmena_Id { get; set; }
    }
}
