using System;
using ServiceStack;
using ServiceStack.DataAnnotations;
using System.Runtime.Serialization;
using WebEas.ServiceModel;
using WebEas.Esam.ServiceModel.Office.Bds.Types;

namespace WebEas.Esam.ServiceModel.Office.Bds.Dto
{
    // Create
    [WebEasRequiresAnyRole(RolesDefinition.Bds.Roles.BdsMember)]
    [Route("/CreateDefPrrProgramovyRozpocet", "POST")]
    [Api("DefPrrProgramovyRozpocet")]
    [DataContract]
    public class CreateDefPrrProgramovyRozpocet : DefPrrProgramovyRozpocetViewDto { }

    // Update
    [WebEasRequiredRole(Roles.Admin)]
    [Route("/UpdateDefPrrProgramovyRozpocet", "PUT")]
    [Api("DefPrrProgramovyRozpocet")]
    [DataContract]
    public class UpdateDefPrrProgramovyRozpocet : DefPrrProgramovyRozpocetViewDto
    {
        [ServiceStack.DataAnnotations.PrimaryKey]
        [DataMember(IsRequired = true)]
        public long D_Program_Id { get; set; }
    }

    // Delete
    [WebEasRequiresAnyRole(RolesDefinition.Bds.Roles.BdsMember)]
    [Route("/DeleteDefPrrProgramovyRozpocet", "DELETE")]
    [Api("DefPrrProgramovyRozpocet")]
    [DataContract]
    public class DeleteDefPrrProgramovyRozpocet
    {
        [DataMember(IsRequired = true)]
        public long D_Program_Id { get; set; }
    }

    #region DTO
    [DataContract]
    public class DefPrrProgramovyRozpocetViewDto : BaseDto<Program>
    {
        //[DataMember]
        //public long D_Program_Id { get; set; }

        //[DataMember]
        //public int? Rok { get; set; }

        [DataMember]
        public byte PRTyp { get; set; }

        //[DataMember]
        //public string PRTypMeno { get; set; }

        //[DataMember]
        //public long? D_Program_Id_Parent { get; set; }

        [DataMember]
        public short? program { get; set; }

        [DataMember]
        public short? podprogram { get; set; }

        [DataMember]
        public short? prvok { get; set; }

        //[DataMember]
        //public string PRKod { get; set; }

        [DataMember]
        public string PRNazov { get; set; }

        [DataMember]
        public string PRZamer { get; set; }

        [DataMember]
        public string PRPopis { get; set; }

        [DataMember]
        public Guid? D_User_Id_Zodp1 { get; set; }

        //[DataMember]
        //public string Zodpovedny1 { get; set; }

        [DataMember]
        public Guid? D_User_Id_Zodp2 { get; set; }

        //[DataMember]
        //public string Zodpovedny2 { get; set; }

        [DataMember]
        public string Komentar { get; set; }

        [DataMember]
        public DateTime PlatnostOd { get; set; }

        [DataMember]
        public DateTime? PlatnostDo { get; set; }

        //[DataMember]
        //public DateTime? DatumPlatnosti { get; set; }

        //audit stlpce
        //[DataMember]
        //public string VytvorilMeno { get; set; }

        //[DataMember]
        //public string ZmenilMeno { get; set; }

        /// <summary>
        /// Binds to entity.
        /// </summary>
        /// <param name="data"></param>
        protected override void BindToEntity(Program data)
        {

            //data.D_Program_Id = this.D_Program_Id;
            //data.Rok = this.Rok;
            data.PRTyp = this.PRTyp;
            //data.D_Program_Id_Parent = this.D_Program_Id_Parent;
            data.program = this.program;
            data.podprogram = this.podprogram;
            data.prvok = this.prvok;
            //data.PRKod = this.PRKod;
            data.PRNazov = this.PRNazov;
            data.PRZamer = this.PRZamer;
            data.PRPopis = this.PRPopis;
            data.D_User_Id_Zodp1 = this.D_User_Id_Zodp1;
            data.D_User_Id_Zodp2 = this.D_User_Id_Zodp2;
            data.Komentar = this.Komentar;
            data.PlatnostOd = this.PlatnostOd;
            data.PlatnostDo = this.PlatnostDo;
            
        }
    }
    #endregion
}



