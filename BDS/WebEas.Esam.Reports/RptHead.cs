using System;
using System.Collections.Generic;

namespace WebEas.Esam.Reports
{
    [System.ComponentModel.DataObject]
    public class RptHead : IReportData
    {
        public long Id { get; set; } // povinne
        public long D_PO_Osoba_Id { get; set; }
        public string ICO { get; set; }
        public string IcDph { get; set; }
        public string Dic { get; set; }
        public string Nazov { get; set; }
        public string Ulica { get; set; }
        public string PSC { get; set; }
        public string Obec { get; set; }
        public string OrganizaciaTyp { get; set; }
        public byte OrganizaciaTyp_Id { get; set; }
        public string Telefon { get; set; }
        public string Fax { get; set; }
        public string Mail { get; set; }
        public string Vytlacil { get; set; }
        public DateTime Datum { get; set; } = DateTime.Today; // Zostavene dna
        public string RptPath { get; set; } = "bin/Reports/"; // Design a Release cesta byva ina (pouziva sa pre subreporty)
        public string StrediskoCaption { get; set; }  // volitelny nazov strediska
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
