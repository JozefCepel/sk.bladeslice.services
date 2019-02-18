using System;
using System.Diagnostics;
using System.ServiceModel;
using WebEas.Services.Esb.Upvs;

namespace WebEas.Services.Esb
{
    /// <summary>
    /// UPVS proxy
    /// </summary>
    /// <seealso cref="WebEas.Services.Esb.ProxyBase" />
    /// <seealso cref="System.IDisposable" />
    public class UpvsProxy : ProxyBase, IDisposable
    {
        /// <summary>
        /// The service URL
        /// </summary>
        private const string ServiceUrl = "https://lbsoa.intra.dcom.sk/soa/upvs/2.0/upvsGwServices";

        /// <summary>
        /// The proxy
        /// </summary>
        private UpvsGwServicesChannel proxy;

        /// <summary>
        /// Initializes a new instance of the <see cref="UpvsProxy"/> class.
        /// </summary>
        /// <param name="stsThumbprint">The STS thumbprint.</param>
        public UpvsProxy(string stsThumbprint) : base(stsThumbprint)
        {
            proxy = WebEas.Core.Sts.SecurityTokenServiceHelper.CreateChannelProxy<UpvsGwServicesChannel>(ServiceUrl, stsThumbprint);
        }

        /// <summary>
        /// Converts the PDF.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        /// <exception cref="WebEasValidationException">null</exception>
        /// <exception cref="WebEasProxyException"></exception>
        public ConvertPDFResponse ConvertPDF(PDFConversionValidationRequest request)
        {
            try
            {
                WebEas.Core.Sts.SecurityTokenServiceHelper.CheckService(ref proxy, ServiceUrl, stsThumbPrint);

                var stopwatch = new Stopwatch();
                stopwatch.Start();
                var response = proxy.ConvertPDF(new ConvertPDFRequest(request));
                stopwatch.Stop();
                LogRequestDuration(ServiceUrl, stopwatch);
                return response.ConvertPDFResponse;
            }
            catch (FaultException<DcomFaultType> ex)
            {
                var msg = ex.Detail == null || string.IsNullOrEmpty(ex.Detail.faultMessage) ? ex.Message : ex.Detail.faultMessage;
                throw new WebEasValidationException(null, msg, ex);
            }
            catch (Exception ex)
            {
                CheckGlobalExceptions(ex);
                throw new WebEasProxyException($"Error in {System.Reflection.MethodBase.GetCurrentMethod().Name}", ex, request);
            }
        }

        /// <summary>
        /// Echoes the specified request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        /// <exception cref="WebEasValidationException">null</exception>
        /// <exception cref="WebEasProxyException"></exception>
        public EchoResponse Echo(Echo request)
        {
            try
            {
                WebEas.Core.Sts.SecurityTokenServiceHelper.CheckService(ref proxy, ServiceUrl, stsThumbPrint);

                var stopwatch = new Stopwatch();
                stopwatch.Start();
                var response = proxy.Echo(request);
                stopwatch.Stop();
                LogRequestDuration(ServiceUrl, stopwatch);
                return response;
            }
            catch (FaultException<DcomFaultType> ex)
            {
                var msg = ex.Detail == null || string.IsNullOrEmpty(ex.Detail.faultMessage) ? ex.Message : ex.Detail.faultMessage;
                throw new WebEasValidationException(null, msg, ex);
            }
            catch (Exception ex)
            {
                CheckGlobalExceptions(ex);
                throw new WebEasProxyException($"Error in {System.Reflection.MethodBase.GetCurrentMethod().Name}", ex, request);
            }
        }

        /// <summary>
        /// Fetchs the form template.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        /// <exception cref="WebEasValidationException">null</exception>
        /// <exception cref="WebEasProxyException"></exception>
        public FetchFormTemplateResponse FetchFormTemplate(FetchFormTemplateType request)
        {
            try
            {
                WebEas.Core.Sts.SecurityTokenServiceHelper.CheckService(ref proxy, ServiceUrl, stsThumbPrint);

                var stopwatch = new Stopwatch();
                stopwatch.Start();
                var response = proxy.FetchFormTemplate(new FetchFormTemplate(request));
                stopwatch.Stop();
                LogRequestDuration(ServiceUrl, stopwatch);
                return response;
            }
            catch (FaultException<DcomFaultType> ex)
            {
                var msg = ex.Detail == null || string.IsNullOrEmpty(ex.Detail.faultMessage) ? ex.Message : ex.Detail.faultMessage;
                throw new WebEasValidationException(null, msg, ex);
            }
            catch (Exception ex)
            {
                CheckGlobalExceptions(ex);
                throw new WebEasProxyException($"Error in {System.Reflection.MethodBase.GetCurrentMethod().Name}", ex, request);
            }
        }

