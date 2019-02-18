using System;
using System.Diagnostics;
using System.Linq;
using System.ServiceModel;

namespace WebEas.Services.Esb
{
    /// <summary>
    /// DMS Document proxy
    /// </summary>
    /// <example>
    /// using(DmsDocumentProxy proxy = new DmsDocumentProxy())
    /// {
    /// }
    /// </example>
    public class DmsDocumentProxy : ProxyBase, IDisposable
    {
        private const string ServiceUrl = "https://lbsoa.intra.dcom.sk/soa/dms/document/1.0/documentservice";
        //private const string openAMSessionId = "AQIC5wM2LY4Sfcx5Ag7Qaezwk0RrSwdNHEQNKCNCwwsR52I.*AAJTSQACMDIAAlNLABQtNTI5ODkxOTM4MDYxNDU2ODk0OAACUzEAAjAx*";

        //private Dms.Document.DocumentServiceClient client;
        private Dms.Document.DocumentServiceChannel proxy;

        /// <summary>
        /// Initializes a new instance of the <see cref="DmsDocumentProxy" /> class.
        /// </summary>
        public DmsDocumentProxy(string stsThumbprint) : base(stsThumbprint)
        {
            this.proxy = WebEas.Core.Sts.SecurityTokenServiceHelper.CreateChannelProxy<Dms.Document.DocumentServiceChannel>(ServiceUrl, stsThumbprint);
            //if (https)
            //{
            //    this.client = new Dms.Document.DocumentServiceClient(new CustomBinding(new WebEas.Client.Encoder.WebEasTextMessageBindingElement("UTF-8", "text/xml", MessageVersion.Soap11), new HttpTransportBindingElement()), new System.ServiceModel.EndpointAddress(ServiceUrl));
            //    this.client.Endpoint.EndpointBehaviors.Add(new WebEas.Client.Encoder.EmptyActionBehaviour());
            //    System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) { return true; };
            //}
            //else
            //{
            //    HttpTransportBindingElement httpTransport = new HttpTransportBindingElement();
            //    CustomBinding binding = new CustomBinding();
            //    TextMessageEncodingBindingElement oTextMessageEncod = new TextMessageEncodingBindingElement();
            //    oTextMessageEncod.MessageVersion = MessageVersion.Soap12;
            //    oTextMessageEncod.WriteEncoding = System.Text.Encoding.UTF8;
            //    binding.Elements.Add(oTextMessageEncod);
            //    binding.Elements.Add(httpTransport);
            //    this.client = new Dms.Document.DocumentServiceClient(binding, new System.ServiceModel.EndpointAddress(ServiceUrl));
            //    this.client.Endpoint.EndpointBehaviors.Add(new WebEas.Client.Encoder.EmptyActionBehaviour());
            //}
            //SecurityToken stsToken = SecurityTokenServiceHelper.GetSTSToken(openAMSessionId);
            //CreateChannelProxy(stsToken);
        }

        //private void CreateChannelProxy(SecurityToken token)
        //{
        //    var binding = new WS2007FederationHttpBinding(
        //      WSFederationHttpSecurityMode.TransportWithMessageCredential);
        //    binding.Security.Message.EstablishSecurityContext = false;
        //    // set up channel factory
        //    var factory = new ChannelFactory<Dms.Document.DocumentServiceChannel>(
        //      binding,
        //      new EndpointAddress(ServiceUrl));

