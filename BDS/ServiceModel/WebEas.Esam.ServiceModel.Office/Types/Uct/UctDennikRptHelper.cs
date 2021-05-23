using ServiceStack.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using WebEas.ServiceModel;

namespace WebEas.Esam.ServiceModel.Office.Types.Uct
{
    [DataContract]
    [Schema("uct")]
    [Alias("V_UctDennik")]
    public class UctDennikRptHelper : BaseTenantEntity, IBaseView, IAfterGetList, IBeforeGetList
    {
        [DataMember]
        [PrimaryKey]
        [AutoIncrement]
        public long D_UctDennik_Id { get; set; }

        [DataMember]
        public byte? UOMesiac { get; set; }

        [DataMember]
        public int C_TypBiznisEntity_Kniha_Id { get; set; }

        [DataMember]
        public string Kniha { get; set; }

        [DataMember]
        public short Rok { get; set; }

        [DataMember]
        public long D_BiznisEntita_Id { get; set; }

        [DataMember]
        public string BiznisEntitaPopis { get; set; }

        [DataMember]
        public string URL { get; set; }

        [DataMember]
        public short C_TypBiznisEntity_Id { get; set; }

        [DataMember]
        public long? D_DokladBANPol_Id { get; set; }

        [DataMember]
        public long? D_UhradaParovanie_Id { get; set; }

        //[DataMember]
        //public string SDK { get; set; }

        //[DataMember]
        //public string SU { get; set; }

        [DataMember]
        public bool U { get; set; }

        //[DataMember]
        //public long? D_Osoba_Id { get; set; }

        [DataMember]
        public string VS { get; set; }

        [DataMember]
        public string CisloInterne { get; set; }

        [DataMember]
        public DateTime DatumDokladu { get; set; }

        [DataMember]
        public DateTime DatumUctovania { get; set; }

        [DataMember]
        public long? C_UctRozvrh_Id { get; set; }

        [DataMember]
        public string RozvrhUcet { get; set; }

        [DataMember]
        public string RozvrhUcetNazov { get; set; }

        [DataMember]
        public int Poradie { get; set; }

        [DataMember]
        public decimal SumaMD { get; set; }

        [DataMember]
        public decimal SumaDal { get; set; }

        [DataMember]
        public string Popis { get; set; }

        [DataMember]
        public int? C_Stredisko_Id { get; set; }

        [DataMember]
        public long? C_Projekt_Id { get; set; }

        //[DataMember]
        //public long? C_UctKluc_Id1 { get; set; }

        //[DataMember]
        //public long? C_UctKluc_Id2 { get; set; }

        //[DataMember]
        //public long? C_UctKluc_Id3 { get; set; }

        //[DataMember]
        //public int? C_Typ_Id { get; set; }

        //[DataMember]
        //public long? D_VymerPol_Id { get; set; }

        [DataMember]
        public int C_StavEntity_Id { get; set; }

        [DataMember]
        public string StrediskoKod { get; set; }

        [DataMember]
        public string StrediskoNazov { get; set; }

        [DataMember]
        public bool PodnCinn { get; set; }

        [DataMember]
        public string ProjektKod { get; set; }

        [DataMember]
        public string ProjektNazov { get; set; }

        [DataMember]
        public string VytvorilMeno { get; set; }

        [DataMember]
        public string ZmenilMeno { get; set; }

        public void AfterGetList<T>(IWebEasRepositoryBase repository, ref List<T> data, Filter filter)
        {
            //Identický kód v UctDennikView
            string fmt = repository.GetNastavenieS("uct", "FormatUctuVRozvrhu");

            foreach (var row in data as List<UctDennikRptHelper>)
            {
                row.RozvrhUcet = ((IRepositoryBase)repository).FormatujUcet(row.RozvrhUcet, fmt);
                row.RozvrhUcetNazov = ((IRepositoryBase)repository).FormatujUcet(row.RozvrhUcetNazov, fmt);
            }
        }

        public void BeforeGetList(IWebEasRepositoryBase repository, HierarchyNode node, ref string sql, ref Filter filter, ref string sqlFromAlias, string sqlOrderPart)
        {
            BookFilterGenerator.UctDennik_BeforeGetList(repository, node, ref sql, ref filter, ref sqlFromAlias, sqlOrderPart);
        }
    }
}
