using ServiceStack;
using ServiceStack.DataAnnotations;
using System;
using System.Runtime.Serialization;
using WebEas.Esam.ServiceModel.Office.Types.Reg;
using WebEas.ServiceModel;

namespace WebEas.Esam.ServiceModel.Office.Reg.Dto
{
    // Create
    [Route("/CreateProjekt", "POST")]
    [Api("Projekt")]
    [DataContract]
    public class CreateProjekt : ProjektDto, IReturn<ProjektView> { }

    // Update
    [Route("/UpdateProjekt", "PUT")]
    [Api("Projekt")]
    [DataContract]
    public class UpdateProjekt : ProjektDto, IReturn<ProjektView>
    {
        [PrimaryKey]
        [DataMember(IsRequired = true)]
        public long C_Projekt_Id { get; set; }
    }

    // Delete
    [Route("/DeleteProjekt", "DELETE")]
    [Api("Projekt")]
    [DataContract]
    public class DeleteProjekt
    {
        [DataMember(IsRequired = true)]
        public long[] C_Projekt_Id { get; set; }
    }

    #region DTO
    [DataContract]
    public class ProjektDto : BaseDto<Projekt>
    {
        [DataMember]
        public string C_Projekt_Id_Externe { get; set; }

        [DataMember]
        public string Kod { get; set; }

        [DataMember]
        public string Nazov { get; set; }

        [DataMember]
        public string Popis { get; set; }

        [DataMember]
        public int? C_RzpTyp_Id { get; set; }

        [DataMember]
        [NotEmptyOrDefault]
        public DateTime PlatnostOd { get; set; }

        [DataMember]
        public DateTime? PlatnostDo { get; set; }

        [DataMember]
        public DateTime? DatumDotacie { get; set; }

        [DataMember]
        public Guid? Poskytovatel { get; set; }

        [DataMember]
        public long? C_FRZdroj_Id { get; set; }

        [DataMember]
        public decimal? Suma { get; set; }

        /// <summary>
        /// Binds to entity.
        /// </summary>
        /// <param name="data"></param>
        protected override void BindToEntity(Projekt data)
        {
            data.C_Projekt_Id_Externe = C_Projekt_Id_Externe;
            data.Kod = Kod;
            data.Nazov = Nazov;
            data.Popis = Popis;
            data.C_RzpTyp_Id = C_RzpTyp_Id;
            data.PlatnostOd = PlatnostOd;
            data.PlatnostDo = PlatnostDo;
            data.DatumDotacie = DatumDotacie;
            data.Poskytovatel = Poskytovatel;
            data.C_FRZdroj_Id = C_FRZdroj_Id;
            data.Suma = Suma;
        }
    }
    #endregion
}