        /// <summary>
        /// Gets the current time.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="WebEasValidationException">null</exception>
        /// <exception cref="WebEasProxyException"></exception>
        public DateTime? GetCurrentTime()
        {
            try
            {
                WebEas.Core.Sts.SecurityTokenServiceHelper.CheckService(ref proxy, ServiceUrl, stsThumbPrint);

                var stopwatch = new Stopwatch();
                stopwatch.Start();
                var response = proxy.GetCurrentTime(new GetCurrentTime());
                stopwatch.Stop();
                LogRequestDuration(ServiceUrl, stopwatch);
                return response.CurrentTimeResponse;
            }
            catch (FaultException<DcomFaultType> ex)
            {
                var msg = ex.Detail == null || string.IsNullOrEmpty(ex.Detail.faultMessage) ? ex.Message : ex.Detail.faultMessage;
                throw new WebEasValidationException(null, msg, ex);
            }
            catch (Exception ex)
            {
                CheckGlobalExceptions(ex);
                throw new WebEasProxyException($"Error in {System.Reflection.MethodBase.GetCurrentMethod().Name}", ex);
            }
        }

        /// <summary>
        /// Gets the edesk history.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        /// <exception cref="WebEasValidationException">null</exception>
        /// <exception cref="WebEasProxyException"></exception>
        public IdentityHistory[] GetEdeskHistory(GetEdeskHistoryRequestType request)
        {
            try
            {
                WebEas.Core.Sts.SecurityTokenServiceHelper.CheckService(ref proxy, ServiceUrl, stsThumbPrint);

                var stopwatch = new Stopwatch();
                stopwatch.Start();
                var response = proxy.GetEdeskHistory(new GetEdeskHistory(request));
                stopwatch.Stop();
                LogRequestDuration(ServiceUrl, stopwatch);
                return response.GetEdeskHistoryResponse1;
            }
            catch (FaultException<DcomFaultType> ex)
            {
                var msg = ex.Detail == null || string.IsNullOrEmpty(ex.Detail.faultMessage) ? ex.Message : ex.Detail.faultMessage;
                throw new WebEasValidationException(null, msg, ex);
            }
            catch (Exception ex)
            {
                CheckGlobalExceptions(ex);
                throw new WebEasProxyException($"Error in {System.Reflection.MethodBase.GetCurrentMethod().Name}", ex, request);
            }
        }

        /// <summary>
        /// Gets the objects from signed container.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        /// <exception cref="WebEasValidationException">null</exception>
        /// <exception cref="WebEasProxyException"></exception>
        public ObjectFromSignedContainer[] GetObjectsFromSignedContainer(GetObjectsFromSignedContainerRequest request)
        {
            try
            {
                WebEas.Core.Sts.SecurityTokenServiceHelper.CheckService(ref proxy, ServiceUrl, stsThumbPrint);

                var stopwatch = new Stopwatch();
                stopwatch.Start();
                var response = proxy.GetObjectsFromSignedContainer(new GetObjectsFromSignedContainerRequest1(request));
                stopwatch.Stop();
                LogRequestDuration(ServiceUrl, stopwatch);
                return response.GetObjectsFromSignedContainerResponse;
            }
            catch (FaultException<DcomFaultType> ex)
            {
                var msg = ex.Detail == null || string.IsNullOrEmpty(ex.Detail.faultMessage) ? ex.Message : ex.Detail.faultMessage;
                throw new WebEasValidationException(null, msg, ex);
            }
            catch (Exception ex)
            {
                CheckGlobalExceptions(ex);
                throw new WebEasProxyException($"Error in {System.Reflection.MethodBase.GetCurrentMethod().Name}", ex, request);
            }
        }

