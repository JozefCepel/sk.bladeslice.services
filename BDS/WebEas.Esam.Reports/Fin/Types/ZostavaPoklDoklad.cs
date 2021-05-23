using System;
using System.Collections.Generic;
using static WebEas.Esam.Reports.Rzp.Types.ZostavaRzpDennik;
using static WebEas.Esam.Reports.Uct.Types.ZostavaUctDennik;

namespace WebEas.Esam.Reports.Fin.Types
{
    [System.ComponentModel.DataObject]
    public class ZostavaPoklDoklad : RptHead
    {
        public ZostavaPoklDoklad(bool testData = false)
        {
            if (testData) SetTestData(this);
        }

        // vola sa aj z Telerik designera (nefunguje z optional parametrom)
        public ZostavaPoklDoklad()
        {
            if (System.Diagnostics.Process.GetCurrentProcess().ProcessName == "Telerik.ReportDesigner")
            {
                SetTestData(this);
                this.RptPath = ""; // Designer ich potrebuje mat vedla seba
            }
        }

        public void SetTestData(ZostavaPoklDoklad rpt)
        {
            rpt.ICO = "12345678";
            rpt.IcDph = "SK7865430968";
            rpt.Dic = "7865430968";
            rpt.Nazov = "Mesto Gbely";
            rpt.Ulica = "Nám. Slobody 1261";
            rpt.PSC = "90845";
            rpt.Obec = "Gbely";
            rpt.Vytlacil = "Jozko Mrkvicka";
            rpt.StrediskoCaption = "Oddelenie";
            rpt.ViacZaznamov = true;
            rpt.Hlavicky = new List<ZostavaPoklDokladHla>();

            var h = new ZostavaPoklDokladHla();
            h.Ico = rpt.ICO;
            h.IcDph = rpt.IcDph;
            h.Dic = rpt.Dic;
            h.NazovSidlo = rpt.NazovSidlo;
            h.KodPokl = "99";
            h.StrediskoCaption = rpt.StrediskoCaption;
            h.ViacZaznamov = rpt.ViacZaznamov;

            h.PV = true;
            h.ObpNazovSidlo = "Agrofarma Skalka s.r.o (Záhumenice 1393/25, Gbely)";
            h.ObpIco = "09831654";
            h.ObpIcDph = "SK7865430968";
            h.ObpDic = "7865430968";
            h.Datum = new DateTime(2020, 1, 17);
            h.CisloDkl = "21-20.10/0027";
            h.Ucel = "Platba za DaP: TKO vs 5566688800; DZN vs 1234567890; DZN vs 7890445678; faktúry: OFA vs 1234567890";
            h.Poznamka = "2019 PEN TKO Poplatok za komunálny odpad – obyvatelia Σ 38,20 € ( č. 9) ; 2020 DAN DZN Daň z nehnuteľností, psov, predajných automatov a nevýherných hracích prístrojov Σ 1 450,00 € (č. 155) ; 2020 DAN DZN Daň z nehnuteľností, psov, predajných automatov a nevýherných hracích prístrojov Σ 10,00 € (č. 173) ; 2020 Odberateľská faktúra: prenájom traktora s vlečkou Σ 350,00 € (č. DE-21.02/0002)";
            h.Suma = 1840.20M;
            h.Vyhotovil = "Ing. Elena Držgrošová";
            h.Schvalil = "Ing. Gabriela Kováčová";
            h.Zauctoval = "Ing. Ivan Účtovník";
            h.Prijemca = "Fero Berho";
            h.UctPolozky = new List<ZostavaUctDennikPol>();
            h.RzpPolozky = new List<ZostavaRzpDennikPol>();
            h.TxtPolozky = new List<ZostavaTextaciaPol>();

            // -----
            var p = new ZostavaUctDennikPol();
            p.StrediskoCaption = h.StrediskoCaption;
            p.RozvrhUcet = "YYYYYY 11";
            p.SumaMD = 1000000M;
            p.SumaDal = 1000000M;
            h.UctPolozky.Add(p);

            p = new ZostavaUctDennikPol();
            p.StrediskoCaption = h.StrediskoCaption;
            p.SumaMD = 200M;
            h.UctPolozky.Add(p);

            p = new ZostavaUctDennikPol();
            p.StrediskoCaption = h.StrediskoCaption;
            p.RozvrhUcet = "AAAA 11";
            p.SumaMD = 300M;
            h.UctPolozky.Add(p);

            p = new ZostavaUctDennikPol();
            p.StrediskoCaption = h.StrediskoCaption;
            p.RozvrhUcet = "BBBB 11";
            p.SumaMD = 200M;
            h.UctPolozky.Add(p);

            p = new ZostavaUctDennikPol();
            p.StrediskoCaption = h.StrediskoCaption;
            p.Suv = true;
            p.VS = "Pr.: 1.2.3";
            p.RozvrhUcet = "suv suv";
            p.Popis = "123,00 Popis";
            h.UctPolozky.Add(p);

            p = new ZostavaUctDennikPol();
            p.StrediskoCaption = h.StrediskoCaption;
            p.RozvrhUcet = "Z21 11";
            p.SumaMD = 153.45M;
            p.SumaDal = 35.12M;
            p.Popis = "Služby spojená s nájmom";
            p.StrediskoNazov = "Banska Bystrica";
            p.ProjektNazov = "Project Green";
            h.UctPolozky.Add(p);

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
            r.Suma = 99999.99M;
            r.Popis = "rrr32222";
            r.StrediskoNazov = "chránené pracovisko";
            p.ProjektNazov = "Projekt 01";
            h.RzpPolozky.Add(r);

            r = new ZostavaRzpDennikPol();
            r.HlavickaPV = h.PV;
            r.ZD = "Z21 E1";
            r.Suma = 153.45M;
            r.Popis = "Služby spojená s nájmom";
            r.StrediskoNazov = "Banska Bystrica";
            r.ProjektNazov = "Project Green";
            h.RzpPolozky.Add(r);

            // -----
            var t = new ZostavaTextaciaPol();
            t.Text = "Finančnú operáciu, alebo jej časť  je - nie je možné vykonať";
            t.PismoTucne = true;
            h.TxtPolozky.Add(t);

            t = new ZostavaTextaciaPol();
            t.Text = "Finančnú operáciu vykonal: vedúci zamestnanec";
            t.Vykonal = "Kováčová Gabriela, Ing.";
            t.Datum = new DateTime(2020, 1, 17);
            h.TxtPolozky.Add(t);

            t = new ZostavaTextaciaPol();
            h.TxtPolozky.Add(t);

            t = new ZostavaTextaciaPol();
            t.Text = "Finančnú operáciu, alebo jej časť je -nie je možné vykonať";
            t.PismoTucne = true;
            h.TxtPolozky.Add(t);

            t = new ZostavaTextaciaPol();
            t.Text = "Finančnú operáciu vykonal:";
            t.Vykonal = "Kasanická Zuzana";
            h.TxtPolozky.Add(t);

            t = new ZostavaTextaciaPol();
            t.Text = "Finančnú operáciu vykonal:";
            t.Vykonal = "Hamadej Martin";
            t.Datum = new DateTime(2020, 1, 17);
            h.TxtPolozky.Add(t);

            t = new ZostavaTextaciaPol();
            t.Text = "Finančnú operáciu vykonal:";
            t.Vykonal = "Čunderlík Marián";
            h.TxtPolozky.Add(t);

            rpt.Hlavicky.Add(h);

            // ---------------------------------------------------

            h = new ZostavaPoklDokladHla();
            h.FP = true;
            h.Ico = rpt.ICO;
            h.Dic = rpt.Dic;
            h.NazovSidlo = rpt.NazovSidlo;
            h.KodPokl = "99";
            h.StrediskoCaption = rpt.StrediskoCaption;
            h.ViacZaznamov = rpt.ViacZaznamov;

            h.ObpNazovSidlo = "Agrofarma Skalka s.r.o (Záhumenice 1393/25, Gbely)";
            h.ObpIco = "09831654";
            h.ObpDic = "7865430968";
            h.Datum = new DateTime(2020, 1, 17);
            h.CisloDkl = "123456789012345";
            h.Ucel = "Faktúra za dodávku... 12345345";
            h.Suma = 123.45M;
            h.Vyhotovil = "Elena Držgrošová";
            h.Schvalil = "Ing. Gabriela Kováčová";
            h.Zauctoval = "Ing. Ivan Účtovník";
            h.Prijemca = "Fero Berho";
            h.UctPolozky = new List<ZostavaUctDennikPol>();
            h.RzpPolozky = new List<ZostavaRzpDennikPol>();

            // -----
            p = new ZostavaUctDennikPol();
            p.StrediskoCaption = h.StrediskoCaption;
            p.RozvrhUcet = "AAAA 22";
            p.SumaMD = 300M;
            h.UctPolozky.Add(p);

            p = new ZostavaUctDennikPol();
            p.StrediskoCaption = h.StrediskoCaption;
            p.RozvrhUcet = "BBBB 22";
            p.SumaDal = 200M;
            h.UctPolozky.Add(p);

            p = new ZostavaUctDennikPol();
            p.StrediskoCaption = h.StrediskoCaption;
            p.Suv = true;
            p.VS = "Pr.: 1.2.3";
            p.RozvrhUcet = "suv suv";
            p.Popis = "123,00 Popis";
            h.UctPolozky.Add(p);

            p = new ZostavaUctDennikPol();
            p.StrediskoCaption = h.StrediskoCaption;
            p.RozvrhUcet = "Z21 22";
            p.SumaMD = 1000000.45M;
            p.SumaDal = 1000000.12M;
            p.Popis = "Služby spojená s nájmom";
            p.StrediskoNazov = "Banska Bystrica";
            p.ProjektNazov = "Project Green";
            h.UctPolozky.Add(p);

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
            r.Suma = 99999.99M;
            r.Popis = "rrr32222";
            r.StrediskoNazov = "chránené pracovisko";
            p.ProjektNazov = "Projekt 01";
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

            h = new ZostavaPoklDokladHla();
            h.Ico = rpt.ICO;
            h.IcDph = rpt.IcDph;
            h.NazovSidlo = rpt.NazovSidlo;
            h.KodPokl = "99";
            h.StrediskoCaption = rpt.StrediskoCaption;
            h.ViacZaznamov = rpt.ViacZaznamov;

            h.PV = true;
            h.ObpNazovSidlo = "Gastro s.r.o (Cervena 1393/25, Bystrica)";
            h.ObpIco = "09831654";
            h.ObpIcDph = "SK7865430968";
            h.Datum = new DateTime(2020, 1, 17);
            h.Suma = 1.5M;
            h.NoDataMsgUct = "Kde bolo tam bolo";
            h.NoDataMsgRzp = "Bola raz jedna chalupka";
            h.Vyhotovil = "Ing. Gabriela Kováčová";
            h.Schvalil = "Elena Držgrošová";
            h.Zauctoval = "Ing. Ivan Účtovník";

            rpt.Hlavicky.Add(h);

            // ---------------------------------------------------

            h = new ZostavaPoklDokladHla();
            h.Ico = rpt.ICO;
            h.Dic = rpt.Dic;
            h.NazovSidlo = rpt.NazovSidlo;
            h.KodPokl = "99";
            h.StrediskoCaption = rpt.StrediskoCaption;
            h.ViacZaznamov = rpt.ViacZaznamov;

            h.ObpNazovSidlo = "Najdlhsia firma na svete s.r.o (Dlha ulica 1393/255, Dlhe Mesto)";
            h.FP = true;
            h.Datum = new DateTime(2020, 1, 17);
            h.Suma = 1.5M;

            rpt.Hlavicky.Add(h);
        }

