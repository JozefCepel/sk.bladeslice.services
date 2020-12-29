using ServiceStack;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace WebEas.Esam.ServiceModel.Office.Bds.Dto
{
    [Route("/D3DData", "POST")]
    [Api("Doklady")]
    [DataContract]
    public class D3DData
    {
        public string Type { get; set; }
        public string SN { get; set; }
        public string Sarza { get; set; }
        public string Location { get; set; }
        public decimal SklCena { get; set; }
        public List<Blok> Blok { get; set; }
        public List<Valec> Valec { get; set; }

        // Dodatočné parametre výstupu
        public int PocKs { get; set; }
        public decimal ObjemVyrez { get; set; }
        public decimal ObjemZvysok { get; set; }
        public decimal ObjemPlt { get; set; }
        public string OuterSize { get; set; }
        public string OuterSizeFinal { get; set; }
    }

    public class Blok
    {
        public bool Add { get; set; }
        public int a1 { get; set; }
        public int a2 { get; set; }
        public int b1 { get; set; }
        public int b2 { get; set; }
        public int L1 { get; set; }
        public int L2 { get; set; }
    }

    public class Valec
    {
        public bool Add { get; set; }
        public int D1 { get; set; }
        public int d2 { get; set; }
        public int L1 { get; set; }
        public int L2 { get; set; }
    }
}