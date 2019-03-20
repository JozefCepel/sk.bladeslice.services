using ServiceStack.DataAnnotations;
using System;
using System.Runtime.Serialization;
using WebEas.ServiceModel;

namespace WebEas.Esam.ServiceModel.Office.Rzp.Types
{
    [Schema("rzp")]
    [Alias("D_Fin1Pol")]
    [DataContract]
    public class Fin1Pol : Fin1PolBase
    {
        [PrimaryKey]
        [AutoIncrement]
        [DataMember]
        public long D_Fin1Pol_Id { get; set; }

        [DataMember]
        [PfeColumn(Text = "_D_Fin1_Id", ReadOnly = true)]
        public long D_Fin1_Id { get; set; }

    }
}
