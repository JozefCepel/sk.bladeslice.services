using System;
using System.Diagnostics;
using System.Linq;

namespace WebEas.Services.Esb
{
    public class IamUserProxy : ProxyBase, IDisposable
    {
        private const string ServiceUrl = "https://lbsoa.intra.dcom.sk/soa/iam/1.0/user";

        private Iam.User.UserServicePortTypeChannel proxy;

        /// <summary>
        /// Initializes a new instance of the <see cref="IamRoleProxy" /> class.
        /// </summary>
        public IamUserProxy(string stsThumbprint) : base(stsThumbprint)
        {
            this.proxy = WebEas.Core.Sts.SecurityTokenServiceHelper.CreateChannelProxy<Iam.User.UserServicePortTypeChannel>(ServiceUrl, stsThumbprint);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="IamRoleProxy" /> class.
        /// </summary>
        public IamUserProxy(string openAmSessionIs, string tenantId, string stsThumbprint) : base(stsThumbprint)
        {
            this.proxy = WebEas.Core.Sts.SecurityTokenServiceHelper.CreateChannelProxy<Iam.User.UserServicePortTypeChannel>(ServiceUrl, openAmSessionIs, tenantId, stsThumbprint);
        }

        /// <summary>
        /// Gets the user.
        /// </summary>
        /// <param name="userId">The user id.</param>
        /// <returns></returns>
        public WebEas.Services.Esb.Iam.User.GetUserResponse GetUser(string userId)
        {
            try
            {
                WebEas.Core.Sts.SecurityTokenServiceHelper.CheckService(ref proxy, ServiceUrl, stsThumbPrint);

                if (String.IsNullOrEmpty(userId))
                {
                    throw new ArgumentNullException("User ID not defined");
                }

                var request = new Iam.User.GetUserRequest();
                request.userid = userId;
                var stopwatch = new Stopwatch();
                stopwatch.Start();
                Iam.User.GetUserResponse response = this.proxy.getUser(request);
                stopwatch.Stop();
                LogRequestDuration(ServiceUrl, stopwatch);

                return response;
            }
            catch (Exception ex)
            {
                this.CheckGlobalExceptions(ex);
                throw new WebEasProxyException("Error in GetUser", ex, userId);
            }
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing,
        /// or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            try
            {
                if (this.proxy != null && this.proxy is IDisposable)
                {
                    if (this.proxy.State == System.ServiceModel.CommunicationState.Faulted)
                        this.proxy.Abort();

                    ((IDisposable)this.proxy).Dispose();
                }
                this.proxy = null;
            }
            catch (Exception ex)
            {
                throw new WebEasException("Error in dispose", ex);
            }
        }
    }
}