        /// <summary>
        /// Gets the sign certificate metadata.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        /// <exception cref="WebEasValidationException">null</exception>
        /// <exception cref="WebEasProxyException"></exception>
        public GetSignCertificateMetadataResponse GetSignCertificateMetadata(GetSignCertificateMetadataRequest request)
        {
            try
            {
                WebEas.Core.Sts.SecurityTokenServiceHelper.CheckService(ref proxy, ServiceUrl, stsThumbPrint);

                var stopwatch = new Stopwatch();
                stopwatch.Start();
                var response = proxy.GetSignCertificateMetadata(new GetSignCertificateMetadataRequest1(request));
                stopwatch.Stop();
                LogRequestDuration(ServiceUrl, stopwatch);
                return response.GetSignCertificateMetadataResponse;
            }
            catch (FaultException<DcomFaultType> ex)
            {
                var msg = ex.Detail == null || string.IsNullOrEmpty(ex.Detail.faultMessage) ? ex.Message : ex.Detail.faultMessage;
                throw new WebEasValidationException(null, msg, ex);
            }
            catch (Exception ex)
            {
                CheckGlobalExceptions(ex);
                throw new WebEasProxyException($"Error in {System.Reflection.MethodBase.GetCurrentMethod().Name}", ex, request);
            }
        }

        /// <summary>
        /// Gets the upvs identity.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        /// <exception cref="WebEasValidationException">null</exception>
        /// <exception cref="WebEasProxyException"></exception>
        public IdentityData GetUpvsIdentity(UpvsIdentifierType request)
        {
            try
            {
                WebEas.Core.Sts.SecurityTokenServiceHelper.CheckService(ref proxy, ServiceUrl, stsThumbPrint);

                var stopwatch = new Stopwatch();
                stopwatch.Start();
                var response = proxy.GetUpvsIdentity(new GetUpvsIdentity(request));
                stopwatch.Stop();
                LogRequestDuration(ServiceUrl, stopwatch);
                return response.IdentityDataResponse;
            }
            catch (FaultException<DcomFaultType> ex)
            {
                var msg = ex.Detail == null || string.IsNullOrEmpty(ex.Detail.faultMessage) ? ex.Message : ex.Detail.faultMessage;
                throw new WebEasValidationException(null, msg, ex);
            }
            catch (Exception ex)
            {
                CheckGlobalExceptions(ex);
                throw new WebEasProxyException($"Error in {System.Reflection.MethodBase.GetCurrentMethod().Name}", ex, request);
            }
        }

        /// <summary>
        /// Gets the upvs identity delegations.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        /// <exception cref="WebEasValidationException">null</exception>
        /// <exception cref="WebEasProxyException"></exception>
        public Delegation[] GetUpvsIdentityDelegations(GetDelegationsRequestType request)
        {
            try
            {
                WebEas.Core.Sts.SecurityTokenServiceHelper.CheckService(ref proxy, ServiceUrl, stsThumbPrint);

                var stopwatch = new Stopwatch();
                stopwatch.Start();
                var response = proxy.GetUpvsIdentityDelegations(new GetUpvsIdentityDelegations(request));
                stopwatch.Stop();
                LogRequestDuration(ServiceUrl, stopwatch);
                return response.GetDelegationsResponse;
            }
            catch (FaultException<DcomFaultType> ex)
            {
                var msg = ex.Detail == null || string.IsNullOrEmpty(ex.Detail.faultMessage) ? ex.Message : ex.Detail.faultMessage;
                throw new WebEasValidationException(null, msg, ex);
            }
            catch (Exception ex)
            {
                CheckGlobalExceptions(ex);
                throw new WebEasProxyException($"Error in {System.Reflection.MethodBase.GetCurrentMethod().Name}", ex, request);
            }
        }

