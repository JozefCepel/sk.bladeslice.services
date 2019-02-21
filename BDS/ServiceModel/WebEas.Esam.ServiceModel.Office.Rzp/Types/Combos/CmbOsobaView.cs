using ServiceStack.DataAnnotations;
using System;
using System.Runtime.Serialization;
using WebEas.ServiceModel;

namespace WebEas.Esam.ServiceModel.Office.Bds.Types
{
    [Schema("cfe")]
    [Alias("V_cmbOsoba")]
    [PfeCaption("Zoznam zamestnancov")]
    [DataContract]
    public class CmbOsobaView
    {
        [DataMember]
        [PrimaryKey]
        public Guid D_User_Id { get; set; }

        [DataMember]
        [PfeColumn(Text = "Meno")]
        public string FullName { get; set; }

    }
}