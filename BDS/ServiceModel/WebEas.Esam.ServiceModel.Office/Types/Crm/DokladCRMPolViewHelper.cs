using ServiceStack.DataAnnotations;
using System;
using System.Runtime.Serialization;
using WebEas.ServiceModel;

namespace WebEas.Esam.ServiceModel.Office.Types.Crm
{
    [DataContract]
    [Schema("crm")]
    [Alias("V_DokladCRMPol")]
    public class DokladCRMPolViewHelper : BaseTenantEntity
    {
        [DataMember]
        [AutoIncrement]
        [PrimaryKey]
        public long D_DokladCRMPol_Id { get; set; }

        [DataMember]
        public long D_BiznisEntita_Id { get; set; }

        [DataMember]
        public short Rok { get; set; }

        [DataMember]
        public int C_Typ_Id { get; set; }

        [DataMember]
        public int Poradie { get; set; }

        [DataMember]
        public int? C_Stredisko_Id { get; set; }

        [DataMember]
        public long? C_Projekt_Id { get; set; }

        [DataMember]
        public decimal DM_Cena { get; set; }

        [DataMember]
        public decimal Mnozstvo { get; set; }

        [DataMember]
        public byte DPH { get; set; }

        [DataMember]
        public long? C_UctKluc1_Id { get; set; }

        [DataMember]
        public long? C_UctKluc2_Id { get; set; }

        [DataMember]
        public long? C_UctKluc3_Id { get; set; }
    }
}
