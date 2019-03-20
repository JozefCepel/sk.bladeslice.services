using ServiceStack.DataAnnotations;
using System;
using System.Runtime.Serialization;
using WebEas.ServiceModel;

namespace WebEas.Esam.ServiceModel.Office.Rzp.Types
{
    [DataContract]
    public class Fin1PolBase: BaseTenantEntity 
    {
        [DataMember]
        [PfeColumn(Text = "Provizórium", ReadOnly = true)]
        public bool Provizorium { get; set; }

        [DataMember]
        [PfeColumn(Text = "_PrijemVydaj")]
        public short PrijemVydaj { get; set; }

        [DataMember]
        [PfeCombo(typeof(PrijemVydajCombo), NameColumn = "PrijemVydaj")]
        [PfeColumn(Text = "P/V", Tooltip = "Príjmová alebo výdavková položka", ReadOnly = true)]
        [Ignore]
        public string PrijemVydajText
        {
            get
            {
                return PrijemVydajCombo.GetText(PrijemVydaj);
            }
        }

        [DataMember]
        [PfeColumn(Text = "_Typ rozpočtu", ReadOnly = true)]
        public int EKRzpTyp { get; set; }

        [DataMember]
        [PfeColumn(Text = "Typ rozpočtu")]
        [PfeCombo(typeof(RzpTypCis), NameColumn = "C_RzpTyp_Id", DisplayColumn = "Nazov")]
        [IgnoreInsertOrUpdate]
        public string RzpTypName { get; set; }

        [DataMember]
        [PfeColumn(Text = "Druh rozpočtu", ReadOnly = true)]
        public string DruhRzp { get; set; }

        [DataMember]
        [PfeColumn(Text = "Zdroj", ReadOnly = true)]
        public string ZdrojKod { get; set; }

        [DataMember]
        [PfeColumn(Text = "_D_Program_Id", ReadOnly = true)]
        public long? D_Program_Id { get; set; }

        [DataMember]
        [PfeColumn(Text = "Program", ReadOnly = true)]
        public string PRKod { get; set; }

        [DataMember]
        [PfeColumn(Text = "Oddiel", ReadOnly = true)]
        public string FKOddiel { get; set; }

        [DataMember]
        [PfeColumn(Text = "Skupina", ReadOnly = true)]
        public string FKSkupina { get; set; }

        [DataMember]
        [PfeColumn(Text = "Trieda", ReadOnly = true)]
        public string FKTrieda { get; set; }

        [DataMember]
        [PfeColumn(Text = "Podtrieda")]
        public string FKPodtrieda { get; set; }

        [DataMember]
        [PfeColumn(Text = "FK", ReadOnly = true)]
        public string FK { get; set; }

        [DataMember]
        [PfeColumn(Text = "Položka", ReadOnly = true)]
        public string EKPolozka { get; set; }

        [DataMember]
        [PfeColumn(Text = "Podpoložka", ReadOnly = true)]
        public string EKPodpolozka { get; set; }

        [DataMember]
        [PfeColumn(Text = "EK", ReadOnly = true)]
        public string EK { get; set; }

        [DataMember]
        [PfeColumn(Text = "Názov", ReadOnly = true)]
        public string EKNazov { get; set; }

        [DataMember]
        [PfeColumn(Text = "Schválený rozpočet", ReadOnly = true)]
        public decimal SchvalenyRzp { get; set; }

        [DataMember]
        [PfeColumn(Text = "Zmeny rozpočtu", ReadOnly = true)]
        public decimal ZmenyRzp { get; set; }

        [DataMember]
        [PfeColumn(Text = "Rozpočet po zmenách", ReadOnly = true)]
        public decimal RzpPoZmenach { get; set; }

        [DataMember]
        [PfeColumn(Text = "Očakávaná skutočnosť", ReadOnly = true)]
        public decimal OcakavanyRzp { get; set; }

        [DataMember]
        [PfeColumn(Text = "Skutočnosť", ReadOnly = true)]
        public decimal SkutocnyRzp { get; set; }

    }
}
