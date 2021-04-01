using System;
using System.Collections.Generic;

namespace WebEas.Esam.Reports.Rzp.Types
{
    [System.ComponentModel.DataObject]
    public class ZostavaRzpDennik : RptHead
    {
        public ZostavaRzpDennik()
        {
            if (System.Diagnostics.Process.GetCurrentProcess().ProcessName == "Telerik.ReportDesigner")
            {
                SetTestData(this);
            }
        }

        public void SetTestData(ZostavaRzpDennik dennik)
        {
            dennik.ICO = "12345678";
            dennik.Nazov = "Mesto Gbely";
            dennik.Ulica = "Nám. Slobody 1261";
            dennik.PSC = "90845";
            dennik.Obec = "Gbely";
            dennik.Vytlacil = "Jozko Mrkvicka";
            dennik.StrediskoCaption = "Stredisko XX";
            dennik.Filtre = new List<ZostavaFilter>();
            dennik.Polozky = new List<ZostavaRzpDennik.ZostavaRzpDennikPol>();

            var f1 = new ZostavaFilter();
            f1.Filter = "Zakaznik = Dezko";
            dennik.Filtre.Add(f1);
            var f2 = new ZostavaFilter();
            f2.Filter = "Datum = 90.99.2099";
            dennik.Filtre.Add(f2);

            var p = new ZostavaRzpDennik.ZostavaRzpDennikPol();
            p.DatumUctovania = new DateTime(2020, 1, 17);
            p.BiznisEntitaPopis = "Doc2";
            p.PV = "P";
            p.ZD = "11GH";
            p.EK = "625001";
            p.FK = "1401";
            p.A1 = "123";
            p.A2 = "123";
            p.A3 = "123";
            p.NazovPolozky = "Na nemocenské poistenie";
            p.ProgramFull = "12.3.5 - Test prvok zo sumárneho pohľadu";
            p.Suma = 1000000M;
            p.Popis = "rrr32222";
            p.StrediskoNazov = "chránené pracovisko";
            p.ProjektNazov = "Projekt 01";
            dennik.Polozky.Add(p);

            p = new ZostavaRzpDennik.ZostavaRzpDennikPol();
            p.DatumUctovania = new DateTime(2020, 1, 17);
            p.BiznisEntitaPopis = "Doc1";
            p.Suma = 200M;
            dennik.Polozky.Add(p);

            p = new ZostavaRzpDennik.ZostavaRzpDennikPol();
            p.DatumUctovania = new DateTime(2020, 1, 17);
            p.BiznisEntitaPopis = "Doc1";
            p.ZD = "AAAA 45";
            p.Suma = 300M;
            dennik.Polozky.Add(p);

            p = new ZostavaRzpDennik.ZostavaRzpDennikPol();
            p.DatumUctovania = new DateTime(2020, 1, 17);
            p.BiznisEntitaPopis = "Doc1";
            p.ZD = "BBBB 12";
            p.Suma = 200M;
            dennik.Polozky.Add(p);

            p = new ZostavaRzpDennik.ZostavaRzpDennikPol();
            p.Suv = true;
            p.DatumUctovania = new DateTime(2020, 1, 17);
            p.BiznisEntitaPopis = "Doc1";
            p.NazovPolozky = "suv suv suv";
            p.Popis = "200,00 Popis";
            dennik.Polozky.Add(p);

            p = new ZostavaRzpDennik.ZostavaRzpDennikPol();
            p.DatumUctovania = new DateTime(2020, 1, 15);
            p.BiznisEntitaPopis = "DFA 01-20.09/0001 (520104810)";
            p.ZD = "Z21 E1";
            p.Suma = 153.45M;
            p.Popis = "Služby spojená s nájmom";
            p.StrediskoNazov = "Banska Bystrica";
            p.ProjektNazov = "Project Green";

            dennik.Polozky.Add(p);
        }
        public List<ZostavaRzpDennikPol> Polozky { get; set; }

        [System.ComponentModel.DataObject]
        public class ZostavaRzpDennikPol
        {
            public bool Suv { get; set; }

            public DateTime DatumUctovania { get; set; }  // Dátum
            public string BiznisEntitaPopis { get; set; } // Doklad
            public int Poradie { get; set; }
            public string PV { get; set; }
            public string ZD { get; set; }
            public string FK { get; set; }
            public string EK { get; set; }
            public string A1 { get; set; }
            public string A2 { get; set; }
            public string A3 { get; set; }
            public string NazovPolozky { get; set; }
            public string ProgramFull { get; set; }
            public decimal? Suma { get; set; }
            public string Popis { get; set; }
            public string StrediskoNazov { get; set; }    // Oddelenie
            public string ProjektNazov { get; set; }
        }
    }
}
