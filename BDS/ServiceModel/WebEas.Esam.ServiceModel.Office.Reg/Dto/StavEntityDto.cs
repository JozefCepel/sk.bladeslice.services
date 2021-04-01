using ServiceStack;
using ServiceStack.DataAnnotations;
using System.Collections.Generic;
using System.Runtime.Serialization;
using WebEas.ServiceModel;
using WebEas.ServiceModel.Office.Egov.Reg.Types;
using WebEas.ServiceModel.Reg.Types;

namespace WebEas.Esam.ServiceModel.Office.Reg.Dto
{
    #region StavovyPriestor

    #region Operations

    // Create
    [Route("/cis/CreateStavovyPriestor", "POST")]
    [Api("Reg")]
    [DataContract]
    public class CreateStavovyPriestor : StavovyPriestorDto, IReturn<StavovyPriestorView>
    {
    }

    // Update
    [Route("/cis/UpdateStavovyPriestor", "PUT")]
    [Api("Reg")]
    [DataContract]
    public class UpdateStavovyPriestor : StavovyPriestorDto, IReturn<StavovyPriestorView>
    {
        [ServiceStack.DataAnnotations.PrimaryKey]
        [DataMember(IsRequired = true)]
        public int C_StavovyPriestor_Id { get; set; }
    }

    // Delete
    [Route("/cis/DeleteStavovyPriestor", "DELETE")]
    [Api("Reg")]
    [DataContract]
    public class DeleteStavovyPriestor
    {
        [ServiceStack.DataAnnotations.PrimaryKey]
        [DataMember(IsRequired = true)]
        public int[] C_StavovyPriestor_Id { get; set; }
    }

    // List
    [Route("/cis/ListStavovyPriestor", "GET")]
    [Api("Reg")]
    [DataContract]
    public class ListStavovyPriestor : BaseListDto
    {
    }

    #endregion

    #region DTO

    [DataContract]
    public class StavovyPriestorDto : BaseDto<StavovyPriestor>
    {
        // + Id column in update and delete
        [DataMember]
        [Required]
        public string Nazov { get; set; }

        /// <summary>
        /// Binds to entity.
        /// </summary>
        /// <param name="data"></param>
        protected override void BindToEntity(StavovyPriestor data)
        {
            data.Nazov = this.Nazov;
        }
    }

    #endregion

    #endregion

    #region Nasledovny stav entity

    [Route("/CreateNasledovnyStavEntity", "POST")]
    [Api("Evidencia stavov")]
    [DataContract]
    public class CreateNasledovnyStavEntity : NasledovnyStavEntityDto
    {
    }

    [DataContract]
    public class NasledovnyStavEntityDto : StavEntityDto
    {
        [DataMember]
        public int C_StavovyPriestor_Id { get; set; }

        [DataMember]
        public int C_StavEntity_Id_Predchadzajuci { get; set; }
    }

    [Route("/ListNaslStavEntity/{C_StavEntity_Id}", "GET")]
    [Api("Evidencia stavov")]
    [DataContract]
    public class ListNaslStavEntity
    {
        [DataMember]
        [ApiMember(Name = "C_StavEntity_Id", Description = "Id stav entity", DataType = "int", IsRequired = true)]
        public int C_StavEntity_Id { get; set; }
    }

    [DataContract]
    public class ListNaslStavEntityResponse : ResultResponse<List<StavEntityDto>>
    {
    }

    #endregion

    #region StavEntity

    #region Operations

    // Create
    [Route("/cis/CreateStavEntity", "POST")]
    [Api("Reg")]
    [DataContract]
    public class CreateStavEntity : StavEntityDto, IReturn<StavEntityView>
    {
    }

    // Update
    [Route("/cis/UpdateStavEntity", "PUT")]
    [Api("Reg")]
    [DataContract]
    public class UpdateStavEntity : StavEntityDto, IReturn<StavEntityView>
    {
        [ServiceStack.DataAnnotations.PrimaryKey]
        [DataMember(IsRequired = true)]
        public int C_StavEntity_Id { get; set; }
    }

    // Delete
    [Route("/cis/DeleteStavEntity", "DELETE")]
    [Api("Reg")]
    [DataContract]
    public class DeleteStavEntity
    {
        [ServiceStack.DataAnnotations.PrimaryKey]
        [DataMember(IsRequired = true)]
        public int[] C_StavEntity_Id { get; set; }
    }

    // List
    [Route("/cis/ListStavEntity", "GET")]
    [Api("Reg")]
    [DataContract]
    public class ListStavEntity : BaseListDto
    {
    }

    #endregion

    #region DTO

    [DataContract]
    public class StavEntityDto : BaseDto<StavEntity>
    {
        // + Id column in update and delete
        [DataMember]
        public string Kod { get; set; }

