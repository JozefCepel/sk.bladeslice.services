using System;
using System.Collections.Generic;
using System.Linq;
using ServiceStack;
using ServiceStack.Web;

namespace WebEas
{
    /// <summary>
    /// Require Any Role
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = true)]
    public class WebEasRequiresAnyRole : WebEasAuthenticateAttribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EgovRequiredRoleAttribute" /> class.
        /// </summary>
        /// <param name="applyTo">The apply to.</param>
        /// <param name="roles">The roles.</param>
        public WebEasRequiresAnyRole(ApplyTo applyTo, params string[] roles)
        {
            this.RequiredRoles = roles.ToList();
            this.ApplyTo = applyTo;
            this.Priority = (int)RequestFilterPriority.RequiredRole;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EgovRequiredRoleAttribute" /> class.
        /// </summary>
        /// <param name="roles">The roles.</param>
        public WebEasRequiresAnyRole(params string[] roles) : this(ApplyTo.All, roles)
        {
        }

        /// <summary>
        /// Gets or sets the required roles.
        /// </summary>
        /// <value>The required roles.</value>
        public List<string> RequiredRoles { get; set; }

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
            if (res.IsClosed)
            {
                return; //AuthenticateAttribute already closed the request (ie auth failed)
            }

            //if (ses != null && ses.HasRole(RoleNames.Admin))
            //{
            //    return;
            //}

            if (this.AnyRoles(ses))
            {
                return;
            }

            if (this.DoHtmlRedirectIfConfigured(req, res))
            {
                return;
            }

            throw new WebEasUnauthorizedAccessException();
            //res.StatusCode = (int)HttpStatusCode.Forbidden;
            //res.StatusDescription = "Invalid Role";
            //res.EndRequest();
        }

        /// <summary>
        /// Determines whether [has all roles] [the specified session].
        /// </summary>
        /// <param name="session">The session.</param>
        /// <returns></returns>
        public bool AnyRoles(IWebEasSession session)
        {
            if (session == null)
            {
                return false;
            }

            return this.RequiredRoles.Any(session.HasRole);
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
        public override object BeforeCall(string operationName, object[] inputs)
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

            //if (ses != null && ses.HasRole(RoleNames.Admin))
            //{
            //    return null;
            //}

            if (this.AnyRoles(ses))
            {
                return null;
            }

            throw new WebEasUnauthorizedAccessException();
            //if (WebOperationContext.Current != null)
            //{
            //    WebOperationContext.Current.OutgoingResponse.StatusCode = HttpStatusCode.Forbidden;
            //}
            //throw new WebFaultException<string>("Forbidden", HttpStatusCode.Forbidden);
        }
    }
}