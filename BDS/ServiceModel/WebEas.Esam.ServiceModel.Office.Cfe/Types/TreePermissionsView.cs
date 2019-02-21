using System;
using System.Runtime.Serialization;
using ServiceStack.DataAnnotations;
using WebEas.ServiceModel;

namespace WebEas.Esam.ServiceModel.Office.Cfe.Types
{
    [Schema("cfe")]
    [Alias("V_TreePermissions")]
    [DataContract]
    public class TreePermissionsView : BaseEntity
    {
        [PrimaryKey]
        [AutoIncrement]
        [DataMember]        
        public int D_TreePermissions_Id { get; set; }

        [DataMember]
        public Guid D_Tenant_Id { get; set; }

        [DataMember]
        public int C_Module_Id { get; set; }

        [DataMember]
        public string Kod { get; set; }

        [DataMember]
        [PfeColumn(Text = "_Pravo")]
        public short Pravo { get; set; }

        [DataMember]
        [PfeCombo(typeof(PravoCombo), NameColumn = "Pravo")]
        [PfeColumn(Text = "Právo")]
        [Ignore]
        public string PravoText
        {
            get
            {
                return PravoCombo.GetText(Pravo);
            }
        }

        [DataMember]
        public int C_Role_Id { get; set; }

        [DataMember]
        public Guid D_User_Id { get; set; }

    }
}
