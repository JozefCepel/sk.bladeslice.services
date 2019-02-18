using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using ServiceStack;
using ServiceStack.Logging;
using WebEas.Exceptions;

namespace WebEas.Core.Base
{
    public static class WebEasErrorHandling
    {
        public const string ErrorProxyCaption = "Chyba komunikácie s externou službou";
        public const string ErrorValidCaption = "Validačná chyba";
        public const string ErrorAuthCaption = "Neautorizovaný prístup";
        public const string ErrorDefaultCaption = "Chyba";
        public static readonly ILog Log = LogManager.GetLogger("UnHandled");

        /// <summary>
        /// Creates the error response.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <param name="requestDto">The request dto.</param>
        /// <param name="ex">The ex.</param>
        /// <returns></returns>
        public static HttpError CreateErrorResponse(global::ServiceStack.Web.IRequest request, object requestDto, Exception ex)
        {
            try
            {
                //Trace.TraceInformation("Creating error response");
                if (WebEas.Context.Current.HasHttpContext && WebEas.Context.Current.HttpContext.Items["currentRequest"] != null)
                {
                    WebEas.Context.Current.HttpContext.Items["currentRequest"] = requestDto;
                }

                LogError(ex);

                WebEasResponseStatus errorResponse = CreateWebEasResponseStatus(ex, requestDto);
                #if DEBUG
                errorResponse.StackTrace = GetStackTrace(request, requestDto, ex);
                errorResponse.Errors = GetErrors(ex, new List<ResponseError>());
                #endif
                return new HttpError(errorResponse, GetStatusCode(ex), GetErrorCode(ex), errorResponse.Message);
            }
            catch (Exception exx)
            {
                // Trace.TraceInformation(exx.Message);
                return new HttpError(exx, GetStatusCode(ex), GetErrorCode(ex), ex.Message);
            }
            finally
            {
                if (WebEas.Context.Current.HasHttpContext)
                {
                    WebEas.Context.Current.HttpContext.Items["currentRequest"] = null;
                }
            }
        }

        /// <summary>
        /// Creates the web eas response status.
        /// </summary>
        /// <param name="exception">The exception.</param>
        /// <param name="dto">The dto.</param>
        /// <returns></returns>
        public static WebEasResponseStatus CreateWebEasResponseStatus(Exception exception, object dto)
        {
            var errorResponse = new WebEasResponseStatus
            {
                Caption = GetCaption(exception)
            };

            errorResponse.Message = GetMessage(exception) ?? string.Format("Nastala interná chyba pri volaní {0}. Chyba bola zaznamenaná k riešeniu.", dto == null ? "unknown" : dto.GetType().Name);

            #if DEBUG

            if (exception is WebEasException && ((WebEasException)exception).HasDetailMessage)
            {
                errorResponse.DetailMessage = string.Format("{0} {1}{2}{3}", exception.GetExceptionTypeName(), exception.GetIdentifier(), Environment.NewLine, ((WebEasException)exception).DetailMessage);
            }
            else
            {
                errorResponse.DetailMessage = string.Format("{0} {1}{2}{3}", exception.GetExceptionTypeName(), exception.GetIdentifier(), Environment.NewLine, GetDetailMessage(exception, new StringBuilder()).ToString());
            }

            #else
            errorResponse.DetailMessage = "Kontaktujte Call Centrum s kódom : " + exception.GetIdentifier();
            #endif

            return errorResponse;
        }

        /// <summary>
        /// Logs the error.
        /// </summary>
        /// <param name="ex">The ex.</param>
        private static void LogError(Exception ex)
        {
            if (ex is WebEasAuthenticationException || ex is WebEasUnauthorizedAccessException)
            {
                Log.Debug(ex);
            }
            else if (ex is WebEasValidationException)
            {
                Log.Warn(ex);
            }
            else
            {
                Log.Error(ex);
            }
        }