        //    factory.Credentials.SupportInteractive = false;
        //    factory.Credentials.UseIdentityConfiguration = true;
        //    // create channel with specified token
        //    this.proxy = factory.CreateChannelWithIssuedToken(token);
        //} 
        /// <summary>
        /// Moves the folder documents.
        /// </summary>
        /// <param name="tenant">The tenant.</param>
        /// <param name="folderId">The folder id.</param>
        /// <param name="newFolderId">The new folder id.</param>
        public void MoveFolderDocuments(string tenant, long folderId, long newFolderId)
        {
            try
            {
                WebEas.Core.Sts.SecurityTokenServiceHelper.CheckService(ref proxy, ServiceUrl, stsThumbPrint);

                var stopwatch = new Stopwatch();
                stopwatch.Start();
                this.proxy.moveFolderDocuments(tenant, folderId, newFolderId);
                stopwatch.Stop();
                LogRequestDuration(ServiceUrl, stopwatch);
            }
            catch (FaultException<Dms.Document.DMSFault> ex)
            {
                throw new WebEasProxyException(null, ex.Message, ex, tenant, folderId, newFolderId);
            }
            catch (FaultException ex)
            {
                throw new WebEasProxyException("Error in MoveFolderDocuments", ex, tenant, folderId, newFolderId);
            }
            catch (Exception ex)
            {
                this.CheckGlobalExceptions(ex);
                throw new WebEasProxyException("Error in MoveFolderDocuments", ex, tenant, folderId, newFolderId);
            }
        }

        /// <summary>
        /// Reads the folders.
        /// </summary>
        /// <param name="tenant">The tenant.</param>
        /// <param name="folderId">The folder id.</param>
        /// <returns></returns>
        public WebEas.Services.Esb.Dms.Document.folder[] ReadFolders(string tenant, long folderId)
        {
            try
            {
                WebEas.Core.Sts.SecurityTokenServiceHelper.CheckService(ref proxy, ServiceUrl, stsThumbPrint);

                var readFoldersRequest = new Dms.Document.readFolders()
                {
                    folderId = folderId,
                    tenant = tenant
                };
                var stopwatch = new Stopwatch();
                stopwatch.Start();
                var res = this.proxy.readFolders(readFoldersRequest).@return;
                stopwatch.Stop();
                LogRequestDuration(ServiceUrl, stopwatch);
                return res;
            }
            catch (FaultException<Dms.Document.DMSFault> ex)
            {
                throw new WebEasProxyException(null, ex.Message, ex, tenant, folderId);
            }
            catch (FaultException ex)
            {
                throw new WebEasProxyException("Error in ReadFolders", ex, tenant, folderId);
            }
            catch (Exception ex)
            {
                this.CheckGlobalExceptions(ex);
                throw new WebEasProxyException("Error in ReadFolders", ex, tenant, folderId);
            }
        }

        /// <summary>
        /// Moves the folder.
        /// </summary>
        /// <param name="tenant">The tenant.</param>
        /// <param name="folderId">The folder id.</param>
        /// <param name="parentFolderId">The parent folder id.</param>
        public void MoveFolder(string tenant, long folderId, long parentFolderId)
        {
            try
            {
                WebEas.Core.Sts.SecurityTokenServiceHelper.CheckService(ref proxy, ServiceUrl, stsThumbPrint);

                var stopwatch = new Stopwatch();
                stopwatch.Start();
                this.proxy.moveFolder(tenant, folderId, parentFolderId);
                stopwatch.Stop();
                LogRequestDuration(ServiceUrl, stopwatch);
            }
            catch (FaultException<Dms.Document.DMSFault> ex)
            {
                throw new WebEasProxyException(null, ex.Message, ex, tenant, folderId, parentFolderId);
            }
            catch (FaultException ex)
            {
                throw new WebEasProxyException("Error in MoveFolder", ex, tenant, folderId, parentFolderId);
            }
            catch (Exception ex)
            {
                this.CheckGlobalExceptions(ex);
                throw new WebEasProxyException("Error in MoveFolder", ex, tenant, folderId, parentFolderId);
            }
        }

