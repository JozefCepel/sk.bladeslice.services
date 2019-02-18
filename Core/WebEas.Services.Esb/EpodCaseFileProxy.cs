using System;
using System.Diagnostics;
using System.Linq;
using System.ServiceModel;
using WebEas.Services.Esb.Epod.CaseFile;

namespace WebEas.Services.Esb
{
    /// <summary>
    /// Epodatelna case file proxy
    /// </summary>
    /// <example>
    /// using(EpodCaseFileProxy proxy = new EpodCaseFileProxy())
    /// {
    /// }
    /// </example>
    public class EpodCaseFileProxy : ProxyBase, IDisposable
    {
        private const string ServiceUrl = "https://lbsoa.intra.dcom.sk/soa/epodatelna/casefiles/1.0/casefiles";

        private Epod.CaseFile.CaseFileServicesChannel proxy;

        /// <summary>
        /// Initializes a new instance of the <see cref="EpodCaseFileProxy" /> class.
        /// </summary>
        public EpodCaseFileProxy(string stsThumbprint)
            : base(stsThumbprint)
        {
            this.proxy = WebEas.Core.Sts.SecurityTokenServiceHelper.CreateChannelProxy<Epod.CaseFile.CaseFileServicesChannel>(ServiceUrl, stsThumbprint);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EpodCaseFileProxy" /> class.
        /// </summary>
        public EpodCaseFileProxy(string stsThumbprint, IWebEasSession session)
            : base(stsThumbprint)
        {
            this.proxy = WebEas.Core.Sts.SecurityTokenServiceHelper.CreateChannelProxy<Epod.CaseFile.CaseFileServicesChannel>(session, ServiceUrl, stsThumbprint);
        }

        /// <summary>
        /// Inserts the bulk record.
        /// </summary>
        /// <param name="insertBulkRecordDto">The insert bulk record dto.</param>
        /// <returns></returns>
        public WebEas.Services.Esb.Epod.CaseFile.InsertBulkRecordResponseDto InsertBulkRecord(WebEas.Services.Esb.Epod.CaseFile.InsertBulkRecordDtoList insertBulkRecordDto)
        {
            try
            {
                WebEas.Core.Sts.SecurityTokenServiceHelper.CheckService(ref this.proxy, ServiceUrl, this.stsThumbPrint);

                var request = new insertBulkRecord(insertBulkRecordDto);
                var stopwatch = new Stopwatch();
                stopwatch.Start();
                insertBulkRecordResponse response = this.proxy.insertBulkRecord(request);
                stopwatch.Stop();
                LogRequestDuration(ServiceUrl, stopwatch);
                return response.insertBulkRecordResponseDto;
            }
            catch (FaultException<WebEas.Services.Esb.Epod.CaseFile.DcomFault> ex)
            {
                string message = "DcomFault - InsertBulkRecord";
                if (ex.Detail != null)
                {
                    message += string.Format(" {0}({1})", ex.Detail.faultMessage, ex.Detail.faultCode);

                    if (ex.Detail.faultMessage.Contains("DMS_NOT_AVAILABLE"))
                    {
                        throw new WebEasProxyException(message, "Úložisko dokumentov nie je dostupné (DMS)", ex, insertBulkRecordDto);
                    }
                }

                throw new WebEasProxyException(message, string.Format("Nastala chyba pri volaní služby podateľne - {0}", ex.Detail.faultMessage), ex, insertBulkRecordDto);
            }
            catch (Exception ex)
            {
                this.CheckGlobalExceptions(ex);
                throw new WebEasProxyException("Error in InsertBulkRecord", "Nastala chyba pri volaní služby podateľne", ex, insertBulkRecordDto);
            }
        }

        /// <summary>
        /// Renumbers the files.
        /// </summary>
        /// <param name="fileRenumberingDtoList">The file renumbering dto list.</param>
        public void RenumberFiles(WebEas.Services.Esb.Epod.CaseFile.FileRenumberingDto[] fileRenumberingDtoList)
        {
            try
            {
                WebEas.Core.Sts.SecurityTokenServiceHelper.CheckService(ref this.proxy, ServiceUrl, this.stsThumbPrint);

                var request = new renumberFiles(fileRenumberingDtoList);
                var stopwatch = new Stopwatch();
                stopwatch.Start();
                this.proxy.renumberFiles(request);
                stopwatch.Stop();
                LogRequestDuration(ServiceUrl, stopwatch);
            }
            catch (FaultException<WebEas.Services.Esb.Epod.CaseFile.DcomFault> ex)
            {
                string message = "DcomFault - RenumberFiles";
                if (ex.Detail != null)
                {
                    message += string.Format(" {0}({1})", ex.Detail.faultMessage, ex.Detail.faultCode);
                    if (ex.Detail.faultMessage.Contains("DMS_NOT_AVAILABLE"))
                    {
                        throw new WebEasProxyException(message, "Úložisko dokumentov nie je dostupné (DMS)", ex, fileRenumberingDtoList);
                    }
                }

                throw new WebEasProxyException(message, string.Format("Nastala chyba pri volaní služby podateľne - {0}", ex.Detail.faultMessage), ex, fileRenumberingDtoList);
            }
            catch (Exception ex)
            {
                this.CheckGlobalExceptions(ex);
                throw new WebEasProxyException("Error in RenumberFiles", "Nastala chyba pri volaní služby podateľne", ex, fileRenumberingDtoList);
            }
        }

        /// <summary>
        /// Notifies the record related event.
        /// </summary>
        /// <param name="events">The events.</param>
        /// <returns></returns>
        public EventProcessingResultDto[] NotifyRecordRelatedEvent(EventDto[] events)
        {
            try
            {
                WebEas.Core.Sts.SecurityTokenServiceHelper.CheckService(ref this.proxy, ServiceUrl, this.stsThumbPrint);

                var request = new notifyRecordRelatedEvent(events);
                var stopwatch = new Stopwatch();
                stopwatch.Start();
                notifyRecordRelatedEventResponse response = this.proxy.notifyRecordRelatedEvent(request);
                stopwatch.Stop();
                LogRequestDuration(ServiceUrl, stopwatch);
                return response.notifyRecordRelatedEventResponseDto;
            }
            catch (FaultException<WebEas.Services.Esb.Epod.CaseFile.DcomFault> ex)
            {
                string message = "DcomFault - NotifyRecordRelatedEvent";
                if (ex.Detail != null)
                {
                    message += string.Format(" {0}({1})", ex.Detail.faultMessage, ex.Detail.faultCode);
                    if (ex.Detail.faultMessage.Contains("DMS_NOT_AVAILABLE"))
                    {
                        throw new WebEasProxyException(message, "Úložisko dokumentov nie je dostupné (DMS)", ex, events);
                    }
                }

                throw new WebEasProxyException(message, string.Format("Nastala chyba pri volaní služby podateľne - {0}", ex.Detail.faultMessage), ex, events);
            }
            catch (Exception ex)
            {
                this.CheckGlobalExceptions(ex);
                throw new WebEasProxyException("Error in NotifyRecordRelatedEvent", "Nastala chyba pri volaní služby podateľne", ex, events);
            }
        }

        /// <summary>
        /// Gets the records in file.
        /// </summary>
        /// <param name="caseFileId">The case file id.</param>
        /// <returns></returns>
        public WebEas.Services.Esb.Epod.CaseFile.RecordShortDto[] GetRecordsInFile(System.Nullable<long> caseFileId)
        {
            try
            {
                WebEas.Core.Sts.SecurityTokenServiceHelper.CheckService(ref this.proxy, ServiceUrl, this.stsThumbPrint);

                var request = new getRecordsInFile(caseFileId);
                var stopwatch = new Stopwatch();
                stopwatch.Start();
                getRecordsInFileResponse response = this.proxy.getRecordsInFile(request);
                stopwatch.Stop();
                LogRequestDuration(ServiceUrl, stopwatch);
                return response.recordShortDtoList;
            }
            catch (FaultException<WebEas.Services.Esb.Epod.CaseFile.DcomFault> ex)
            {
                if (ex.Detail.faultMessage.Contains("DMS_NOT_AVAILABLE"))
                {
                    throw new WebEasProxyException("DMS", "Úložisko dokumentov nie je dostupné (DMS)", ex, caseFileId);
                }
                throw new WebEasProxyException("Fault Exception in GetRecordsInFile", "Nastala chyba pri volaní služby podateľne", ex, caseFileId);
            }
            catch (FaultException<WebEas.Services.Esb.Epod.CaseFile.ERegistryException> ex)
            {
                string message = "ERegistryException - GetRecordDetails ";
                if (ex.Detail != null && !String.IsNullOrEmpty(ex.Detail.message))
                {
                    message += ex.Detail.message;
                }
                if (ex.Detail != null && ex.Detail.ValidationError != null && ex.Detail.ValidationError.Length > 0)
                {
                    message += string.Format(" {0}({1})", ex.Detail.ValidationError[0].message, ex.Detail.ValidationError[0].type);
                    if (ex.Detail.ValidationError[0].type == ValidationErrorType.DMS_NOT_AVAILABLE)
                    {
                        throw new WebEasProxyException(message, "Úložisko dokumentov nie je dostupné (DMS)", ex, caseFileId);
                    }
                }

                throw new WebEasProxyException(message, "Nastala chyba pri volaní služby podateľne", ex, caseFileId);
            }
            catch (Exception ex)
            {
                this.CheckGlobalExceptions(ex);
                throw new WebEasProxyException("Error in getRecordDetails", "Nastala chyba pri volaní služby podateľne", ex, caseFileId);
            }
        }

        /// <summary>
        /// Inserts the record.
        /// </summary>
        /// <param name="insertRecordDto">The insert record dto.</param>
        /// <returns></returns>
        public System.Nullable<long> InsertRecord(WebEas.Services.Esb.Epod.CaseFile.InsertRecordDraftDto insertRecordDto)
        {
            try
            {
                WebEas.Core.Sts.SecurityTokenServiceHelper.CheckService(ref this.proxy, ServiceUrl, this.stsThumbPrint);

                //return this.client.insertRecord(insertRecordDto);
                var request = new insertRecordDraft(insertRecordDto);
                var stopwatch = new Stopwatch();
                stopwatch.Start();
                insertRecordDraftResponse response = this.proxy.insertRecordDraft(request);
                stopwatch.Stop();
                LogRequestDuration(ServiceUrl, stopwatch);
                return response.recordId;
            }
            catch (FaultException<WebEas.Services.Esb.Epod.CaseFile.DcomFault> ex)
            {
                if (ex.Detail.faultMessage.Contains("DMS_NOT_AVAILABLE"))
                {
                    throw new WebEasProxyException("DMS", "Úložisko dokumentov nie je dostupné (DMS)", ex, insertRecordDto);
                }
                throw new WebEasProxyException("Fault Exception in InsertRecord", "Nastala chyba pri volaní služby podateľne", ex, insertRecordDto);
            }
            catch (FaultException<WebEas.Services.Esb.Epod.CaseFile.ERegistryException> ex)
            {
                string message = "ERegistryException - InsertRecord";
                if (ex.Detail != null && !String.IsNullOrEmpty(ex.Detail.message))
                {
                    message += ex.Detail.message;
                }
                if (ex.Detail != null && ex.Detail.ValidationError != null && ex.Detail.ValidationError.Length > 0)
                {
                    message += string.Format(" {0}({1})", ex.Detail.ValidationError[0].message, ex.Detail.ValidationError[0].type);
                    if (ex.Detail.ValidationError[0].type == ValidationErrorType.DMS_NOT_AVAILABLE)
                    {
                        throw new WebEasProxyException(message, "Úložisko dokumentov nie je dostupné (DMS)", ex, insertRecordDto);
                    }
                    else if (ex.Detail.ValidationError[0].type == ValidationErrorType.UNAUTHORIZED_ACCESS)
                    {
                        throw new WebEasProxyException(message, "K vyrúbeniu poplatku musíte byť riešiteľom spisu!", ex, insertRecordDto);
                    }
                    else if (ex.Detail.ValidationError[0].type == ValidationErrorType.CREATE_OR_UPDATE_RECORD_CASE_FILE_NOT_OPEN)
                    {
                        throw new WebEasProxyException(message, "Nie je možné vložiť záznam do spisu, spis je uzavretý!", ex, insertRecordDto);
                    }
                    else
                    {
                        throw new WebEasProxyException(message, string.Format("Nastala chyba pri volaní služby podateľne {0}({1})", ex.Detail.ValidationError[0].message, ex.Detail.ValidationError[0].type), ex, insertRecordDto);
                    }
                }

                throw new WebEasProxyException(message, "Nastala chyba pri volaní služby podateľne", ex, insertRecordDto);
            }
            catch (Exception ex)
            {
                this.CheckGlobalExceptions(ex);
                throw new WebEasProxyException("Error in InsertRecord", "Nastala chyba pri volaní služby podateľne", ex, insertRecordDto);
            }
        }

        /// <summary>
        /// Gets the record details.
        /// </summary>
        /// <param name="recordDetailRequestDto">The record detail request dto.</param>
        /// <returns></returns>
        public WebEas.Services.Esb.Epod.CaseFile.RecordDto GetRecordDetails(WebEas.Services.Esb.Epod.CaseFile.RecordDetailRequestDto recordDetailRequestDto)
        {
            try
            {
                WebEas.Core.Sts.SecurityTokenServiceHelper.CheckService(ref this.proxy, ServiceUrl, this.stsThumbPrint);

                var request = new getRecordDetails(recordDetailRequestDto);
                var stopwatch = new Stopwatch();
                stopwatch.Start();
                getRecordDetailsResponse response = this.proxy.getRecordDetails(request);
                stopwatch.Stop();
                LogRequestDuration(ServiceUrl, stopwatch);
                return response.recordDto;
            }
            catch (FaultException<WebEas.Services.Esb.Epod.CaseFile.DcomFault> ex)
            {
                if (ex.Detail.faultMessage.Contains("DMS_NOT_AVAILABLE"))
                {
                    throw new WebEasProxyException("DMS", "Úložisko dokumentov nie je dostupné (DMS)", ex, recordDetailRequestDto);
                }
                throw new WebEasProxyException("Fault Exception in GetRecordDetails", "Nastala chyba pri volaní služby podateľne", ex, recordDetailRequestDto);
            }
            catch (FaultException<WebEas.Services.Esb.Epod.CaseFile.ERegistryException> ex)
            {
                string message = "ERegistryException - GetRecordDetails ";
                if (ex.Detail != null && !String.IsNullOrEmpty(ex.Detail.message))
                {
                    message += ex.Detail.message;
                }
                if (ex.Detail != null && ex.Detail.ValidationError != null && ex.Detail.ValidationError.Length > 0)
                {
                    if (ex.Detail.ValidationError[0].type == ValidationErrorType.DMS_NOT_AVAILABLE)
                    {
                        throw new WebEasProxyException("DMS", "Úložisko dokumentov nie je dostupné (DMS)", ex, recordDetailRequestDto);
                    }
                    message += string.Format(" {0}({1})", ex.Detail.ValidationError[0].message, ex.Detail.ValidationError[0].type);
                }

                throw new WebEasProxyException(message, "Nastala chyba pri volaní služby podateľne", ex, recordDetailRequestDto);
            }
            catch (FaultException ex)
            {
                throw new WebEasProxyException(string.Format("Fault Exception in GetRecordDetails {0}", ex.Reason.ToString()), "Nastala chyba pri volaní služby podateľne", ex, recordDetailRequestDto);
            }
            catch (Exception ex)
            {
                this.CheckGlobalExceptions(ex);
                throw new WebEasProxyException("Error in getRecordDetails", "Nastala chyba pri volaní služby podateľne", ex, recordDetailRequestDto);
            }
        }

        /// <summary>
        /// Discards the record.
        /// </summary>
        /// <param name="recordId">The record id.</param>
        public void DiscardRecord(System.Nullable<long> recordId)
        {
            try
            {
                WebEas.Core.Sts.SecurityTokenServiceHelper.CheckService(ref this.proxy, ServiceUrl, this.stsThumbPrint);

                var request = new discardRecord(recordId);
                var stopwatch = new Stopwatch();
                stopwatch.Start();
                this.proxy.discardRecord(request);
                stopwatch.Stop();
                LogRequestDuration(ServiceUrl, stopwatch);
            }
            catch (FaultException<WebEas.Services.Esb.Epod.CaseFile.DcomFault> ex)
            {
                if (ex.Detail.faultMessage.Contains("DMS_NOT_AVAILABLE"))
                {
                    throw new WebEasProxyException("DMS", "Úložisko dokumentov nie je dostupné (DMS)", ex, recordId);
                }
                throw new WebEasProxyException("Fault Exception in DiscardRecordBulk", "Nastala chyba pri volaní služby podateľne", ex, recordId);
            }
            catch (FaultException<WebEas.Services.Esb.Epod.CaseFile.ERegistryException> ex)
            {
                if (ex.Detail.ValidationError[0].type == ValidationErrorType.DMS_NOT_AVAILABLE)
                {
                    throw new WebEasProxyException("DMS", "Úložisko dokumentov nie je dostupné (DMS)", ex, recordId);
                }
                throw new WebEasProxyException("Fault Exception in DiscardRecord", "Nastala chyba pri volaní služby podateľne", ex, recordId);
            }
            catch (Exception ex)
            {
                this.CheckGlobalExceptions(ex);
                throw new WebEasProxyException("Error in DiscardRecord", "Nastala chyba pri volaní služby podateľne", ex, recordId);
            }
        }

        /// <summary>
        /// Discards the record bulk.
        /// </summary>
        /// <param name="recordBulkId">The record bulk id.</param>
        public void DiscardRecordBulk(long recordBulkId)
        {
            try
            {
                WebEas.Core.Sts.SecurityTokenServiceHelper.CheckService(ref this.proxy, ServiceUrl, this.stsThumbPrint);

                var request = new discardRecordBulk(new RecordBulkDto { recordBulkId = recordBulkId });
                var stopwatch = new Stopwatch();
                stopwatch.Start();
                this.proxy.discardRecordBulk(request);
                stopwatch.Stop();
                LogRequestDuration(ServiceUrl, stopwatch);
            }
            catch (FaultException<WebEas.Services.Esb.Epod.CaseFile.DcomFault> ex)
            {
                if (ex.Detail.faultMessage.Contains("DMS_NOT_AVAILABLE"))
                {
                    throw new WebEasProxyException("DMS", "Úložisko dokumentov nie je dostupné (DMS)", ex, recordBulkId);
                }
                throw new WebEasProxyException("Fault Exception in DiscardRecordBulk", "Nastala chyba pri volaní služby podateľne", ex, recordBulkId);
            }
            catch (FaultException<WebEas.Services.Esb.Epod.CaseFile.ERegistryException> ex)
            {
                if (ex.Detail.ValidationError[0].type == ValidationErrorType.DMS_NOT_AVAILABLE)
                {
                    throw new WebEasProxyException("DMS", "Úložisko dokumentov nie je dostupné (DMS)", ex, recordBulkId);
                }
                throw new WebEasProxyException("Fault Exception in DiscardRecordBulk", "Nastala chyba pri volaní služby podateľne", ex, recordBulkId);
            }
            catch (Exception ex)
            {
                this.CheckGlobalExceptions(ex);
                throw new WebEasProxyException("Error in DiscardRecordBulk", "Nastala chyba pri volaní služby podateľne", ex, recordBulkId);
            }
        }

        /// <summary>
        /// Finishes the record bulk.
        /// </summary>
        /// <param name="recordBulkId">The record bulk id.</param>
        public void FinishRecordBulk(long recordBulkId)
        {
            try
            {
                WebEas.Core.Sts.SecurityTokenServiceHelper.CheckService(ref this.proxy, ServiceUrl, this.stsThumbPrint);

                var request = new finishRecordBulk(new RecordBulkDto { recordBulkId = recordBulkId });
                var stopwatch = new Stopwatch();
                stopwatch.Start();
                this.proxy.finishRecordBulk(request);
                stopwatch.Stop();
                LogRequestDuration(ServiceUrl, stopwatch);
            }
            catch (FaultException<WebEas.Services.Esb.Epod.CaseFile.DcomFault> ex)
            {
                if (ex.Detail.faultMessage.Contains("DMS_NOT_AVAILABLE"))
                {
                    throw new WebEasProxyException("DMS", "Úložisko dokumentov nie je dostupné (DMS)", ex, recordBulkId);
                }
                throw new WebEasProxyException("Fault Exception in FinishRecordBulk", "Nastala chyba pri volaní služby podateľne", ex, recordBulkId);
            }
            catch (FaultException<WebEas.Services.Esb.Epod.CaseFile.ERegistryException> ex)
            {
                if (ex.Detail.ValidationError[0].type == ValidationErrorType.DMS_NOT_AVAILABLE)
                {
                    throw new WebEasProxyException("DMS", "Úložisko dokumentov nie je dostupné (DMS)", ex, recordBulkId);
                }
                throw new WebEasProxyException("Fault Exception in FinishRecordBulk", "Nastala chyba pri volaní služby podateľne", ex, recordBulkId);
            }
            catch (Exception ex)
            {
                this.CheckGlobalExceptions(ex);
                throw new WebEasProxyException("Error in FinishRecordBulk", "Nastala chyba pri volaní služby podateľne", ex, recordBulkId);
            }
        }

        /// <summary>
        /// Gets the user agenda.
        /// </summary>
        /// <param name="userAgendaRequestDto">The user agenda request dto.</param>
        /// <returns></returns>
        public WebEas.Services.Esb.Epod.CaseFile.UserAgendaItemDto[] GetUserAgenda(WebEas.Services.Esb.Epod.CaseFile.UserAgendaRequestDto userAgendaRequestDto)
        {
            try
            {
                WebEas.Core.Sts.SecurityTokenServiceHelper.CheckService(ref this.proxy, ServiceUrl, this.stsThumbPrint);

                var request = new getUserAgenda();
                request.userAgendaRequestDto = userAgendaRequestDto;
                var stopwatch = new Stopwatch();
                stopwatch.Start();
                getUserAgendaResponse response = this.proxy.getUserAgenda(request);
                stopwatch.Stop();
                LogRequestDuration(ServiceUrl, stopwatch);

                return response.userAgendaItemDtos;
            }
            catch (FaultException<WebEas.Services.Esb.Epod.CaseFile.DcomFault> ex)
            {
                if (ex.Detail.faultMessage.Contains("DMS_NOT_AVAILABLE"))
                {
                    throw new WebEasProxyException("DMS", "Úložisko dokumentov nie je dostupné (DMS)", ex, userAgendaRequestDto);
                }
                throw new WebEasProxyException("Fault Exception in GetUserAgenda", string.Format("Nastala chyba pri volaní služby podateľne - {0}", ex.Detail.faultMessage), ex, userAgendaRequestDto);
            }
            catch (Exception ex)
            {
                this.CheckGlobalExceptions(ex);
                throw new WebEasProxyException("Error in GetUserAgenda", "Nastala chyba pri volaní služby podateľne", ex, userAgendaRequestDto);
            }
        }

        /// <summary>
        /// Discard record from bulk.
        /// </summary>
        /// <param name="recordIds">Record ids</param>
        /// <returns></returns>
        public discardRecordFromBulkResponse DiscardRecordFromBulk(long[] recordIds)
        {
            try
            {
                WebEas.Core.Sts.SecurityTokenServiceHelper.CheckService(ref proxy, ServiceUrl, stsThumbPrint);

                var request = new discardRecordFromBulk(recordIds);
                var stopwatch = new Stopwatch();
                stopwatch.Start();
                var response = proxy.discardRecordFromBulk(request);
                stopwatch.Stop();
                LogRequestDuration(ServiceUrl, stopwatch);
                return response;
            }
            catch (FaultException<DcomFault> ex)
            {
                string message = "DcomFault - DiscardRecordFromBulk";
                if (ex.Detail != null)
                {
                    message += string.Format(" {0}({1})", ex.Detail.faultMessage, ex.Detail.faultCode);

                    if (ex.Detail.faultMessage.Contains("DMS_NOT_AVAILABLE"))
                    {
                        throw new WebEasProxyException(message, "Úložisko dokumentov nie je dostupné (DMS)", ex, recordIds);
                    }
                }

                throw new WebEasProxyException(message, string.Format("Nastala chyba pri volaní služby podateľne - {0}", ex.Detail.faultMessage), ex, recordIds);
            }
            catch (Exception ex)
            {
                CheckGlobalExceptions(ex);
                throw new WebEasProxyException("Error in DiscardRecordFromBulk", "Nastala chyba pri volaní služby podateľne", ex, recordIds);
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
                    {
                        this.proxy.Abort();
                    }

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