using System;
using System.Collections.Generic;

namespace WebEas.Esam.Reports.Rzp.Types
{
    [System.ComponentModel.DataObject]
    public class ZostavaVykazODaniExample : IReportData
    {
        public ZostavaVykazODaniExample()
        {
            if (System.Diagnostics.Process.GetCurrentProcess().ProcessName == "Telerik.ReportDesigner")
            {
                SetTestData(this);
            }
        }

        public void SetTestData(ZostavaVykazODaniExample vykaz)
        {
            vykaz.UradKodOkresu = 605;
            vykaz.UradICOObce = "00320102";
            vykaz.UradKodObce = 518611;
            vykaz.UradNazov = "Litava";
            vykaz.UradSidlo = "Testovacia ulica 987/65, 12345 Test obec.";
            vykaz.ParamDruh = "STA,BYT,POZ";
            vykaz.TextNadpis = "Výkaz o dani z nehnuteľnosti";
            vykaz.StavKDatumu = DateTime.Now;
            vykaz.ParamZdanovacieObdobie = 2017;
            vykaz.TextSumar = "Výkaz o dani z nehnuteľnosti";
            vykaz.ParamTypOsoby = "všetky osoby";
            vykaz.OdpocitaneOdpisy = "NIE";
            vykaz.DatumZostavy = DateTime.Now;
            vykaz.VybavujeMeno = "Dežko";
            vykaz.VybavujePriezvisko = "Mrkvička";
            vykaz.VybavujeTitulPred = "Phd.";
            vykaz.VybavujeTitulZa = "Bc.Mgr.Ing.Dipl.";
            vykaz.Pocet = new List<ZostavaVykazODaniExample.ZostavaVykazODaniPocet>
            {
                new ZostavaVykazODaniExample.ZostavaVykazODaniPocet
                {
                    ZdanovacieObdobie=2017,
                    DanovnikovSpolu = 5,
                    OslobodenychPOZ = 0,
                    OslobodenychSTA = 1,
                    OslobodenychBYT = 0,
                    PocetObyvatelov = 30
                }
            };
            vykaz.Polozky = new List<ZostavaVykazODaniExample.ZostavaVykazODaniPolozky>
            {
                new ZostavaVykazODaniExample.ZostavaVykazODaniPolozky
                {
                    Kod="POZ",
                    Odsek = "A",
                    Popis = "Orná pôda, vinice, chmeľnice, ovocné sady",
                    Typ="1",
                    CisloRiadku=1,
                    PocetDanovnikov= 1,
                    PocetVymNeoslob = 1,
                    PocetVymOslob = 1,
                    PocetVymZnizena = 0,
                    VymNeoslob = 0,
                    VymOslob = 1478.0M,
                    VymZnizena = 0.0M,
                    SumaZnizDane = 0.0M,
                    PredpisDan = 0.08M,
                    ZaplateneDan = 0.0M,
                    RozdielDan = 0.08M,
                    ZaplatenePrislusenstvo = 0.0M,
                    CelkovaSumaNedoplatku = 10.0M,
                    ZaplPenPokUro = 0.0M

                },
                new ZostavaVykazODaniExample.ZostavaVykazODaniPolozky
                {
                    Kod="POZ",
                    Odsek = "B",
                    Popis = "Trvalé trávne porasty",
                    Typ="1",
                    CisloRiadku=2,
                    PocetDanovnikov= 0,
                    PocetVymNeoslob = 0,
                    PocetVymOslob = 0,
                    PocetVymZnizena = 0,
                    VymNeoslob = 0,
                    VymOslob = 0.0M,
                    VymZnizena = 0.0M,
                    SumaZnizDane = 0.0M,
                    PredpisDan = 0.0M,
                    ZaplateneDan = 0.0M,
                    RozdielDan = 0.0M,
                    ZaplatenePrislusenstvo = 0.0M,
                    CelkovaSumaNedoplatku = 10.0M,
                    ZaplPenPokUro = 0.0M
                },
                new ZostavaVykazODaniExample.ZostavaVykazODaniPolozky
                {
                    Kod="POZ",
                    Odsek = "C",
                    Popis = "Záhrady",
                    Typ="1",
                    CisloRiadku=3,
                    PocetDanovnikov= 1,
                    PocetVymNeoslob = 1,
                    PocetVymOslob = 0,
                    PocetVymZnizena = 0,
                    VymNeoslob = 0,
                    VymOslob = 256.0M,
                    VymZnizena = 0.0M,
                    SumaZnizDane = 0.0M,
                    PredpisDan = 0.49M,
                    ZaplateneDan = 0.0M,
                    RozdielDan = 0.49M,
                    ZaplatenePrislusenstvo = 0.0M,
                    CelkovaSumaNedoplatku = 10.0M,
                    ZaplPenPokUro = 0.0M

                },
                new ZostavaVykazODaniExample.ZostavaVykazODaniPolozky
                {
                    Kod="POZ",
                    Odsek = "F",
                    Popis = "Zastavané plochy a nádvoria",
                    Typ="1",
                    CisloRiadku=4,
                    PocetDanovnikov= 0,
                    PocetVymNeoslob = 0,
                    PocetVymOslob = 0,
                    PocetVymZnizena = 0,
                    VymNeoslob = 0,
                    VymOslob = 0.0M,
                    VymZnizena = 0.0M,
                    SumaZnizDane = 0.0M,
                    PredpisDan = 0.0M,
                    ZaplateneDan = 0.0M,
                    RozdielDan = 0.0M,
                    ZaplatenePrislusenstvo = 0.0M,
                    CelkovaSumaNedoplatku = 10.0M,
                    ZaplPenPokUro = 0.0M

                },
                new ZostavaVykazODaniExample.ZostavaVykazODaniPolozky
                {
                    Kod="POZ",
                    Odsek = "H",
                    Popis = "Ostatné plochy okrem stavebných pozemkov",
                    Typ="1",
                    CisloRiadku=5,
                    PocetDanovnikov= 0,
                    PocetVymNeoslob = 0,
                    PocetVymOslob = 0,
                    PocetVymZnizena = 0,
                    VymNeoslob = 0,
                    VymOslob = 0.0M,
                    VymZnizena = 0.0M,
                    SumaZnizDane = 0.0M,
                    PredpisDan = 0.0M,
                    ZaplateneDan = 0.0M,
                    RozdielDan = 0.0M,
                    ZaplatenePrislusenstvo = 0.0M,
                    CelkovaSumaNedoplatku = 10.0M,
                    ZaplPenPokUro = 0.0M

                },
                new ZostavaVykazODaniExample.ZostavaVykazODaniPolozky
                {
                    Kod="POZ",
                    Odsek = "E",
                    Popis = "Rybníky a ostatné hospodársky využívané vodné plochy",
                    Typ="1",
                    CisloRiadku=6,
                    PocetDanovnikov= 0,
                    PocetVymNeoslob = 0,
                    PocetVymOslob = 0,
                    PocetVymZnizena = 0,
                    VymNeoslob = 0,
                    VymOslob = 0.0M,
                    VymZnizena = 0.0M,
                    SumaZnizDane = 0.0M,
                    PredpisDan = 0.0M,
                    ZaplateneDan = 0.0M,
                    RozdielDan = 0.0M,
                    ZaplatenePrislusenstvo = 0.0M,
                    CelkovaSumaNedoplatku = 10.0M,
                    ZaplPenPokUro = 0.0M

                },
                new ZostavaVykazODaniExample.ZostavaVykazODaniPolozky
                {
                    Kod="POZ",
                    Odsek = "D",
                    Popis = "Lesné pozemky",
                    Typ="1",
                    CisloRiadku=7,
                    PocetDanovnikov= 0,
                    PocetVymNeoslob = 0,
                    PocetVymOslob = 0,
                    PocetVymZnizena = 0,
                    VymNeoslob = 0,
                    VymOslob = 0.0M,
                    VymZnizena = 0.0M,
                    SumaZnizDane = 0.0M,
                    PredpisDan = 0.0M,
                    ZaplateneDan = 0.0M,
                    RozdielDan = 0.0M,
                    ZaplatenePrislusenstvo = 0.0M,
                    CelkovaSumaNedoplatku = 10.0M,
                    ZaplPenPokUro = 0.0M

                },
                new ZostavaVykazODaniExample.ZostavaVykazODaniPolozky
                {
                    Kod="POZ",
                    Odsek = "G",
                    Popis = "Stavebné pozemky",
                    Typ="1",
                    CisloRiadku=8,
                    PocetDanovnikov= 1,
                    PocetVymNeoslob = 1,
                    PocetVymOslob = 0,
                    PocetVymZnizena = 0,
                    VymNeoslob = 1500.0M,
                    VymOslob = 0.0M,
                    VymZnizena = 0.0M,
                    SumaZnizDane = 0.0M,
                    PredpisDan = 300.0M,
                    ZaplateneDan = 0.0M,
                    RozdielDan = 300.0M,
                    ZaplatenePrislusenstvo = 0.0M,
                    CelkovaSumaNedoplatku = 10.0M,
                    ZaplPenPokUro = 0.0M

                },
                new ZostavaVykazODaniExample.ZostavaVykazODaniPolozky
                {
                    Kod="STA",
                    Odsek = "A",
                    Popis = "Stavby na bývanie",
                    Typ="2",
                    CisloRiadku=1,
                    PocetDanovnikov= 2,
                    PocetVymNeoslob = 3,
                    PocetVymOslob = 0,
                    PocetVymZnizena = 0,
                    VymNeoslob = 700.0M,
                    VymOslob = 0.0M,
                    VymZnizena = 0.0M,
                    SumaZnizDane = 0.0M,
                    PredpisDan = 94.166M,
                    ZaplateneDan = 0.0M,
                    RozdielDan = 94.166M,
                    ZaplatenePrislusenstvo = 0.0M,
                    CelkovaSumaNedoplatku = 10.0M,
                    ZaplPenPokUro = 0.0M

                },
                new ZostavaVykazODaniExample.ZostavaVykazODaniPolozky
                {
                    Kod="STA",
                    Odsek = "B",
                    Popis = "Poľnohospodárske stavby",
                    Typ="2",
                    CisloRiadku=2,
                    PocetDanovnikov= 0,
                    PocetVymNeoslob = 0,
                    PocetVymOslob = 0,
                    PocetVymZnizena = 0,
                    VymNeoslob = 0.0M,
                    VymOslob = 0.0M,
                    VymZnizena = 0.0M,
                    SumaZnizDane = 0.0M,
                    PredpisDan = 0.0M,
                    ZaplateneDan = 0.0M,
                    RozdielDan = 0.0M,
                    ZaplatenePrislusenstvo = 0.0M,
                    CelkovaSumaNedoplatku = 10.0M,
                    ZaplPenPokUro = 0.0M

                },
                new ZostavaVykazODaniExample.ZostavaVykazODaniPolozky
                {
                    Kod="STA",
                    Odsek = "C",
                    Popis = "Chaty",
                    Typ="2",
                    CisloRiadku=3,
                    PocetDanovnikov= 0,
                    PocetVymNeoslob = 0,
                    PocetVymOslob = 0,
                    PocetVymZnizena = 0,
                    VymNeoslob = 0.0M,
                    VymOslob = 0.0M,
                    VymZnizena = 0.0M,
                    SumaZnizDane = 0.0M,
                    PredpisDan = 0.0M,
                    ZaplateneDan = 0.0M,
                    RozdielDan = 0.0M,
                    ZaplatenePrislusenstvo = 0.0M,
                    CelkovaSumaNedoplatku = 10.0M,
                    ZaplPenPokUro = 0.0M

                },
                new ZostavaVykazODaniExample.ZostavaVykazODaniPolozky
                {
                    Kod="STA",
                    Odsek = "D",
                    Popis = "Garáže",
                    Typ="2",
                    CisloRiadku=4,
                    PocetDanovnikov= 0,
                    PocetVymNeoslob = 0,
                    PocetVymOslob = 0,
                    PocetVymZnizena = 0,
                    VymNeoslob = 0.0M,
                    VymOslob = 0.0M,
                    VymZnizena = 0.0M,
                    SumaZnizDane = 0.0M,
                    PredpisDan = 0.0M,
                    ZaplateneDan = 0.0M,
                    RozdielDan = 0.0M,
                    ZaplatenePrislusenstvo = 0.0M,
                    CelkovaSumaNedoplatku = 10.0M,
                    ZaplPenPokUro = 0.0M

                },
                new ZostavaVykazODaniExample.ZostavaVykazODaniPolozky
                {
                    Kod="STA",
                    Odsek = "E",
                    Popis = "Priemyselné stavby",
                    Typ="2",
                    CisloRiadku=5,
                    PocetDanovnikov= 0,
                    PocetVymNeoslob = 0,
                    PocetVymOslob = 0,
                    PocetVymZnizena = 0,
                    VymNeoslob = 0.0M,
                    VymOslob = 0.0M,
                    VymZnizena = 0.0M,
                    SumaZnizDane = 0.0M,
                    PredpisDan = 0.0M,
                    ZaplateneDan = 0.0M,
                    RozdielDan = 0.0M,
                    ZaplatenePrislusenstvo = 0.0M,
                    CelkovaSumaNedoplatku = 10.0M,
                    ZaplPenPokUro = 0.0M

                },
                new ZostavaVykazODaniExample.ZostavaVykazODaniPolozky
                {
                    Kod="STA",
                    Odsek = "F",
                    Popis = "Stavby na podnikanie",
                    Typ="2",
                    CisloRiadku=6,
                    PocetDanovnikov= 0,
                    PocetVymNeoslob = 0,
                    PocetVymOslob = 0,
                    PocetVymZnizena = 0,
                    VymNeoslob = 0.0M,
                    VymOslob = 0.0M,
                    VymZnizena = 0.0M,
                    SumaZnizDane = 0.0M,
                    PredpisDan = 0.0M,
                    ZaplateneDan = 0.0M,
                    RozdielDan = 0.0M,
                    ZaplatenePrislusenstvo = 0.0M,
                    CelkovaSumaNedoplatku = 10.0M,
                    ZaplPenPokUro = 0.0M

                },
                new ZostavaVykazODaniExample.ZostavaVykazODaniPolozky
                {
                    Kod="STA",
                    Odsek = "G",
                    Popis = "Ostatné stavby",
                    Typ="2",
                    CisloRiadku=7,
                    PocetDanovnikov= 0,
                    PocetVymNeoslob = 0,
                    PocetVymOslob = 0,
                    PocetVymZnizena = 0,
                    VymNeoslob = 0.0M,
                    VymOslob = 0.0M,
                    VymZnizena = 0.0M,
                    SumaZnizDane = 0.0M,
                    PredpisDan = 0.0M,
                    ZaplateneDan = 0.0M,
                    RozdielDan = 0.0M,
                    ZaplatenePrislusenstvo = 0.0M,
                    CelkovaSumaNedoplatku = 10.0M,
                    ZaplPenPokUro = 0.0M

                },
                new ZostavaVykazODaniExample.ZostavaVykazODaniPolozky
                {
                    Kod="STA",
                    Odsek = "H",
                    Popis = "Viacúčelové stavby",
                    Typ="2",
                    CisloRiadku=8,
                    PocetDanovnikov= 3,
                    PocetVymNeoslob = 3,
                    PocetVymOslob = 1,
                    PocetVymZnizena = 0,
                    VymNeoslob = 1827.0M,
                    VymOslob = 10.0M,
                    VymZnizena = 0.0M,
                    SumaZnizDane = 1201.656M,
                    PredpisDan = 0.0M,
                    ZaplateneDan = 0.0M,
                    RozdielDan = 1201.656M,
                    ZaplatenePrislusenstvo = 0.0M,
                    CelkovaSumaNedoplatku = 10.0M,
                    ZaplPenPokUro = 0.0M

                },
                new ZostavaVykazODaniExample.ZostavaVykazODaniPolozky
                {
                    Kod="STA",
                    Odsek = "I",
                    Popis = "Ostatné stavby",
                    Typ="2",
                    CisloRiadku=9,
                    PocetDanovnikov= 0,
                    PocetVymNeoslob = 0,
                    PocetVymOslob = 0,
                    PocetVymZnizena = 0,
                    VymNeoslob = 0.0M,
                    VymOslob = 0.0M,
                    VymZnizena = 0.0M,
                    SumaZnizDane = 0.0M,
                    PredpisDan = 0.0M,
                    ZaplateneDan = 0.0M,
                    RozdielDan = 0.0M,
                    ZaplatenePrislusenstvo = 0.0M,
                    CelkovaSumaNedoplatku = 10.0M,
                    ZaplPenPokUro = 0.0M

                },
                new ZostavaVykazODaniExample.ZostavaVykazODaniPolozky
                {
                    Kod="STA",
                    Odsek = "J",
                    Popis = "Viacúčelové stavby",
                    Typ="2",
                    CisloRiadku=10,
                    PocetDanovnikov= 0,
                    PocetVymNeoslob = 0,
                    PocetVymOslob = 0,
                    PocetVymZnizena = 0,
                    VymNeoslob = 0.0M,
                    VymOslob = 0.0M,
                    VymZnizena = 0.0M,
                    SumaZnizDane = 0.0M,
                    PredpisDan = 0.0M,
                    ZaplateneDan = 0.0M,
                    RozdielDan = 0.0M,
                    ZaplatenePrislusenstvo = 0.0M,
                    CelkovaSumaNedoplatku = 10.0M,
                    ZaplPenPokUro = 0.0M

                },
                new ZostavaVykazODaniExample.ZostavaVykazODaniPolozky
                {
                    Kod="BYT",
                    Odsek = "A",
                    Popis = "Byty",
                    Typ="3",
                    CisloRiadku=1,
                    PocetDanovnikov= 1,
                    PocetVymNeoslob = 2,
                    PocetVymOslob = 0,
                    PocetVymZnizena = 0,
                    VymNeoslob = 580.0M,
                    VymOslob = 0.0M,
                    VymZnizena = 0.0M,
                    SumaZnizDane = 0.0M,
                    PredpisDan = 181.2M,
                    ZaplateneDan = 0.0M,
                    RozdielDan = 181.200M,
                    ZaplatenePrislusenstvo = 0.0M,
                    CelkovaSumaNedoplatku = 10.0M,
                    ZaplPenPokUro = 0.0M

                },
                new ZostavaVykazODaniExample.ZostavaVykazODaniPolozky
                {
                    Kod="BYT",
                    Odsek = "B",
                    Popis = "Nebytové priestory",
                    Typ="3",
                    CisloRiadku=2,
                    PocetDanovnikov= 1,
                    PocetVymNeoslob = 0,
                    PocetVymOslob = 0,
                    PocetVymZnizena = 0,
                    VymNeoslob = 0.0M,
                    VymOslob = 0.0M,
                    VymZnizena = 0.0M,
                    SumaZnizDane = 0.0M,
                    PredpisDan = 0.0M,
                    ZaplateneDan = 0.0M,
                    RozdielDan = 0.0M,
                    ZaplatenePrislusenstvo = 0.0M,
                    CelkovaSumaNedoplatku = 10.0M,
                    ZaplPenPokUro = 0.0M

                }

            };
            vykaz.SadzbyPOZ = new List<ZostavaVykazODaniExample.ZostavaVykazODaniSadzbyPOZ>
            {
                new ZostavaVykazODaniExample.ZostavaVykazODaniSadzbyPOZ
                {
                    CastObceKU = "Litava",
                    SadzbaA1 = 10.567M,
                    HodnotaA2 = 10.567M,
                    HodnotaA3 = 10.567M,
                    SadzbaA4 = 10.567M,
                    HodnotaA5 = 10.567M,
                    SadzbaA6 = 10.567M,
                    HodnotaA7 = 10.567M,
                    SadzbaA8 = 10.567M,
                    HodnotaA9 = 10.567M,
                    SadzbaA10 = 10.567M,
                    HodnotaA11 = 10.567M

                },
                new ZostavaVykazODaniExample.ZostavaVykazODaniSadzbyPOZ
                {
                    CastObceKU = "Litava1",
                    SadzbaA1 = 10.567M,
                    HodnotaA2 = 10.567M,
                    HodnotaA3 = 10.567M,
                    SadzbaA4 = 10.567M,
                    HodnotaA5 = 10.567M,
                    SadzbaA6 = 10.567M,
                    HodnotaA7 = 10.567M,
                    SadzbaA8 = 10.567M,
                    HodnotaA9 = 10.567M,
                    SadzbaA10 = 10.567M,
                    HodnotaA11 = 10.567M

                },
                new ZostavaVykazODaniExample.ZostavaVykazODaniSadzbyPOZ
                {
                    CastObceKU = "Litava3",
                    SadzbaA1 = 10.567M,
                    HodnotaA2 = 10.567M,
                    HodnotaA3 = 10.567M,
                    SadzbaA4 = 10.567M,
                    HodnotaA5 = 10.567M,
                    SadzbaA6 = 10.567M,
                    HodnotaA7 = 10.567M,
                    SadzbaA8 = 10.567M,
                    HodnotaA9 = 10.567M,
                    SadzbaA10 = 10.567M,
                    HodnotaA11 = 10.567M

                }

            };
            vykaz.SadzbySTA = new List<ZostavaVykazODaniExample.ZostavaVykazODaniSadzbySTA>
            {
                new ZostavaVykazODaniExample.ZostavaVykazODaniSadzbySTA
                {
                    CastObceKU = "Litava",
                    A1 = 10.567M,
                    A2 = 10.567M,
                    A3 = 10.567M,
                    A4 = 10.567M,
                    A5 = 10.567M,
                    A6 = 10.567M,
                    A7 = 10.567M,
                    A8 = 10.567M,
                    A9 = 10.567M

                },
                new ZostavaVykazODaniExample.ZostavaVykazODaniSadzbySTA
                {
                    CastObceKU = "Litava1",
                    A1 = 10.567M,
                    A2 = 10.567M,
                    A3 = 10.567M,
                    A4 = 10.567M,
                    A5 = 10.567M,
                    A6 = 10.567M,
                    A7 = 10.567M,
                    A8 = 10.567M,
                    A9 = 10.567M

                },
                new ZostavaVykazODaniExample.ZostavaVykazODaniSadzbySTA
                {
                    CastObceKU = "Litava2",
                    A1 = 10.567M,
                    A2 = 10.567M,
                    A3 = 10.567M,
                    A4 = 10.567M,
                    A5 = 10.567M,
                    A6 = 10.567M,
                    A7 = 10.567M,
                    A8 = 10.567M,
                    A9 = 10.567M

                }

            };
            vykaz.SadzbyBYT = new List<ZostavaVykazODaniExample.ZostavaVykazODaniSadzbyBYT>
            {
                new ZostavaVykazODaniExample.ZostavaVykazODaniSadzbyBYT
                {
                    CastObceKU = "Litava",
                    A1 = 0.3600M,
                    A2 = 1.6600M

                },
                new ZostavaVykazODaniExample.ZostavaVykazODaniSadzbyBYT
                {
                    CastObceKU = "Malá Hradná",
                    A1 = 0.3600M,
                    A2 = 1.6600M

                },
                new ZostavaVykazODaniExample.ZostavaVykazODaniSadzbyBYT
                {
                    CastObceKU = "ul Májová",
                    A1 = 0.3600M,
                    A2 = 1.6600M

                }
            };
        }

