using ServiceStack.DataAnnotations;
using System;
using System.Linq;
using System.Runtime.Serialization;
using WebEas.ServiceModel;

namespace WebEas.Esam.ServiceModel.Office.Rzp.Types
{
    [Schema("rzp")]
    [Alias("C_RzpPol")]
    [DataContract]
    public class RzpPol : BaseTenantEntity, IPfeCustomize
    {
        [PrimaryKey]
        [AutoIncrement]
        [DataMember]
        public long C_RzpPol_Id { get; set; }

        [DataMember]
        [PfeColumn(Text = "_C_FRZdroj_Id")]
        public long? C_FRZdroj_Id { get; set; }

        [DataMember]
        [PfeColumn(Text = "_C_FRFK_Id")]
        public int? C_FRFK_Id { get; set; }

        [DataMember]
        [PfeColumn(Text = "_C_FREK_Id")]
        public int? C_FREK_Id { get; set; }

        [DataMember]
        [PfeColumn(Text = "A1")]
        public string A1 { get; set; }

        [DataMember]
        [PfeColumn(Text = "A2")]
        public string A2 { get; set; }

        [DataMember]
        [PfeColumn(Text = "A3")]
        public string A3 { get; set; }

        [DataMember]
        [PfeColumn(Text = "Názov", Mandatory = true)]
        public string RzpNazov { get; set; }

        [DataMember]
        [PfeColumn(Text = "Platnosť od", Type = PfeDataType.Date, Mandatory = true)]
        public DateTime PlatnostOd { get; set; }

        [DataMember]
        [PfeColumn(Text = "Platnosť do", Type = PfeDataType.Date)]
        public DateTime? PlatnostDo { get; set; }

        [DataMember]
        [PfeColumn(Text = "_Príjem/Výdaj")]
        public byte PrijemVydaj { get; set; }

        [DataMember]
        [PfeColumn(Text = "Stredisko")]
        public bool Stredisko { get; set; }

        [DataMember]
        [PfeColumn(Text = "Projekt")]
        public bool Projekt { get; set; }

        [DataMember]
        [PfeColumn(Text = "Účtovať na opačnej strane")]
        public bool OpacnaStrana { get; set; }

        public void CustomizeModel(PfeDataModel model, IWebEasRepositoryBase repository, HierarchyNode node, string filter)
        {
            if (model.Fields != null)
            {
                model.Fields.FirstOrDefault(p => p.Name == "A1").Text = ((ServiceInterface.Office.RepositoryBase)repository).GetNastavenieS("rzp", "A1Popis");
                model.Fields.FirstOrDefault(p => p.Name == "A2").Text = ((ServiceInterface.Office.RepositoryBase)repository).GetNastavenieS("rzp", "A2Popis");
                model.Fields.FirstOrDefault(p => p.Name == "A3").Text = ((ServiceInterface.Office.RepositoryBase)repository).GetNastavenieS("rzp", "A3Popis");
            }
        }
    }
}
