using System;
using System.Collections.Generic;

namespace WebEas.Esam.Reports.Uct.Types
{
    [System.ComponentModel.DataObject]
    public class ZostavaUctDennik : RptHead
    {
        public ZostavaUctDennik()
        {
            if (System.Diagnostics.Process.GetCurrentProcess().ProcessName == "Telerik.ReportDesigner")
            {
                 SetTestData(this);
            }
        }

        public void SetTestData(ZostavaUctDennik dennik)
        {
            dennik.ICO = "12345678";
            dennik.Nazov = "Mesto Gbely";
            dennik.Ulica = "Nám. Slobody 1261";
            dennik.PSC = "90845";
            dennik.Obec = "Gbely";
            dennik.Vytlacil = "Jozko Mrkvicka";
            dennik.Filtre = new List<ZostavaFilter>();
            dennik.Polozky = new List<ZostavaUctDennik.ZostavaUctDennikPol>();

            dennik.Filtre.Add(new ZostavaFilter("Zakaznik = Dezko"));
            dennik.Filtre.Add(new ZostavaFilter("Datum = 90.99.2099"));

            var p = new ZostavaUctDennik.ZostavaUctDennikPol();
            p.DatumUctovania = new DateTime(2020, 1, 17);
            p.BiznisEntitaPopis = "Doc2";
            p.RozvrhUcet = "YYYYYY 45";
            p.SumaMD = 1000000M;
            p.SumaDal = 1000000M;
            dennik.Polozky.Add(p);

            p = new ZostavaUctDennik.ZostavaUctDennikPol();
            p.DatumUctovania = new DateTime(2020, 1, 17);
            p.BiznisEntitaPopis = "Doc1";
            p.SumaMD = 200M;
            dennik.Polozky.Add(p);

            p = new ZostavaUctDennik.ZostavaUctDennikPol();
            p.DatumUctovania = new DateTime(2020, 1, 17);
            p.BiznisEntitaPopis = "Doc1";
            p.RozvrhUcet = "AAAA 45";
            p.SumaMD = 300M;
            dennik.Polozky.Add(p);

            p = new ZostavaUctDennik.ZostavaUctDennikPol();
            p.DatumUctovania = new DateTime(2020, 1, 17);
            p.BiznisEntitaPopis = "Doc1";
            p.RozvrhUcet = "BBBB 12";
            p.SumaMD = 200M;
            dennik.Polozky.Add(p);

            p = new ZostavaUctDennik.ZostavaUctDennikPol();
            p.Suv = true;
            p.VS = "Pr.: 1.2.3";
            p.DatumUctovania = new DateTime(2020, 1, 17);
            p.BiznisEntitaPopis = "Doc1";
            p.RozvrhUcet = "suv suv";
            p.Popis = "123,00 Popis";
            dennik.Polozky.Add(p);

            p = new ZostavaUctDennik.ZostavaUctDennikPol();
            p.DatumUctovania = new DateTime(2020, 1, 15);
            p.BiznisEntitaPopis = "DFA 01-20.09/0001 (520104810)";
            p.RozvrhUcet = "Z21 E1";
            p.SumaMD = 153.45M;
            p.SumaDal = 35.12M;
            p.Popis = "Služby spojená s nájmom";
            p.StrediskoNazov = "Banska Bystrica";
            p.ProjektNazov = "Project Green";

            dennik.Polozky.Add(p);
        }

        public List<ZostavaUctDennikPol> Polozky { get; set; }

        [System.ComponentModel.DataObject]
        public class ZostavaUctDennikPol
        {
            public string StrediskoCaption { get; set; }  // vies to potom nadesignovat priamo v reporte (nemusis robit Find Object)
            public bool Suv { get; set; }
            public DateTime DatumUctovania { get; set; }  // Dátum
            public string BiznisEntitaPopis { get; set; } // Doklad
            public int Poradie { get; set; }
            public string RozvrhUcet { get; set; }
            public decimal SumaMD { get; set; }
            public decimal? SumaDal { get; set; }
            public string SumaMDSuv
            {
                get { return Suv ? VS : SumaMD.ToString("N2"); }
            }
            public string Popis { get; set; }
            public string VS { get; set; }
            public string StrediskoNazov { get; set; }    // Oddelenie
            public string ProjektNazov { get; set; }
        }

    }
}
