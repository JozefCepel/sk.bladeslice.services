using ServiceStack.OrmLite;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Transactions;
using WebEas.ServiceModel;
using WebEas.ServiceModel.Office.Egov.Reg.Types;
using WebEas.ServiceModel.Reg.Types;

namespace WebEas.Esam.ServiceInterface.Office
{
    public class OfficeRepository : RepositoryBase, IOfficeRepository
    {
        #region NasledovnyStavEntity

        public NasledovnyStavEntity Save(NasledovnyStavEntity entity)
        {
            using (var txScope = new TransactionScope())
            {
                long stavEntityId = this.InsertData<StavEntityView>(entity.NasledovnyStav);
                entity.C_StavEntity_Id_Child = (int)stavEntityId;
                this.InsertData<NasledovnyStavEntity>(entity);

                txScope.Complete();
            }

            return entity;
        }

        public List<StavEntityView> GetListNaslStavEntity(int idStavEntity)
        {
            var filter = new Filter("C_StavEntity_Id_Parent", idStavEntity);
            List<NasledovnyStavEntity> naslStavy = this.GetList<NasledovnyStavEntity>(filter);

            filter = new Filter(FilterElement.In("C_StavEntity_Id", naslStavy.Select(nav => nav.C_StavEntity_Id_Child)));
            return this.GetList<StavEntityView>(filter);
        }

        #endregion

        #region Translations

        public ColumnTranslation Save(ColumnTranslation entity)
        {
            this.InsertData<ColumnTranslation>(entity);
            return entity;
        }

        #endregion

        #region Logging

        public LoggingConfig Save(LoggingConfig entity)
        {
            if (entity.C_LoggingConfig_Id == default(int))
            {
                entity.C_LoggingConfig_Id = (int)this.InsertData<LoggingConfig>(entity);
            }
            else
            {
                this.UpdateData<LoggingConfig>(entity);
            }
            return entity;
        }

        public void ResetLoggingCache()
        {
            base.RemoveFromCacheOptimizedTenant("db:columnslog");
        }

        #endregion // Logging

        #region AccessFlag

        public override void SetAccessFlag(object viewData)
        {
            base.SetAccessFlag(viewData);
        }

        #endregion
    }
}
