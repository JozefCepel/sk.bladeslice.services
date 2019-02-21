using ServiceStack.OrmLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebEas.Esam.ServiceModel.Office.Cfe.Dto;
using WebEas.Esam.ServiceModel.Office.Cfe.Types;
using WebEas.ServiceInterface;
using WebEas.ServiceModel.Dto;
using WebEas.ServiceModel.Types;

namespace WebEas.Esam.ServiceInterface.Office.Cfe
{
    public partial class CfeRepository : RepositoryBase, ICfeRepository
    {
        #region Long Operations

        protected override void LongOperationProcess(string processKey, string operationName, string operationParameters)
        {
            switch (operationName)
            {
                case OperationsList.InternyPoplatok:
                    this.InternyPoplatok(processKey, operationParameters);
                    break;
                default:
                    throw new WebEasException(string.Format("Long operation with the name {0} is not implemented", operationName), "Operácia nie je implementovaná!");
            }
        }

        private void InternyPoplatok(string processKey, string parameters)
        {
            string actionName = "Vyrubenie dane/poplatku";

            LongOperationStarted(processKey, actionName);
            LongOperationReportProgress(processKey, actionName, 0, 1);
            LongOperationSetStateFinished(processKey, string.Empty, "Hotovo", state: LongOperationState.Done);
        }

        #endregion

        #region GetRowDefaultValues

        public override object GetRowDefaultValues(string code, string masterCode, string masterRowId)
        {
            var node = Modules.TryFindNode(code);
            var masternode = Modules.TryFindNode(masterCode);

            #region Users

            if (node != null && node.ModelType == typeof(UserView))
            {
                return new UserView()
                {
                    DateStart = DateTime.Now
                };
            }

            #endregion

            #region RoleUsers

            if (node != null && node.ModelType == typeof(RoleUsersView))
            {
                return new RoleUsersView()
                {
                    PlatnostOd = DateTime.Now
                };
            }

            #endregion

            #region Department

            if (node != null && node.ModelType == typeof(DepartmentView))
            {
                return new DepartmentView()
                {
                    PlatnostOd = DateTime.Now
                };
            }

            #endregion

            return new object();
        }

        #endregion

        #region Users

        // specialita: primary key je GUID a to framework nevie
        public void CreateUser(CreateUser request)
        {
            var rec = request.ConvertToEntity();
            rec.DatumVytvorenia = DateTime.Now;
            rec.DatumZmeny = DateTime.Now;
            rec.Vytvoril = Session.DcomId;
            Db.Insert(rec);
            // nevraciame last ID
        }

        #endregion

        #region TenantUsers

        // specialita: framework nedokaze spravit UPDATE D_Tenant_ID
        public TenantUsersView UpdateTenantUsers(UpdateTenantUsers request)
        {
            var rec = GetById<TenantUsers>(request.D_TenantUsers_Id);
            request.UpdateEntity(rec);
            rec.DatumZmeny = DateTime.Now;
            rec.Zmenil = Session.DcomId;
            Db.Update(rec);
            return GetById<TenantUsersView>(rec.D_TenantUsers_Id); 
        }

        #endregion
    }
}
