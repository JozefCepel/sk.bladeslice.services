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
        [PfeColumn(Text = "Úhrady", Mandatory = true, DefaultValue = 0)]
        public decimal DM_Uhrady { get; set; }

        [DataMember]
        [PfeColumn(Text = "_Úhrady CM", Mandatory = true, DefaultValue = 0)]
        public decimal CM_Uhrady { get; set; }

        [DataMember]
        [PfeColumn(Text = "DCOM", DefaultValue = 0, ReadOnly = true)]
        public bool DCOM { get; set; }

        [DataMember]
        [PfeColumn(Text = "_D_User_Id_DokladVyhotovil", Mandatory = true)]
        public Guid D_User_Id_DokladVyhotovil { get; set; }

        [DataMember]
        [PfeColumn(Text = "_D_OsobaKontakt_Id_Komu")]
        public long? D_OsobaKontakt_Id_Komu { get; set; }
    }
}
