﻿using System;
using System.Collections.Generic;
using static WebEas.Esam.Reports.Rzp.Types.ZostavaRzpDennik;
using static WebEas.Esam.Reports.Uct.Types.ZostavaUctDennik;

namespace WebEas.Esam.Reports.Uct.Types
{
    [System.ComponentModel.DataObject]
    public class ZostavaUctDoklad : RptHead
    {
        public ZostavaUctDoklad(bool testData = false)
        {
            if (testData) SetTestData(this);
        }

        // vola sa aj z Telerik designera (nefunguje z optional parametrom)
        public ZostavaUctDoklad()
        {
            if (System.Diagnostics.Process.GetCurrentProcess().ProcessName == "Telerik.ReportDesigner")
            {
                SetTestData(this);
                this.RptPath = ""; // Designer ich potrebuje mat vedla seba
            }
        }

        public void SetTestData(ZostavaUctDoklad rpt)
        {
            rpt.ICO = "12345678";
            rpt.Nazov = "Mesto Gbely";
            rpt.Ulica = "Nám. Slobody 1261";
            rpt.PSC = "90845";
            rpt.Obec = "Gbely";
            rpt.Vytlacil = "Jozko Mrkvicka";
            rpt.StrediskoCaption = "Oddelenie";
            rpt.ViacZaznamov = true;
            // dennik.Filtre = new List<ZostavaFilter>();
            rpt.Hlavicky = new List<ZostavaUctDokladHla>();

            var h = new ZostavaUctDokladHla();
            h.Datum = new DateTime(2020, 1, 17);
            h.DokladCaption = "Dodávateľská faktúra";
            h.DodavatelCaption = "Dodávateľ";
            h.StrediskoCaption = rpt.StrediskoCaption;
            h.ViacZaznamov = rpt.ViacZaznamov;
            h.Doklad = "MDFA 0170031DFA";
            h.Dodavatel = "AXOL, s.r.o., Nová Baňa";
            h.Stredisko = "Stredisko 1";
            h.Projekt = "Projekt 1";
            h.Ucel = "Faktúra za dodávku...";
            h.Suma = 123.45M;
            h.UctPolozky = new List<ZostavaUctDennikPol>();
            h.RzpPolozky = new List<ZostavaRzpDennikPol>();

            // -----
            var u = new ZostavaUctDennikPol();
            u.StrediskoCaption = h.StrediskoCaption;
            u.RozvrhUcet = "YYYYYY 11";
            u.SumaMD = 1000000M;
            u.SumaDal = 1000000M;
            h.UctPolozky.Add(u);

            u = new ZostavaUctDennikPol();
            u.StrediskoCaption = h.StrediskoCaption;
            u.SumaMD = 200M;
            h.UctPolozky.Add(u);

            u = new ZostavaUctDennikPol();
            u.StrediskoCaption = h.StrediskoCaption;
            u.RozvrhUcet = "AAAA 11";
            u.SumaMD = 300M;
            h.UctPolozky.Add(u);

            u = new ZostavaUctDennikPol();
            u.StrediskoCaption = h.StrediskoCaption;
            u.RozvrhUcet = "BBBB 11";
            u.SumaMD = 200M;
            h.UctPolozky.Add(u);

            u = new ZostavaUctDennikPol();
            u.StrediskoCaption = h.StrediskoCaption;
            u.Suv = true;
            u.VS = "Pr.: 1.2.3";
            u.RozvrhUcet = "suv suv";
            u.Popis = "123,00 Popis";
            h.UctPolozky.Add(u);

            u = new ZostavaUctDennikPol();
            u.StrediskoCaption = h.StrediskoCaption;
            u.RozvrhUcet = "Z21 11";
            u.SumaMD = 153.45M;
            u.SumaDal = 35.12M;
            u.Popis = "Služby spojená s nájmom";
            u.StrediskoNazov = "Banska Bystrica";
            u.ProjektNazov = "Project Green";
            h.UctPolozky.Add(u);

            // -----
            var r = new ZostavaRzpDennikPol();
            r.HlavickaPV = h.PV;
            r.PV = "P";
            r.ZD = "11GH";
            r.EK = "625001";
            r.FK = "1401";
            r.A1 = "123";
            r.A2 = "123";
            r.A3 = "123";
            r.NazovPolozky = "Na nemocenské poistenie";
            r.ProgramFull = "12.3.5 - Test prvok zo sumárneho pohľadu";
            r.Suma = 1000000M;
            r.Popis = "rrr32222";
            r.StrediskoNazov = "chránené pracovisko";
            r.ProjektNazov = "Projekt 01";
            h.RzpPolozky.Add(r);

            r = new ZostavaRzpDennikPol();
            r.HlavickaPV = h.PV;
            r.ZD = "Z21 E1";
            r.Suma = 153.45M;
            r.Popis = "Služby spojená s nájmom";
            r.StrediskoNazov = "Banska Bystrica";
            r.ProjektNazov = "Project Green";
            h.RzpPolozky.Add(r);

            rpt.Hlavicky.Add(h);

            // ---------------------------------------------------

            h = new ZostavaUctDokladHla();
            h.Datum = new DateTime(2020, 1, 17);
            h.DokladCaption = "Interný doklad";
            h.DodavatelCaption = "Odberateľ";
            h.StrediskoCaption = rpt.StrediskoCaption;
            h.ViacZaznamov = rpt.ViacZaznamov;
            h.Doklad = "MDFA 22222222222222";
            h.Dodavatel = "KJHEWk lkwfj we le4rpr ";
            h.Stredisko = "Stredisko 2";
            h.Projekt = "Projekt 2";
            h.Ucel = "Faktúra za dodávku... 12345345";
            h.Suma = 123.45M;
            h.UctPolozky = new List<ZostavaUctDennikPol>();

            u = new ZostavaUctDennikPol();
            u.StrediskoCaption = h.StrediskoCaption;
            u.RozvrhUcet = "AAAA 22";
            u.SumaMD = 300M;
            h.UctPolozky.Add(u);

            u = new ZostavaUctDennikPol();
            u.StrediskoCaption = h.StrediskoCaption;
            u.RozvrhUcet = "BBBB 22";
            u.SumaDal = 200M;
            h.UctPolozky.Add(u);

            u = new ZostavaUctDennikPol();
            u.StrediskoCaption = h.StrediskoCaption;
            u.Suv = true;
            u.VS = "Pr.: 1.2.3";
            u.RozvrhUcet = "suv suv";
            u.Popis = "123,00 Popis";
            h.UctPolozky.Add(u);

            u = new ZostavaUctDennikPol();
            u.StrediskoCaption = h.StrediskoCaption;
            u.RozvrhUcet = "Z21 22";
            u.SumaMD = 1000000.45M;
            u.SumaDal = 1000000.12M;
            u.Popis = "Služby spojená s nájmom";
            u.StrediskoNazov = "Banska Bystrica";
            u.ProjektNazov = "Project Green";
            h.UctPolozky.Add(u);

            rpt.Hlavicky.Add(h);

            // ---------------------------------------------------

            h = new ZostavaUctDokladHla();
            h.PV = true;
            h.Datum = new DateTime(2020, 1, 17);
            h.DokladCaption = "Vystavena faktura 2";
            h.DodavatelCaption = "Odberateľ";
            h.StrediskoCaption = rpt.StrediskoCaption;
            h.ViacZaznamov = rpt.ViacZaznamov;
            h.Suma = 1.5M;
            h.RzpPolozky = new List<ZostavaRzpDennikPol>();

            // -----
            r = new ZostavaRzpDennikPol();
            r.HlavickaPV = h.PV;
            r.PV = "P";
            r.ZD = "11GH";
            r.EK = "625001";
            r.FK = "1401";
            r.A1 = "123";
            r.A2 = "123";
            r.A3 = "123";
            r.NazovPolozky = "Na nemocenské poistenie";
            r.ProgramFull = "12.3.5 - Test prvok zo sumárneho pohľadu";
            r.Suma = 1000000M;
            r.Popis = "rrr32222";
            r.StrediskoNazov = "chránené pracovisko";
            r.ProjektNazov = "Projekt 01";
            h.RzpPolozky.Add(r);

            r = new ZostavaRzpDennikPol();
            r.HlavickaPV = h.PV;
            r.ZD = "Z21 E1";
            r.Suma = 153.45M;
            r.Popis = "Služby spojená s nájmom";
            r.StrediskoNazov = "Banska Bystrica";
            r.ProjektNazov = "Project Green";
            h.RzpPolozky.Add(r);

            rpt.Hlavicky.Add(h);

            // ---------------------------------------------------

            h = new ZostavaUctDokladHla();
            h.Datum = new DateTime(2020, 1, 17);
            h.DokladCaption = "Interný doklad 3";
            h.DodavatelCaption = "Odberateľ";
            h.StrediskoCaption = rpt.StrediskoCaption;
            h.ViacZaznamov = rpt.ViacZaznamov;
            h.Suma = 1.5M;
            h.NoDataMsg = "  Rozpočtový predpis - vlastna hlaska";
            h.UctPolozky = new List<ZostavaUctDennikPol>();  // zobraz hlasku

            rpt.Hlavicky.Add(h);
        }