        public long Id { get; set; }

        public string VybavujeTitulPred { get; set; }

        public string VybavujeTitulZa { get; set; }

        public string VybavujeMeno { get; set; }

        public string VybavujePriezvisko { get; set; }

        public string VybavujeTelefon { get; set; }

        public string VybavujeEmail { get; set; }

        public string ZostavaCislo { get; set; }

        public short? UradKodOkresu { get; set; }

        public string UradICOObce { get; set; }

        public int? UradKodObce { get; set; }

        public string UradNazov { get; set; }

        public string UradSidlo { get; set; }

        public string TextNadpis { get; set; }

        public DateTime? StavKDatumu { get; set; }

        public string TextSumar { get; set; }

        public string ParamDruh { get; set; }

        public short? ParamZdanovacieObdobie { get; set; }

        public string ParamNazovOsobyOd { get; set; }

        public string ParamNazovOsobyDo { get; set; }

        public string ParamTypOsoby { get; set; }

        public string OdpocitaneOdpisy { get; set; }

        public string PomerneKPolozkam { get; set; }

        public DateTime? DatumZostavy { get; set; }

        public List<ZostavaVykazODaniPocet> Pocet { get; set; }
        [System.ComponentModel.DataObject]
        public class ZostavaVykazODaniPocet
        {
            public short? ZdanovacieObdobie { get; set; }