        /// <summary>
        /// Reads the document by id.
        /// </summary>
        /// <param name="tenant">The tenant.</param>
        /// <param name="documentId">The document id.</param>
        /// <param name="version">The version.</param>
        /// <param name="includeContent">Content of the include.</param>
        /// <returns></returns>
        public WebEas.Services.Esb.Dms.Document.document ReadDocumentById(string tenant, string documentId, string version)
        {
            try
            {
                WebEas.Core.Sts.SecurityTokenServiceHelper.CheckService(ref proxy, ServiceUrl, stsThumbPrint);

                var stopwatch = new Stopwatch();
                stopwatch.Start();
                var res = this.proxy.readDocumentById(tenant, documentId, version);
                stopwatch.Stop();
                LogRequestDuration(ServiceUrl, stopwatch);
                return res;
            }
            catch (FaultException<Dms.Document.DMSFault> ex)
            {
                throw new WebEasProxyException(null, ex.Message, ex, tenant, documentId, version);
            }
            catch (FaultException ex)
            {
                throw new WebEasProxyException("Error in ReadDocumentById", ex, tenant, documentId, version);
            }
            catch (Exception ex)
            {
                this.CheckGlobalExceptions(ex);
                throw new WebEasProxyException("Error in ReadDocumentById", ex, tenant, documentId, version);
            }
        }

        /// <summary>
        /// Finds the documents.
        /// </summary>
        /// <param name="tenant">The tenant.</param>
        /// <param name="query">The query.</param>
        /// <param name="metadata">The metadata.</param>
        /// <param name="highlight">The highlight.</param>
        /// <param name="fromPage">From page.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <returns></returns>
        public WebEas.Services.Esb.Dms.Document.searchResult FindDocuments(string tenant, string query, WebEas.Services.Esb.Dms.Document.metadata[] metadata, bool highlight, long fromPage, long pageSize)
        {
            try
            {
                WebEas.Core.Sts.SecurityTokenServiceHelper.CheckService(ref proxy, ServiceUrl, stsThumbPrint);

                var findDocumentsRequest = new Dms.Document.findDocuments()
                {
                    tenant = tenant,
                    query = query,
                    metadata = metadata,
                    highlight = highlight,
                    fromPage = fromPage,
                    pageSize = pageSize
                };
                var stopwatch = new Stopwatch();
                stopwatch.Start();
                var res = this.proxy.findDocuments(findDocumentsRequest).@return;
                stopwatch.Stop();
                LogRequestDuration(ServiceUrl, stopwatch);
                return res;
            }
            catch (FaultException<Dms.Document.DMSFault> ex)
            {
                throw new WebEasProxyException(null, ex.Message, ex, tenant, query, metadata, highlight, fromPage, pageSize);
            }
            catch (FaultException ex)
            {
                throw new WebEasProxyException("Error in FindDocuments", ex, tenant, query, metadata, highlight, fromPage, pageSize);
            }
            catch (Exception ex)
            {
                this.CheckGlobalExceptions(ex);
                throw new WebEasProxyException("Error in FindDocuments", ex, tenant, query, metadata, highlight, fromPage, pageSize);
            }
        }

        /// <summary>
        /// Reads the folder documents.
        /// </summary>
        /// <param name="tenant">The tenant.</param>
        /// <param name="folderId">The folder id.</param>
        /// <returns></returns>
        public WebEas.Services.Esb.Dms.Document.document[] ReadFolderDocuments(string tenant, long folderId)
        {
            try
            {
                WebEas.Core.Sts.SecurityTokenServiceHelper.CheckService(ref proxy, ServiceUrl, stsThumbPrint);

                var readFolderDocumentsRequest = new Dms.Document.readFolderDocuments()
                {
                    tenant = tenant,
                    folderId = folderId
                };
                var stopwatch = new Stopwatch();
                stopwatch.Start();
                var res = this.proxy.readFolderDocuments(readFolderDocumentsRequest).@return;
                stopwatch.Stop();
                LogRequestDuration(ServiceUrl, stopwatch);
                return res;
            }
            catch (FaultException<Dms.Document.DMSFault> ex)
            {
                throw new WebEasProxyException(null, ex.Message, ex, tenant, folderId);
            }
            catch (FaultException ex)
            {
                throw new WebEasProxyException("Error in ReadFolderDocuments", ex, tenant, folderId);
            }
            catch (Exception ex)
            {
                this.CheckGlobalExceptions(ex);
                throw new WebEasProxyException("Error in ReadFolderDocuments", ex, tenant, folderId);
            }
        }

