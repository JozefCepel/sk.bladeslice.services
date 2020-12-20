using ServiceStack.DataAnnotations;
using System.Runtime.Serialization;
using WebEas.ServiceModel;

namespace WebEas.Esam.ServiceModel.Office.Types.Osa
{
    [DataContract]
    [Schema("osa")]
    [Alias("D_OsobaKontakt")]
    public class OsobaKontakt : BaseTenantEntity
    {
        [DataMember]
        [AutoIncrement]
        [PrimaryKey]
        public long D_OsobaKontakt_Id { get; set; }

        [DataMember]
        [PfeColumn(Text = "ID osoby")]
        public long D_Osoba_Id { get; set; }

        [DataMember]
        [PfeColumn(Text = "Telefón 1")]
        [StringLength(100)]
        public string Telefon_1 { get; set; }

        [DataMember]
        [PfeColumn(Text = "Telefón 2")]
        [StringLength(100)]
        public string Telefon_2 { get; set; }

        [DataMember]
        [PfeColumn(Text = "Telefón 3")]
        [StringLength(100)]
        public string Telefon_3 { get; set; }

        [DataMember]
        [PfeColumn(Text = "Fax")]
        [StringLength(100)]
        public string Fax { get; set; }

        [DataMember]
        [PfeColumn(Text = "E-mail")]
        [StringLength(100)]
        public string Email { get; set; }

        [DataMember]
        [PfeColumn(Text = "Web")]
        [StringLength(100)]
        public string Web { get; set; }

        [DataMember]
        [PfeColumn(Text = "Iné")]
        [StringLength(100)]
        public string Ine { get; set; }

        [DataMember]
        [PfeColumn(Text = "Kontaktná osoba meno")]
        [StringLength(450)]
        public string ZastupcaMeno { get; set; }

        [DataMember]
        [PfeColumn(Text = "Kontaktná osoba funkcia")]
        [StringLength(255)]
        public string ZastupcaFunkcia { get; set; }

        [DataMember]
        [PfeColumn(Text = "Hlavný kontakt")]
        public bool? Hlavny { get; set; }

        [DataMember]
        [PfeColumn(Text = "Štatutár")]
        public bool? Statutar { get; set; }

    }
}
