using ServiceStack.DataAnnotations;
using System.Runtime.Serialization;
using WebEas.ServiceModel;

namespace WebEas.Esam.ServiceModel.Office.Types.Reg
{
    [DataContract]
    [Schema("uct")]
    [Alias("C_Predkontacia")]
    public class PredkontaciaCis : BaseTenantEntity, IValidateConstraint
    {
        [DataMember]
        [AutoIncrement]
        [PrimaryKey]
        [PfeColumn(Text = "Identifikátor")]
        public long C_Predkontacia_Id { get; set; }

        [DataMember]
        [PfeColumn(Text = "Pč")]
        public short Poradie { get; set; }

        [DataMember]
        [PfeColumn(Text = "_SkupinaPredkont_Id", Mandatory = true)]
        [HierarchyNodeParameter]
        public int SkupinaPredkont_Id { get; set; }

        [DataMember]
        [PfeColumn(Text = "_C_Predkontacia_Id_Parent")]
        [HierarchyNodeParameter]
        public long? C_Predkontacia_Id_Parent { get; set; }

        [DataMember]
        [PfeColumn(Text = "Názov")]
        [StringLength(100)]
        public string Nazov { get; set; }

        public string ChangeConstraintMessage(string constraintName, int errorCode, WebEasSqlKnownErrorType errorType)
        {
            if (constraintName == "FK_D_BiznisEntita_C_Predkontacia")
            {
                return "Predkontáciu nie je možné zmazať, je priradená dokladom!";
            }

            return null;
        }
    }
}
