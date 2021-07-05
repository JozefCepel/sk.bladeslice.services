using System;

namespace WebEas.Esam.Reports
{
    [System.ComponentModel.DataObject]
    public class ZostavaFilter
    {
        public ZostavaFilter(string text)
        {
            this.Filter = text;
        }

        public string Filter { get; set; }
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
