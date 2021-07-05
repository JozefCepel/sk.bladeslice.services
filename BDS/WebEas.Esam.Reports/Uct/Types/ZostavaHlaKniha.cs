using System;
using System.Collections.Generic;

namespace WebEas.Esam.Reports.Uct.Types
{
    [System.ComponentModel.DataObject]
    public class ZostavaHlaKniha : RptHead
    {
        public ZostavaHlaKniha()
        {
            if (System.Diagnostics.Process.GetCurrentProcess().ProcessName == "Telerik.ReportDesigner")
            {
                SetTestData(this);
            }
        }

        public void SetTestData(ZostavaHlaKniha kniha)
        {
            kniha.ICO = "12345678";
            kniha.Nazov = "Mesto Gbely";
            kniha.Ulica = "Nám. Slobody 1261";
            kniha.PSC = "90845";
            kniha.Obec = "Gbely";
            kniha.Vytlacil = "Jozko Mrkvicka";
            kniha.showUO = false;
            kniha.showStredisko = false;
            kniha.showProjekt = true;
            kniha.showKluc1 = true;
            kniha.showKluc2 = true;
            kniha.showKluc3 = true;
            kniha.klucCaption1 = "";
            kniha.klucCaption2 = "PHM";
            kniha.klucCaption3 = "Môj kľúč 3";

            kniha.Filtre = new List<ZostavaFilter>();
            kniha.Polozky = new List<ZostavaHlaKniha.ZostavaHlaKnihaPol>();

            kniha.Filtre.Add(new ZostavaFilter("Zakaznik = Dezko"));
            kniha.Filtre.Add(new ZostavaFilter("Datum = 90.99.2099"));

            var p = new ZostavaHlaKniha.ZostavaHlaKnihaPol();
            p.Radenie = "2";
            p.RozvrhUcet = "012.439x";
            p.UOMesiac = 5;
            p.RozvrhNazov = "Aktivované náklady na vývoj";
            p.PStavR = 3676901.11M;
            p.PStavM = 3676901.12M;
            p.MObratMD = 3676901.13M;
            p.MObratDal = 3676901.14M;
            p.RObratMD = 3676901.15M;
            p.RObratDal = 3676901.16M;
            p.KStav = -3676901.17M;
            p.StrediskoNazov = "chránené pracovisko";
            p.ProjektNazov = "Projekt 01";
            p.KlucNazov1 = "Kod1";
            p.KlucNazov2 = "Kod2";
            p.KlucNazov3 = "Kod3";
            kniha.Polozky.Add(p);

            p = new ZostavaHlaKniha.ZostavaHlaKnihaPol();
            p.Radenie = "1";
            p.RozvrhUcet = "012";
            p.UOMesiac = 5;
            p.RozvrhNazov = "Vývoj";
            p.PStavR = 2;
            p.PStavM = 3;
            p.MObratMD = 4;
            p.MObratDal = 5;
            p.RObratMD = 6;
            p.RObratDal = 7;
            p.KStav = 8;
            p.StrediskoNazov = "P 3";
            p.ProjektNazov = "Projekt 02";
            p.KlucNazov1 = "Kod1";
            p.KlucNazov2 = "Kod2";
            p.KlucNazov3 = "Kod3";
            kniha.Polozky.Add(p);
        }

        public bool showUO { get; set; }
        public bool showStredisko { get; set; }
        public bool showProjekt { get; set; }
        public bool showKluc1 { get; set; }
        public bool showKluc2 { get; set; }
        public bool showKluc3 { get; set; }
        public string klucCaption1 { get; set; }
        public string klucCaption2 { get; set; }
        public string klucCaption3 { get; set; }
        public int rok { get; set; }
        public byte? obdobieOd { get; set; }
        public DateTime? datumOd { get; set; }

        public List<ZostavaHlaKnihaPol> Polozky { get; set; }

        [System.ComponentModel.DataObject]
        public class ZostavaHlaKnihaPol
        {
            public string Radenie { get; set; } // len na sort
            public string RozvrhUcet { get; set; }
            public byte? UOMesiac { get; set; }
            public string RozvrhNazov { get; set; }
            public decimal? PStavR { get; set; }
            public decimal? PStavM { get; set; }
            public decimal? MObratMD { get; set; }
            public decimal? MObratDal { get; set; }
            public decimal? RObratMD { get; set; }
            public decimal? RObratDal { get; set; }
            public decimal? KStav { get; set; }
            public string StrediskoNazov { get; set; }
            public string ProjektNazov { get; set; }
            public string KlucNazov1 { get; set; }
            public string KlucNazov2 { get; set; }
            public string KlucNazov3 { get; set; }
        }
    }
}
