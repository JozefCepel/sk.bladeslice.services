using ServiceStack;
using ServiceStack.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using WebEas.ServiceModel;

namespace WebEas.Esam.ServiceModel.Office.Bds.Types
{
    [Schema("bds")]
    [Alias("fnStsFifo (@DatTo, @K_SKL_0, @K_TSK_0, @ShowLocation, @ShowSN, @ShowSarza, @ShowSklCena)")]
    [DataContract]
    public class STS_FIFOFunction: IBeforeGetList, IPfeCustomize
    {
        [DataMember]
        [PrimaryKey]
        [Ignore]
        public Guid STS_FIFOFunction_Guid => Guid.NewGuid();

        [DataMember]
        [PfeColumn(Text = "_K_SKL_0")]
        public int? K_SKL_0 { get; set; }

        [DataMember]
        [PfeColumn(Text = "_K_TSK_0")]
        public int? K_TSK_0 { get; set; }

        [DataMember]
        [PfeColumn(Text = "_K_TYP_0")]
        public int K_TYP_0 { get; set; }

        [DataMember]
        [PfeColumn(Text = "Warehouse group")]
        public string SKL_GRP { get; set; }

        [DataMember]
        [PfeColumn(Text = "Warehouse")]
        [PfeCombo(typeof(V_SKL_0View), ComboIdColumn = nameof(V_SKL_0View.K_SKL_0), ComboDisplayColumn = nameof(V_SKL_0View.SKL))]
        public string SKL { get; set; }

        [DataMember]
        [PfeColumn(Text = "Mat. group")]
        [PfeCombo(typeof(V_TSK_0View), ComboIdColumn = nameof(V_TSK_0View.K_TSK_0), ComboDisplayColumn = nameof(V_TSK_0View.TSK))]
        public string TSK { get; set; }

        [DataMember]
        [PfeColumn(Text = "_Typ")]
        public string Typ { get; set; }

        [DataMember]
        [PfeColumn(Text = "Code", Mandatory = true, Xtype = PfeXType.SearchFieldSS)] //, RequiredFields = new[] { "K_TSK_0" }
        [PfeCombo(typeof(V_MAT_0View), ComboIdColumn = nameof(V_MAT_0View.KOD), ComboDisplayColumn = nameof(V_MAT_0View.KOD), IdColumn = nameof(KOD),
            AdditionalFields = new[] { nameof(V_MAT_0View.NAZOV), nameof(V_MAT_0View.TSK), nameof(V_MAT_0View.K_TSK_0), nameof(V_MAT_0View.EAN), nameof(V_MAT_0View.MJ), nameof(V_MAT_0View.N_CENA) },
            Tpl = "{value};{TSK}")]
        public string KOD { get; set; }

        [DataMember]
        [PfeColumn(Text = "EAN code")]
        public string EAN { get; set; }

        [DataMember]
        [PfeColumn(Text = "Name")]
        public string NAZOV { get; set; }

        [DataMember]
        [PfeColumn(Text = "UoM", Tooltip = "Unit of Measure")]
        public string MJ { get; set; }

        [DataMember]
        [PfeColumn(Text = "Purchase price", DefaultValue = 0)]
        public decimal N_CENA { get; set; }

        [DataMember]
        [PfeColumn(Text = "Note")]
        public string POZN { get; set; }

        [DataMember]
        [PfeColumn(Text = "Density")]
        public decimal HUST { get; set; }

        [DataMember]
        [PfeColumn(Text = "Weight / UoM")]
        public decimal WT { get; set; }

        [DataMember]
        [PfeColumn(Text = "Total weight")]
        public decimal WT_TOTAL { get; set; }

        [DataMember]
        [PfeColumn(Text = "Weight UoM")]
        public string WT_MJ { get; set; }

        [DataMember]
        [PfeColumn(Text = "Position")]
        public string LOCATION { get; set; }

        [DataMember]
        [PfeColumn(Text = "Serial No.")]
        public string SN { get; set; }

        [DataMember]
        [PfeColumn(Text = "Batch")]
        public string SARZA { get; set; }

        [DataMember]
        [PfeColumn(Text = "Price")]
        public decimal? SKL_CENA { get; set; }

        [DataMember]
        [PfeColumn(Text = "Amount", DecimalPlaces = 4)]
        public decimal? POC_KS { get; set; }

