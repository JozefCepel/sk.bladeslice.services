using ServiceStack.DataAnnotations;
using System.Runtime.Serialization;
using WebEas.ServiceModel;

namespace WebEas.Esam.ServiceModel.Office.Types.Rzp
{
    [Schema("rzp")]
    [Alias("V_RzpPolNavrh")]
    [DataContract]
    public class RzpPolNavrhViewHelper
    {
        [PrimaryKey]
        [AutoIncrement]
        [DataMember]
        public long D_RzpPolNavrh_Id { get; set; }
    }
}
