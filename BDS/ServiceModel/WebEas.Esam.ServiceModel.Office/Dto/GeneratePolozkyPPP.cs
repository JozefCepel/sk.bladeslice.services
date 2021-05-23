using ServiceStack;
using System;
using System.Runtime.Serialization;

namespace WebEas.Esam.ServiceModel.Office.Dto
{
    [Route("/GeneratePolozkyPPP", "POST")]
    [Api("Doklad")]
    [DataContract]
    public class GeneratePolozkyPPPDto
    {
        [DataMember]
        public long[] D_BiznisEntita_Id_Predpis { get; set; }

        [DataMember]
        public long[] D_Vymer_Id { get; set; }

        [DataMember]
        public int? C_BankaUcet_Id { get; set; }

        [DataMember]
        public long? D_BiznisEntita_Id { get; set; }

        [DataMember]
        public string SelectedEvidence { get; set; }

        [DataMember]
        public DateTime Date { get; set; }

        [DataMember]
        public DateTime DateMaturity { get; set; }

        [DataMember]
        public string Description { get; set; }

        [DataMember]
        public bool ProcessDocument { get; set; }
    }
}
