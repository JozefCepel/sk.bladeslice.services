using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using static WebEas.Esam.Reports.Rzp.Types.ZostavaRzpDennik;
using static WebEas.Esam.Reports.Uct.Types.ZostavaUctDennik;

namespace WebEas.Esam.Reports.Crm.Types
{
    [System.ComponentModel.DataObject]
    public class ZostavaKryciList : RptHead
    {
        public ZostavaKryciList(bool testData = false)
        {
            if (testData) SetTestData(this);
        }

        // vola sa aj z Telerik designera (nefunguje z optional parametrom)
        public ZostavaKryciList()
        {
            if (System.Diagnostics.Process.GetCurrentProcess().ProcessName == "Telerik.ReportDesigner")
            {
                SetTestData(this);
                this.RptPath = ""; // Designer ich potrebuje mat vedla seba
            }
        }

        public void SetTestData(ZostavaKryciList rpt)
        {
            rpt.ICO = "12345678";
            rpt.IcDph= "SK7865430968";
            rpt.Dic = "7865430968";
            rpt.Nazov = "Mesto Gbely";
            rpt.Ulica = "Nám. Slobody 1261";
            rpt.PSC = "90845";
            rpt.Obec = "Gbely";
            rpt.Vytlacil = "Jozko Mrkvicka";
            rpt.StrediskoCaption = "Oddelenie";

            rpt.FP = false;
            rpt.BEPopis = "DFA001: 21-20.10/0027";
            rpt.CisloDkl = "HUHI778";
            rpt.Suma = 1840.20M;
            rpt.Kontakt = "Ing. Elena Držgrošová";

            rpt.DatPrijatia = new DateTime(2021, 12, 17);
            rpt.DatSplatnosti = new DateTime(2021, 12, 18);
            rpt.Objednavka = "08-20.09/0002 (03.10.2020)";
            rpt.Zmluva = "03-20.02/0002 (18.02.2020)";

            rpt.ObpNazovSidlo = "Agrofarma Skalka s.r.o (Zárpt.menice 1393/25, Gbely)";
            rpt.ObpIco = "09831654";
            rpt.ObpIcDph = "SK7865430968";
            rpt.ObpDic = "7865430968";

            rpt.PrijNazovSidlo = "Agrofarma Skalka s.r.o (Zárpt.menice 1393/25, Gbely)";
            rpt.PrijIban = "SK1711110000001555715004";
            rpt.PrijBic = "UNCRSKBX";
            rpt.PrijVS = "2020315";
            rpt.PrijKS = "0308";

            rpt.Ucel = "Platba za DaP: TKO vs 5566688800; DZN vs 1234567890; DZN vs 7890445678; faktúry: OFA vs 1234567890";
            rpt.Zauctoval = "Ing. Ivan Účtovník";
            rpt.DatDokladu = new DateTime(2021, 12, 17);

            rpt.UctPolozky = new List<ZostavaUctDennikPol>();
            rpt.RzpPolozky = new List<ZostavaRzpDennikPol>();
            rpt.TxtPolozky = new List<ZostavaTextaciaPol>();

            // -----
            var p = new ZostavaUctDennikPol();
            p.StrediskoCaption = rpt.StrediskoCaption;
            p.RozvrhUcet = "YYYYYY 11";
            p.SumaMD = 1000000M;
            p.SumaDal = 1000000M;
            rpt.UctPolozky.Add(p);

            p = new ZostavaUctDennikPol();
            p.StrediskoCaption = rpt.StrediskoCaption;
            p.SumaMD = 200M;
            rpt.UctPolozky.Add(p);

            p = new ZostavaUctDennikPol();
            p.StrediskoCaption = rpt.StrediskoCaption;
            p.RozvrhUcet = "AAAA 11";
            p.SumaMD = 300M;
            rpt.UctPolozky.Add(p);

            p = new ZostavaUctDennikPol();
            p.StrediskoCaption = rpt.StrediskoCaption;
            p.RozvrhUcet = "BBBB 11";
            p.SumaMD = 200M;
            rpt.UctPolozky.Add(p);

            p = new ZostavaUctDennikPol();
            p.StrediskoCaption = rpt.StrediskoCaption;
            p.Suv = true;
            p.VS = "Pr.: 1.2.3";
            p.RozvrhUcet = "suv suv";
            p.Popis = "123,00 Popis";
            rpt.UctPolozky.Add(p);

            p = new ZostavaUctDennikPol();
            p.StrediskoCaption = rpt.StrediskoCaption;
            p.RozvrhUcet = "Z21 11";
            p.SumaMD = 153.45M;
            p.SumaDal = 35.12M;
            p.Popis = "Služby spojená s nájmom";
            p.StrediskoNazov = "Banska Bystrica";
            p.ProjektNazov = "Project Green";
            rpt.UctPolozky.Add(p);

            // -----
            var r = new ZostavaRzpDennikPol();
            r.HlavickaPV = rpt.PV;
            r.PV = "P";
            r.ZD = "11GH";
            r.EK = "625001";
            r.FK = "1401";
            r.A1 = "123";
            r.A2 = "123";
            r.A3 = "123";
            r.NazovPolozky = "Na nemocenské poistenie";
            r.ProgramFull = "12.3.5 - Test prvok zo sumárnerpt. porpt.adu";
            r.Suma = 99999.99M;
            r.Popis = "rrr32222";
            r.StrediskoNazov = "crpt.ánené pracovisko";
            p.ProjektNazov = "Projekt 01";
            rpt.RzpPolozky.Add(r);

            r = new ZostavaRzpDennikPol();
            r.HlavickaPV = rpt.PV;
            r.ZD = "Z21 E1";
            r.Suma = 153.45M;
            r.Popis = "Služby spojená s nájmom";
            r.StrediskoNazov = "Banska Bystrica";
            r.ProjektNazov = "Project Green";
            rpt.RzpPolozky.Add(r);

            // -----
            var t = new ZostavaTextaciaPol();
            t.Text = "Finančnú operáciu, alebo jej časť  je - nie je možné vykonať";
            t.PismoTucne = true;
            rpt.TxtPolozky.Add(t);

            t = new ZostavaTextaciaPol();
            t.Text = "Finančnú operáciu vykonal: vedúci zamestnanec";
            t.Vykonal = "Kováčová Gabriela, Ing.";
            t.Datum = new DateTime(2020, 1, 17);
            rpt.TxtPolozky.Add(t);

            t = new ZostavaTextaciaPol();
            rpt.TxtPolozky.Add(t);

            t = new ZostavaTextaciaPol();
            t.Text = "Finančnú operáciu, alebo jej časť je -nie je možné vykonať";
            t.PismoTucne = true;
            rpt.TxtPolozky.Add(t);

            t = new ZostavaTextaciaPol();
            t.Text = "Finančnú operáciu vykonal:";
            t.Vykonal = "Kasanická Zuzana";
            rpt.TxtPolozky.Add(t);

            t = new ZostavaTextaciaPol();
            t.Text = "Finančnú operáciu vykonal:";
            t.Vykonal = "rpt.madej Martin";
            t.Datum = new DateTime(2020, 1, 17);
            rpt.TxtPolozky.Add(t);

            t = new ZostavaTextaciaPol();
            t.Text = "Finančnú operáciu vykonal:";
            t.Vykonal = "Čunderlík Marián";
            rpt.TxtPolozky.Add(t);

            t = new ZostavaTextaciaPol();
            rpt.TxtPolozky.Add(t);

            t = new ZostavaTextaciaPol();
            t.Text = "Vecnú správnosť overil:";
            rpt.TxtPolozky.Add(t);

            t = new ZostavaTextaciaPol();
            t.Text = "Formálnu správnosť overil:";
            rpt.TxtPolozky.Add(t);
        }

