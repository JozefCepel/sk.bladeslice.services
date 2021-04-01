using ServiceStack.DataAnnotations;
using System;
using System.Runtime.Serialization;
using WebEas.ServiceModel;

namespace WebEas.Esam.ServiceModel.Office.Bds.Types
{
    [Schema("bds")]
    [Alias("V_VYD_0")]
    [DataContract]
    public class V_VYD_0View : tblD_VYD_0
    {
        [DataMember]
        [PfeColumn(Text = "_C_StavEntity_Id")]
        public int C_StavEntity_Id { get; set; }

        [DataMember]
        [PfeColumn(Text = "Organisation unit")]
        [PfeCombo(typeof(tblK_ORJ_0), ComboIdColumn = "K_ORJ_0", ComboDisplayColumn = "ORJ")]
        public string ORJ { get; set; }

        [DataMember]
        [PfeColumn(Text = "Warehouse", RequiredFields = new[] { "K_ORJ_0" })]
        [PfeCombo(typeof(V_ORJ_1View), ComboIdColumn = "K_SKL_0", ComboDisplayColumn = "SKL")]
        public string SKL { get; set; }

        [DataMember]
        [PfeColumn(Text = "Customer")]
        [PfeCombo(typeof(tblK_OBP_0), ComboIdColumn = "K_OBP_0", ComboDisplayColumn = "NAZOV1", AdditionalWhereSql = "K_TOB_0 IN  (1, 3)")] //Odberateľ
        public string NAZOV1 { get; set; }

        [DataMember]
        [PfeColumn(Text = "Name 2", ReadOnly = true, Editable = false)]
        public string NAZOV2 { get; set; }

        [DataMember]
        [PfeColumn(Text = "Business ID No.", ReadOnly = true, Editable = false)]
        //[PfeCombo(typeof(tblK_OBP_0), ComboIdColumn = "K_OBP_0", ComboDisplayColumn = "ICO")]
        public string ICO { get; set; }

        [DataMember]
        [PfeColumn(Text = "Street", ReadOnly = true, Editable = false)]
        public string ULICA_S { get; set; }

        [DataMember]
        [PfeColumn(Text = "ZIP code", ReadOnly = true, Editable = false)]
        public string PSC_S { get; set; }

        [DataMember]
        [PfeColumn(Text = "City", ReadOnly = true, Editable = false)]
        public string OBEC_S { get; set; }

        [DataMember]
        [PfeColumn(Text = "State", ReadOnly = true, Editable = false)]
        public string STAT { get; set; }

        //audit stlpce
        [DataMember]
        [PfeColumn(Text = "Created by", Hidden = true, Editable = false, ReadOnly = true)]
        public string VytvorilMeno { get; set; }

        [DataMember]
        [PfeColumn(Text = "Edited by", Hidden = true, Editable = false, ReadOnly = true)]
        public string ZmenilMeno { get; set; }

        //[DataMember]
        //[PfeColumn(Text = "_K_OBP_0_ICO")]
        //[Ignore]
        //public int? K_OBP_0_ICO
        //{
        //    get
        //    {
        //        return K_OBP_0;
        //    }
        //}

        //[DataMember]
        //[PfeColumn(Text = "_K_OBP_0_NAZOV1")]
        //[Ignore]
        //public int? K_OBP_0_NAZOV1
        //{
        //    get
        //    {
        //        return K_OBP_0;
        //    }
        //}
    }
}