        /// <summary>
        /// Gets the status code.
        /// </summary>
        /// <param name="ex">The ex.</param>
        /// <returns></returns>
        private static int GetStatusCode(Exception ex)
        {
            if (ex is TargetInvocationException && ex.InnerException != null)
            {
                return GetStatusCode(ex.InnerException);
            }
            else
            {
                int code = ex.ToStatusCode();

                if (ex.InnerException != null && ex.InnerException is WebEasException)
                {
                    return GetStatusCode(ex.InnerException);
                }
                else if (ex is WebEasException)
                {
                    code = (int)((WebEasException)ex).StatusCode;
                }
                if (code == 500) // Nech app neposiela 500
                {
                    code = 409;
                }
                return code;
            }
        }

        /// <summary>
        /// Gets the message.
        /// </summary>
        /// <param name="ex">The ex.</param>
        /// <param name="sb">The sb.</param>
        /// <returns></returns>
        private static string GetMessage(Exception ex)
        {
            if (ex is WebEasException && ((WebEasException)ex).HasMessageUser)
            {
                return ((WebEasException)ex).MessageUser;
            }

            if (ex.InnerException != null)
            {
                return GetMessage(ex.InnerException);
            }

            return null;
        }

        /// <summary>
        /// Gets the detail message.
        /// </summary>
        /// <param name="ex">The ex.</param>
        /// <param name="sb">The sb.</param>
        /// <returns></returns>
        private static StringBuilder GetDetailMessage(Exception ex, StringBuilder sb)
        {
            if (sb.Length != 0)
            {
                sb.Append(" - ");
            }

            sb.Append(ex.Message);

            if (ex.InnerException != null)
            {
                GetDetailMessage(ex.InnerException, sb);
            }

            return sb;
        }

        /// <summary>
        /// Gets the errors.
        /// </summary>
        /// <param name="ex">The ex.</param>
        /// <param name="list">The list.</param>
        /// <returns></returns>
        private static List<ResponseError> GetErrors(Exception ex, List<ResponseError> list)
        {
            if (ex.InnerException != null)
            {
                GetErrors(ex.InnerException, list);
            }

            list.Add(new ResponseError { ErrorCode = GetStatusCode(ex).ToString(), FieldName = ex.GetType().Name, Message = ex.Message });
            return list;
        }

        /// <summary>
        /// Gets the error code.
        /// </summary>
        /// <param name="ex">The ex.</param>
        /// <returns></returns>
        private static string GetErrorCode(Exception ex)
        {
            if (ex.InnerException != null)
            {
                return GetErrorCode(ex.InnerException);
            }
            return ex.GetType().Name;
        }

        /// <summary>
        /// Gets the stack trace.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <param name="ex">The ex.</param>
        /// <returns></returns>
        private static string GetStackTrace(global::ServiceStack.Web.IRequest httpRequest, object request, Exception ex)
        {
            return string.Format("[{0}: {1}]:\n[REQUEST: {2}]\n{3}", (request ?? new object()).GetType().GetOperationName(), DateTime.UtcNow, httpRequest.AbsoluteUri, ex.ToDescription());
        }

        /// <summary>
        /// Gets the caption.
        /// </summary>
        /// <param name="ex">The ex.</param>
        /// <param name="sb">The sb.</param>
        /// <returns></returns>
        private static string GetCaption(Exception ex)
        {
            string caption = ex.InnerException == null ? ErrorDefaultCaption : GetCaption(ex.InnerException);

            if (caption != ErrorDefaultCaption)
            {
                return caption;
            }

            if (!(ex is WebEasException))
            {
                return ErrorDefaultCaption;
            }

            if (((WebEasException)ex).HasCaption)
            {
                return ((WebEasException)ex).Caption;
            }

            if (ex is WebEasUnauthorizedAccessException || ex is WebEasAuthenticationException)
            {
                return ErrorAuthCaption;
            }

            if (ex is WebEasProxyException)
            {
                return ErrorProxyCaption;
            }

            if (ex is WebEasValidationException)
            {
                return ErrorValidCaption;
            }

            return ErrorDefaultCaption;
        }
    }
}