using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using ServiceStack;

namespace WebEas.Esam.ServiceModel.Pfe.Dto
{
    [Route("/exportxlsx", "POST")]
    [Api("Export")]
    
    [DataContract]
    public class ExportDto
    {
        [DataMember]
        public string KodPolozky { get; set; }

        [DataMember]
        public string DocumentTitle { get; set; }

        [DataMember]
        public string Items { get; set; }
    }

    [DataContract]
    public class ExportItemDto
    {
        [DataMember]
        public int D_Pohlad_Id { get; set; }

        [DataMember]
        public string KodPolozky { get; set; }

        [DataMember]
        public string PohladNazov { get; set; }

        [DataMember]
        public string PolozkaNazov { get; set; }

        [DataMember]
        public string Filter { get; set; }

        [DataMember]
        public string TextFilter { get; set; }

        [DataMember]
        public string Sort { get; set; }

        [DataMember]
        public string Group { get; set; }

        [DataMember]        
        public List<ExportItemColDto> Columns { get; set; }

        [DataMember]
        public List<Dictionary<string, object>> Values { get; set; }
    }

    [DataContract]
    public class ExportItemColDto
    {
        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public string Text { get; set; }

        [DataMember]
        public int Width { get; set; }

        [DataMember]
        public string Type { get; set; }

        [DataMember]
        public string Align { get; set; }
    }
}