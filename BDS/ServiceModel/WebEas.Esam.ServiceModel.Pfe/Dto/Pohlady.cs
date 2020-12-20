using ServiceStack;
using ServiceStack.DataAnnotations;
using System.Runtime.Serialization;
using WebEas.ServiceModel.Pfe.Types;

namespace WebEas.Esam.ServiceModel.Pfe.Dto
{
    #region CRUD

    // Create
    [Route("/createpohlad", "POST")]
    [Api("Vytvorenie nového pohľadu")]
    [DataContract]
    public class CreatePohlad : PohladDto, IReturn<PohladViewResponse>
    {
        /// <summary>
        /// copy from view - id zdrojoveho pohladu
        /// </summary>
        [DataMember(Name = "cfv")]
        public int? SourceId { get; set; }
    }

    // Update
    [Route("/updatepohlad", "PUT")]
    [Api("Úprava existujúceho pohľadu")]
    [DataContract]
    public class UpdatePohlad : PohladDto, IReturn<PohladViewResponse>
    {
        [PrimaryKey]
        [DataMember(IsRequired = true)]
        public long D_Pohlad_Id { get; set; }
    }
    
    // Delete
    [Route("/deletepohlad", "DELETE")]
    [Api("Zmazanie existujucého pohľadu")]
    
    [DataContract]
    public class DeletePohlad
    {
        [DataMember]
        public int[] Id { get; set; }
    }

    // Update
    [Route("/updatepohladcustom", "PUT")]
    [Api("Úprava customizácie existujúceho pohľadu")]
    [DataContract]
    public class UpdatePohladCustom : PohladDto, IReturn<PohladViewResponse>
    {
        [PrimaryKey]
        [DataMember(IsRequired = true)]
        public long D_Pohlad_Id { get; set; }
    }

