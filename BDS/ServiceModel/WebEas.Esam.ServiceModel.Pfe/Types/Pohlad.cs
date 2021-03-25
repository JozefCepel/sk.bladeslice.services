using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text.RegularExpressions;
using ServiceStack.DataAnnotations;
using ServiceStack.Model;

namespace WebEas.ServiceModel.Pfe.Types
{
    [Schema("pfe")]
    [Alias("D_Pohlad")]
    [TenantUpdatable]
    [DataContract]
    public class Pohlad : BaseEntity, IHasId<int>, IPohlad, IValidate
    {
        [AutoIncrement]
        [PrimaryKey]
        [DataMember]
        [Alias("D_Pohlad_Id")]
        public int Id { get; set; }

        [DataMember]
        public string Nazov { get; set; }

        [DataMember]
        public string Typ { get; set; }

        /* Dohoda s Ludkou aby sa necistil kod polozky - zatial tu necham keby nieco
         * private string kodPolozky { get; set; }
         * [DataMember]
        public string KodPolozky
        {
            get
            {
                return this.kodPolozky;
            }
            set
            {
                this.kodPolozky = CleanCode(value);
            }
        }*/

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
        public string TypAkcie { get; set; }

        [DataMember]
        public int ViewSharing { get; set; }

        [DataMember]
        public int PageSize { get; set; }

        [DataMember]
        public string RibbonFilters { get; set; }

        [DataMember]
        public Guid? D_Tenant_Id { get; set; }

        /// <summary>
        /// Cleans the code.
        /// </summary>
        /// <param name="code">The code.</param>
        /// <returns></returns>
        public static string CleanCode(string code)
        {
            if (string.IsNullOrEmpty(code))
            {
                return code;
            }

            string ret = code.Contains('!') ? Regex.Replace(code, "(![^-]*)", "") : code;

            if (ret.StartsWith("dms-dok") &&
                !ret.StartsWith("dms-dok-dod-") &&
                !ret.StartsWith("dms-dok-odb-") &&
                !ret.StartsWith("dms-dok-fin-") &&
                !ret.StartsWith("dms-dok-uct-") &&
                !ret.StartsWith("dms-dok-osa-"))
            {
                ret = "dms-dok";
            }
            return ret;
        }

        /// <summary>
        /// Validovanie dat pred insertom a updatom
        /// </summary>
        public void Validate()
        {
            //CK_D_Pohlad_ViewSharing	([ViewSharing]=(2) OR [ViewSharing]=(1) OR [ViewSharing]=(0))
            if (!(new List<int> { 0, 1, 2 }).Contains(this.ViewSharing))
            {
                throw new WebEasValidationException("CK_D_Pohlad_ViewSharing - ([ViewSharing]=(2) OR [ViewSharing]=(1) OR [ViewSharing]=(0))", "Nepovolené hodnoty zdieľania!");
            }
        }
    }

    [Schema("pfe")]
    [Alias("D_Pohlad")]
    [TenantUpdatable]
    [DataContract]
    public class PohladLockModel : BaseEntity, IHasId<int>
    {
        [AutoIncrement]
        [PrimaryKey]
        [DataMember]
        [Alias("D_Pohlad_Id")]
        public int Id { get; set; }

        [DataMember]
        [Compute]
        public string Nazov { get; set; }

        [DataMember]
        [Compute]
        public string Typ { get; set; }

        [DataMember]
        [Compute]
        public string Data { get; set; }

        [DataMember]
        [Compute]
        public string KodPolozky { get; set; }

        [DataMember]
        [Compute]
        public int ViewSharing { get; set; }

        [DataMember]
        public bool Zamknuta { get; set; }

        [DataMember]
        public Guid? D_Tenant_Id { get; set; }
    }
}