        /// <summary>
        /// Gets the upvs identity roles.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        /// <exception cref="WebEasValidationException">null</exception>
        /// <exception cref="WebEasProxyException"></exception>
        public string[] GetUpvsIdentityRoles(GetUpvsIdentityRolesType request)
        {
            try
            {
                WebEas.Core.Sts.SecurityTokenServiceHelper.CheckService(ref proxy, ServiceUrl, stsThumbPrint);

                var stopwatch = new Stopwatch();
                stopwatch.Start();
                var response = proxy.GetUpvsIdentityRoles(new GetUpvsIdentityRoles(request));
                stopwatch.Stop();
                LogRequestDuration(ServiceUrl, stopwatch);
                return response.GetUpvsIdentityRolesResponse1;
            }
            catch (FaultException<DcomFaultType> ex)
            {
                var msg = ex.Detail == null || string.IsNullOrEmpty(ex.Detail.faultMessage) ? ex.Message : ex.Detail.faultMessage;
                throw new WebEasValidationException(null, msg, ex);
            }
            catch (Exception ex)
            {
                CheckGlobalExceptions(ex);
                throw new WebEasProxyException($"Error in {System.Reflection.MethodBase.GetCurrentMethod().Name}", ex, request);
            }
        }

        /// <summary>
        /// Determines whether is edesk deliverable.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        /// <exception cref="WebEasValidationException">null</exception>
        /// <exception cref="WebEasProxyException"></exception>
        public bool? IsEdeskDeliverable(IsEdeskDeliverableType request)
        {
            try
            {
                WebEas.Core.Sts.SecurityTokenServiceHelper.CheckService(ref proxy, ServiceUrl, stsThumbPrint);

                var stopwatch = new Stopwatch();
                stopwatch.Start();
                var response = proxy.IsEdeskDeliverable(new IsEdeskDeliverable(request));
                stopwatch.Stop();
                LogRequestDuration(ServiceUrl, stopwatch);
                return response.isDeliverable;
            }
            catch (FaultException<DcomFaultType> ex)
            {
                var msg = ex.Detail == null || string.IsNullOrEmpty(ex.Detail.faultMessage) ? ex.Message : ex.Detail.faultMessage;
                throw new WebEasValidationException(null, msg, ex);
            }
            catch (Exception ex)
            {
                CheckGlobalExceptions(ex);
                throw new WebEasProxyException($"Error in {System.Reflection.MethodBase.GetCurrentMethod().Name}", ex, request);
            }
        }

        /// <summary>
        /// Determines whether is edesk deliverable with upvs identifier.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        /// <exception cref="WebEasValidationException">null</exception>
        /// <exception cref="WebEasProxyException"></exception>
        public bool? IsEdeskDeliverableWithUpvsIdentifier(IsEdeskDeliverableWithUpvsIdentifierType request)
        {
            try
            {
                WebEas.Core.Sts.SecurityTokenServiceHelper.CheckService(ref proxy, ServiceUrl, stsThumbPrint);

                var stopwatch = new Stopwatch();
                stopwatch.Start();
                var response = proxy.IsEdeskDeliverableWithUpvsIdentifier(new IsEdeskDeliverableWithUpvsIdentifier(request));
                stopwatch.Stop();
                LogRequestDuration(ServiceUrl, stopwatch);
                return response.IsEdeskDeliverableWithUpvsIdentifierResponse1;
            }
            catch (FaultException<DcomFaultType> ex)
            {
                var msg = ex.Detail == null || string.IsNullOrEmpty(ex.Detail.faultMessage) ? ex.Message : ex.Detail.faultMessage;
                throw new WebEasValidationException(null, msg, ex);
            }
            catch (Exception ex)
            {
                CheckGlobalExceptions(ex);
                throw new WebEasProxyException($"Error in {System.Reflection.MethodBase.GetCurrentMethod().Name}", ex, request);
            }
        }

        /// <summary>
        /// Determines whether is upvs identity in role.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        /// <exception cref="WebEasValidationException">null</exception>
        /// <exception cref="WebEasProxyException"></exception>
        public bool? IsUpvsIdentityInRole(IsUpvsIdentityInRoleType request)
        {
            try
            {
                WebEas.Core.Sts.SecurityTokenServiceHelper.CheckService(ref proxy, ServiceUrl, stsThumbPrint);

                var stopwatch = new Stopwatch();
                stopwatch.Start();
                var response = proxy.IsUpvsIdentityInRole(new IsUpvsIdentityInRole(request));
                stopwatch.Stop();
                LogRequestDuration(ServiceUrl, stopwatch);
                return response.IsUpvsIdentityInRoleResponse1;
            }
            catch (FaultException<DcomFaultType> ex)
            {
                var msg = ex.Detail == null || string.IsNullOrEmpty(ex.Detail.faultMessage) ? ex.Message : ex.Detail.faultMessage;
                throw new WebEasValidationException(null, msg, ex);
            }
            catch (Exception ex)
            {
                CheckGlobalExceptions(ex);
                throw new WebEasProxyException($"Error in {System.Reflection.MethodBase.GetCurrentMethod().Name}", ex, request);
            }
        }

