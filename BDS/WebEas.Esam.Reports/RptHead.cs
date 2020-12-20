using System.Collections.Generic;

namespace WebEas.Esam.Reports
{
    [System.ComponentModel.DataObject]
    public class RptHead : IReportData
    {
        public long Id { get; set; } // povinne
        public string ICO { get; set; }
        public string Nazov { get; set; }
        public string Ulica { get; set; }
        public string PSC { get; set; }
        public string Obec { get; set; }
        public string OrganizaciaTyp { get; set; }
        public int OrganizaciaTyp_Id { get; set; }
        public string Telefon { get; set; }
        public string Fax { get; set; }
        public string Mail { get; set; }
        public string Vytlacil { get; set; }
        public string Sidlo
        {
            get { return Ulica + ", " + PSC + " " + Obec; }
        }
        public string NazovSidlo
        {
            get { return Nazov + " (" + Sidlo + ")"; }
        }

        public List<ZostavaFilter> Filtre { get; set; }

        // ---------------------------------

    }
}
