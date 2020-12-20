using ServiceStack.DataAnnotations;

namespace WebEas.Esam.ServiceModel.Office.Types.Crm
{
    public class FaHelper
    {
        [AutoIncrement]
        [PrimaryKey]
        public long D_BiznisEntita_Id { get; set; }

        public short C_TypBiznisEntity_Id { get; set; }

        public decimal? DM_Neuhradene { get; set; }

        public decimal DM_Suma { get; set; }

        public string VS { get; set; }

        public long? D_Osoba_Id { get; set; }
    }
}
