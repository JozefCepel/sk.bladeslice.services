using System;
using System.Collections.Generic;

namespace WebEas.Esam.Reports.Crm.Types
{
    [System.ComponentModel.DataObject]
    public class ZostavaKnihaFaktur : RptHead
    {
        public ZostavaKnihaFaktur()
        {
            if (System.Diagnostics.Process.GetCurrentProcess().ProcessName == "Telerik.ReportDesigner")
            {
                 SetTestData(this);
            }
        }

        public void SetTestData(ZostavaKnihaFaktur h)
        {
            h.ICO = "12345678";
            h.Nazov = "Mesto Gbely";
            h.Ulica = "Nám. Slobody 1261";
            h.PSC = "90845";
            h.Obec = "Gbely";
            h.Vytlacil = "Jozko Mrkvicka";
            h.Filtre = new List<ZostavaFilter>();
            h.Polozky = new List<ZostavaKnihaFakturPol>();
            h.DodOdb = false;
            h.Filtre.Add(new ZostavaFilter("Kniha: MDFA, JDZF"));
            h.Filtre.Add(new ZostavaFilter("Obdobie: 01.01.2020 - 31.01.2020"));

            var p = new ZostavaKnihaFakturPol();
            p.DodOdb = h.DodOdb;
            p.Datum = new DateTime(2020, 1, 17);
            p.DatSplatnosti = new DateTime(2020, 10, 18);
            p.Doklad = "MDFA 21-20.01/0002";
            p.CisloFA = "2850028913";
            p.Partner = "Železnice Slovenskej republiky";
            p.Popis = "prenájom v zmysle zmluvy";
            p.DatDodania = new DateTime(2020, 11, 18);
            p.DatVystavenia = new DateTime(2020, 12, 18);
            p.Suma = 1111316.25M;
            p.Zaloha = 0;
            p.Uhradene = 0;
            p.Neuhradene = 1111316.25M;
            p.DatPoslUhrady = new DateTime(2022, 10, 31);
            p.DokladUhrady = "PDK-V: 00-21.04/0004";
            h.Polozky.Add(p);

            p = new ZostavaKnihaFakturPol();
            p.DodOdb = h.DodOdb;
            p.Datum = new DateTime(2020, 1, 17);
            p.DatSplatnosti = new DateTime(2020, 10, 18);
            p.Doklad = "MDFA 21-20.01/0002";
            p.CisloFA = "2850028913";
            p.Partner = "Chudý Patrik";
            p.Popis = "plošina";
            p.DatDodania = new DateTime(2020, 11, 18);
            p.DatVystavenia = new DateTime(2020, 12, 18);
            p.Suma = 157.20M;
            p.Zaloha = 50;
            p.Uhradene = 50;
            p.Neuhradene = 107.20M;
            p.DatPoslUhrady = new DateTime(2022, 10, 31);
            p.DokladUhrady = "PDK-V: 00-21.04/0004";
            h.Polozky.Add(p);

            p = new ZostavaKnihaFakturPol();
            p.DodOdb = h.DodOdb;
            p.P = true;
            p.Datum = new DateTime(2020, 1, 17);
            p.DatSplatnosti = new DateTime(2020, 10, 18);
            p.Doklad = "MDFA 21-20.01/0004";
            p.CisloFA = "132020";
            p.Partner = "IVTEKO";
            p.Popis = "kurz pre hasičov - obsluha píly";
            p.DatDodania = new DateTime(2020, 11, 18);
            p.DatVystavenia = new DateTime(2020, 12, 18);
            p.Suma = -450;
            p.Zaloha = 0;
            p.Uhradene = -450;
            p.Neuhradene = 0;
            p.DatPoslUhrady = new DateTime(2022, 10, 31);
            p.DokladUhrady = "PDK-V: 00-21.04/0004";
            h.Polozky.Add(p);

            p = new ZostavaKnihaFakturPol();
            p.DodOdb = h.DodOdb;
            p.X = true;
            p.Datum = new DateTime(2020, 1, 17);
            p.DatSplatnosti = new DateTime(2020, 10, 18);
            p.Doklad = "MDFA 21-20.01/0004";
            p.CisloFA = "132020";
            p.Partner = "VEPOS - SKALICA s.r.o.";
            p.Popis = "odvoz odpadu";
            p.DatDodania = new DateTime(2020, 11, 18);
            p.DatVystavenia = new DateTime(2020, 12, 18);
            p.Suma = 736.72M;
            p.Zaloha = 0;
            p.Uhradene = 736.72M;
            p.Neuhradene = 0;
            p.DatPoslUhrady = new DateTime(2022, 10, 31);
            p.DokladUhrady = "PDK-V: 00-21.04/0004";
            h.Polozky.Add(p);
        }

        public bool DodOdb { get; set; }  // aby sme na reporte vedeli robit designove zmeny (skryvania atd ...)
        public List<ZostavaKnihaFakturPol> Polozky { get; set; }

        [System.ComponentModel.DataObject]
        public class ZostavaKnihaFakturPol
        {
            public bool DodOdb { get; set; } // aby sme na reporte vedeli robit designove zmeny (skryvania atd ...)
            public DateTime? Datum { get; set; } // DFA: D. prijatia, VFA: D.vystavenia
            public DateTime? DatSplatnosti { get; set; }
            public string Doklad { get; set; }
            public string CisloFA { get; set; } // DFA: Číslo fakt.dodávateľ
            public string Partner { get; set; } // Dodávateľ / Odberateľ
            public string Popis { get; set; }
            public DateTime? DatDodania { get; set; }
            public DateTime? DatVystavenia { get; set; } // DFA - nepomylit s dat.vystavenia kt sa uklada do pola Datum (VFA)
            public decimal Suma { get; set; }
            public decimal? Zaloha { get; set; }
            public decimal Uhradene { get; set; }
            public decimal? Neuhradene { get; set; }
            public DateTime? DatPoslUhrady { get; set; }
            public string DokladUhrady { get; set; }
            public bool P { get; set; } // RZP predpisoch dokladu je aspoň jedna položka príjmová
            public bool X { get; set; } // Vrátený dodávateľovi
        }

    }
}
