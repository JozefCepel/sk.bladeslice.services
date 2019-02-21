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
        /// <returns></returns>
        T GetRecord<T>(object id) where T : class;

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
        List<T> GetList<T>(Filter filter, PaggingParameters pagging, List<PfeSortAttribute> userSort = null, List<string> columnsWithData = null, Filter ribbonFilter = null) where T : class;

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
        /// Gets the list combo.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        List<ComboResult> GetListCombo(IListComboDto request);

        /// <summary>
        /// Gets the list combo.
        /// </summary>
        /// <param name="modelType">Type of the model.</param>
        /// <param name="col">The col.</param>
        /// <param name="requestFields">The request fields.</param>
        /// <returns></returns>
        List<ComboResult> GetListCombo(Type modelType, string col, string[] requestFields = null);

        /// <summary>
        /// Deletes the entity.
        /// </summary>
        /// <typeparam name="T">The type of the entity.</typeparam>
        int Delete<T>(object id) where T : class, IBaseEntity;

        /// <summary>
        /// Creates the ciselnik row.
        /// </summary>
        /// <typeparam name="T">The type of the T.</typeparam>
        /// <param name="obj">The obj.</param>
        /// <returns></returns>
        int Create<T>(T obj) where T : class, IBaseEntity;

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
    }
}