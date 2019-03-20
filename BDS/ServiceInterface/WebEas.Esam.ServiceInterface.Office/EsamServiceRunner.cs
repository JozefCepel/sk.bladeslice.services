using System;
using System.Linq;
using ServiceStack;
using ServiceStack.Host;
using WebEas.Core.Base;
using WebEas.Exceptions;

namespace WebEas.Esam.ServiceInterface.Office
{
    public class EsamServiceRunner<T> : WebEasServiceRunner<T>
    {
        public EsamServiceRunner(IAppHost appHost, ActionContext actionContext) : base(appHost, actionContext)
        {
        }

        /// <summary>
        /// Handles the exception.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <param name="requestDto">The request dto.</param>
        /// <param name="ex">The ex.</param>
        /// <returns></returns>
        public override object HandleException(global::ServiceStack.Web.IRequest request, T requestDto, Exception ex)
        {
            HttpError response = WebEasErrorHandling.CreateErrorResponse(request, requestDto, ex);    
            #if INT || DEVELOP || DEBUG
            if (response.Response is WebEasResponseStatus)
            {
#if DEBUG || DEVELOP
                ((WebEasResponseStatus)response.Response).DetailMessage += $"{Environment.NewLine}http://localhost:85/esam/api/pfe/lll/{ex.GetIdentifier()}";
#endif

            }
            #endif

            this.AfterEachRequest(request, requestDto, response == null ? ex : response.Response ?? ex);
            return response;
        }
    }
}