    // Delete
    [Route("/deletepohladcustom", "DELETE")]
    [Api("Zmazanie existujúcej customizácie pohľadu")]
    [DataContract]
    public class DeletePohladCustom : IReturn<PohladViewResponse>
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public string KodPolozky { get; set; }
    }

    #endregion CRUD

    /// <summary>
    /// RS 3 - Získanie existujúcich pohľadov pre kód položky
    /// </summary>
    [Route("/pohlady/{KodPolozky}", "GET")]
    [Api("Získanie existujúcich pohľadov pre kód položky")]
    [DataContract]
    public class ListPohlady
    {
        [ApiMember(Name = "KodPolozky", Description = "Kód položky", DataType = "string", IsRequired = true)]
        [DataMember]
        public string KodPolozky { get; set; }
    }

    /// <summary>
    /// RS 3 - Získanie existujúcich pohľadov pre kód položky
    /// </summary>
    [Route("/pohladyWithDefault/{KodPolozky}", "GET")]
    [Route("/pohladyWithDefault/{KodPolozky}/{Id}", "GET")]
    [Route("/pohladyWithDefault/{KodPolozky}/{Id}/{Browser}", "GET")]
    [Api("Získanie existujúcich pohľadov pre kód položky")]    
    [DataContract]
    public class ListPohladyWithDefault
    {
        [ApiMember(Name = "KodPolozky", Description = "Kód položky", DataType = "string", IsRequired = true)]
        [DataMember]
        public string KodPolozky { get; set; }

        [ApiMember(Name = "Id", Description = "Id pohladu", DataType = "int", IsRequired = false)]
        [DataMember]
        public int? Id { get; set; }

        [ApiMember(Name = "Browser", Description = "Vráť pohľady, ktoré sú zaškrtnuté ako BrowserDialóg", DataType = "bool", IsRequired = false)]
        [DataMember]
        public bool? Browser { get; set; }

        [DataMember(Name = "filters")]
        public string Filter { get; set; }
    }

    [Route("/pohlad/{Id}", "GET")]
    [Route("/pohlad/{Id}/{KodPolozky}", "GET")]
    [Api("Získanie pohľadu")]
    [DataContract]
    public class GetPohlad  //: IReturn<Pohlad> - NEPLATÍ vracia rôzne response
    {
        [ApiMember(Name = "Id", Description = "Id pohladu", DataType = "int", IsRequired = true)]
        [DataMember]
        public int Id { get; set; }

        [DataMember(Name = "filters")]
        public string Filter { get; set; }

        [ApiMember(Name = "KodPolozky", Description = "Kód položky", DataType = "string", IsRequired = true)]
        [DataMember]
        public string KodPolozky { get; set; }
    }

    [Route("/pohlad", "POST")]
    [Api("Uloženie pohľadu")]
    [DataContract]
    public class SavePohlad : IReturn<Pohlad>
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public string Nazov { get; set; }

        [DataMember]
        public string Typ { get; set; }

        [DataMember]
        public string KodPolozky { get; set; }

        [DataMember]
        public bool ShowInActions { get; set; }

        [DataMember]
        public bool DefaultView { get; set; }

        [DataMember]
        public string Data { get; set; }

        [DataMember]
        public string FilterText { get; set; }

        [DataMember]
        public bool Zamknuta { get; set; }

        [DataMember]
        public bool Poznamka { get; set; }

        [DataMember]
        public string TypAkcie { get; set; }

        [DataMember]
        public int ViewSharing { get; set; }

        [DataMember]
        public int PageSize { get; set; }

        [DataMember]
        public string RibbonFilters { get; set; }
    }

    /// <summary>
    /// Gets the all dependencies for layout.
    /// </summary>
    [Route("/layout/{ItemCode}", "GET")]
    [Api("Získanie všetkých pohľadov pre zadané kódy položiek.")]
    [DataContract]
    public class ListLayoutDependencies
    {
        [DataMember]
        public string ItemCode { get; set; }

        [DataMember(Name = "linked")]
        public bool Linked { get; set; }
    }

    /// <summary>
    /// Gets the all dependencies for layout.
    /// </summary>
    [Route("/layout2/{ItemCode}", "GET")]
    [Api("Získanie všetkých pohľadov pre zadané kódy položiek.")]
    [DataContract]
    public class ListLayoutDependencies2
    {
        [DataMember]
        public string ItemCode { get; set; }
    }

    [DataContract]
    public class PohladDto : IReturn<Pohlad>
    {
        [DataMember(Name = "id")]
        public int Id { get; set; }

        [DataMember(Name = "nam")]
        public string Nazov { get; set; }

        [DataMember(Name = "typ")]
        public string Typ { get; set; }

        [DataMember(Name = "kod")]
        public string KodPolozky { get; set; }

        [DataMember(Name = "sia")]
        public bool? ShowInActions { get; set; }

        [DataMember(Name = "dvw")]
        public bool? DefaultView { get; set; }

        [DataMember(Name = "dat")]
        public string Data { get; set; }

        [DataMember(Name = "flt")]
        public string FilterText { get; set; }

        [DataMember(Name = "loc")]
        public bool Zamknuta { get; set; }

        [DataMember(Name = "pzn")]
        public bool Poznamka { get; set; }

        [DataMember(Name = "tpa")]
        public string TypAkcie { get; set; }

        [DataMember(Name = "vsh")]
        public int ViewSharing { get; set; }

        [DataMember(Name = "pgs")]
        public int? PageSize { get; set; }

        [DataMember(Name = "rbf")]
        public string RibbonFilters { get; set; }
    }

    [Route("/locklayout/{Id}", "GET")]
    [Api("Zamknutie pohladu")]
    public class LockLayoutRequest
    {
        [DataMember]
        public int Id { get; set; }
    }

    [Route("/unlocklayout/{Id}", "GET")]
    [Api("Odomknutie pohladu")]
    public class UnlockLayoutRequest
    {
        [DataMember]
        public int Id { get; set; }
    }
}