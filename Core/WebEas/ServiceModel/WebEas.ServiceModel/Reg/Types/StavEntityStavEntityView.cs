using ServiceStack.DataAnnotations;
using System.Runtime.Serialization;

namespace WebEas.ServiceModel.Reg.Types
{
    [Schema("reg")]
    [Alias("V_StavEntity_StavEntity")]
    [DataContract]
    public class StavEntityStavEntityView : BaseEntity
    {
        [PrimaryKey]
        [DataMember]
        [PfeSort(Rank = 4)]
        public int C_StavEntity_StavEntity_Id { get; set; }

        [DataMember]
        [PfeSort(Rank = 1)]
        public int C_StavovyPriestor_Id { get; set; }

        [DataMember]
        [PfeColumn(Text = "Oblasť", Width = 270, ReadOnly = true)]
        [PfeCombo(typeof(StavovyPriestor), IdColumnCombo = "C_StavovyPriestor_Id")]
        public string C_StavovyPriestor_Nazov { get; set; }

        [DataMember]
        [PfeColumn(Hidden = true, Hideable = false)]
        [PfeSort(Rank = 2)]
        public int C_StavEntity_Id_Parent { get; set; }

        [DataMember]
        [PfeColumn(Text = "Východiskový stav", Width = 270, RequiredFields = new string[] { "C_StavovyPriestor_Id" }, ReadOnly = true)]
        [PfeCombo(typeof(StavEntityView), IdColumnCombo = "C_StavEntity_Id")]
        public string C_StavEntity_Id_Parent_Nazov { get; set; }

        [DataMember]
        [PfeColumn(Hidden = true, Hideable = false)]
        [PfeSort(Rank = 3)]
        public int C_StavEntity_Id_Child { get; set; }

        [DataMember]
        [PfeColumn(Text = "Cieľový stav", Width = 270, RequiredFields = new string[] { "C_StavovyPriestor_Id" }, ReadOnly = true)]
        [PfeCombo(typeof(StavEntityView), IdColumnCombo = "C_StavEntity_Id")]
        public string C_StavEntity_Id_Child_Nazov { get; set; }

        [DataMember]
        [PfeColumn(Text = "Úkon")]
        [StringLength(255)]
        public string NazovUkonu { get; set; }

        [DataMember]
        [PfeColumn(Text = "Formulár", ReadOnly = true)]
        public string C_Formular_Id_Nazov { get; set; }

        [DataMember]
        [PfeColumn(Hidden = true, Hideable = false)]
        public byte? ePodatelnaEvent { get; set; }
    }
}
