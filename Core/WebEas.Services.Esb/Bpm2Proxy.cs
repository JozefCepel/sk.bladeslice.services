using System;
using System.Diagnostics;
using System.ServiceModel;
using WebEas.Services.Esb.Bpm2;

namespace WebEas.Services.Esb
{
    public class Bpm2Proxy : ProxyBase, IDisposable
    {
        private const string ServiceUrl = "https://lbsoa.intra.dcom.sk/soa/bpm/2.0/bpmService";

        private BpmServicePortTypeChannel proxy;

        /// <summary>
        /// Initializes a new instance of the <see cref="Bpm2Proxy" /> class.
        /// </summary>
        public Bpm2Proxy(string stsThumbprint) : base(stsThumbprint)
        {
            proxy = Core.Sts.SecurityTokenServiceHelper.CreateChannelProxy<BpmServicePortTypeChannel>(ServiceUrl, stsThumbprint);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Bpm2Proxy" /> class.
        /// </summary>
        public Bpm2Proxy(string stsThumbprint, IWebEasSession session) : base(stsThumbprint)
        {
            proxy = Core.Sts.SecurityTokenServiceHelper.CreateChannelProxy<BpmServicePortTypeChannel>(session, ServiceUrl, stsThumbprint);
        }

        /// <summary>
        /// Completes the task.
        /// </summary>
        /// <param name="completeTaskRequest">The complete task request.</param>
        /// <returns></returns>
        public CompleteTaskResponse CompleteTask(CompleteTaskRequest completeTaskRequest)
        {
            try
            {
                Core.Sts.SecurityTokenServiceHelper.CheckService(ref proxy, ServiceUrl, stsThumbPrint);

                var request = new CompleteTaskRequest1(completeTaskRequest);
                var stopwatch = new Stopwatch();
                stopwatch.Start();
                CompleteTaskResponse response = proxy.CompleteTask(request);
                stopwatch.Stop();
                LogRequestDuration(ServiceUrl, stopwatch);
                return response;
            }
            catch (FaultException<BpmFault> ex)
            {
                throw new WebEasProxyException("Fault Exception in CompleteTask", string.Format("Nastala chyba pri volaní služby Bpm2 - {0}", ex.Detail.message), ex, completeTaskRequest);
            }
            catch (Exception ex)
            {
                CheckGlobalExceptions(ex);
                throw new WebEasProxyException("Error in CompleteTask", "Nastala chyba pri volaní služby Bpm2", ex, completeTaskRequest);
            }
        }

        /// <summary>
        /// Gets the process variables.
        /// </summary>
        /// <param name="getProcessVariablesRequest">The get process variables request.</param>
        /// <returns></returns>
        public GetProcessVariablesResponse GetProcessVariables(GetProcessVariablesRequest getProcessVariablesRequest)
        {
            try
            {
                Core.Sts.SecurityTokenServiceHelper.CheckService(ref proxy, ServiceUrl, stsThumbPrint);

                var request = new GetProcessVariablesRequest1(getProcessVariablesRequest);
                var stopwatch = new Stopwatch();
                stopwatch.Start();
                GetProcessVariablesResponse1 response = proxy.GetProcessVariables(request);
                stopwatch.Stop();
                LogRequestDuration(ServiceUrl, stopwatch);
                return response.GetProcessVariablesResponse;
            }
            catch (FaultException<BpmFault> ex)
            {
                throw new WebEasProxyException("Fault Exception in GetProcessVariables", string.Format("Nastala chyba pri volaní služby Bpm2 - {0}", ex.Detail.message), ex, getProcessVariablesRequest);
            }
            catch (Exception ex)
            {
                CheckGlobalExceptions(ex);
                throw new WebEasProxyException("Error in GetProcessVariables", "Nastala chyba pri volaní služby Bpm2", ex, getProcessVariablesRequest);
            }
        }

        /// <summary>
        /// Gets the single task.
        /// </summary>
        /// <param name="getSingleTaskRequest">The get single task request.</param>
        /// <returns></returns>
        public GetSingleTaskResponse GetSingleTask(GetSingleTaskRequest getSingleTaskRequest)
        {
            try
            {
                Core.Sts.SecurityTokenServiceHelper.CheckService(ref proxy, ServiceUrl, stsThumbPrint);

                var request = new GetSingleTaskRequest1(getSingleTaskRequest);
                var stopwatch = new Stopwatch();
                stopwatch.Start();
                GetSingleTaskResponse1 response = proxy.GetSingleTask(request);
                stopwatch.Stop();
                LogRequestDuration(ServiceUrl, stopwatch);
                return response.GetSingleTaskResponse;
            }
            catch (FaultException<BpmFault> ex)
            {
                throw new WebEasProxyException("Fault Exception in GetSingleTask", string.Format("Nastala chyba pri volaní služby Bpm2 - {0}", ex.Detail.message), ex, getSingleTaskRequest);
            }
            catch (Exception ex)
            {
                CheckGlobalExceptions(ex);
                throw new WebEasProxyException("Error in GetSingleTask", "Nastala chyba pri volaní služby Bpm2", ex, getSingleTaskRequest);
            }
        }

        /// <summary>
        /// Sends the message.
        /// </summary>
        /// <param name="sendMessageRequest">The send message request.</param>
        /// <returns></returns>
        public SendMessageResponse SendMessage(SendMessageRequest sendMessageRequest)
        {
            try
            {
                Core.Sts.SecurityTokenServiceHelper.CheckService(ref proxy, ServiceUrl, stsThumbPrint);

                var request = new SendMessageRequest1(sendMessageRequest);
                var stopwatch = new Stopwatch();
                stopwatch.Start();
                SendMessageResponse response = proxy.SendMessage(request);
                stopwatch.Stop();
                LogRequestDuration(ServiceUrl, stopwatch);
                return response;
            }
            catch (FaultException<BpmFault> ex)
            {
                throw new WebEasProxyException("Fault Exception in SendMessage", string.Format("Nastala chyba pri volaní služby Bpm2 - {0}", ex.Detail.message), ex, sendMessageRequest);
            }
            catch (Exception ex)
            {
                CheckGlobalExceptions(ex);
                throw new WebEasProxyException("Error in SendMessage", "Nastala chyba pri volaní služby Bpm2", ex, sendMessageRequest);
            }
        }

        /// <summary>
        /// Sets the assignee.
        /// </summary>
        /// <param name="setAssigneeRequest">The set assignee request.</param>
        /// <returns></returns>
        public SetAssigneeResponse SetAssignee(SetAssigneeRequest setAssigneeRequest)
        {
            try
            {
                Core.Sts.SecurityTokenServiceHelper.CheckService(ref proxy, ServiceUrl, stsThumbPrint);

                var request = new SetAssigneeRequest1(setAssigneeRequest);
                var stopwatch = new Stopwatch();
                stopwatch.Start();
                SetAssigneeResponse response = proxy.SetAssignee(request);
                stopwatch.Stop();
                LogRequestDuration(ServiceUrl, stopwatch);
                return response;
            }
            catch (FaultException<BpmFault> ex)
            {
                throw new WebEasProxyException("Fault Exception in SetAssignee", string.Format("Nastala chyba pri volaní služby Bpm2 - {0}", ex.Detail.message), ex, setAssigneeRequest);
            }
            catch (Exception ex)
            {
                CheckGlobalExceptions(ex);
                throw new WebEasProxyException("Error in SetAssignee", "Nastala chyba pri volaní služby Bpm2", ex, setAssigneeRequest);
            }
        }

        /// <summary>
        /// Starts the process instance.
        /// </summary>
        /// <param name="startProcessInstanceRequest">The start process instance request.</param>
        /// <returns></returns>
        public StartProcessInstanceResponse StartProcessInstance(StartProcessInstanceRequest startProcessInstanceRequest)
        {
            try
            {
                Core.Sts.SecurityTokenServiceHelper.CheckService(ref proxy, ServiceUrl, stsThumbPrint);

                var request = new StartProcessInstanceRequest1(startProcessInstanceRequest);
                var stopwatch = new Stopwatch();
                stopwatch.Start();
                StartProcessInstanceResponse1 response = proxy.StartProcessInstance(request);
                stopwatch.Stop();
                LogRequestDuration(ServiceUrl, stopwatch);
                return response.StartProcessInstanceResponse;
            }
            catch (FaultException<BpmFault> ex)
            {
                string message = "Bpm2FaultException - StartProcessInstance ";
                if (ex.Detail != null && !string.IsNullOrEmpty(ex.Detail.message))
                {
                    message += ex.Detail.message;
                }

                throw new WebEasProxyException(message, string.Format("Nastala chyba pri volaní služby Bpm2 - {0}", ex.Detail.message), ex, startProcessInstanceRequest);
            }
            catch (Exception ex)
            {
                CheckGlobalExceptions(ex);
                throw new WebEasProxyException("Error in StartProcessInstance", "Nastala chyba pri volaní služby Bpm2", ex, startProcessInstanceRequest);
            }
        }

        /// <summary>
        /// Submits the task form.
        /// </summary>
        /// <param name="submitTaskFormRequest">The submit task form request.</param>
        /// <returns></returns>
        public SubmitTaskFormResponse SubmitTaskForm(SubmitTaskFormRequest submitTaskFormRequest)
        {
            try
            {
                Core.Sts.SecurityTokenServiceHelper.CheckService(ref proxy, ServiceUrl, stsThumbPrint);

                var request = new SubmitTaskFormRequest1(submitTaskFormRequest);
                var stopwatch = new Stopwatch();
                stopwatch.Start();
                SubmitTaskFormResponse response = proxy.SubmitTaskForm(request);
                stopwatch.Stop();
                LogRequestDuration(ServiceUrl, stopwatch);
                return response;
            }
            catch (FaultException<BpmFault> ex)
            {
                throw new WebEasProxyException("Fault Exception in SubmitTaskForm", string.Format("Nastala chyba pri volaní služby Bpm2 - {0}", ex.Detail.message), ex, submitTaskFormRequest);
            }
            catch (Exception ex)
            {
                CheckGlobalExceptions(ex);
                throw new WebEasProxyException("Error in SubmitTaskForm", "Nastala chyba pri volaní služby Bpm2", ex, submitTaskFormRequest);
            }
        }

        /// <summary>
        /// Gets the user agenda.
        /// </summary>
        /// <param name="userAgendaRequestDto">The user agenda request dto.</param>
        /// <returns></returns>
        public UserAgendaItemDto[] GetUserAgenda(UserAgendaRequestDto userAgendaRequestDto)
        {
            try
            {
                Core.Sts.SecurityTokenServiceHelper.CheckService(ref proxy, ServiceUrl, stsThumbPrint);

                var request = new GetUserAgendaRequest();
                request.UserAgendaRequestDto = userAgendaRequestDto;
                var stopwatch = new Stopwatch();
                stopwatch.Start();
                GetUserAgendaResponse response = proxy.GetUserAgenda(request);
                stopwatch.Stop();
                LogRequestDuration(ServiceUrl, stopwatch);

                return response.UserAgendaResponseDto;
            }
            catch (FaultException<BpmFault> ex)
            {
                throw new WebEasProxyException("Fault Exception in GetUserAgenda", string.Format("Nastala chyba pri volaní služby Bpm - {0}", ex.Detail.message), ex, userAgendaRequestDto);
            }
            catch (Exception ex)
            {
                CheckGlobalExceptions(ex);
                throw new WebEasProxyException("Error in GetUserAgenda", "Nastala chyba pri volaní služby Bpm", ex, userAgendaRequestDto);
            }
        }

        /// <summary>
        /// Get process instances
        /// </summary>
        /// <param name="processInstancesRequestDto">Get process instances request dto.</param>
        /// <returns></returns>
        public ProcessInstanceDto[] GetProcessInstances(GetProcessInstancesRequestDto processInstancesRequestDto)
        {
            try
            {
                Core.Sts.SecurityTokenServiceHelper.CheckService(ref proxy, ServiceUrl, stsThumbPrint);

                var request = new GetProcessInstancesRequest();
                request.GetProcessInstancesRequestDto = processInstancesRequestDto;
                var stopwatch = new Stopwatch();
                stopwatch.Start();
                GetProcessInstancesResponse response = proxy.GetProcessInstances(request);
                stopwatch.Stop();
                LogRequestDuration(ServiceUrl, stopwatch);

                return response.GetProcessInstancesResponseDto;
            }
            catch (FaultException<BpmFault> ex)
            {
                throw new WebEasProxyException("Fault Exception in GetProcessInstances", string.Format("Nastala chyba pri volaní služby Bpm - {0}", ex.Detail.message), ex, processInstancesRequestDto);
            }
            catch (Exception ex)
            {
                CheckGlobalExceptions(ex);
                throw new WebEasProxyException("Error in GetProcessInstances", "Nastala chyba pri volaní služby Bpm", ex, processInstancesRequestDto);
            }
        }

        /// <summary>
        /// Gets the user agenda count.
        /// </summary>
        /// <param name="userAgendaCountRequestDto">The user agenda count request dto.</param>
        /// <returns></returns>
        public UserAgendaCountResponseDto GetUserAgendaCount(UserAgendaCountRequestDto userAgendaCountRequestDto)
        {
            try
            {
                Core.Sts.SecurityTokenServiceHelper.CheckService(ref proxy, ServiceUrl, stsThumbPrint);

                var request = new GetUserAgendaCountRequest();
                request.UserAgendaCountRequestDto = userAgendaCountRequestDto;
                var stopwatch = new Stopwatch();
                stopwatch.Start();
                GetUserAgendaCountResponse response = proxy.GetUserAgendaCount(request);
                stopwatch.Stop();
                LogRequestDuration(ServiceUrl, stopwatch);

                return response.UserAgendaCountResponseDto;
            }
            catch (FaultException<BpmFault> ex)
            {
                // nechceme skarede hlasky na FE
                if (ex.Detail.message.Contains("org.apache"))
                {
                    throw new WebEasProxyException("Fault Exception in GetUserAgendaCount", "Nastala chyba pri volaní služby Bpm, zopakujte akciu neskôr prosím.", ex, userAgendaCountRequestDto);
                }

                throw new WebEasProxyException("Fault Exception in GetUserAgendaCount", string.Format("Nastala chyba pri volaní služby Bpm - {0}", ex.Detail.message), ex, userAgendaCountRequestDto);
            }
            catch (Exception ex)
            {
                CheckGlobalExceptions(ex);
                throw new WebEasProxyException("Error in GetUserAgendaCount", "Nastala chyba pri volaní služby Bpm", ex, userAgendaCountRequestDto);
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