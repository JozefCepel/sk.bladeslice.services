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
        /// Gets the db environment.
        /// </summary>
        /// <value>The db environment.</value>
        string DbEnvironment { get; }

        /// <summary>
        /// Gets the db deploy time.
        /// </summary>
        /// <value>The db deploy time.</value>
        DateTime? DbDeployTime { get; }

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
        List<LongOperationStatus> LongOperationList(bool perTenant, int skip, int take);

        #endregion

        #region Nastavenie

        /// <summary>
        /// Update nastavenia v aktuálnom module
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        WebEas.ServiceModel.Types.NastavenieView UpdateNastavenie(UpdateNastavenieBase updateNastavenie);

        /// <summary>
        /// Update nastavenie do konkrétneho modulu
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        WebEas.ServiceModel.Types.NastavenieView UpdateNastavenie(UpdateNastavenieBase updateNastavenie, string explicitModul);

        /// <summary>
        /// Vráti hodnotu používateľského alebo tenantského nastavenia podľa zadaných kritérií.
        /// Ak sa dopytuje na používateľské nastavenie ktoré používateľ nemá zadané, použije sa príslušné tenantské. Ak nastavenie nie je zadané vôbec, vráti sa preddefinovaná hodnota.
        /// </summary>
        /// <param name="modul">Kód modulu</param>
        /// <param name="kod">Kód nastavenia</param>
        /// <returns>Boolean</returns>
        bool GetNastavenieB(string modul, string kod);

        /// <summary>
        /// Vráti hodnotu používateľského alebo tenantského nastavenia podľa zadaných kritérií.
        /// Ak sa dopytuje na používateľské nastavenie ktoré používateľ nemá zadané, použije sa príslušné tenantské. Ak nastavenie nie je zadané vôbec, vráti sa preddefinovaná hodnota.
        /// </summary>
        /// <param name="modul">Kód modulu</param>
        /// <param name="kod">Kód nastavenia</param>
        /// <returns>Long</returns>
        long GetNastavenieI(string modul, string kod);

        /// <summary>
        /// Vráti hodnotu používateľského alebo tenantského nastavenia podľa zadaných kritérií.
        /// Ak sa dopytuje na používateľské nastavenie ktoré používateľ nemá zadané, použije sa príslušné tenantské. Ak nastavenie nie je zadané vôbec, vráti sa preddefinovaná hodnota.
        /// </summary>
        /// <param name="modul">Kód modulu</param>
        /// <param name="kod">Kód nastavenia</param>
        /// <returns>String</returns>
        string GetNastavenieS(string modul, string kod);

        /// <summary>
        /// Vráti hodnotu používateľského alebo tenantského nastavenia podľa zadaných kritérií.
        /// Ak sa dopytuje na používateľské nastavenie ktoré používateľ nemá zadané, použije sa príslušné tenantské. Ak nastavenie nie je zadané vôbec, vráti sa preddefinovaná hodnota.
        /// </summary>
        /// <param name="modul">Kód modulu</param>
        /// <param name="kod">Kód nastavenia</param>
        /// <returns>String</returns>
        DateTime? GetNastavenieD(string modul, string kod);

        string GetTypBiznisEntityNastav(TypBiznisEntityEnum typBiznisEntity, LokalitaEnum lokalita);

        #endregion

        #region Doklady

        void SetCislovanie();

        void DeleteDoklad<T>(long[] id) where T : class, IBaseEntity;

        #endregion

        List<TypBiznisEntityNastavView> GetTypBiznisEntityNastavView();

        string FormatujUcet(string ucet, string fmt);

        void OdstranitFormatovanieUctuFiltra(string ucet, ref Filter filter);
    }
}