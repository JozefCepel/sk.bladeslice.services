using System;
using System.Linq;
using System.Runtime.Serialization;
using ServiceStack.DataAnnotations;
using ServiceStack.Model;
using System.Collections.Generic;

namespace WebEas.ServiceModel.Pfe.Types
{
    [DataContract]
    public class PohladActions
    {
        [DataMember(Name = "id")]
        public int Id { get; set; }

        [DataMember(Name = "nam")]
        public string Nazov { get; set; }

        [DataMember(Name = "typ")]
        public string Typ { get; set; }

        [DataMember(Name = "kod")]
        public string KodPolozky { get; set; }

        [DataMember(Name = "sia")]
        public bool ShowInActions { get; set; }

        [DataMember(Name = "dvw")]
        public bool DefaultView { get; set; }

        [DataMember(Name = "dat")]
        public PfeDataModel Data { get; set; }

        [DataMember(Name = "flt")]
        public string FilterText { get; set; }

        [DataMember(Name = "loc")]
        public bool Zamknuta { get; set; }

        [DataMember(Name = "acts")]
        public List<NodeAction> Actions { get; set; }

        [DataMember(Name = "tpa")]
        public string TypAkcie { get; set; }

        [DataMember(Name = "vsh")]
        public int ViewSharing { get; set; }

        [DataMember(Name = "pgs")]
        public int PageSize { get; set; }

        [DataMember(Name = "rbf")]
        public string RibbonFilters { get; set; }

        /// <summary>
        /// id pre detail view pohlad a bude sa posielat len pre formular a grid
        /// </summary>
        [DataMember(Name = "dvi")]
        public int? DetailViewId { get; set; }
    }
}
