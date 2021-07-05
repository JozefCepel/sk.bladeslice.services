using ServiceStack.DataAnnotations;
using System;
using System.Runtime.Serialization;
using WebEas.ServiceModel;

namespace WebEas.Esam.ServiceModel.Office.Types.Uct
{
    [DataContract]
    [Schema("uct")]
    [Alias("D_Transfer")]
    public class Transfer : BaseTenantEntity
    {
        [DataMember]
        [AutoIncrement]
        [PrimaryKey]
        public long D_Transfer_Id { get; set; }

        [DataMember]
        [PfeColumn(Text = "_D_BiznisEntita_Id")]
        public long D_BiznisEntita_Id { get; set; }

        [DataMember]
        [PfeColumn(Text = "_D_DokladBANPol_Id")]
        public long? D_DokladBANPol_Id { get; set; }

        [DataMember]
        [PfeColumn(Text = "_D_UhradaParovanie_Id")]
        public long? D_UhradaParovanie_Id { get; set; }

        [DataMember]
        [PfeColumn(Text = "_D_DokladCRMPol_Id")]
        public long? D_DokladCRMPol_Id { get; set; }

        [DataMember]
        [PfeColumn(Text = "_D_DokladMAJPoh_Id")]
        public long? D_DokladMAJPoh_Id { get; set; }

        [DataMember]
        [PfeColumn(Text = "_C_TransferCis_Id")]
        public int C_TransferCis_Id { get; set; }

        [DataMember]
        [PfeColumn(Text = "Rok", Editable = false)]
        public short Rok { get; set; }

        [DataMember]
        [PfeColumn(Text = "Pč", Mandatory = true)]
        public int Poradie { get; set; }

        [DataMember]
        [PfeColumn(Text = "%", DefaultValue = 100, Mandatory = true, Format = "0| %")]
        public int Percento { get; set; }

        [DataMember]
        [PfeColumn(Text = "Suma", DefaultValue = 0, Mandatory = true)]
        public decimal DM_Suma { get; set; }

        [DataMember]
        [PfeColumn(Text = "Účtovať s výdavkom", Mandatory = true)]
        public bool UctovatSVydavkom { get; set; }

        [DataMember]
        [PfeColumn(Text = "Popis")]
        [StringLength(512)]
        public string Popis { get; set; }
    }
}