            public int? DanovnikovSpolu { get; set; }

            public int? OslobodenychPOZ { get; set; }

            public int? OslobodenychSTA { get; set; }

            public int? OslobodenychBYT { get; set; }

            public int? PocetObyvatelov { get; set; }
        }
        public List<ZostavaVykazODaniPolozky> Polozky { get; set; }

        [System.ComponentModel.DataObject]
        public class ZostavaVykazODaniPolozky
        {
            public string Kod { get; set; }

            public string Odsek { get; set; }

            public string Popis { get; set; }

            public string Typ { get; set; }

            public int? CisloRiadku { get; set; }

            public int? PocetDanovnikov { get; set; }

            public int? PocetVymNeoslob { get; set; }

            public int? PocetVymOslob { get; set; }

            public int? PocetVymZnizena { get; set; }

            public decimal? VymNeoslob { get; set; }

            public decimal? VymOslob { get; set; }

            public decimal? VymZnizena { get; set; }

            public decimal? SumaZnizDane { get; set; }

            public decimal? PredpisDan { get; set; }

            public decimal? ZaplateneDan { get; set; }

            public decimal? RozdielDan { get; set; }

            public decimal? ZaplatenePrislusenstvo { get; set; }

            public decimal? CelkovaSumaNedoplatku { get; set; }

