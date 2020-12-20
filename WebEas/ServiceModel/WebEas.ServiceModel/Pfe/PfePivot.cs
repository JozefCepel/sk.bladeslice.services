using System.Runtime.Serialization;

namespace WebEas.ServiceModel
{
    [DataContract]
    public class PfePivot
    {
        [DataMember(Name = "mtx")]
        public PfeMatrix Matrix { get; set; }
    }

    [DataContract]
    public class PfeMatrix
    {
        [DataMember(Name = "las")]
        public PfePivotAxis[] LeftAxis { get; set; }

        [DataMember(Name = "tas")]
        public PfePivotAxis[] TopAxis { get; set; }

        [DataMember(Name = "are")]
        public object[] Aggregate { get; set; }

        [DataMember(Name = "cgp")]
        public int ColGrandTotalsPosition { get; set; }

        [DataMember(Name = "csp")]
        public int ColSubTotalsPosition { get; set; }

        [DataMember(Name = "rgp")]
        public int RowGrandTotalsPosition { get; set; }

        [DataMember(Name = "rsp")]
        public int RowSubTotalsPosition { get; set; }

        [DataMember(Name = "szb")]
        public bool ShowZeroAsBlank { get; set; }

        [DataMember(Name = "vlt")]
        public int ViewLayoutType { get; set; }
    }

    [DataContract]
    public class PfePivotAxis
    {
        [DataMember(Name = "agr")]
        public string Aggregator { get; set; }

        [DataMember(Name = "aln")]
        public int Align { get; set; }

        [DataMember(Name = "dix")]
        public string DataIndex { get; set; }

        [DataMember(Name = "dcn")]
        public int Direction { get; set; }

        [DataMember(Name = "flx")]
        public int Flex { get; set; }

        [DataMember(Name = "fmr")]
        public string Formatter { get; set; }

        [DataMember(Name = "szk")]
        public bool ShowZeroAsBlank { get; set; }

        [DataMember(Name = "six")]
        public string SortIndex { get; set; }

        [DataMember(Name = "ste")]
        public bool Sortable { get; set; }

        [DataMember(Name = "wid")]
        public int Width { get; set; }

        [DataMember(Name = "lrn")]
        public string LabelRendererName { get; set; }
    }
}