        [DataMember]
        public string Strom { get; set; }

        [DataMember]
        [Required]
        public int C_Formular_Id { get; set; }

        [DataMember]
        [Required]
        public string Nazov { get; set; }

        [DataMember]
        public bool JePociatocnyStav { get; set; }

        [DataMember]
        public bool JeKoncovyStav { get; set; }

        [DataMember]
        public bool JeKladneVybavenie { get; set; }

        [DataMember]
        public string Textacia { get; set; }

        [DataMember]
        public string BiznisAkcia { get; set; }

        [DataMember]
        public bool PovinnyDokument { get; set; }

        /// <summary>
        /// Binds to entity.
        /// </summary>
        /// <param name="data"></param>
        protected override void BindToEntity(StavEntity data)
        {
            data.BiznisAkcia = this.BiznisAkcia;
            data.C_Formular_Id = this.C_Formular_Id;
            data.JeKladneVybavenie = this.JeKladneVybavenie;
            data.JePociatocnyStav = this.JePociatocnyStav;
            data.JeKoncovyStav = this.JeKoncovyStav;
            data.Kod = this.Kod;
            data.Nazov = this.Nazov;
            data.PovinnyDokument = this.PovinnyDokument;
            data.Strom = this.Strom;
            data.Textacia = this.Textacia;
        }
    }

    #endregion

    #endregion

    #region StavEntityStavEntity

    #region Operations

    // Create
    [Route("/cis/CreateStavEntityStavEntity", "POST")]
    [Api("Reg")]
    [DataContract]
    public class CreateStavEntityStavEntity : StavEntityStavEntityDto
    {
    }

    // Update
    [Route("/cis/UpdateStavEntityStavEntity", "PUT")]
    [Api("Reg")]
    [DataContract]
    public class UpdateStavEntityStavEntity : StavEntityStavEntityDto
    {
        [ServiceStack.DataAnnotations.PrimaryKey]
        [DataMember(IsRequired = true)]
        public int C_StavEntity_StavEntity_Id { get; set; }
    }

    // Delete
    [Route("/cis/DeleteStavEntityStavEntity", "DELETE")]
    [Api("Reg")]
    [DataContract]
    public class DeleteStavEntityStavEntity
    {
        [ServiceStack.DataAnnotations.PrimaryKey]
        [DataMember(IsRequired = true)]
        public int[] C_StavEntity_StavEntity_Id { get; set; }
    }

    // List
    [Route("/cis/ListStavEntityStavEntity", "GET")]
    [Api("Reg")]
    [DataContract]
    public class ListStavEntityStavEntity : BaseListDto
    {
    }

    #endregion

    #region DTO

    [DataContract]
    public class StavEntityStavEntityDto : BaseDto<StavEntityStavEntity>
    {
        // + Id column in update and delete
        [DataMember]
        [Required]
        public int C_StavEntity_Id_Parent { get; set; }

        [DataMember]
        [Required]
        public int C_StavEntity_Id_Child { get; set; }

        [DataMember]
        [Required]
        public int C_StavovyPriestor_Id { get; set; }

        [DataMember]
        [Required]
        public string NazovUkonu { get; set; }

        [DataMember]
        public byte? ePodatelnaEvent { get; set; }

        /// <summary>
        /// Binds to entity.
        /// </summary>
        /// <param name="data"></param>
        protected override void BindToEntity(StavEntityStavEntity data)
        {
            data.C_StavEntity_Id_Parent = this.C_StavEntity_Id_Parent;
            data.C_StavEntity_Id_Child = this.C_StavEntity_Id_Child;
            data.C_StavovyPriestor_Id = this.C_StavovyPriestor_Id;
            data.NazovUkonu = this.NazovUkonu;
            data.ePodatelnaEvent = this.ePodatelnaEvent;
        }
    }

    #endregion

    #endregion

    #region TypBiznisEntity

    #region Operations

    // List
    [Route("/cis/ListTypBiznisEntity", "GET")]
    [Api("Reg")]
    [DataContract]
    public class ListTypBiznisEntity : BaseListDto
    {
    }

    #endregion

    #region DTO

    [DataContract]
    public class TypBiznisEntityDto : BaseDto<TypBiznisEntity>
    {
        // + Id column in update and delete
        [DataMember]
        [Required]
        public int C_StavovyPriestor_Id { get; set; }

        [DataMember]
        [Required]
        public string Nazov { get; set; }

        /// <summary>
        /// Binds to entity.
        /// </summary>
        /// <param name="data"></param>
        protected override void BindToEntity(TypBiznisEntity data)
        {
            data.C_StavovyPriestor_Id = this.C_StavovyPriestor_Id;
            data.Nazov = this.Nazov;
        }
    }

    #endregion

    #endregion
}
