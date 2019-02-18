using ServiceStack;
using System.Runtime.Serialization;
using WebEas.ServiceModel;
using WebEas.ServiceModel.Office.Egov.Reg.Types;

namespace WebEas.Esam.ServiceModel.Office.Reg.Dto
{
    [WebEasRequiresAnyRole(WebEas.ServiceModel.Office.Egov.Reg.Roles.RegWriter)]
    [DataContract]
    [Route("/cis/CreateLoggingConfig", "POST")]
    [Api("Reg")]
    public class CreateLoggingConfig : LoggingConfigDto { }

    [WebEasRequiresAnyRole(WebEas.ServiceModel.Office.Egov.Reg.Roles.RegWriter)]
    [DataContract]
    [Route("/cis/UpdateLoggingConfig", "PUT")]
    [Api("Reg")]
    public class UpdateLoggingConfig : LoggingConfigDto
    {
        [ServiceStack.DataAnnotations.PrimaryKey]
        [DataMember(IsRequired = true)]
        public int C_LoggingConfig_Id { get; set; }
    }

    [WebEasRequiresAnyRole(WebEas.ServiceModel.Office.Egov.Reg.Roles.RegWriter)]
    [DataContract]
    [Route("/cis/DeleteLoggingConfig", "DELETE")]
    [Api("Reg")]
    public class DeleteLoggingConfig
    {
        [ServiceStack.DataAnnotations.PrimaryKey]
        [DataMember(IsRequired = true)]
        public int C_LoggingConfig_Id { get; set; }
    }

    #region DTO

    [DataContract]
    public class LoggingConfigDto : BaseDto<LoggingConfig>
    {
        // + Id column in update and delete

        [DataMember]
        public string Schema { get; set; }

        [DataMember]
        public string NazovTabulky { get; set; }

        [DataMember]
        public string NazovStlpca { get; set; }

        [DataMember]
        public string TypStlpca { get; set; }

        [DataMember]
        public string PopisZmeny { get; set; }

        /// <summary>
        /// Binds to entity.
        /// </summary>
        /// <param name="data"></param>
        protected override void BindToEntity(LoggingConfig data)
        {
            data.Schema = this.Schema;
            data.NazovTabulky = this.NazovTabulky;
            data.NazovStlpca = this.NazovStlpca;
            data.TypStlpca = this.TypStlpca;
            data.PopisZmeny = this.PopisZmeny;
        }
    }

    #endregion
}