        /// <summary>
        /// Searchs the record in mdurz.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        /// <exception cref="WebEasValidationException">null</exception>
        /// <exception cref="WebEasProxyException"></exception>
        public MdurzSearchResultType SearchRecordInMdurz(MdurzSearchRequestType request)
        {
            try
            {
                WebEas.Core.Sts.SecurityTokenServiceHelper.CheckService(ref proxy, ServiceUrl, stsThumbPrint);

                var stopwatch = new Stopwatch();
                stopwatch.Start();
                var response = proxy.SearchRecordInMdurz(new SearchRecordInMdurz(request));
                stopwatch.Stop();
                LogRequestDuration(ServiceUrl, stopwatch);
                return response.SearchRecordInMdurzResponse1;
            }
            catch (FaultException<DcomFaultType> ex)
            {
                var msg = ex.Detail == null || string.IsNullOrEmpty(ex.Detail.faultMessage) ? ex.Message : ex.Detail.faultMessage;
                throw new WebEasValidationException(null, msg, ex);
            }
            catch (Exception ex)
            {
                CheckGlobalExceptions(ex);
                throw new WebEasProxyException($"Error in {System.Reflection.MethodBase.GetCurrentMethod().Name}", ex, request);
            }
        }

        /// <summary>
        /// Searchs the upvs identities.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        /// <exception cref="WebEasValidationException">null</exception>
        /// <exception cref="WebEasProxyException"></exception>
        public SearchIdentitiesResponse[] SearchUpvsIdentities(SearchUpvsIdentitiesRequestType request)
        {
            try
            {
                WebEas.Core.Sts.SecurityTokenServiceHelper.CheckService(ref proxy, ServiceUrl, stsThumbPrint);

                var stopwatch = new Stopwatch();
                stopwatch.Start();
                var response = proxy.SearchUpvsIdentities(new SearchUpvsIdentities(request));
                stopwatch.Stop();
                LogRequestDuration(ServiceUrl, stopwatch);
                return response.SearchUpvsIdentitiesResponse1;
            }
            catch (FaultException<DcomFaultType> ex)
            {
                var msg = ex.Detail == null || string.IsNullOrEmpty(ex.Detail.faultMessage) ? ex.Message : ex.Detail.faultMessage;
                throw new WebEasValidationException(null, msg, ex);
            }
            catch (Exception ex)
            {
                CheckGlobalExceptions(ex);
                throw new WebEasProxyException($"Error in {System.Reflection.MethodBase.GetCurrentMethod().Name}", ex, request);
            }
        }

        /// <summary>
        /// Sends the eform request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        /// <exception cref="WebEasValidationException">null</exception>
        /// <exception cref="WebEasProxyException"></exception>
        public SendEformRequestResponse SendEformRequest(UpvsCustomMessageType request)
        {
            try
            {
                WebEas.Core.Sts.SecurityTokenServiceHelper.CheckService(ref proxy, ServiceUrl, stsThumbPrint);

                var stopwatch = new Stopwatch();
                stopwatch.Start();
                var response = proxy.SendEformRequest(new SendEformRequest(request));
                stopwatch.Stop();
                LogRequestDuration(ServiceUrl, stopwatch);
                return response;
            }
            catch (FaultException<DcomFaultType> ex)
            {
                var msg = ex.Detail == null || string.IsNullOrEmpty(ex.Detail.faultMessage) ? ex.Message : ex.Detail.faultMessage;
                throw new WebEasValidationException(null, msg, ex);
            }
            catch (Exception ex)
            {
                CheckGlobalExceptions(ex);
                throw new WebEasProxyException($"Error in {System.Reflection.MethodBase.GetCurrentMethod().Name}", ex, request);
            }
        }

