using ServiceStack;
using ServiceStack.DataAnnotations;
using System;
using System.Runtime.Serialization;
using WebEas.Esam.ServiceModel.Office.Rzp.Types;
using WebEas.ServiceModel;

namespace WebEas.Esam.ServiceModel.Office.Rzp.Dto
{
    // Create
    [WebEasRequiresAnyRole(RolesDefinition.Rzp.Roles.RzpMember)]
    [Route("/CreateNavrhZmenyRzp", "POST")]
    [Api("NavrhZmenyRzp")]
    [DataContract]
    public class CreateNavrhZmenyRzp : NavrhZmenyRzpDto { }

    // Update
    [WebEasRequiredRole(RolesDefinition.Rzp.Roles.RzpMember)]
    [Route("/UpdateNavrhZmenyRzp", "PUT")]
    [Api("NavrhZmenyRzp")]
    [DataContract]
    public class UpdateNavrhZmenyRzp : NavrhZmenyRzpDto
    {
        [PrimaryKey]
        [DataMember(IsRequired = true)]
        public long D_NavrhZmenyRzp_Id { get; set; }
    }

    // Delete
    [WebEasRequiresAnyRole(RolesDefinition.Rzp.Roles.RzpMember)]
    [Route("/DeleteNavrhZmenyRzp", "DELETE")]
    [Api("NavrhZmenyRzp")]
    [DataContract]
    public class DeleteNavrhZmenyRzp
    {
        [DataMember(IsRequired = true)]
        public long D_NavrhZmenyRzp_Id { get; set; }
    }

    [WebEasRequiresAnyRole(RolesDefinition.Rzp.Roles.RzpMember)]
    [Route("/PocetOdoslanychZaznamovKDatumu", "GET")]
    [Api("NavrhZmenyRzp")]
    [DataContract]
    public class PocetOdoslanychZaznamovKDatumu
    {
        [DataMember(IsRequired = true)]
        public DateTime Datum { get; set; }
    }

    [WebEasRequiresAnyRole(RolesDefinition.Rzp.Roles.RzpMember)]
    [Route("/PrevziatNavrhRozpoctu", "POST")]
    [Api("NavrhZmenyRzp")]
    public class PrevziatNavrhRozpoctuDto
    {
        [ApiMember(IsRequired = true)]
        public long D_NavrhZmenyRzp_Id { get; set; }

        [ApiMember(IsRequired = true, Description = "1=Schválený rozpočet (@Rok-1), 2=Upravený rozpočet (@Rok-1), 3=Čerpanie/Plnenie (@Rok-1), 4=Návrh (@Rok), 5=Návrh (@Rok+1), 6=Nulové hodnoty")]
        public byte Navrh { get; set; }

        [ApiMember(IsRequired = true, Description = "1=Schválený rozpočet (@Rok-1), 2=Upravený rozpočet (@Rok-1), 3=Čerpanie/Plnenie (@Rok-1), 4=Návrh (@Rok), 5=Návrh (@Rok+1), 6=Nulové hodnoty")]
        public byte Rok1 { get; set; }

        [ApiMember(IsRequired = true, Description = "1=Schválený rozpočet (@Rok-1), 2=Upravený rozpočet (@Rok-1), 3=Čerpanie/Plnenie (@Rok-1), 4=Návrh (@Rok), 5=Návrh (@Rok+1), 6=Nulové hodnoty")]
        public byte Rok2 { get; set; }

        [ApiMember(IsRequired = true, Description = "Pre položky existujúce v návrhu (@Rok) aktualizovať hodnoty")]
        public bool AktualizovatHodnoty { get; set; }

        [ApiMember(IsRequired = true, Description = "Odstrániť položky, ktoré sú v návrhu (@Rok) navyše")]
        public bool OdstranitPolozkyNavyse { get; set; }

        [ApiMember(IsRequired = true, Description = "Vynechať nulové riadky")]
        public bool VynechatNulove { get; set; }

    }

    #region DTO
    [DataContract]
    public class NavrhZmenyRzpDto : BaseDto<NavrhZmenyRzp>
    {
        [DataMember]
        public string Nazov { get; set; }

        [DataMember]
        public string Popis { get; set; }

        [DataMember]
        public DateTime Datum { get; set; }

        [DataMember]
        public Guid D_User_Id_Zodp { get; set; }

        [DataMember]
        public int Rok { get; set; }

        [DataMember]
        public bool Typ { get; set; }

        [DataMember]
        public string Uznesenie { get; set; }

        [DataMember]
        [NotEmptyOrDefault]
        public int C_StavEntity_Id { get; set; }

        /// <summary>
        /// Binds to entity.
        /// </summary>
        /// <param name="data"></param>
        protected override void BindToEntity(NavrhZmenyRzp data)
        {
            data.Nazov = Nazov;
            data.Popis = Popis;
            data.Datum = Datum;
            data.D_User_Id_Zodp = D_User_Id_Zodp;
            data.Rok = Rok;
            data.Typ = Typ;
            data.Uznesenie = Uznesenie;
            data.C_StavEntity_Id = C_StavEntity_Id;
        }
    }
    #endregion
}