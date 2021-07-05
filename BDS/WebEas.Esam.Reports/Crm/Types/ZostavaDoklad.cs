using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace WebEas.Esam.Reports.Crm.Types
{
    [System.ComponentModel.DataObject]
    public class ZostavaDoklad : RptHead
    {
        public ZostavaDoklad(bool testData = false)
        {
            if (testData) SetTestData(this);
        }

        // vola sa aj z Telerik designera (nefunguje z optional parametrom)
        public ZostavaDoklad()
        {
            if (System.Diagnostics.Process.GetCurrentProcess().ProcessName == "Telerik.ReportDesigner")
            {
                SetTestData(this);
                this.RptPath = ""; // Designer ich potrebuje mat vedla seba
            }
        }

        public void SetTestData(ZostavaDoklad rpt)
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
            rpt.RezimDph = 2;

            rpt.TypBE = (short)TypDklEnum.OFA;
            rpt.CisloDkl = "JP-21.05/1234";
            rpt.Suma = 280;
            rpt.Zak0 = 100;
            rpt.Zak1 = 100;
            rpt.Zak2 = 100;
            rpt.SumDph1 = 10;
            rpt.SumDph2 = 20;
            rpt.Dph1 = 10;
            rpt.Dph2 = 20;
            rpt.UhradZaloha = 50;
            rpt.DatVystavenia = new DateTime(2021, 12, 16);
            rpt.DatDodania = new DateTime(2021, 12, 17);
            rpt.DatSplatnosti = new DateTime(2021, 12, 18);
            rpt.FormaUhrady = "bankový prevod";
            rpt.Iban = "SK1711110000001555715004";
            rpt.Bic = "UNCRSKBX";
            rpt.VS = "2020315";
            rpt.KS = "0308";
            rpt.SS = "0123456789";

            rpt.FP = false;
            rpt.CisloDklPovodne = "AA-21.05/0007";
            rpt.ObpNazov = "Agrofarma Skalka s.r.o";
            rpt.ObpUlica = "Záhumenice  1393/25";
            rpt.ObpPSC = "908 45 Praha";
            rpt.ObpStat = "Česká republika";
            rpt.ObpIco = "09831654";
            rpt.ObpDic = "7865430968";
            rpt.ObpIcDph = "SK7865430968";
            rpt.Objednavka = "08-20.09/0002 (03.10.2020)";
            rpt.Zmluva = "03-20.02/0002 (18.02.2020)";

            rpt.UT = "Lorem  ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor";
            rpt.ZT = "Platba za DaP: TKO vs 5566688800; DZN vs 1234567890; DZN vs 7890445678; faktúry: OFA vs 1234567890";
            rpt.Vystavil = "Ing. Janka Esamová";
            rpt.VystavilKontakt = "0948 202 324   janka_esamova@gbely.sk";

            rpt.Polozky = new List<ZostavaDokladPol>();

            var p = new ZostavaDokladPol();
            p.PC = 1;
            p.Kod = "909";
            p.Polozka = "voda";
            p.Mnozstvo = -1148.1234M;
            p.MJ = "m3";
            p.Cena = 1.1765M;
            p.DPH = 0;
            p.Suma = 1343.16M;
            rpt.Polozky.Add(p);

            p = new ZostavaDokladPol();
            p.PC = 2;
            p.Kod = "5889DJH";
            p.Polozka = "plyn";
            p.Mnozstvo = 1408;
            p.MJ = "m3";
            p.Cena = 0.25M;
            p.DPH = 10;
            p.Suma = 387.20M;
            rpt.Polozky.Add(p);

            p = new ZostavaDokladPol();
            p.PC = 3;
            p.Kod = "87HU";
            p.Polozka = "elektrická energia";
            p.Mnozstvo = 30.879M;
            p.MJ = "kWh";
            p.Cena = 3.18M;
            p.DPH = 20;
            p.Suma = 117.843M;
            rpt.Polozky.Add(p);
        }

        // ********************************************************************
        public short TypBE { get; set; }

        public bool ShowDphSum
        {
            get { return (TypBE == (short)TypDklEnum.OFA) && PlatcaDph; }
        }
        public bool ShowUhrady
        {
            get { return (TypBE == (short)TypDklEnum.OFA); }
        }

        public bool DodOdb
        {
            get { return (TypBE == (short)TypDklEnum.DOB || TypBE == (short)TypDklEnum.DDP || TypBE == (short)TypDklEnum.DOL); }
        }

        public string CisloDklPovodne { get; set; } = ""; // OFA, OZF: D_BiznisEntita_Id_Povodne
        public string CisloDklPovodneText
        {
            get { return CisloDklPovodne != "" ? "k faktúre: " + Formatuj(CisloDklPovodne, true, 12) : ""; }
        }
        public string CisloDkl { get; set; }
        public decimal Suma { get; set; }
        public string DklCaption
        {
            get
            {
                string s;
                switch (TypBE)
                {
                    case (short)TypDklEnum.OFA:
                        if (CisloDklPovodne != "")
                        {
                            if (Suma >= 0)
                                s = "Ťarchopis";
                            else
                                s = "Dobropis";
                        }
                        else
                            s = "Faktúra";
                        s += " – daňový doklad";
                        break;
                    case (short)TypDklEnum.OZF:
                        s = "Zálohová faktúra";
                        break;
                    case (short)TypDklEnum.DOB:
                        s = "Objednávka";
                        break;
                    case (short)TypDklEnum.DDP:
                        s = "Dopyt";
                        break;
                    case (short)TypDklEnum.OZM:
                        s = "Zmluva";
                        break;
                    case (short)TypDklEnum.OCP:
                        s = "Cenová ponuka";
                        break;
                    default: // DOL
                        s = "Dodací list";
                        break;
                }
                return s + " č. " + CisloDkl;
            }
        }
        public string NazovSidloBold
        {
            get { return Formatuj(Nazov, true, 12) + " (" + Sidlo + ")"; }
        }
        public string IcDphText
        {
            get { return (RezimDph == 0 || RezimDph == 1) ? "Nie sme platiteľmi DPH" : IcDph; }
        }
        public DateTime? DatVystavenia { get; set; }
        public DateTime? DatDodania { get; set; }
        public DateTime? DatSplatnosti { get; set; }
        public string FormaUhrady { get; set; } = "";
        public string Iban { get; set; } = "";
        public string IbanFmt
        {
            get { return string.IsNullOrEmpty(Iban) ? null : Regex.Replace(Iban, ".{4}", "$0 ").Trim(); }
        }
        public string Bic { get; set; } = "";
        public string VS { get; set; }
        public string KS { get; set; }
        public string SS { get; set; }

        // Odberatel
        public bool FP { get; set; } = false;    // typ osoby Fyz / Prav
        public string ObpNazov { get; set; } = "";
        public string ObpAdresa { get; set; } // CRLF
        public string ObpUlica { get; set; }  // nepouzite, cez CRLF verziu
        public string ObpPSC { get; set; }    // nepouzite, cez CRLF verziu
        public string ObpStat { get; set; }   // nepouzite, cez CRLF verziu
        public string ObpIco { get; set; } = "";
        public string ObpDic { get; set; } = "";
        public string ObpIcDph { get; set; } = "";
        public string Objednavka { get; set; }
        public string Zmluva { get; set; }

        public string UT { get; set; } = "";
        public string ZT { get; set; } = "";
        public string Vystavil { get; set; }  // D_User_Id_Vyhotovil
        public string VystavilKontakt { get; set; }

        // DPH Tabulka
        public int Dph2 { get; set; }
        public int Dph1 { get; set; }
        public decimal Zak2 { get; set; }
        public decimal Zak1 { get; set; }
        public decimal Zak0 { get; set; }
        public decimal SumDph2 { get; set; }
        public decimal SumDph1 { get; set; }
        public decimal? UhradZaloha { get; set; } // len OFA

        // Data
        public List<ZostavaDokladPol> Polozky { get; set; }

        [System.ComponentModel.DataObject]
        public class ZostavaDokladPol
        {
            public int PC { get; set; }
            public string Kod { get; set; }
            public string Polozka { get; set; }
            public decimal Mnozstvo { get; set; }
            public string MJ { get; set; }
            public decimal Cena { get; set; }  // Jedn. cena
            public byte DPH { get; set; }
            public decimal? Suma { get; set; }  // Suma s DPH / Suma
        }
    }
}