        /// <summary>
        /// Sends the messages.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        /// <exception cref="WebEasValidationException">null</exception>
        /// <exception cref="WebEasProxyException"></exception>
        public SendMessagesResponse SendMessages(OutgoingMessage[] request)
        {
            try
            {
                WebEas.Core.Sts.SecurityTokenServiceHelper.CheckService(ref proxy, ServiceUrl, stsThumbPrint);

                var stopwatch = new Stopwatch();
                stopwatch.Start();
                var response = proxy.SendMessages(new SendMessages(request));
                stopwatch.Stop();
                LogRequestDuration(ServiceUrl, stopwatch);
                return response;
            }
            catch (FaultException<DcomFaultType> ex)
            {
                var msg = ex.Detail == null || string.IsNullOrEmpty(ex.Detail.faultMessage) ? ex.Message : ex.Detail.faultMessage;
                throw new WebEasValidationException(null, msg, ex);
            }
            catch (Exception ex)
            {
                CheckGlobalExceptions(ex);
                throw new WebEasProxyException($"Error in {System.Reflection.MethodBase.GetCurrentMethod().Name}", ex, request);
            }
        }

        /// <summary>
        /// Sends the notification.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        /// <exception cref="WebEasValidationException">null</exception>
        /// <exception cref="WebEasProxyException"></exception>
        public SendNotificationResponse SendNotification(SendNotificationType request)
        {
            try
            {
                WebEas.Core.Sts.SecurityTokenServiceHelper.CheckService(ref proxy, ServiceUrl, stsThumbPrint);

                var stopwatch = new Stopwatch();
                stopwatch.Start();
                var response = proxy.SendNotification(new SendNotification(request));
                stopwatch.Stop();
                LogRequestDuration(ServiceUrl, stopwatch);
                return response;
            }
            catch (FaultException<DcomFaultType> ex)
            {
                var msg = ex.Detail == null || string.IsNullOrEmpty(ex.Detail.faultMessage) ? ex.Message : ex.Detail.faultMessage;
                throw new WebEasValidationException(null, msg, ex);
            }
            catch (Exception ex)
            {
                CheckGlobalExceptions(ex);
                throw new WebEasProxyException($"Error in {System.Reflection.MethodBase.GetCurrentMethod().Name}", ex, request);
            }
        }

        /// <summary>
        /// Sends the upvs message container.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        /// <exception cref="WebEasValidationException">null</exception>
        /// <exception cref="WebEasProxyException"></exception>
        public SendUpvsMessageContainerResponse SendUpvsMessageContainer(UpvsMessageContainerType request)
        {
            try
            {
                WebEas.Core.Sts.SecurityTokenServiceHelper.CheckService(ref proxy, ServiceUrl, stsThumbPrint);

                var stopwatch = new Stopwatch();
                stopwatch.Start();
                var response = proxy.SendUpvsMessageContainer(new SendUpvsMessageContainer(request));
                stopwatch.Stop();
                LogRequestDuration(ServiceUrl, stopwatch);
                return response;
            }
            catch (FaultException<DcomFaultType> ex)
            {
                var msg = ex.Detail == null || string.IsNullOrEmpty(ex.Detail.faultMessage) ? ex.Message : ex.Detail.faultMessage;
                throw new WebEasValidationException(null, msg, ex);
            }
            catch (Exception ex)
            {
                CheckGlobalExceptions(ex);
                throw new WebEasProxyException($"Error in {System.Reflection.MethodBase.GetCurrentMethod().Name}", ex, request);
            }
        }

        /// <summary>
        /// Sends the upvs message container with timeout.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        /// <exception cref="WebEasValidationException">null</exception>
        /// <exception cref="WebEasProxyException"></exception>
        public SendUpvsMessageContainerWithTimeoutResponse SendUpvsMessageContainerWithTimeout(UpvsMessageContainerWrapperWithTimeoutType request)
        {
            try
            {
                WebEas.Core.Sts.SecurityTokenServiceHelper.CheckService(ref proxy, ServiceUrl, stsThumbPrint);

                var stopwatch = new Stopwatch();
                stopwatch.Start();
                var response = proxy.SendUpvsMessageContainerWithTimeout(new SendUpvsMessageContainerWithTimeout(request));
                stopwatch.Stop();
                LogRequestDuration(ServiceUrl, stopwatch);
                return response;
            }
            catch (FaultException<DcomFaultType> ex)
            {
                var msg = ex.Detail == null || string.IsNullOrEmpty(ex.Detail.faultMessage) ? ex.Message : ex.Detail.faultMessage;
                throw new WebEasValidationException(null, msg, ex);
            }
            catch (Exception ex)
            {
                CheckGlobalExceptions(ex);
                throw new WebEasProxyException($"Error in {System.Reflection.MethodBase.GetCurrentMethod().Name}", ex, request);
            }
        }

