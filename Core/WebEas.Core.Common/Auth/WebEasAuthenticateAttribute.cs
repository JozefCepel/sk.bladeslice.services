using System;
using System.Collections.Specialized;
using System.Linq;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;
using Ninject;
using ServiceStack;
using ServiceStack.Web;
using WebEas.Core;

namespace WebEas
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = false)]
    public class WebEasAuthenticateAttribute : RequestFilterAttribute, IOperationBehavior, IParameterInspector
    {
        private static IWebEasSessionProvider sessionProvider;

        //private EgovSession session;
        /// <summary>
        /// Initializes a new instance of the <see cref="WebEasAuthenticateAttribute" /> class.
        /// </summary>
        /// <param name="applyTo">The apply to.</param>
        public WebEasAuthenticateAttribute(ApplyTo applyTo) : base(applyTo)
        {
            this.Priority = (int)RequestFilterPriority.Authenticate;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WebEasAuthenticateAttribute" /> class.
        /// </summary>
        public WebEasAuthenticateAttribute() : this(ApplyTo.All)
        {
        }

        /// <summary>
        /// Redirect the client to a specific URL if authentication failed.
        /// If this property is null, simply `401 Unauthorized` is returned.
        /// </summary>
        public string HtmlRedirect { get; set; }

        /// <summary>
        /// Gets the session.
        /// </summary>
        /// <value>The session.</value>
        protected IWebEasSession Session
        {
            get
            { 
                if (sessionProvider == null)
                {
                    sessionProvider = Ninject.NinjectServiceLocator.Kernel.Get<IWebEasSessionProvider>();
                }
                return sessionProvider.GetSession();                
            }
        }

        /// <summary>
        /// Does the HTML redirect.
        /// </summary>
        /// <param name="redirectUrl">The redirect URL.</param>
        /// <param name="req">The req.</param>
        /// <param name="res">The res.</param>
        /// <param name="includeRedirectParam">The include redirect param.</param>
        public static void DoHtmlRedirect(string redirectUrl, IRequest req, IResponse res, bool includeRedirectParam)
        {
            var url = req.ResolveAbsoluteUrl(redirectUrl);
            if (includeRedirectParam)
            {
                var absoluteRequestPath = req.ResolveAbsoluteUrl(string.Format("~{0}{1}", req.PathInfo, ToQueryString(req.QueryString)));
                url = url.AddQueryParam(HostContext.ResolveLocalizedString(LocalizedStrings.Redirect), absoluteRequestPath);
            }

            res.RedirectToUrl(url);
        }

        /// <summary>
        /// This method is only executed if the HTTP method matches the <see cref="P:ServiceStack.RequestFilterAttribute.ApplyTo" />
        /// property.
        /// </summary>
        /// <param name="req">The http request wrapper</param>
        /// <param name="res">The http response wrapper</param>
        /// <param name="requestDto">The request DTO</param>
        public override void Execute(IRequest req, IResponse res, object requestDto)
        {
            var ses = this.Session;
            if (ses == null || !ses.IsAuthorized)
            {
                if (this.DoHtmlRedirectIfConfigured(req, res, true))
                {
                    return;
                }

                throw new WebEasAuthenticationException();
                //res.StatusCode = (int)HttpStatusCode.Unauthorized;
                //res.EndRequest(false);
            }
        }

        /// <summary>
        /// Implement to pass data at runtime to bindings to support custom behavior.
        /// </summary>
        /// <param name="operationDescription">The operation being examined. Use for examination
        /// only. If the operation description is modified, the results are undefined.</param>
        /// <param name="bindingParameters">The collection of objects that binding elements
        /// require to support the behavior.</param>
        public void AddBindingParameters(OperationDescription operationDescription, System.ServiceModel.Channels.BindingParameterCollection bindingParameters)
        {
        }

        /// <summary>
        /// Implements a modification or extension of the client across an operation.
        /// </summary>
        /// <param name="operationDescription">The operation being examined. Use for examination
        /// only. If the operation description is modified, the results are undefined.</param>
        /// <param name="clientOperation">The run-time object that exposes customization
        /// properties for the operation described by <paramref name="operationDescription" />.</param>
        public void ApplyClientBehavior(OperationDescription operationDescription, ClientOperation clientOperation)
        {
        }

        /// <summary>
        /// Implements a modification or extension of the service across an operation.
        /// </summary>
        /// <param name="operationDescription">The operation being examined. Use for examination
        /// only. If the operation description is modified, the results are undefined.</param>
        /// <param name="dispatchOperation">The run-time object that exposes customization
        /// properties for the operation described by <paramref name="operationDescription" />.</param>
        public void ApplyDispatchBehavior(OperationDescription operationDescription, DispatchOperation dispatchOperation)
        {
            dispatchOperation.ParameterInspectors.Add(this);
        }

        /// <summary>
        /// Implement to confirm that the operation meets some intended criteria.
        /// </summary>
        /// <param name="operationDescription">The operation being examined. Use for examination
        /// only. If the operation description is modified, the results are undefined.</param>
        public void Validate(OperationDescription operationDescription)
        {
        }

        /// <summary>
        /// Called after client calls are returned and before service responses
        /// are sent.
        /// </summary>
        /// <param name="operationName">The name of the invoked operation.</param>
        /// <param name="outputs">Any output objects.</param>
        /// <param name="returnValue">The return value of the operation.</param>
        /// <param name="correlationState">Any correlation state returned from the <see cref="M:System.ServiceModel.Dispatcher.IParameterInspector.BeforeCall(System.String,System.Object[])" />
        /// method, or null.</param>
        public void AfterCall(string operationName, object[] outputs, object returnValue, object correlationState)
        {
        }

        /// <summary>
        /// Called before client calls are sent and after service responses are
        /// returned.
        /// </summary>
        /// <param name="operationName">The name of the operation.</param>
        /// <param name="inputs">The objects passed to the method by the client.</param>
        /// <returns>
        /// The correlation state that is returned as the <paramref name="correlationState" />
        /// parameter in <see cref="M:System.ServiceModel.Dispatcher.IParameterInspector.AfterCall(System.String,System.Object[],System.Object,System.Object)" />.
        /// Return null if you do not intend to use correlation state.
        /// </returns>
        public virtual object BeforeCall(string operationName, object[] inputs)
        {
            var ses = this.Session;
            if (ses == null || !ses.IsAuthorized)
            {
                throw new WebEasAuthenticationException();
                //if (WebOperationContext.Current != null)
                //{
                //    WebOperationContext.Current.OutgoingResponse.StatusCode = HttpStatusCode.Unauthorized;
                //}
                //throw new WebFaultException<string>("Unauthorized", HttpStatusCode.Unauthorized);
            }

            return null;
        }

        /// <summary>
        /// Does the HTML redirect if configured.
        /// </summary>
        /// <param name="req">The req.</param>
        /// <param name="res">The res.</param>
        /// <param name="includeRedirectParam">The include redirect param.</param>
        /// <returns></returns>
        protected bool DoHtmlRedirectIfConfigured(IRequest req, IResponse res, bool includeRedirectParam = false)
        {
            var htmlRedirect = this.HtmlRedirect ?? null;
            if (htmlRedirect != null && req.ResponseContentType == MimeTypes.Html)
            {
                DoHtmlRedirect(htmlRedirect, req, res, includeRedirectParam);
                return true;
            }
            return false;
        }

        /// <summary>
        /// Toes the query string.
        /// </summary>
        /// <param name="queryStringCollection">The query string collection.</param>
        /// <returns></returns>
        private static string ToQueryString(INameValueCollection queryStringCollection)
        {
            return ToQueryString((NameValueCollection)queryStringCollection.Original);
        }

        /// <summary>
        /// Toes the query string.
        /// </summary>
        /// <param name="queryStringCollection">The query string collection.</param>
        /// <returns></returns>
        private static string ToQueryString(NameValueCollection queryStringCollection)
        {
            if (queryStringCollection == null || queryStringCollection.Count == 0)
            {
                return String.Empty;
            }

            return string.Format("?{0}", queryStringCollection.ToFormUrlEncoded());
        }
    }
}