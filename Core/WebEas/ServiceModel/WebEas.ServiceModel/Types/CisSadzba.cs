using System.Runtime.Serialization;
using ServiceStack.DataAnnotations;

namespace WebEas.ServiceModel.Office.Egov.Types
{
    [DataContract]
    [Schema("zdv_int")]
	[Alias("V_DAP_C_Sadzba")]
    public class CisSadzba
	{
        [DataMember]
        public int C_Sadzba_Id { get; set; }

        [DataMember]
        public string Popis { get; set; }

        [DataMember]
        public decimal Sadzba { get; set; }

        [DataMember]
        public string Druh { get; set; }

		[IgnoreDataMember]
		public string Kod { get; set; }

        [DataMember]
        public string Odsek { get; set; }

		[IgnoreDataMember]
		public short Rok { get; set; }

        [DataMember]
        public byte Cislo { get; set; }
	}
}
