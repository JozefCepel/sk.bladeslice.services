using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebEas.Esam.ServiceModel.Office.Bds.Dto;
using WebEas.Esam.ServiceModel.Office.Bds.Types;
using WebEas.ServiceInterface;

namespace WebEas.Esam.ServiceInterface.Office.Bds
{
    public partial class BdsService : ServiceBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BdsService" /> class.
        /// </summary>
        /// <param name="repository">The repository.</param>
        public BdsService(IBdsRepository repository)
            : base(repository)
        {
        }

        /// <summary>
        /// Gets or sets the repository.
        /// </summary>
        /// <value>The repository.</value>
        public new IBdsRepository Repository
        {
            get
            {
                return (IBdsRepository)repository;
            }
        }

        #region Globalne, ale musia byt v moduloch

        public object Get(AppStatus.HealthCheckDto request)
        {
            return GetHealthCheck(request);
        }

        public object Get(ListDto request)
        {
            return base.GetList(request);
        }

        public object Get(ListComboDto request)
        {
            return this.Repository.GetListCombo(request);
        }

        public void Post(ChangeStateDto request)
        {            
            Repository.ChangeState(request);
        }


        public object Post(GetTreeCountsDto request)
        {
            return base.GetTreeCounts(request);
        }

        public object Get(DataChangesRequest request)
        {
            return Repository.GetTableLogging(request.Code, request.RowId);
        }

        #endregion

        #region Globalne, Long operations

        public object Post(LongOperationStartDto request)
        {
            return Repository.LongOperationStart(request.OperationName, request.OperationParameters, request.OperationInfo);
        }

        public object Post(LongOperationRestartDto request)
        {
            return Repository.LongOperationRestart(request.ProcessKey);
        }

        public object Get(LongOperationProgressDto request)
        {
            return Repository.LongOperationStatus(request.ProcessKey);
        }

        public object Get(LongOperationResultDto request)
        {
            return Repository.LongOperationResult(request.ProcessKey);
        }

        public object Post(LongOperationCancelDto request)
        {
            return Repository.LongOperationCancel(request.ProcessKey, request.NotRemove);
        }

        public object Get(LongOperationListDto request)
        {
            return Repository.LongOperationList(request.PerTenant, request.Skip, request.Take);
        }

        #endregion

        #region Default row values

        public object Any(RowDefaultValues request)
        {
            return Repository.GetRowDefaultValues(request.code, request.masterCode, request.masterRowId);
        }

        #endregion

        #region D_PRI_0

        public object Any(CreateD_PRI_0 request)
        {
            return Repository.Create<V_PRI_0View>(request);
        }

        public object Any(UpdateD_PRI_0 request)
        {
            return Repository.Update<V_PRI_0View>(request);
        }

        public void Any(DeleteD_PRI_0 request)
        {
            Repository.Delete<tblD_PRI_0>(request.D_PRI_0);
        }

        #endregion

        #region D_PRI_1

        public object Any(CreateD_PRI_1 request)
        {
            return Repository.Create<V_PRI_1View>(request);
        }

        public object Any(UpdateD_PRI_1 request)
        {
            return Repository.Update<V_PRI_1View>(request);
        }

        public void Any(DeleteD_PRI_1 request)
        {
            Repository.Delete<tblD_PRI_1>(request.D_PRI_1);
        }

        public object Any(GetVybavDokladyReq request)
        {
            return Repository.GetVybavDoklady(request);
        }

        public object Any(GetOdvybavDokladyReq request)
        {
            return Repository.GetOdvybavDoklady(request);
        }

        #endregion

        #region D_SIM_0

        public object Any(CreateD_SIM_0 request)
        {
            return Repository.Create<V_SIM_0View>(request);
        }

        public object Any(UpdateD_SIM_0 request)
        {
            return Repository.Update<V_SIM_0View>(request);
        }

        public void Any(DeleteD_SIM_0 request)
        {
            Repository.Delete<tblD_SIM_0>(request.D_SIM_0);
        }

        #endregion

        #region D_VYD_0

        public object Any(CreateD_VYD_0 request)
        {
            return Repository.Create<V_VYD_0View>(request);
        }

        public object Any(UpdateD_VYD_0 request)
        {
            return Repository.Update<V_VYD_0View>(request);
        }

        public void Any(DeleteD_VYD_0 request)
        {
            Repository.Delete<tblD_VYD_0>(request.D_VYD_0);
        }

        #endregion

        #region D_VYD_1

        public object Any(CreateD_VYD_1 request)
        {
            return Repository.Create<V_VYD_1View>(request);
        }

