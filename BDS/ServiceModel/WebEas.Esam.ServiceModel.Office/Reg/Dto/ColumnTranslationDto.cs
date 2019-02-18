using ServiceStack;
using ServiceStack.DataAnnotations;
using System;
using System.Runtime.Serialization;
using WebEas.ServiceModel;

namespace WebEas.Esam.ServiceModel.Office.Reg.Dto
{
    #region Translations

    [DataContract]
    [Route("/createtranslationdictionary", "POST")]
    [Api("Vytvoriť preklad")]
    [WebEasRequiredRole(WebEas.ServiceModel.Office.Egov.Reg.Roles.RegWriter)]
    public class CreateTranslationDictionary : ColumnTranslationDto
    {
    }

    [DataContract]
    [Route("/updatetranslationdictionary", "PUT")]
    [Api("Upraviť preklad")]
    [WebEasRequiredRole(WebEas.ServiceModel.Office.Egov.Reg.Roles.RegWriter)]
    public class UpdateTranslationDictionary : ColumnTranslationDto
    {
        [DataMember]
        [PrimaryKey]
        public long? D_PrekladovySlovnik_Id { get; set; }
    }

    #endregion

    #region DTO

    [DataContract]
    public class ColumnTranslationDto : BaseDto<ColumnTranslation>
    {
        [DataMember]
        public string ModulName { get; set; }

        [DataMember]
        public string TableName { get; set; }

        [DataMember]
        public string ColumnName { get; set; }

        [DataMember]
        public string Identifier { get; set; }

        [DataMember]
        public string PovodnaHodnota { get; set; }

        [DataMember]
        public string Cs { get; set; }

        [DataMember]
        public string De { get; set; }

        [DataMember]
        public string En { get; set; }

        [DataMember]
        public string Hu { get; set; }

        [DataMember]
        public string Pl { get; set; }

        [DataMember]
        public string Uk { get; set; }

        [DataMember]
        public string Rom { get; set; }

        [DataMember]
        public string Rue { get; set; }

        [DataMember]
        public string UniqueIdentifier { get; set; }

        [DataMember]
        public Guid? D_Tenant_Id { get; set; }

        protected override void BindToEntity(ColumnTranslation data)
        {
            data.ModulName = this.ModulName;
            data.TableName = this.TableName;
            data.ColumnName = this.ColumnName;
            data.Identifier = this.Identifier;
            data.Cs = string.IsNullOrWhiteSpace(this.Cs) ? null : this.Cs;
            data.De = string.IsNullOrWhiteSpace(this.De) ? null : this.De;
            data.En = string.IsNullOrWhiteSpace(this.En) ? null : this.En;
            data.Hu = string.IsNullOrWhiteSpace(this.Hu) ? null : this.Hu;
            data.Pl = string.IsNullOrWhiteSpace(this.Pl) ? null : this.Pl;
            data.Uk = string.IsNullOrWhiteSpace(this.Uk) ? null : this.Uk;
            data.Rom = string.IsNullOrWhiteSpace(this.Rom) ? null : this.Rom;
            data.Rue = string.IsNullOrWhiteSpace(this.Rue) ? null : this.Rue;
            data.D_Tenant_Id = this.D_Tenant_Id;
        }
    }

    #endregion
}
