namespace WebEas.Esam.ServiceModel.Office.Types.Osa
{
    public interface IOsobaExt
    {
        public bool? Danovnik { get; set; }
        public bool? Dodavatel { get; set; }
        public bool? Odberatel { get; set; }
        public decimal? DM_Neuhradene_DAP { get; set; }
        public decimal? DM_Neuhradene_DFA { get; set; }
        public decimal? DM_Neuhradene_DZF { get; set; }
        public decimal? DM_Neuhradene_OFA { get; set; }
        public decimal? DM_Neuhradene_OZF { get; set; }
        public decimal? DM_Neuhradene { get; set; }
        public bool? P { get; set; }
    }
}
