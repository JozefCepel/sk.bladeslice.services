using System.Collections.Generic;
using WebEas.Esam.ServiceModel.Office;
using WebEas.Esam.ServiceModel.Office.Dto;
using WebEas.Esam.ServiceModel.Office.Reg.Dto;
using WebEas.Esam.ServiceModel.Office.Types.Reg;
//using WebEas.Esam.ServiceModel.Urbis.Dto;
using WebEas.ServiceInterface;
using WebEas.ServiceModel;
using WebEas.ServiceModel.Office.Egov.Reg.Types;
using WebEas.ServiceModel.Reg.Types;

namespace WebEas.Esam.ServiceInterface.Office.Reg
{
    /// <summary>
    /// 
    /// </summary>
    public interface IRegRepository : IRepositoryBase,
        ISave<NasledovnyStavEntity>,
        ISave<ColumnTranslation>,
        ISave<LoggingConfig>
    {
        List<StavEntityView> GetListNaslStavEntity(int idStavEntity);
        void ResetLoggingCache();
        TypBiznisEntityNastavView UpdateTypBiznisEntityNastav(UpdateTypBiznisEntityNastav data);
        void RefreshDefaultTypBiznisEntityNastav(RefreshDefault data);
        List<DatabaseHierarchyNode> RenderCisTree(string code, DatabaseHierarchyNode staticData);
        StrediskoView CreateStredisko(CreateStredisko data);
        StrediskoView UpdateStredisko(UpdateStredisko data);
        void DeleteStredisko(DeleteStredisko request);
        PokladnicaView CreatePokladnica(CreatePokladnica data);
        PokladnicaView UpdatePokladnica(UpdatePokladnica data);
        void DeletePokladnica(DeletePokladnica data);
        BankaUcetView CreateBankaUcet(CreateBankaUcet data);
        BankaUcetView UpdateBankaUcet(UpdateBankaUcet data);
        void DeleteBankaUcet(DeleteBankaUcet data);
        CislovanieView UpdateCislovanie(UpdateCislovanie data);
        void DeleteCislovanie(int[] C_Cislovanie_Id);
        TypBiznisEntity_KnihaView CreateTypBiznisEntity_Kniha(CreateTypBiznisEntity_Kniha data);
        TypBiznisEntity_KnihaView UpdateTypBiznisEntity_Kniha(UpdateTypBiznisEntity_Kniha data);
        TypBiznisEntity_ParovanieDefView CreateTypBiznisEntity_ParovanieDef(CreateTypBiznisEntity_ParovanieDef data);
        TypBiznisEntity_ParovanieDefView UpdateTypBiznisEntity_ParovanieDef(UpdateTypBiznisEntity_ParovanieDef data);
        BiznisEntita_ParovanieView CreateBiznisEntita_Parovanie(CreateBiznisEntita_Parovanie data);
        BiznisEntita_ParovanieView UpdateBiznisEntita_Parovanie(UpdateBiznisEntita_Parovanie data);
        BiznisEntita_ZalohaView CreateBiznisEntita_Zaloha(CreateBiznisEntita_Zaloha data);
        BiznisEntita_ZalohaView UpdateBiznisEntita_Zaloha(UpdateBiznisEntita_Zaloha data);
        void UpdateZalohaInHead(long id, int typ, short rok);
        List<StrediskoView> GetListStredisko();
        List<DKLVratCisloResponseDto> DKLGetCislo(DKLVratCisloDto request);
    }
}