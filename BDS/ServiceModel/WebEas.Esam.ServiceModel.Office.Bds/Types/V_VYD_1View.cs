using ServiceStack.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using WebEas.ServiceModel;

namespace WebEas.Esam.ServiceModel.Office.Bds.Types
{
    [Schema("bds")]
    [Alias("V_VYD_1")]
    [DataContract]
    public class V_VYD_1View : tblD_VYD_1, IPfeCustomize
    {
        [DataMember]
        [PfeColumn(Text = "Expense No.")]
        [PfeCombo(typeof(tblD_VYD_0), ComboIdColumn = "D_VYD_0", ComboDisplayColumn = "DKL_C")]
        public string DKL_C { get; set; }

        [DataMember]
        [PfeColumn(Text = "A", Editable = false, ReadOnly = true)]
        public bool V { get; set; }

        [DataMember]
        [PfeColumn(Text = "Expense item", ReadOnly = true)]
        public string VydPol { get; set; }

        [DataMember]
        [PfeColumn(Text = "_K_SKL_0", ReadOnly = true)] //Iba kvoli RequiredField
        public int K_SKL_0 { get; set; }

        //[PfeColumn(Text = "Mat. group", RequiredFields = new[] { "K_SKL_0" })]
        //[PfeCombo(typeof(V_SKL_1View), ComboIdColumn = "K_TSK_0", ComboDisplayColumn = "TSK")]
        [DataMember]
        [PfeColumn(Text = "Mat. group", ReadOnly = true)]
        public string TSK { get; set; }

        [DataMember]
        [PfeColumn(Text = "Stock item", RequiredFields = new[] { nameof(K_SKL_0), nameof(K_TSK_0), nameof(KOD) }, Mandatory = true)]
        [PfeCombo(typeof(STS_FIFOFull), ComboIdColumn = nameof(STS_FIFOFull.STS_ITEM), ComboDisplayColumn = nameof(STS_FIFOFull.STS_ITEM), IdColumn = nameof(STS_ITEM),
            AdditionalFields = new[] { nameof(STS_FIFOFull.SN), nameof(STS_FIFOFull.SARZA), nameof(STS_FIFOFull.LOCATION), nameof(STS_FIFOFull.POC_KS), nameof(STS_FIFOFull.SKL_CENA) })]
        public string STS_ITEM { get; set; }

        [DataMember]
        [PfeColumn(Text = "_3D simulácia")]
        public byte SKL_SIMULATION { get; set; }

        [DataMember]
        [PfeCombo(typeof(SimulationType), IdColumn = nameof(SKL_SIMULATION))]
        [PfeColumn(Text = "3D simulation", ReadOnly = true, Editable = false)]
        [Ignore]
        public string SKL_SIMULATIONText => SimulationType.GetText(SKL_SIMULATION);

        //audit stlpce
        [DataMember]
        [PfeColumn(Text = "Created by", Hidden = true, Editable = false, ReadOnly = true)]
        public string VytvorilMeno { get; set; }

        [DataMember]
        [PfeColumn(Text = "Edited by", Hidden = true, Editable = false, ReadOnly = true)]
        public string ZmenilMeno { get; set; }

        public void CustomizeModel(PfeDataModel model, IWebEasRepositoryBase repository, HierarchyNode node, string filter, object masterNodeParameter, string masterNodeKey)
        {
            if (model.Fields != null)
            {
                var kodField = model.Fields.FirstOrDefault(p => p.Name == nameof(KOD));
                if (kodField != null)
                {
                    kodField.SearchFieldDefinition = new List<PfeSearchFieldDefinition>
                    {
                        new PfeSearchFieldDefinition
                        {
                            Code = "bds-kat-mat",
                            NameField = nameof(V_MAT_0View.KOD),
                            DisplayField = nameof(V_MAT_0View.KOD),
                            /*
                            Condition = new List<PfeFilterAttribute>
                            {
                                new PfeFilterAttribute
                                {
                                    Field = nameof(ExpenseBy),
                                    ComparisonOperator = "eq",
                                    Value = 2,             //3D - to znamená, že vyberám z katalógu
                                }
                            },
                            */
                        },
                        /*
                        new PfeSearchFieldDefinition
                        {
                            Code = "bds-skl-sts",
                            NameField = nameof(V_MAT_0View.KOD),
                            DisplayField = nameof(V_MAT_0View.KOD),
                            Condition = new List<PfeFilterAttribute>
                            {
                                new PfeFilterAttribute
                                {
                                    Field = nameof(ExpenseBy),
                                    ComparisonOperator = "eq",
                                    Value = 1,             //Stock - to znamená, že vyberám zo stavu skladu
                                }
                            },
                        }
                        */

                    };
                }
            }
        }
    }
}