        // Systemove nastavenia
        public string NoDataMsgUct { get; set; } = "Účtovací predpis - nezaúčtovaný";  // nepouzite, nastavene na pevno v Designe (dynamicky sa nastavuje v Binding)
        public string NoDataMsgRzp { get; set; } = "Rozpočtový predpis - nezaúčtovaný"; // nepouzite, nastavene na pevno v Designe (dynamicky sa nastavuje v Binding)

        // My
        public string IcDicCaption
        {
            get { return !string.IsNullOrEmpty(IcDph) ? "IČ DPH: " + IcDph : "DIČ: " + Dic; }
        }
        // Dodavatel Odberatel
        public bool FP { get; set; } = false;    // typ osoby Fyz / Prav
        public string ObpNazovSidlo { get; set; } = ""; // (FormatMenoSort a AdresaTPSidlo)
        public string ObpNazovSidloCaption       // kvoli peknemu zarovnaniu a BOLD
        {
            get { return "<strong>" + ((TypBE == (short)TypDklEnum.DFA || TypBE == (short)TypDklEnum.DZF) ? "Dodávateľ:" : "Odberateľ:") + " </strong>" + ObpNazovSidlo; }
        }
        public string ObpIco { get; set; } = "";
        public string ObpDic { get; set; } = "";
        public string ObpIcDph { get; set; } = "";
        public string ObpIcDicCaption
        {
            get { return !string.IsNullOrEmpty(ObpIcDph) ? "IČ DPH: " + ObpIcDph : "DIČ: " + ObpDic; }
        }
        // Prijemca
        public string PrijNazovSidlo { get; set; } = ""; // (FormatMenoSort a AdresaTPSidlo)
        public string PrijNazovSidloCaption              // kvoli peknemu zarovnaniu a BOLD
        {
            get { return "<strong>Príjemca: </strong>" + PrijNazovSidlo; }
        }
        public string PrijIban { get; set; } = "";
        public string PrijIbanFmt
        {
            get { return Regex.Replace(PrijIban, ".{4}", "$0 ").Trim(); }
        }
        public string PrijBic { get; set; } = "";
        public string PrijVS { get; set; } = "";
        public string PrijVSCaption                 // kvoli BOLD
        {
            get { return "Variabilný symbol: <strong>" + PrijVS + "</strong>"; }
        }
        public string PrijKS { get; set; } = "";
        // Doklad
        public bool PV { get; set; } = false;    // aby sme na reporte vedeli robit designove zmeny (skryvanie FK a Program)
        public short TypBE { get; set; }
        public string BEPopis { get; set; }
        public string CisloDkl { get; set; }
        public decimal Suma { get; set; }
        public string SumaCaption                // kvoli peknemu zarovnaniu do prava sa to riesi tu
        {
            get { return "suma: <strong><span style=\"font - size: 14pt\">" + Suma.ToString("N2") + " € </span></strong>"; }
        }
        public string Kontakt { get; set; }
        public string KontaktCaption
        {
            get { return ((TypBE == (short)TypDklEnum.DFA || TypBE == (short)TypDklEnum.DZF) ? "kontakt: " : "vyhotovil: ") + Kontakt; }
        }
        public DateTime? DatPrijatia { get; set; }
        public DateTime? DatSplatnosti { get; set; }
        public string Objednavka { get; set; }
        public string Zmluva { get; set; }
        //public string DatumCisloCaption          // kvoli peknemu zarovnaniu do prava sa to riesi tu
        //{
        //    get { return "z " + Datum.ToString("dd.MM.yyyy") + " č. " + CisloDkl; }
        //}
        public string Ucel { get; set; }
        public string UcelCaption                // kvoli peknemu zarovnaniu a BOLD
        {
            get { return "<strong>Účel: </strong>" + Ucel; }
        }
        public string Zauctoval { get; set; }
        public DateTime DatDokladu { get; set; }
        // Data
        public List<ZostavaUctDennikPol> UctPolozky { get; set; }

        public List<ZostavaRzpDennikPol> RzpPolozky { get; set; }

        public List<ZostavaTextaciaPol> TxtPolozky { get; set; }

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