        /// <summary>
        /// Creates the document.
        /// </summary>
        /// <param name="tenant">The tenant.</param>
        /// <param name="categoryId">The category id.</param>
        /// <param name="document">The document.</param>
        /// <param name="content">The content.</param>
        /// <returns></returns>
        public string CreateDocument(string tenant, string categoryId, WebEas.Services.Esb.Dms.Document.document document, byte[] content)
        {
            try
            {
                WebEas.Core.Sts.SecurityTokenServiceHelper.CheckService(ref proxy, ServiceUrl, stsThumbPrint);

                var request = new Dms.Document.createDocument()
                {
                    tenant = tenant,
                    categoryId = categoryId,
                    document = document,
                    content = content
                };
                var stopwatch = new Stopwatch();
                stopwatch.Start();
                var res = this.proxy.createDocument(request).@return;
                stopwatch.Stop();
                LogRequestDuration(ServiceUrl, stopwatch);
                return res;
            }
            catch (FaultException<Dms.Document.DMSFault> ex)
            {
                throw new WebEasProxyException(null, ex.Message, ex, ex, tenant, categoryId, document, content);
            }
            catch (FaultException ex)
            {
                throw new WebEasProxyException("Error in CreateDocument", ex, ex, tenant, categoryId, document, content);
            }
            catch (Exception ex)
            {
                this.CheckGlobalExceptions(ex);
                throw new WebEasProxyException("Error in CreateDocument", ex, tenant, categoryId, document, content);
            }
        }

        /// <summary>
        /// Clears the metadata.
        /// </summary>
        /// <param name="tenant">The tenant.</param>
        /// <param name="documentId">The document id.</param>
        /// <returns></returns>
        public bool ClearMetadata(string tenant, string documentId)
        {
            try
            {
                WebEas.Core.Sts.SecurityTokenServiceHelper.CheckService(ref proxy, ServiceUrl, stsThumbPrint);

                var stopwatch = new Stopwatch();
                stopwatch.Start();
                var res = this.proxy.clearMetadata(tenant, documentId);
                stopwatch.Stop();
                LogRequestDuration(ServiceUrl, stopwatch);
                return res;
            }
            catch (FaultException<Dms.Document.DMSFault> ex)
            {
                throw new WebEasProxyException(null, ex.Message, ex, ex, tenant, documentId);
            }
            catch (FaultException ex)
            {
                throw new WebEasProxyException("Error in ClearMetadata", ex, ex, tenant, documentId);
            }
            catch (Exception ex)
            {
                this.CheckGlobalExceptions(ex);
                throw new WebEasProxyException("Error in ClearMetadata", ex, tenant, documentId);
            }
        }

        /// <summary>
        /// Deletes the folder.
        /// </summary>
        /// <param name="tenant">The tenant.</param>
        /// <param name="folderId">The folder id.</param>
        /// <returns></returns>
        public bool DeleteFolder(string tenant, long folderId)
        {
            try
            {
                WebEas.Core.Sts.SecurityTokenServiceHelper.CheckService(ref proxy, ServiceUrl, stsThumbPrint);

                var stopwatch = new Stopwatch();
                stopwatch.Start();
                var res = this.proxy.deleteFolder(tenant, folderId);
                stopwatch.Stop();
                LogRequestDuration(ServiceUrl, stopwatch);
                return res;
            }
            catch (FaultException<Dms.Document.DMSFault> ex)
            {
                throw new WebEasProxyException(null, ex.Message, ex, ex, tenant, folderId);
            }
            catch (FaultException ex)
            {
                throw new WebEasProxyException("Error in DeleteFolder", ex, ex, tenant, folderId);
            }
            catch (Exception ex)
            {
                this.CheckGlobalExceptions(ex);
                throw new WebEasProxyException("Error in DeleteFolder", ex, tenant, folderId);
            }
        }