        /// <summary>
        /// Signs the objects.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        /// <exception cref="WebEasValidationException">null</exception>
        /// <exception cref="WebEasProxyException"></exception>
        public SignedObject[] SignObjects(SignObjectsRequest request)
        {
            try
            {
                WebEas.Core.Sts.SecurityTokenServiceHelper.CheckService(ref proxy, ServiceUrl, stsThumbPrint);

                var stopwatch = new Stopwatch();
                stopwatch.Start();
                var response = proxy.SignObjects(new SignObjectsRequest1(request));
                stopwatch.Stop();
                LogRequestDuration(ServiceUrl, stopwatch);
                return response.SignObjectsResponse;
            }
            catch (FaultException<DcomFaultType> ex)
            {
                var msg = ex.Detail == null || string.IsNullOrEmpty(ex.Detail.faultMessage) ? ex.Message : ex.Detail.faultMessage;
                throw new WebEasValidationException(null, msg, ex);
            }
            catch (Exception ex)
            {
                CheckGlobalExceptions(ex);
                throw new WebEasProxyException($"Error in {System.Reflection.MethodBase.GetCurrentMethod().Name}", ex, request);
            }
        }

        /// <summary>
        /// Validates the form status.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        /// <exception cref="WebEasValidationException">null</exception>
        /// <exception cref="WebEasProxyException"></exception>
        public ValidateFormStatusResponse ValidateFormStatus(FormTemplateID request)
        {
            try
            {
                WebEas.Core.Sts.SecurityTokenServiceHelper.CheckService(ref proxy, ServiceUrl, stsThumbPrint);

                var stopwatch = new Stopwatch();
                stopwatch.Start();
                var response = proxy.ValidateFormStatus(new ValidateFormStatus(request));
                stopwatch.Stop();
                LogRequestDuration(ServiceUrl, stopwatch);
                return response;
            }
            catch (FaultException<DcomFaultType> ex)
            {
                var msg = ex.Detail == null || string.IsNullOrEmpty(ex.Detail.faultMessage) ? ex.Message : ex.Detail.faultMessage;
                throw new WebEasValidationException(null, msg, ex);
            }
            catch (Exception ex)
            {
                CheckGlobalExceptions(ex);
                throw new WebEasProxyException($"Error in {System.Reflection.MethodBase.GetCurrentMethod().Name}", ex, request);
            }
        }

        /// <summary>
        /// Validates the PDF.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        /// <exception cref="WebEasValidationException">null</exception>
        /// <exception cref="WebEasProxyException"></exception>
        public ValidatePDFResponse ValidatePDF(PDFConversionValidationRequest request)
        {
            try
            {
                WebEas.Core.Sts.SecurityTokenServiceHelper.CheckService(ref proxy, ServiceUrl, stsThumbPrint);

                var stopwatch = new Stopwatch();
                stopwatch.Start();
                var response = proxy.ValidatePDF(new ValidatePDFRequest(request));
                stopwatch.Stop();
                LogRequestDuration(ServiceUrl, stopwatch);
                return response.ValidatePDFResponse;
            }
            catch (FaultException<DcomFaultType> ex)
            {
                var msg = ex.Detail == null || string.IsNullOrEmpty(ex.Detail.faultMessage) ? ex.Message : ex.Detail.faultMessage;
                throw new WebEasValidationException(null, msg, ex);
            }
            catch (Exception ex)
            {
                CheckGlobalExceptions(ex);
                throw new WebEasProxyException($"Error in {System.Reflection.MethodBase.GetCurrentMethod().Name}", ex, request);
            }
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        /// <exception cref="WebEasException">Error in dispose</exception>
        public void Dispose()
        {
            try
            {
                if (proxy != null && proxy is IDisposable)
                {
                    if (proxy.State == CommunicationState.Faulted)
                    {
                        proxy.Abort();
                    }

                    proxy.Dispose();
                }
                proxy = null;
            }
            catch (Exception ex)
            {
                throw new WebEasException("Error in dispose", ex);
            }
        }
    }
}