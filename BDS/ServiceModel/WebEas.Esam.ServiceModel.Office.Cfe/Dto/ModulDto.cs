using ServiceStack;
using ServiceStack.DataAnnotations;
using System;
using System.Runtime.Serialization;
using WebEas.Esam.ServiceModel.Office.Cfe.Types;
using WebEas.ServiceModel;

namespace WebEas.Esam.ServiceModel.Office.Cfe.Dto
{
    // RefreshModuleTree
    [Route("/RefreshModuleTree", "POST")]
    [Api("Modul")]
    [DataContract]
    public class RefreshModuleTree
    {
        [DataMember(IsRequired = true)]
        public string IdField { get; set; }

        [DataMember(IsRequired = true)]
        public string[] IDs { get; set; }
    }

    #region DTO
    [DataContract]
    public class ModulDto : BaseDto<Modul>
    {
        [DataMember]
        public string Kod { get; set; }

        [DataMember]
        public string Nazov { get; set; }

        [DataMember]
        public string BasicUrl { get; set; }

        [DataMember]
        public string TreeJson { get; set; }

        /// <summary>
        /// Binds to entity.
        /// </summary>
        /// <param name="data"></param>
        protected override void BindToEntity(Modul data)
        {
            data.Kod = Kod;
            data.Nazov = Nazov;
            data.BasicUrl = BasicUrl;
            data.TreeJson = TreeJson;
        }
    }
    #endregion
}
