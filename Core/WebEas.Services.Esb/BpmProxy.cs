using System;
using System.Diagnostics;
using System.ServiceModel;
using WebEas.Services.Esb.Bpm;

namespace WebEas.Services.Esb
{
    public class BpmProxy : ProxyBase, IDisposable
    {
        private const string ServiceUrl = "https://lbsoa.intra.dcom.sk/soa/bpm/1.0/bpmService";

        private Bpm.BpmServicePortTypeChannel proxy;

        /// <summary>
        /// Initializes a new instance of the <see cref="BpmProxy" /> class.
        /// </summary>
        public BpmProxy(string stsThumbprint) : base(stsThumbprint)
        {
            this.proxy = WebEas.Core.Sts.SecurityTokenServiceHelper.CreateChannelProxy<Bpm.BpmServicePortTypeChannel>(ServiceUrl, stsThumbprint);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BpmProxy" /> class.
        /// </summary>
        public BpmProxy(string stsThumbprint, IWebEasSession session) : base(stsThumbprint)
        {
            this.proxy = WebEas.Core.Sts.SecurityTokenServiceHelper.CreateChannelProxy<Bpm.BpmServicePortTypeChannel>(session, ServiceUrl, stsThumbprint);
        }

        /// <summary>
        /// Completes the task.
        /// </summary>
        /// <param name="completeTaskRequest">The complete task request.</param>
        /// <returns></returns>
        public Bpm.CompleteTaskResponse CompleteTask(Bpm.CompleteTaskRequest completeTaskRequest)
        {
            try
            {
                WebEas.Core.Sts.SecurityTokenServiceHelper.CheckService(ref this.proxy, ServiceUrl, this.stsThumbPrint);

                var request = new Bpm.CompleteTaskRequest1(completeTaskRequest);
                var stopwatch = new Stopwatch();
                stopwatch.Start();
                Bpm.CompleteTaskResponse response = this.proxy.CompleteTask(request);
                stopwatch.Stop();
                LogRequestDuration(ServiceUrl, stopwatch);
                return response;
            }
            catch (FaultException<Bpm.BpmFault> ex)
            {
                throw new WebEasProxyException("Fault Exception in CompleteTask", string.Format("Nastala chyba pri volaní služby Bpm - {0}", ex.Detail.message), ex, completeTaskRequest);
            }
            catch (Exception ex)
            {
                this.CheckGlobalExceptions(ex);
                throw new WebEasProxyException("Error in CompleteTask", "Nastala chyba pri volaní služby Bpm", ex, completeTaskRequest);
            }
        }

        /// <summary>
        /// Gets the process variables.
        /// </summary>
        /// <param name="getProcessVariablesRequest">The get process variables request.</param>
        /// <returns></returns>
        public Bpm.GetProcessVariablesResponse GetProcessVariables(Bpm.GetProcessVariablesRequest getProcessVariablesRequest)
        {
            try
            {
                WebEas.Core.Sts.SecurityTokenServiceHelper.CheckService(ref this.proxy, ServiceUrl, this.stsThumbPrint);

                var request = new Bpm.GetProcessVariablesRequest1(getProcessVariablesRequest);
                var stopwatch = new Stopwatch();
                stopwatch.Start();
                Bpm.GetProcessVariablesResponse1 response = this.proxy.GetProcessVariables(request);
                stopwatch.Stop();
                LogRequestDuration(ServiceUrl, stopwatch);
                return response.GetProcessVariablesResponse;
            }
            catch (FaultException<Bpm.BpmFault> ex)
            {
                throw new WebEasProxyException("Fault Exception in GetProcessVariables", string.Format("Nastala chyba pri volaní služby Bpm - {0}", ex.Detail.message), ex, getProcessVariablesRequest);
            }
            catch (Exception ex)
            {
                this.CheckGlobalExceptions(ex);
                throw new WebEasProxyException("Error in GetProcessVariables", "Nastala chyba pri volaní služby Bpm", ex, getProcessVariablesRequest);
            }
        }

        /// <summary>
        /// Gets the single task.
        /// </summary>
        /// <param name="getSingleTaskRequest">The get single task request.</param>
        /// <returns></returns>
        public Bpm.GetSingleTaskResponse GetSingleTask(Bpm.GetSingleTaskRequest getSingleTaskRequest)
        {
            try
            {
                WebEas.Core.Sts.SecurityTokenServiceHelper.CheckService(ref this.proxy, ServiceUrl, this.stsThumbPrint);

                var request = new Bpm.GetSingleTaskRequest1(getSingleTaskRequest);
                var stopwatch = new Stopwatch();
                stopwatch.Start();
                Bpm.GetSingleTaskResponse1 response = this.proxy.GetSingleTask(request);
                stopwatch.Stop();
                LogRequestDuration(ServiceUrl, stopwatch);
                return response.GetSingleTaskResponse;
            }
            catch (FaultException<Bpm.BpmFault> ex)
            {
                throw new WebEasProxyException("Fault Exception in GetSingleTask", string.Format("Nastala chyba pri volaní služby Bpm - {0}", ex.Detail.message), ex, getSingleTaskRequest);
            }
            catch (Exception ex)
            {
                this.CheckGlobalExceptions(ex);
                throw new WebEasProxyException("Error in GetSingleTask", "Nastala chyba pri volaní služby Bpm", ex, getSingleTaskRequest);
            }
        }

        /// <summary>
        /// Sends the message.
        /// </summary>
        /// <param name="sendMessageRequest">The send message request.</param>
        /// <returns></returns>
        public Bpm.SendMessageResponse SendMessage(Bpm.SendMessageRequest sendMessageRequest)
        {
            try
            {
                WebEas.Core.Sts.SecurityTokenServiceHelper.CheckService(ref this.proxy, ServiceUrl, this.stsThumbPrint);

                var request = new Bpm.SendMessageRequest1(sendMessageRequest);
                var stopwatch = new Stopwatch();
                stopwatch.Start();
                Bpm.SendMessageResponse response = this.proxy.SendMessage(request);
                stopwatch.Stop();
                LogRequestDuration(ServiceUrl, stopwatch);
                return response;
            }
            catch (FaultException<Bpm.BpmFault> ex)
            {
                throw new WebEasProxyException("Fault Exception in SendMessage", string.Format("Nastala chyba pri volaní služby Bpm - {0}", ex.Detail.message), ex, sendMessageRequest);
            }
            catch (Exception ex)
            {
                this.CheckGlobalExceptions(ex);
                throw new WebEasProxyException("Error in SendMessage", "Nastala chyba pri volaní služby Bpm", ex, sendMessageRequest);
            }
        }

        /// <summary>
        /// Sets the assignee.
        /// </summary>
        /// <param name="setAssigneeRequest">The set assignee request.</param>
        /// <returns></returns>
        public Bpm.SetAssigneeResponse SetAssignee(Bpm.SetAssigneeRequest setAssigneeRequest)
        {
            try
            {
                WebEas.Core.Sts.SecurityTokenServiceHelper.CheckService(ref this.proxy, ServiceUrl, this.stsThumbPrint);

                var request = new Bpm.SetAssigneeRequest1(setAssigneeRequest);
                var stopwatch = new Stopwatch();
                stopwatch.Start();
                Bpm.SetAssigneeResponse response = this.proxy.SetAssignee(request);
                stopwatch.Stop();
                LogRequestDuration(ServiceUrl, stopwatch);
                return response;
            }
            catch (FaultException<Bpm.BpmFault> ex)
            {
                throw new WebEasProxyException("Fault Exception in SetAssignee", string.Format("Nastala chyba pri volaní služby Bpm - {0}", ex.Detail.message), ex, setAssigneeRequest);
            }
            catch (Exception ex)
            {
                this.CheckGlobalExceptions(ex);
                throw new WebEasProxyException("Error in SetAssignee", "Nastala chyba pri volaní služby Bpm", ex, setAssigneeRequest);
            }
        }

        /// <summary>
        /// Starts the process instance.
        /// </summary>
        /// <param name="startProcessInstanceRequest">The start process instance request.</param>
        /// <returns></returns>
        public Bpm.StartProcessInstanceResponse StartProcessInstance(Bpm.StartProcessInstanceRequest startProcessInstanceRequest)
        {
            try
            {
                WebEas.Core.Sts.SecurityTokenServiceHelper.CheckService(ref this.proxy, ServiceUrl, this.stsThumbPrint);

                var request = new Bpm.StartProcessInstanceRequest1(startProcessInstanceRequest);
                var stopwatch = new Stopwatch();
                stopwatch.Start();
                Bpm.StartProcessInstanceResponse1 response = this.proxy.StartProcessInstance(request);
                stopwatch.Stop();
                LogRequestDuration(ServiceUrl, stopwatch);
                return response.StartProcessInstanceResponse;
            }
            catch (FaultException<Bpm.BpmFault> ex)
            {
                string message = "BpmFaultException - StartProcessInstance ";
                if (ex.Detail != null && !String.IsNullOrEmpty(ex.Detail.message))
                {
                    message += ex.Detail.message;
                }

                throw new WebEasProxyException(message, string.Format("Nastala chyba pri volaní služby Bpm - {0}", ex.Detail.message), ex, startProcessInstanceRequest);
            }
            catch (Exception ex)
            {
                this.CheckGlobalExceptions(ex);
                throw new WebEasProxyException("Error in StartProcessInstance", "Nastala chyba pri volaní služby Bpm", ex, startProcessInstanceRequest);
            }
        }

        /// <summary>
        /// Submits the task form.
        /// </summary>
        /// <param name="submitTaskFormRequest">The submit task form request.</param>
        /// <returns></returns>
        public Bpm.SubmitTaskFormResponse SubmitTaskForm(Bpm.SubmitTaskFormRequest submitTaskFormRequest)
        {
            try
            {
                WebEas.Core.Sts.SecurityTokenServiceHelper.CheckService(ref this.proxy, ServiceUrl, this.stsThumbPrint);

                var request = new Bpm.SubmitTaskFormRequest1(submitTaskFormRequest);
                var stopwatch = new Stopwatch();
                stopwatch.Start();
                Bpm.SubmitTaskFormResponse response = this.proxy.SubmitTaskForm(request);
                stopwatch.Stop();
                LogRequestDuration(ServiceUrl, stopwatch);
                return response;
            }
            catch (FaultException<Bpm.BpmFault> ex)
            {
                throw new WebEasProxyException("Fault Exception in SubmitTaskForm", string.Format("Nastala chyba pri volaní služby Bpm - {0}", ex.Detail.message), ex, submitTaskFormRequest);
            }
            catch (Exception ex)
            {
                this.CheckGlobalExceptions(ex);
                throw new WebEasProxyException("Error in SubmitTaskForm", "Nastala chyba pri volaní služby Bpm", ex, submitTaskFormRequest);
            }
        }

        /// <summary>
        /// Gets the user agenda.
        /// </summary>
        /// <param name="userAgendaRequestDto">The user agenda request dto.</param>
        /// <returns></returns>
        public Bpm.UserAgendaItemDto[] GetUserAgenda(Bpm.UserAgendaRequestDto userAgendaRequestDto)
        {
            try
            {
                WebEas.Core.Sts.SecurityTokenServiceHelper.CheckService(ref this.proxy, ServiceUrl, this.stsThumbPrint);

                var request = new Bpm.GetUserAgendaRequest();
                request.UserAgendaRequestDto = userAgendaRequestDto;
                var stopwatch = new Stopwatch();
                stopwatch.Start();
                GetUserAgendaResponse response = this.proxy.GetUserAgenda(request);
                stopwatch.Stop();
                LogRequestDuration(ServiceUrl, stopwatch);

                return response.UserAgendaResponseDto;
            }
            catch (FaultException<Bpm.BpmFault> ex)
            {
                throw new WebEasProxyException("Fault Exception in GetUserAgenda", string.Format("Nastala chyba pri volaní služby Bpm - {0}", ex.Detail.message), ex, userAgendaRequestDto);
            }
            catch (Exception ex)
            {
                this.CheckGlobalExceptions(ex);
                throw new WebEasProxyException("Error in GetUserAgenda", "Nastala chyba pri volaní služby Bpm", ex, userAgendaRequestDto);
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