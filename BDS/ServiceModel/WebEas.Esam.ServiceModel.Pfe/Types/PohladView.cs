using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using ServiceStack.DataAnnotations;

namespace WebEas.ServiceModel.Pfe.Types
{
    [Schema("pfe")]
    [Alias("V_Pohlad")]
    [TenantUpdatable]
    [DataContract]
    public class PohladView :  IPohlad, IValidate
    {
        [AutoIncrement]
        [PrimaryKey]
        [Alias("D_Pohlad_Id")]
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
        public string Data { get; set; }

        [DataMember(Name = "flt")]
        public string FilterText { get; set; }

        [DataMember(Name = "loc")]
        public bool Zamknuta { get; set; }

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

        [DataMember]
        public Guid? D_Tenant_Id { get; set; }

        /// <summary>
        /// Validovanie dat pred insertom a updatom
        /// </summary>
        public void Validate()
        {
            //CK_D_Pohlad_ViewSharing	([ViewSharing]=(2) OR [ViewSharing]=(1) OR [ViewSharing]=(0))
            if (!(new List<int> { 0, 1, 2 }).Contains(this.ViewSharing))
            {
                throw new WebEasValidationException("CK_D_Pohlad_ViewSharing - ([ViewSharing]=(2) OR [ViewSharing]=(1) OR [ViewSharing]=(0))", "Nepovolené hodnoty zdieľania!");
            }
        }
    }
}