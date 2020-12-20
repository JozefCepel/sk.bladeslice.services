using ServiceStack.DataAnnotations;
using System.Runtime.Serialization;
using WebEas.ServiceModel;

namespace WebEas.Esam.ServiceModel.Office.Types.Uct
{
    [DataContract]
    [Schema("uct")]
    [Alias("V_UctDennik")]
    public class UctDennikViewHelper : BaseTenantEntity, IBaseView
    {
        [DataMember]
        [PrimaryKey]
        [AutoIncrement]
        public long D_UctDennik_Id { get; set; }

        [DataMember]
        public long D_BiznisEntita_Id { get; set; }

        [DataMember]
        public string SDK { get; set; }

        [DataMember]
        public string SU { get; set; }

        [DataMember]
        public bool U { get; set; }

        [DataMember]
        public long? D_Osoba_Id { get; set; }

        [DataMember]
        public string VS { get; set; }

        [DataMember]
        public long? C_UctRozvrh_Id { get; set; }

        [DataMember]
        public decimal SumaMD { get; set; }

        [DataMember]
        public decimal SumaDal { get; set; }

        [DataMember]
        public int? C_Stredisko_Id { get; set; }

        [DataMember]
        public long? C_Projekt_Id { get; set; }

        [DataMember]
        public long? C_UctKluc_Id1 { get; set; }

        [DataMember]
        public long? C_UctKluc_Id2 { get; set; }

        [DataMember]
        public long? C_UctKluc_Id3 { get; set; }

        [DataMember]
        public int? C_Typ_Id { get; set; }

        [DataMember]
        public long? D_VymerPol_Id { get; set; }

        [DataMember]
        public string VytvorilMeno { get; set; }

        [DataMember]
        public string ZmenilMeno { get; set; }
    }
}
