using ServiceStack.DataAnnotations;
using System.Runtime.Serialization;
using WebEas.ServiceModel;

namespace WebEas.Esam.ServiceModel.Office.Types.Vyk
{
    [Schema("vyk")]
    [Alias("D_VykazFin112")]
    [DataContract]
    public class VykazFin112Helper : Fin1PolBaseHelper
    {
        [PrimaryKey]
        [AutoIncrement]
        [DataMember]
        public long D_VykazFin112_Id { get; set; }

        [DataMember]
        public long D_Vykaz_Id { get; set; }
    }
}
