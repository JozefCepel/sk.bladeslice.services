using ServiceStack.DataAnnotations;

namespace WebEas.Esam.ServiceModel.Office.Types.Crm
{
    public class FaHelperExt : FaHelper 
    {
        public short Rok { get; set; }
        public string SS { get; set; }
        public string KS { get; set; }
        public long? D_OsobaBankaUcet_Id { get; set; }
        public string CisloInterne { get; set; }
    }
}
