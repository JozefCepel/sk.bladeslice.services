using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebEas.Esam.ServiceModel.Office.Cfe.Dto;
using WebEas.ServiceModel.Dto;

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
    }
}
