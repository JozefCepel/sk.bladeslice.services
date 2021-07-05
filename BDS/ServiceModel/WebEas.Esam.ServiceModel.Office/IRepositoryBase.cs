using System;
using System.Collections.Generic;
using WebEas.Esam.ServiceModel.Office.Dto;
using WebEas.Esam.ServiceModel.Office.Types.Reg;
using WebEas.ServiceModel;
using WebEas.ServiceModel.Dto;
using WebEas.ServiceModel.Office.Egov.Reg.Types;

namespace WebEas.Esam.ServiceModel.Office
{
    public interface IRepositoryBase : IWebEasRepositoryBase
    {
        /// <summary>
        /// Changes the state.
        /// </summary>
        /// <param name="state">The state.</param>
        void ChangeState(IChangeState state);

        /// <summary>
        /// Changes the state.
        /// </summary>
        /// <typeparam name="T">The type of the T.</typeparam>
        /// <param name="entityId">The entity id.</param>
        /// <param name="newStateId">The new state id.</param>
        /// <param name="stavovyPriestor">Stavovy priestor v ktorom sa to ma skontrolovat</param>
        /// <returns>povodny stav entity</returns>
        int ChangeState<T>(long entityId, int newStateId, int stavovyPriestor) where T : class, IBaseEntity, IHasStateId;

        /// <summary>
        /// Get counts to be displayed in tree node with specified code (kod polozky)
        /// </summary>
        List<TreeNodeCount> GetTreeCounts(IGetTreeCounts treeCounts);

        #region Long operation

        LongOperationStatus LongOperationStart(LongOperationStartDtoBase request);
        LongOperationStatus LongOperationRestart(string processKey);
        LongOperationStatus LongOperationStatus(string processKey);
        object LongOperationResult(string processKey);
        LongOperationStatus LongOperationCancel(string processKey, bool notRemove);
        LongOperationListResult LongOperationList(LongOperationListDtoBase request);

        #endregion

        string GetTypBiznisEntityNastav(TypBiznisEntityEnum typBiznisEntity, LokalitaEnum lokalita);

        #region Doklady

        void SetCislovanie();

        void DeleteDoklad<T>(long[] id) where T : class, IBaseEntity;

        #endregion

        List<TypBiznisEntityNastavView> GetTypBiznisEntityNastavView();

        string FormatujUcet(string ucet, string fmt);

        void OdstranitFormatovanieUctuFiltra(string ucet, ref Filter filter);
        PostResultResponse<BiznisEntitaDokladView> CopyDoklad(CopyDokladDto request);
        BiznisEntita_ZalohaView CreateBiznisEntita_Zaloha(BiznisEntita_ZalohaDto data);
        BiznisEntita_ZalohaView UpdateBiznisEntita_Zaloha(BiznisEntita_ZalohaDto data);
    }
}