using ServiceStack.DataAnnotations;
using System;
using System.Runtime.Serialization;
using WebEas.ServiceModel;

namespace WebEas.Esam.ServiceModel.Office.Types.Cfe
{
    [Schema("cfe")]
    [Alias("V_ModulUser")]
    [DataContract]
    public class ModulUserView
    {
        [DataMember]        
        [HierarchyNodeParameter]
        [PfeColumn(Text = "C_Modul_Id", Hidden = true, NameField = "C_Modul_Id", IdField = "C_Modul_Id")]
        public int C_Modul_Id { get; set; }

        [DataMember]
        [PfeColumn(Text = "_Kód", ReadOnly = true)]
        public string Kod { get; set; }

        [DataMember]
        [PfeColumn(Text = "_Názov", ReadOnly = true)]
        public string Nazov { get; set; }

        [DataMember]
        [PrimaryKey]
        [PfeColumn(Text = "D_User_Id", Hidden = true, ReadOnly = true)]
        public Guid D_User_Id { get; set; }

        [DataMember]
        [PfeColumn(Text = "Prihlasovacie meno", Mandatory = true)]
        public string LoginName { get; set; }

        [DataMember]
        [PfeColumn(Text = "E-mailová adresa", Mandatory = true)]
        public string Email { get; set; }

        [DataMember]
        [PfeColumn(Text = "Doménové meno")]
        public string DomainName { get; set; }

        [DataMember]
        [PfeColumn(Text = "Evidenčné číslo")]
        public string EC { get; set; }

        [DataMember]
        [PfeColumn(Text = "_Nadriadený")]
        public Guid? D_User_Id_Parent { get; set; }

        [DataMember]
        [PfeColumn(Text = "Dátum začiatku", Type = PfeDataType.Date, Mandatory = true)]
        public DateTime PlatnostOd { get; set; }

        [DataMember]
        [PfeColumn(Text = "Dátum ukončenia", Type = PfeDataType.Date)]
        public DateTime? PlatnostDo { get; set; }

        [DataMember]
        [PfeColumn(Text = "_Krajina")]  // bude sa riesit neskor, nie ja na to cas, defualtne 1 = SVK
        public short? Country { get; set; }

        [DataMember]
        [PfeColumn(Text = "Meno", ReadOnly = true)]
        public string FullName { get; set; }
    }
}
