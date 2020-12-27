using ServiceStack.DataAnnotations;
using System.Runtime.Serialization;

namespace WebEas.ServiceModel.Types
{
    [Schema("cfe")]
    [Alias("V_Modul")]
    [DataContract]
    public class EsamModul: IBaseView
    {
        [PrimaryKey]
        [AutoIncrement]
        [DataMember]
        public byte C_Modul_Id { get; set; }

        [DataMember]
        public string Kod { get; set; }

        [DataMember]
        public string Nazov { get; set; }

        //audit stlpce
        [DataMember]
        [PfeColumn(Text = "Created by", Hidden = true, Editable = false, ReadOnly = true, LoadWhenVisible = true)]
        public string VytvorilMeno { get; set; }

        [DataMember]
        [PfeColumn(Text = "Edited by", Hidden = true, Editable = false, ReadOnly = true, LoadWhenVisible = true)]
        public string ZmenilMeno { get; set; }
    }
}
