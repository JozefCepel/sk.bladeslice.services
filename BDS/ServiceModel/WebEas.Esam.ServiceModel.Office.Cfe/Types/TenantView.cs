using ServiceStack.DataAnnotations;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
//using WebEas.Esam.ServiceModel.Office.Osa;
using WebEas.Esam.ServiceModel.Office.Types.Osa;
using WebEas.ServiceModel;

namespace WebEas.Esam.ServiceModel.Office.Cfe.Types
{
    [Schema("cfe")]
    [Alias("V_Tenant")]
    [DataContract]
    public class TenantView : Tenant, IPfeCustomize, IBaseView
    {
        [DataMember]
        [PfeColumn(Text = "Typ", Mandatory = true)]
        [PfeCombo(typeof(TenantType), IdColumn = nameof(C_TenantType_Id), ComboDisplayColumn = nameof(TenantType.Typ))]
        public string TenantTypeName { get; set; }

        [DataMember]
        [PfeColumn(Text = "Typ organizácie", Mandatory = true)]
        [PfeCombo(typeof(OrganizaciaTypView), IdColumn = nameof(C_OrganizaciaTyp_Id), ComboDisplayColumn = nameof(OrganizaciaTypView.Nazov))]
        public string OrganizaciaTypNazov { get; set; }

        [DataMember]
        [PfeColumn(Text = "IČO", Xtype = PfeXType.SearchFieldSS)]
        [PfeCombo(typeof(OsobaView), IdColumn = nameof(D_PO_Osoba_Id), ComboDisplayColumn = nameof(OsobaView.IdentifikatorMeno))]
        public string ICO { get; set; }

        [DataMember]
        [PfeColumn(Text = "Moja obec/mesto", ReadOnly = true)]
        public string MenoObchodne { get; set; }

        [DataMember]
        [PfeColumn(Text = "Adresa sídla", ReadOnly = true)]
        public string AdresaDlhaByt { get; set; }

        [DataMember]
        [PfeColumn(Text = "Adresa obec", ReadOnly = true)]
        public string AdresaObec { get; set; }

        [DataMember]
        [PfeColumn(Text = "Adresa ulica", ReadOnly = true)]
        public string AdresaUlicaCislo { get; set; }

        [DataMember]
        [PfeColumn(Text = "Adresa PSČ", ReadOnly = true)]
        public string AdresaPSC { get; set; }

        //audit stlpce
        [DataMember]
        [PfeColumn(Text = "Vytvoril", Hidden = true, Editable = false, ReadOnly = true, LoadWhenVisible = true)]
        public string VytvorilMeno { get; set; }

        [DataMember]
        [PfeColumn(Text = "Zmenil", Hidden = true, Editable = false, ReadOnly = true, LoadWhenVisible = true)]
        public string ZmenilMeno { get; set; }

        public void CustomizeModel(PfeDataModel model, IWebEasRepositoryBase repository, HierarchyNode node, string filter, object masterNodeParameter, string masterNodeKey)
        {
            /*
            if (model.Fields != null)
            {
                var icoField = model.Fields.FirstOrDefault(p => p.Name == nameof(ICO));
                if (icoField != null)
                {
                    icoField.SearchFieldDefinition = new List<PfeSearchFieldDefinition>
                    {
                        new PfeSearchFieldDefinition
                        {
                            Code = "osa-oso-po",
                            NameField = nameof(PO_OsobaView.D_PO_Osoba_Id),
                            DisplayField = nameof(PO_OsobaView.ICO),
                            InputSearchField = nameof(PO_OsobaView.InputSearchField)
                        }
                    };
                }
            }
            */
        }
    }
}
