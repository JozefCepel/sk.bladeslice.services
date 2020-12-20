using ServiceStack;
using System;
using System.Linq;
using System.Runtime.Serialization;

namespace WebEas.ServiceModel
{
    [DataContract]
    public class TenantFilter : ITenantFilter
    {
        [DataMember(Name = "tenantId")]
        [ApiMember(Name = "TenantId")]
        public string TenantId { get; set; }
    }

    [DataContract]
    public class TenantLocaleFilter : ITenantFilter, ILocale
    {
        [DataMember(Name = "tenantId")]
        [ApiMember(Name = "TenantId")]
        public string TenantId { get; set; }

        [DataMember(Name = "locale")]
        [ApiMember(Name = "locale")]
        public string Locale { get; set; }
    }

    [DataContract]
    public class LocaleFilter : ILocale
    {
        [DataMember(Name = "locale")]
        [ApiMember(Name = "locale")]
        public string Locale { get; set; }
    }

    [DataContract]
    public class ZdvLocaleFilter : IZdvLocale
    {
        [DataMember(Name = "locale")]
        [ApiMember(Name = "locale")]
        public string Locale { get; set; }
    }

    #region S NAZVOM
    public class ZoznamNazvov
    {
        [DataMember]
        public virtual string Nazov { get; set; }
    }

    public class ZoznamNazvovTenantNullable : ZoznamNazvov, ITenantEntityNullable
    {
        [IgnoreDataMember]
        public virtual Guid? D_Tenant_Id { get; set; }
    }

    public class ZoznamNazvovTenant : ZoznamNazvov, ITenantEntity
    {
        [IgnoreDataMember]
        public virtual Guid D_Tenant_Id { get; set; }
    }

    public class ZoznamNazvovCasovaPlatnost : ICasovaPlatnost
    {
        [DataMember]
        public virtual string Nazov { get; set; }
        [IgnoreDataMember]
        public virtual DateTime? PlatnostOd { get; set; }
        [IgnoreDataMember]
        public virtual DateTime? PlatnostDo { get; set; }
    }

    public class ZoznamNazvovCasovaPlatnostTenantNullable : ITenantEntityNullable, ICasovaPlatnost
    {
        [DataMember]
        public virtual string Nazov { get; set; }
        [IgnoreDataMember]
        public virtual Guid? D_Tenant_Id { get; set; }
        [IgnoreDataMember]
        public virtual DateTime? PlatnostOd { get; set; }
        [IgnoreDataMember]
        public virtual DateTime? PlatnostDo { get; set; }
    }

    public class ZoznamNazvovCasovaPlatnostTenant : ITenantEntity, ICasovaPlatnost
    {
        [DataMember]
        public virtual string Nazov { get; set; }
        [IgnoreDataMember]
        public virtual Guid D_Tenant_Id { get; set; }
        [IgnoreDataMember]
        public virtual DateTime? PlatnostOd { get; set; }
        [IgnoreDataMember]
        public virtual DateTime? PlatnostDo { get; set; }
    }
    #endregion

    #region BEZ NAZVU
    public class ZoznamCasovaPlatnost : ICasovaPlatnost
    {
        [IgnoreDataMember]
        public virtual DateTime? PlatnostOd { get; set; }
        [IgnoreDataMember]
        public virtual DateTime? PlatnostDo { get; set; }
    }

    public class ZoznamCasovaPlatnostTenantNullable : ICasovaPlatnost, ITenantEntityNullable
    {
        [IgnoreDataMember]
        public virtual Guid? D_Tenant_Id { get; set; }
        [IgnoreDataMember]
        public virtual DateTime? PlatnostOd { get; set; }
        [IgnoreDataMember]
        public virtual DateTime? PlatnostDo { get; set; }
    }

    public class ZoznamTenantNullable : ITenantEntityNullable
    {
        [IgnoreDataMember]
        public virtual Guid? D_Tenant_Id { get; set; }
    }

    public class ZoznamCasovaPlatnostTenant : ICasovaPlatnost, ITenantEntity
    {
        [IgnoreDataMember]
        public virtual Guid D_Tenant_Id { get; set; }
        [IgnoreDataMember]
        public virtual DateTime? PlatnostOd { get; set; }
        [IgnoreDataMember]
        public virtual DateTime? PlatnostDo { get; set; }
    }

    public class ZoznamTenant : ITenantEntity
    {
        [IgnoreDataMember]
        public virtual Guid D_Tenant_Id { get; set; }
    }
    #endregion

}