        /// <summary>
        /// Reads the content of the document.
        /// </summary>
        /// <param name="tenant">The tenant.</param>
        /// <param name="documentId">The document id.</param>
        /// <returns></returns>
        public byte[] ReadDocumentContent(string tenant, string documentId)
        {
            try
            {
                WebEas.Core.Sts.SecurityTokenServiceHelper.CheckService(ref proxy, ServiceUrl, stsThumbPrint);

                var request = new Dms.Document.readDocumentContent()
                {
                    tenant = tenant,
                    documentId = documentId
                };
                var stopwatch = new Stopwatch();
                stopwatch.Start();
                var res = this.proxy.readDocumentContent(request).@return;
                stopwatch.Stop();
                LogRequestDuration(ServiceUrl, stopwatch);
                return res;
            }
            catch (FaultException<Dms.Document.DMSFault> ex)
            {
                throw new WebEasProxyException(null, ex.Message, ex, tenant, documentId);
            }
            catch (FaultException ex)
            {
                throw new WebEasProxyException("Error in ReadDocumentContent", ex, tenant, documentId);
            }
            catch (Exception ex)
            {
                this.CheckGlobalExceptions(ex);
                throw new WebEasProxyException("Error in ReadDocumentContent", ex, tenant, documentId);
            }
        }

        /// <summary>
        /// Deletes the document.
        /// </summary>
        /// <param name="tenant">The tenant.</param>
        /// <param name="documentId">The document id.</param>
        public void DeleteDocument(string tenant, string documentId)
        {
            try
            {
                WebEas.Core.Sts.SecurityTokenServiceHelper.CheckService(ref proxy, ServiceUrl, stsThumbPrint);

                var stopwatch = new Stopwatch();
                stopwatch.Start();
                this.proxy.deleteDocument(tenant, documentId);
                stopwatch.Stop();
                LogRequestDuration(ServiceUrl, stopwatch);
            }
            catch (FaultException<Dms.Document.DMSFault> ex)
            {
                throw new WebEasProxyException(null, ex.Message, ex, tenant, documentId);
            }
            catch (FaultException ex)
            {
                throw new WebEasProxyException("Error in DeleteDocument", ex, tenant, documentId);
            }
            catch (Exception ex)
            {
                this.CheckGlobalExceptions(ex);
                throw new WebEasProxyException("Error in DeleteDocument", ex, tenant, documentId);
            }
        }

        /// <summary>
        /// Finds all documents.
        /// </summary>
        /// <param name="tenant">The tenant.</param>
        /// <param name="query">The query.</param>
        /// <param name="metadata">The metadata.</param>
        /// <param name="highlight">The highlight.</param>
        /// <returns></returns>
        public WebEas.Services.Esb.Dms.Document.searchResult FindAllDocuments(string tenant, string query, WebEas.Services.Esb.Dms.Document.metadata[] metadata, bool highlight)
        {
            try
            {
                WebEas.Core.Sts.SecurityTokenServiceHelper.CheckService(ref proxy, ServiceUrl, stsThumbPrint);

                var request = new Dms.Document.findAllDocuments()
                {
                    tenant = tenant,
                    query = query,
                    metadata = metadata,
                    highlight = highlight
                };
                var stopwatch = new Stopwatch();
                stopwatch.Start();
                var res = this.proxy.findAllDocuments(request).@return;
                stopwatch.Stop();
                LogRequestDuration(ServiceUrl, stopwatch);
                return res;
                //Dms.Document.findAllDocuments request = new Dms.Document.findAllDocuments();
                //request.tenant = tenant;
                //request.query = query;
                //request.metadata = metadata;
                //request.highlight = highlight;
                //Dms.Document.findAllDocumentsResponse response = this.proxy.findAllDocuments(request);
                //return response.@return;
            }
            catch (FaultException<Dms.Document.DMSFault> ex)
            {
                throw new WebEasProxyException(null, ex.Message, ex, tenant, query, metadata, highlight);
            }
            catch (FaultException ex)
            {
                throw new WebEasProxyException("Error in FindAllDocuments", ex, tenant, query, metadata, highlight);
            }
            catch (Exception ex)
            {
                this.CheckGlobalExceptions(ex);
                throw new WebEasProxyException("Error in FindAllDocuments", ex, tenant, query, metadata, highlight);
            }
        }