        public object Any(UpdateD_VYD_1 request)
        {
            return Repository.Update<V_VYD_1View>(request);
        }

        public void Any(DeleteD_VYD_1 request)
        {
            Repository.Delete<tblD_VYD_1>(request.D_VYD_1);
        }

        #endregion

        #region K_MAT_0

        public object Any(CreateK_MAT_0 request)
        {
            return Repository.Create<V_MAT_0View>(request);
        }

        public object Any(UpdateK_MAT_0 request)
        {
            return Repository.Update<V_MAT_0View>(request);
        }

        public void Any(DeleteK_MAT_0 request)
        {
            Repository.Delete<tblK_MAT_0>(request.K_MAT_0);
        }

        #endregion

        #region K_OBP_0

        public object Any(CreateK_OBP_0 request)
        {
            return Repository.Create<V_OBP_0View>(request);
        }

        public object Any(UpdateK_OBP_0 request)
        {
            return Repository.Update<V_OBP_0View>(request);
        }

        public void Any(DeleteK_OBP_0 request)
        {
            Repository.Delete<tblK_OBP_0>(request.K_OBP_0);
        }

        #endregion

        #region K_ORJ_0

        public object Any(CreateK_ORJ_0 request)
        {
            return Repository.Create<V_ORJ_0View>(request);
        }

        public object Any(UpdateK_ORJ_0 request)
        {
            return Repository.Update<V_ORJ_0View>(request);
        }

        public void Any(DeleteK_ORJ_0 request)
        {
            Repository.Delete<tblK_ORJ_0>(request.K_ORJ_0);
        }

        #endregion

        #region K_ORJ_1

        public object Any(CreateK_ORJ_1 request)
        {
            return Repository.Create<V_ORJ_1View>(request);
        }

        public object Any(UpdateK_ORJ_1 request)
        {
            return Repository.Update<V_ORJ_1View>(request);
        }

        public void Any(DeleteK_ORJ_1 request)
        {
            Repository.Delete<tblK_ORJ_1>(request.K_ORJ_1);
        }

        #endregion

        #region K_SKL_0

        public object Any(CreateK_SKL_0 request)
        {
            return Repository.Create<V_SKL_0View>(request);
        }

        public object Any(UpdateK_SKL_0 request)
        {
            return Repository.Update<V_SKL_0View>(request);
        }

        public void Any(DeleteK_SKL_0 request)
        {
            Repository.Delete<tblK_SKL_0>(request.K_SKL_0);
        }

        #endregion

        #region K_SKL_1

        public object Any(CreateK_SKL_1 request)
        {
            return Repository.Create<V_SKL_1View>(request);
        }

        public object Any(UpdateK_SKL_1 request)
        {
            return Repository.Update<V_SKL_1View>(request);
        }

        public void Any(DeleteK_SKL_1 request)
        {
            Repository.Delete<tblK_SKL_1>(request.K_SKL_1);
        }

        #endregion

        #region K_SKL_2

        public object Any(CreateK_SKL_2 request)
        {
            return Repository.Create<V_SKL_2View>(request);
        }

        public object Any(UpdateK_SKL_2 request)
        {
            return Repository.Update<V_SKL_2View>(request);
        }

        public void Any(DeleteK_SKL_2 request)
        {
            Repository.Delete<tblK_SKL_2>(request.K_SKL_2);
        }

        #endregion

        #region K_TSK_0

        public object Any(CreateK_TSK_0 request)
        {
            return Repository.Create<V_TSK_0View>(request);
        }

        public object Any(UpdateK_TSK_0 request)
        {
            return Repository.Update<V_TSK_0View>(request);
        }

        public void Any(DeleteK_TSK_0 request)
        {
            Repository.Delete<tblK_TSK_0>(request.K_TSK_0);
        }

        #endregion

        #region K_TYP_0

        public object Any(CreateK_TYP_0 request)
        {
            return Repository.Create<V_TYP_0View>(request);
        }

        public object Any(UpdateK_TYP_0 request)
        {
            return Repository.Update<V_TYP_0View>(request);
        }

        public void Any(DeleteK_TYP_0 request)
        {
            Repository.Delete<tblK_TYP_0>(request.K_TYP_0);
        }

        #endregion

        #region Nastavenie

        public object Any(GetParameterType request)
        {
            return Repository.GetParameterTypeBds(request);
        }

        public object Any(UpdateNastavenie request)
        {
            return Repository.UpdateNastavenieBds(request);
        }

        #endregion
    }
}