        [DataMember]
        [PfeColumn(Text = "Amount - In", DecimalPlaces = 4)]
        public decimal? POC_KS_P { get; set; }

        [DataMember]
        [PfeColumn(Text = "Amount - Out", DecimalPlaces = 4)]
        public decimal? POC_KS_V { get; set; }

        [DataMember]
        [PfeColumn(Text = "Accounting value")]
        public decimal? UCT_SUMA { get; set; }

        [DataMember]
        [PfeColumn(Text = "_STS_ITEM")]
        public string STS_ITEM { get; set; }

        [DataMember]
        [PfeColumn(Text = "Receipt date", Type = PfeDataType.Date)]
        public DateTime? DAT_PRI { get; set; }

        [DataMember]
        [PfeColumn(Text = "Outer dimensions")]
        public string OUTER_SIZE { get; set; }

        public void BeforeGetList(IWebEasRepositoryBase repository, HierarchyNode node, ref string sql, ref Filter filter, ref string sqlFromAlias, string sqlOrderPart)
        {
            var parameters = filter.Parameters;
            var newFilter = new Filter();

            //@DatTo DATE, @ShowLocation bit, @ShowSN bit, @ShowSarza bit, @ShowSklCena bit

            string datDo = parameters.ContainsKey("DATTO") ? "'" + DateTime.Parse(parameters["DATTO"].ToString()).ToString("yyyy-MM-dd") + "'" : "NULL";

            sqlFromAlias = sqlFromAlias.Replace("@DatTo", datDo);

            sqlFromAlias = parseParam(filter, sqlFromAlias, parameters, "K_SKL_0");
            sqlFromAlias = parseParam(filter, sqlFromAlias, parameters, "K_TSK_0");

            sqlFromAlias = sqlFromAlias.Replace("@ShowLocation", parameters.ContainsKey("ShowLocation".ToUpper()) ? "1" : "0");
            sqlFromAlias = sqlFromAlias.Replace("@ShowSN", parameters.ContainsKey("ShowSN".ToUpper()) ? "1" : "0");
            sqlFromAlias = sqlFromAlias.Replace("@ShowSarza", parameters.ContainsKey("ShowSarza".ToUpper()) ? "1" : "0");
            sqlFromAlias = sqlFromAlias.Replace("@ShowSklCena", parameters.ContainsKey("ShowSklCena".ToUpper()) ? "1" : "0");

            if (filter?.Parameters != null)
            {
                filter.ReplaceEqualsToStartsWith(nameof(SN));
                filter.ReplaceEqualsToStartsWith(nameof(SARZA));
                filter.ReplaceEqualsToStartsWith(nameof(LOCATION));

                //filter = BookFilterGenerator.AddNoDialogFilters(filter, newFilter);
                filter = AddNoDialogFilters(filter, newFilter);
            }

            static string parseParam(Filter filter, string sqlFromAlias, Dictionary<string, object> parameters, string fld)
            {
                if (parameters.ContainsKey(fld))
                {
                    string ids = parameters[fld].ToString();
                    if (ids.Contains(","))
                    {
                        sqlFromAlias = sqlFromAlias.Replace("@" + fld, "NULL");
                        filter.Remove(fld);
                        filter.And(FilterElement.In(fld, ids.FromJson<List<int>>()));
                    }
                    else
                    {
                        sqlFromAlias = sqlFromAlias.Replace("@" + fld, ids);
                        filter.Remove(fld);
                    }
                }
                else
                {
                    sqlFromAlias = sqlFromAlias.Replace("@" + fld, "NULL");
                }

                return sqlFromAlias;
            }
        }

        private static Filter AddNoDialogFilters(Filter filter, Filter newFilter)
        {
            Filter pomocnyFilter = filter.CloneAsFilter();
            //Odstránenie použitých filtrov:
            pomocnyFilter.Remove("DatTo");

            pomocnyFilter.Remove("ShowLocation");
            pomocnyFilter.Remove("ShowSN");
            pomocnyFilter.Remove("ShowSarza");
            pomocnyFilter.Remove("ShowSklCena");


            filter = pomocnyFilter.IsEmpty() ? newFilter : newFilter.And(pomocnyFilter);
            return filter;
        }


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
                        },
                    };
                }
            }
        }
    }
}