        /// <summary>
        /// Creates the folder.
        /// </summary>
        /// <param name="tenant">The tenant.</param>
        /// <param name="parentFolderId">The parent folder id.</param>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        public long CreateFolder(string tenant, long parentFolderId, string name)
        {
            try
            {
                WebEas.Core.Sts.SecurityTokenServiceHelper.CheckService(ref proxy, ServiceUrl, stsThumbPrint);

                var stopwatch = new Stopwatch();
                stopwatch.Start();
                var res = this.proxy.createFolder(tenant, parentFolderId, name);
                stopwatch.Stop();
                LogRequestDuration(ServiceUrl, stopwatch);
                return res;
            }
            catch (FaultException<Dms.Document.DMSFault> ex)
            {
                throw new WebEasProxyException(null, ex.Message, ex, tenant, parentFolderId, name);
            }
            catch (FaultException ex)
            {
                throw new WebEasProxyException("Error in CreateFolder", ex, tenant, parentFolderId, name);
            }
            catch (Exception ex)
            {
                this.CheckGlobalExceptions(ex);
                throw new WebEasProxyException("Error in CreateFolder", ex, tenant, parentFolderId, name);
            }
        }

        /// <summary>
        /// Renames the folder.
        /// </summary>
        /// <param name="tenant">The tenant.</param>
        /// <param name="folderId">The folder id.</param>
        /// <param name="name">The name.</param>
        public void RenameFolder(string tenant, long folderId, string name)
        {
            try
            {
                WebEas.Core.Sts.SecurityTokenServiceHelper.CheckService(ref proxy, ServiceUrl, stsThumbPrint);

                var stopwatch = new Stopwatch();
                stopwatch.Start();
                this.proxy.renameFolder(tenant, folderId, name);
                stopwatch.Stop();
                LogRequestDuration(ServiceUrl, stopwatch);
            }
            catch (FaultException<Dms.Document.DMSFault> ex)
            {
                throw new WebEasProxyException(null, ex.Message, ex, tenant, folderId, name);
            }
            catch (FaultException ex)
            {
                throw new WebEasProxyException("Error in RenameFolder", ex, tenant, folderId, name);
            }
            catch (Exception ex)
            {
                this.CheckGlobalExceptions(ex);
                throw new WebEasProxyException("Error in RenameFolder", ex, tenant, folderId, name);
            }
        }

        /// <summary>
        /// Updates the document.
        /// </summary>
        /// <param name="tenant">The tenant.</param>
        /// <param name="document">The document.</param>
        /// <param name="content">The content.</param>
        public void UpdateDocument(string tenant, WebEas.Services.Esb.Dms.Document.document document, byte[] content)
        {
            try
            {
                WebEas.Core.Sts.SecurityTokenServiceHelper.CheckService(ref proxy, ServiceUrl, stsThumbPrint);

                var request = new Dms.Document.updateDocument()
                {
                    tenant = tenant,
                    document = document,
                    content = content
                };
                var stopwatch = new Stopwatch();
                stopwatch.Start();
                this.proxy.updateDocument(request);
                stopwatch.Stop();
                LogRequestDuration(ServiceUrl, stopwatch);
            }
            catch (FaultException<Dms.Document.DMSFault> ex)
            {
                throw new WebEasProxyException(null, ex.Message, ex, tenant, document, content);
            }
            catch (FaultException ex)
            {
                throw new WebEasProxyException("Error in UpdateDocument", ex, tenant, document, content);
            }
            catch (Exception ex)
            {
                this.CheckGlobalExceptions(ex);
                throw new WebEasProxyException("Error in UpdateDocument", ex, tenant, document, content);
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