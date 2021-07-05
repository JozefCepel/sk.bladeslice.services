using ServiceStack.DataAnnotations;
using System;
using System.Runtime.Serialization;
using WebEas.ServiceModel;

namespace WebEas.Esam.ServiceModel.Office.Types.Fin
{
    [Schema("fin")]
    [Alias("D_DokladPDK")]
    [DataContract]
    public class DokladPDK : BaseTenantEntity
    {
        [DataMember]
        [PrimaryKey]
        public long D_DokladPDK_Id { get; set; }

        [DataMember]
        public short Rok { get; set; }

        [DataMember]
        [PfeColumn(Text = "Suma dokladu", Tooltip = "Ručne zadaná celková suma dokladu. Po zadaní nenulovej hodnoty sa na novom doklade automaticky vytvorí položka so zvoleným typom na uvedenú sumu.")]
        public decimal? CM_SumaDoklad { get; set; }

        [DataMember]
        [PfeColumn(Text = "Úhrady P/Z", ReadOnly = true)]
        public decimal DM_Uhrady { get; set; }

        [DataMember]
        [PfeColumn(Text = "_Úhrady CM", ReadOnly = true)]
        public decimal CM_Uhrady { get; set; }

        [DataMember]
        [PfeColumn(Text = "_D_User_Id_DokladVyhotovil", Mandatory = true)]
        public Guid D_User_Id_DokladVyhotovil { get; set; }

        [DataMember]
        [PfeColumn(Text = "_D_User_Id_Podpisal")]
        public Guid? D_User_Id_Podpisal { get; set; }

        [DataMember]
        [PfeColumn(Text = "DCOM", DefaultValue = 0, ReadOnly = true)]
        public bool DCOM { get; set; }

        [DataMember]
        [PfeColumn(Text = "Poznámka", Hidden = true, Xtype = PfeXType.Textarea)]
        public new string Poznamka { get; set; } // tu mame neobmedzenu dlzku (dvojicka: v TAB pre DB kontrolu + vo VIEW pre FE)

    }
}