        public bool ViacZaznamov { get; set; }        // aby sme na reporte vedeli robit designove zmeny (mnozne cislo, skryvania atd ... nevieme to tam zratat)
        public List<ZostavaUctDokladHla> Hlavicky { get; set; }

        [System.ComponentModel.DataObject]
        public class ZostavaUctDokladHla
        {
            public string DokladCaption { get; set; }     // napr. Dodávateľská faktúra
            public string StrediskoCaption { get; set; }  // kvoli subreportu musi byt
            public bool PV { get; set; } = false;         // aby sme na reporte vedeli robit designove zmeny (skryvanie FK a Program)
            public bool ViacZaznamov { get; set; }        // aby sme na reporte vedeli robit designove zmeny (mnozne cislo, skryvania atd ... nevieme to tam zratat)
            public string DodavatelCaption { get; set; }  // Dodávateľ, Odberateľ, Meno/Názov
            public string NoDataMsg { get; set; } = "  Rozpočtový predpis - nepotvrdený rozpočet"; // moze byt aj "nezaúčtovaný"
            public DateTime Datum { get; set; }           // DatumDokladu
            public string Doklad { get; set; }            // Kód knihy + ‘ ’ + CisloInterne
            public int C_TypBiznisEntity_Id { get; set; }
            public string Dodavatel { get; set; }
            public string Stredisko { get; set; }
            public string Projekt { get; set; }
            public string Ucel { get; set; }           // Popis na BE
            public decimal Suma { get; set; }
            public List<ZostavaUctDennikPol> UctPolozky { get; set; }

            public List<ZostavaRzpDennikPol> RzpPolozky { get; set; }
        }
    }
}
