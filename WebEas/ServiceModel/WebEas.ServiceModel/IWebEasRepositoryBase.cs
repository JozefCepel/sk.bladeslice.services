using ServiceStack.OrmLite;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using WebEas.Core;
using WebEas.ServiceModel.Types;

namespace WebEas.ServiceModel
{
    public interface IWebEasRepositoryBase : IWebEasCoreRepositoryBase
    {
        /// <summary>
        /// Gets the record.
        /// </summary>
        /// <typeparam name="T">The type of the T.</typeparam>
        /// <param name="id">The id.</param>
        T GetRecord<T>(object id, params string[] columns) where T : class;

        /// <summary>
        /// Gets the by id filled.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <returns></returns>
        IBaseEntity GetByIdFilled(IDto data);

        /// <summary>
        /// Gets the by id filled.
        /// </summary>
        /// <typeparam name="T">The type of the T.</typeparam>
        /// <param name="data">The data.</param>
        /// <param name="id">The id.</param>
        /// <returns></returns>
        T GetByIdFilled<T>(IDto data, object id) where T : class, IBaseEntity;

        /// <summary>
        /// Gets the list.
        /// </summary>
        /// <typeparam name="T">The type of the T.</typeparam>
        /// <param name="filter">The filter.</param>
        /// <param name="pagging">The pagging.</param>
        /// <returns></returns>
        List<T> GetList<T>(Filter filter = null, PaggingParameters pagging = null) where T : class;

        /// <summary>
        /// Gets the list.
        /// </summary>
        /// <typeparam name="T">The type of the T.</typeparam>
        /// <param name="filter">The filter.</param>
        /// <returns></returns>
        List<T> GetList<T>(Filter filter,
                           PaggingParameters pagging,
                           List<PfeSortAttribute> userSort = null,
                           List<string> hiddenFields = null,
                           List<string> selectedFields = null,
                           Filter ribbonFilter = null,
                           HierarchyNode node = null) where T : class;

        /// <summary>
        /// Gets the list.
        /// </summary>
        /// <typeparam name="T">The type of the T.</typeparam>
        /// <param name="filter">The filter.</param>
        /// <returns></returns>
        List<T> GetList<T>(Expression<Func<T, bool>> filter) where T : class;

        /// <summary>
        /// Gets the list.
        /// </summary>
        /// <typeparam name="T">The type of the T.</typeparam>
        /// <param name="filter">The filter.</param>
        /// <returns></returns>
        List<T> GetList<T>(BaseListDto listDto, HierarchyNode node = null, PaggingParameters pagging = null) where T : class;

        /// <summary>
        /// Gets the list using an sqlexpression.
        /// </summary>
        List<T> GetList<T>(SqlExpression<T> expression) where T : class;

        /// <summary>
        /// Gets the list combo.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        List<object> GetListCombo(IListComboDto request);

        /// <summary>
        /// Deletes the entity.
        /// </summary>
        /// <typeparam name="T">The type of the entity.</typeparam>
        int Delete<T>(params object[] id) where T : class, IBaseEntity;

        /// <summary>
        /// Creates the ciselnik row.
        /// </summary>
        /// <typeparam name="T">The type of the T.</typeparam>
        /// <param name="obj">The obj.</param>
        /// <returns></returns>
        long Create<T>(T obj) where T : class, IBaseEntity;

        /// <summary>
        /// Creates the specified data.
        /// </summary>
        /// <typeparam name="T">The type of the T.</typeparam>
        /// <param name="data">The data.</param>
        /// <returns></returns>
        T Create<T>(IDto data)
            where T : class, IBaseEntity, new();

        /// <summary>
        /// Updates the ciselnik row.
        /// </summary>
        /// <typeparam name="T">The type of the T.</typeparam>
        /// <param name="obj">The obj.</param>
        int Update<T>(T obj) where T : class, IBaseEntity;

        /// <summary>
        /// Updates the specified data.
        /// </summary>
        /// <typeparam name="T">The type of the T.</typeparam>
        /// <param name="data">The data.</param>
        /// <returns></returns>
        T Update<T>(IDto data) where T : class, IBaseEntity, new();

        /// <summary>
        /// Updates the only.
        /// </summary>
        /// <param name="data">The data.</param>
        void UpdateOnly(IDto data);

        /// <summary>
        /// Sets the access flag.
        /// </summary>
        /// <param name="viewData">The view data.</param>
        void SetAccessFlag(object viewData);

        /// <summary>
        /// Check if there is configured logging on some column in specified table (reg.C_LoggingConfig)
        /// </summary>
        bool IsTableLogged(string schema, string table);

        /// <summary>
        /// Check if there is configured logging on some column in specified tables via attribute list (reg.C_LoggingConfig)
        /// </summary>
        bool IsTableLogged(SourceTableAttribute[] list);
        
        /// <summary>
        /// Get list of changes for specified row Id in specified node
        /// </summary>
        /// <param name="code">Kod polozky</param>
        /// <param name="rowId">Unique row ID (source data primary key)</param>
        List<LoggingView> GetTableLogging(string code, long rowId);
        

        object GetRowDefaultValues(string code, string masterCode, string masterRowId);

        /// <summary>
        /// Renders root node for current module
        /// </summary>
        /// <param name="kodPolozky">Toto je iba pomocna info pri renderovani stromu. Vzdy vraciame cely strom, ale napr. na tuto konkretnu polozku ide req. pozri napr. <seealso cref="DapRepository.Modul.cs"/></param>
        /// <returns>Full HierarchyNode of module</returns>
        HierarchyNode RenderModuleRootNode(string kodPolozky);

        /// <summary>
        /// Gets the code.
        /// </summary>
        /// <value>The code.</value>
        string Code { get; }

        /// <summary>
        /// Ak je kodPolozky modul, vrati cely strom prav, inak vrati pravo na konkretnu polozku
        /// </summary>
        /// <param name="kodPolozky"></param>
        /// <returns></returns>
        List<UserNodeRight> GetUserTreeRights(string kodPolozky);

        /// <summary>
        /// Get row by primary key
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="id"></param>
        /// <param name="columns"></param>
        /// <returns></returns>
        T GetById<T>(object id, params string[] columns) where T : class;

        /// <summary>
        /// Invalidate tree counts for given (sub)path
        /// If null/empty string is passed, all counts will be invalidated (in all modules)
        /// </summary>
        /// <param name="path">path</param>
        void InvalidateTreeCountsForPath(string path);

        #region Nastavenie

        /// <summary>
        /// Update nastavenia v aktuálnom module
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        WebEas.ServiceModel.Types.NastavenieView UpdateNastavenie(Dto.UpdateNastavenieBase updateNastavenie);

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
        #endregion
    }
}