            public decimal? ZaplPenPokUro { get; set; }

        }

        public List<ZostavaVykazODaniSadzbyPOZ> SadzbyPOZ { get; set; }

        [System.ComponentModel.DataObject]
        public class ZostavaVykazODaniSadzbyPOZ
        {
            public string CastObceKU { get; set; }

            public decimal? SadzbaA1 { get; set; }

            public decimal? HodnotaA2 { get; set; }

            public decimal? HodnotaA3 { get; set; }

            public decimal? SadzbaA4 { get; set; }

            public decimal? HodnotaA5 { get; set; }

            public decimal? SadzbaA6 { get; set; }

            public decimal? HodnotaA7 { get; set; }

            public decimal? SadzbaA8 { get; set; }

            public decimal? HodnotaA9 { get; set; }

            public decimal? SadzbaA10 { get; set; }

            public decimal? HodnotaA11 { get; set; }

        }

        public List<ZostavaVykazODaniSadzbySTA> SadzbySTA { get; set; }

        [System.ComponentModel.DataObject]
        public class ZostavaVykazODaniSadzbySTA
        {
            public string CastObceKU { get; set; }

            public decimal? A1 { get; set; }

            public decimal? A2 { get; set; }

            public decimal? A3 { get; set; }

            public decimal? A4 { get; set; }

            public decimal? A5 { get; set; }

            public decimal? A6 { get; set; }

            public decimal? A7 { get; set; }

            public decimal? A8 { get; set; }

            public decimal? A9 { get; set; }


        }

        public List<ZostavaVykazODaniSadzbyBYT> SadzbyBYT { get; set; }

        [System.ComponentModel.DataObject]
        public class ZostavaVykazODaniSadzbyBYT
        {
            public string CastObceKU { get; set; }

            public decimal? A1 { get; set; }

            public decimal? A2 { get; set; }

        }

    }
}
