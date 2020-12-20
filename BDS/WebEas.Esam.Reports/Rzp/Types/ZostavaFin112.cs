using System;
using System.Collections.Generic;

namespace WebEas.Esam.Reports.Rzp.Types
{
    [System.ComponentModel.DataObject]
    public class ZostavaFin112 : RptHead
    {
        public ZostavaFin112()
        {
            if (System.Diagnostics.Process.GetCurrentProcess().ProcessName == "Telerik.ReportDesigner")
            {
                SetTestData(this);
            }
        }

        public void SetTestData(ZostavaFin112 fin112)
        {
            fin112.ICO = "12345678";
            fin112.Nazov = "Mesto Gbely";
            fin112.Ulica = "Nám. Slobody 1261";
            fin112.PSC = "90845";
            fin112.Obec = "Gbely";
            fin112.Vytlacil = "Jozko Mrkvicka";
            fin112.Rok = 2020;
            fin112.Mesiac = 12;
            fin112.Polozky = new List<ZostavaFin112.ZostavaFin112Polozky>();

            var p = new ZostavaFin112.ZostavaFin112Polozky();
            p.ZdrojKod = "41";
            p.EK = "121001";
            p.EKPolozka = "312";
            p.EKPodpolozka = "007";
            p.EKNazov = "Z rozpočtu obce";
            p.RzpSchvaleny = 58000;
            p.RzpUpraveny = 10000;
            p.RzpOcakavany = 90000;
            p.RzpSkutocnost = 100000;
            fin112.Polozky.Add(p);

            p = new ZostavaFin112.ZostavaFin112Polozky();
            p.ZdrojKod = "41";
            p.EK = "312007";
            p.EKPolozka = "312";
            p.EKPodpolozka = "007";
            p.EKNazov = "Z rozpočtu obce";
            p.RzpSchvaleny = 1;
            p.RzpUpraveny = 2;
            p.RzpOcakavany = 3;
            p.RzpSkutocnost = 4;
            fin112.Polozky.Add(p);

            p = new ZostavaFin112.ZostavaFin112Polozky();
            p.ZdrojKod = "42";
            p.EK = "312007";
            p.EKNazov = "Z rozpočtu obce";
            p.RzpSchvaleny = 4;
            p.RzpUpraveny = 3;
            p.RzpOcakavany = 2;
            p.RzpSkutocnost = 1;
            fin112.Polozky.Add(p);

            p = new ZostavaFin112.ZostavaFin112Polozky();
            p.ZdrojKod = "11E1";
            p.EK = "312007";
            p.EKNazov = "Z rozpočtu obce";
            p.RzpSchvaleny = 4;
            p.RzpUpraveny = 3;
            p.RzpOcakavany = 2;
            p.RzpSkutocnost = 1;
            fin112.Polozky.Add(p);
        }

        public DateTime ZostaveneDna { get; set; }
        public int Mesiac { get; set; }
        public int Rok { get; set; }
        public string ZaObdobie
        {
            get { return Mesiac + "." + Rok; }
        }
        public string TextNadpis { get; set; }
        // ---------------------------------

        public List<ZostavaFin112Polozky> Polozky { get; set; }

        [System.ComponentModel.DataObject]
        public class ZostavaFin112Polozky
        {
            public int PrijemVydaj { get; set; } // len na filtrovanie
            public int? C_RzpTyp_Id { get; set; } // len na filtrovanie

            public string KodUctu { get; set; }  // toto este nemame, ale je to v reporte PDF

            public string DruhRzp { get; set; }

            public string ZdrojKod { get; set; }

            public string ProgramKod { get; set; }

            public string FKOddiel { get; set; }

            public string FKSkupina { get; set; }

            public string FKTrieda { get; set; }

            public string FKPodtrieda { get; set; }

            public string FK { get; set; }

            public string EKPolozka { get; set; }

            public string EKPodpolozka { get; set; }

            public string EK { get; set; }

            public string EKNazov { get; set; }

            public decimal? RzpSchvaleny { get; set; }

            public decimal? RzpUpraveny { get; set; }

            public decimal? RzpOcakavany { get; set; }

            public decimal? RzpSkutocnost { get; set; }

        }

    }
}
