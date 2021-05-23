using System;
using System.Collections.Generic;

namespace WebEas.Esam.Reports.Rzp.Types
{
    [System.ComponentModel.DataObject]
    public class ZostavaFinHead : RptHead
    {
        public ZostavaFinHead()
        {
            if (System.Diagnostics.Process.GetCurrentProcess().ProcessName == "Telerik.ReportDesigner")
            {
                SetTestData(this);
            }
        }

        public void SetTestData(ZostavaFinHead head)
        {
            head.ICO = "12345678";
            head.Nazov = "Mesto Gbely";
            head.Ulica = "Nám. Slobody 1261";
            head.PSC = "90845";
            head.Obec = "Gbely";
            head.Vytlacil = "Jozko Mrkvicka";
            head.Rok = 2021;
            head.Mesiac = 12;
            head.OrganizaciaTyp = "Príspevková organizácia";
            head.Mail = "zavinac@zvolen.sk";
            head.Fin112 = true;
            head.Fin204 = true;
        }

        // public DateTime ZostaveneDna { get; set; }
        public int Mesiac { get; set; }
        public int Rok { get; set; }
        public bool Fin112 { get; set; }
        public bool Fin204 { get; set; }
        public bool Fin304 { get; set; }
        public bool Fin404 { get; set; }
        public bool Fin504 { get; set; }
        public bool Fin604 { get; set; }
        public string ZaObdobie
        {
            get { return Mesiac + "." + Rok; }
        }
        public string TextNadpis { get; set; }

    }
}
