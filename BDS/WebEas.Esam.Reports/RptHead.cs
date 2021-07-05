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
        public long RezimDph { get; set; }
        public bool PlatcaDph
        {
            get { return RezimDph == 2 || RezimDph == 4; }  // 0 = neplatca; 1 = neplatca s evidenciou DPH na došlých dokladoch; 2 = mesačný platca; 4 = štvrťročný platca
        }
        public List<ZostavaFilter> Filtre { get; set; }

        // ---------------------------------
        public string Formatuj(string text, bool bold, int size = 0)
        {
            var s = text;
            if (size > 0) s = "<span style =\"font - size: " + size + "pt\">" + text + "</span>";
            if (bold) s = "<strong>" + s + "</strong>";
            return s;
        }
    }
}