        public bool ViacZaznamov { get; set; }        // aby sme na reporte vedeli robit designove zmeny (mnozne cislo, skryvania atd ... nevieme to tam zratat)
        public List<ZostavaPoklDokladHla> Hlavicky { get; set; }

        [System.ComponentModel.DataObject]
        public class ZostavaPoklDokladHla
        {
            // Systemove nastavenia
            public string StrediskoCaption { get; set; } = ""; // kvoli subreportu musi byt
            public bool ViacZaznamov { get; set; } = false;    // aby sme na reporte vedeli robit designove zmeny (mnozne cislo, skryvania atd ... nevieme to tam zratat)
            public string NoDataMsgUct { get; set; } = "Účtovací predpis - nezaúčtovaný";  // nepouzite, nastavene na pevno v Designe (dynamicky sa nastavuje v Binding)
            public string NoDataMsgRzp { get; set; } = "Rozpočtový predpis - nezaúčtovaný"; // nepouzite, nastavene na pevno v Designe (dynamicky sa nastavuje v Binding)

            // My
            public string NazovSidlo { get; set; } = "";
            public string Ico { get; set; } = "";
            public string Dic { get; set; } = "";
            public string IcDph { get; set; } = "";
            public string IcDicCaption
            {
                get { return !string.IsNullOrEmpty(IcDph) ? "IČ DPH: " + IcDph : "DIČ: " + Dic; }
            }
            public string KodPokl { get; set; } = "";
            public string KodPoklCaption      // kvoli peknemu zarovnaniu do prava sa to riesi tu
            {
                get { return "pokladňa: " + KodPokl; }
            }
            // Zakaznik
            public string ObpNazovSidlo { get; set; } = ""; // (FormatMenoSort a AdresaTPSidlo)
            public string ObpNazovSidloCaption              // kvoli peknemu zarovnaniu a BOLD
            {
                get { return "<strong>" + ((PV) ? "Platiteľ:" : "Príjemca:") + " </strong>" + ObpNazovSidlo; }
            }
            public bool FP { get; set; } = false; // typ osoby Fyz / Prav
            public string ObpIco { get; set; } = "";
            public string ObpDic { get; set; } = "";
            public string ObpIcDph { get; set; } = "";
            public string ObpIcDicCaption
            {
                get { return !string.IsNullOrEmpty(ObpIcDph) ? "IČ DPH: " + ObpIcDph : "DIČ: " + ObpDic; }
            }
            // Doklad
            public bool PV { get; set; } = false;    // aby sme na reporte vedeli robit designove zmeny
            public DateTime Datum { get; set; }      // DatumDokladu
            public string CisloDkl { get; set; }     // CisloInterne
            public string DatumCisloCaption          // kvoli peknemu zarovnaniu do prava sa to riesi tu
            {
                get { return "z " + Datum.ToString("dd.MM.yyyy") + " č. " + CisloDkl; }
            }
            public decimal Suma { get; set; }
            public string SumaCaption                // kvoli peknemu zarovnaniu do prava sa to riesi tu
            {
                get { return "suma: <strong><span style=\"font - size: 14pt\">" + Suma.ToString("N2") + " € </span></strong>"; }
            }
            public string Ucel { get; set; }         // fin.V_DokladPDK.Popis, alebo Popis na BE      ???????????????
            public string UcelCaption                // kvoli peknemu zarovnaniu a BOLD
            {
                get { return "<strong>Účel: </strong>" + Ucel; }
            }
            public string Poznamka { get; set; }     // fin.V_DokladPDK.Poznamka
            public string PoznamkaCaption            // kvoli peknemu zarovnaniu a BOLD
            {
                get { return "<strong>Poznámka: </strong>" + Poznamka; }
            }
            public string Vyhotovil { get; set; }    // DokladVyhotovil
            public string Schvalil { get; set; }     // PodpisalMeno
            public string Zauctoval { get; set; }    // Zauctoval
            public string Prijemca { get; set; }     // zobrazuje sa len pre VYD: OsobaKontaktKomu inac FormatMenoSort

            public List<ZostavaUctDennikPol> UctPolozky { get; set; }

            public List<ZostavaRzpDennikPol> RzpPolozky { get; set; }

            public List<ZostavaTextaciaPol> TxtPolozky { get; set; }
        }

        [System.ComponentModel.DataObject]
        public class ZostavaTextaciaPol
        {
            public string Text { get; set; }
            public string Vykonal { get; set; }
            public DateTime? Datum { get; set; }
            public bool PismoTucne { get; set; }
        }
    }
}
