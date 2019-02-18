using ServiceStack.DataAnnotations;
using System;
using System.Runtime.Serialization;
using WebEas.ServiceModel;

namespace WebEas.Esam.ServiceModel.Office.Rzp.Types
{
    [Schema("rzp")]
    [Alias("V_Fin112")]
    [DataContract]
    public class Fin1Vykaz // : Fin1Base
    {
        // pouzite pre live data (pamatovy dataset)

        // zatial si tam dam len to co sa bude exportovat, ale pribudne sem toho viac, zoberies potom dedicnost z Fin1Base (vid vysie)
        // R

        [PrimaryKey]
        [DataMember]
        [PfeColumn(Text = "_FinKey", ReadOnly = true)]
        public string FinKey { get; set; }

        [DataMember]
        [PfeColumn(Text = "_PrijemVydaj")]
        public byte PrijemVydaj { get; set; }

        [DataMember]
        [PfeColumn(Text = "P/V", Tooltip = "Príjmová alebo výdavková položka", ReadOnly = true)]
        [PfeCombo(typeof(PrijemVydajCombo), NameColumn = "PrijemVydaj")]
        [Ignore]
        public string PrijemVydajText
        {
            get
            {
                return PrijemVydajCombo.GetText(PrijemVydaj);
            }
        }

        [DataMember]
        [PfeColumn(Text = "_EKTypRzp", ReadOnly = true)]
        public string EKTypRzp { get; set; }

        [DataMember]
        [PfeColumn(Text = "Typ rozpočtu", ReadOnly = true)]
        public string EKTypRzpText { get; set; }

        [DataMember]
        [PfeColumn(Text = "EK", ReadOnly = true)]
        public string EK { get; set; }

        [DataMember]
        [PfeColumn(Text = "Názov", ReadOnly = true)]
        public string EKNazov { get; set; }
        
        [DataMember]
        [PfeColumn(Text = "Zdroj", ReadOnly = true)]
        public string ZdrojKod { get; set; }

        [DataMember]
        [PfeColumn(Text = "Schválený rozpočet", ReadOnly = true)]
        public decimal SchvalenyRzp { get; set; }

        [DataMember]
        [PfeColumn(Text = "Rozpočet po zmenách", ReadOnly = true)]
        public decimal UpravenyRzp { get; set; }

        [DataMember]
        [PfeColumn(Text = "Očakávaná skutočnosť", ReadOnly = true)]
        public decimal OcakavanyRzp { get; set; }

        [DataMember]
        [PfeColumn(Text = "Skutočnosť", ReadOnly = true)]
        public decimal SkutocnostRzp { get; set; }

        [DataMember]
        [PfeColumn(Text = "Popis 1", ReadOnly = true)]
        public string EKTypRzpDesc1 { get; set; }

        [DataMember]
        [PfeColumn(Text = "Popis 2", ReadOnly = true)]
        public string EKTypRzpDesc2 { get; set; }

        //[DataMember]
        //[PfeColumn(Hidden = true, Hideable = false)]
        //public new string Zdroj { get; set; }

    }
}
