using System;
using System.ServiceModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace WebEas.Services.Esb.Test
{
    [TestClass]
    public class BpmServiceTest : TestBaseProxy
    {
        [TestMethod]
        public void StartProcessInstanceTest()
        {
            try
            {
                using (var client = new WebEas.Services.Esb.BpmProxy(null))
                {
                    var response = client.StartProcessInstance(new Bpm.StartProcessInstanceRequest
                    {
                        ProcessDefinition = new Bpm.ProcessDefinition { Item = "dcom-iap", ItemElementName = Bpm.ItemChoiceType.key },
                        businessKey = "29   ",
                        modulName = "iap",
                        Variables = new Bpm.Variable[]
                        { 
                            new Bpm.Variable { name = "assign", value = "1015e14d-2e03-486b-9a96-c5bc1e21aefa", type = "String" },
                            //new Bpm.Variable { name = "tenantid", value = "9ff9c399-6ff3-4efd-891a-7ffafc5c02b4", type = "String" },
                            new Bpm.Variable { name = "InfKanalPortal", value = "false", type = "Boolean" },
                            new Bpm.Variable { name = "InfKanalTlac", value = "false", type = "Boolean" },
                            new Bpm.Variable { name = "InfKanalUrTabula", value = "false", type = "Boolean" },
                            new Bpm.Variable { name = "InfKanalFacebook", value = "true", type = "Boolean" },
                            new Bpm.Variable { name = "InfKanalRozhlas", value = "false", type = "Boolean" },
                            new Bpm.Variable { name = "PredmetOznamu", value = "popis test", type = "String" },
                            new Bpm.Variable { name = "DatumPublikovania", value = "2014-11-27T10:48:46", type = "String" },
                            new Bpm.Variable { name = "D_Databuffer_Id", value = "29", type = "String" },
                            new Bpm.Variable { name = "D_Pohlad_Id", value = "", type = "String" },
                            new Bpm.Variable { name = "Zaznam_Id", value = "", type = "String" }
                        }
                    });
                }
            }
            catch (FaultException<Bpm.BpmFault> ex)
            {
                throw new WebEasProxyException(string.Format("Fault Exception in StartProcessInstance {0}", ex.Detail.message), "Nastala chyba pri volaní služby Bpm", ex);
            }
            catch (FaultException ex)
            {
                throw new WebEasProxyException(string.Format("Fault Exception in StartProcessInstance {0}", ex.Reason.ToString()), "Nastala chyba pri volaní služby Bpm", ex);
            }
            catch (Exception ex)
            {
                throw new WebEasProxyException("Error in StartProcessInstance", "Nastala chyba pri volaní služby Bpm", ex);
            }
        }

        [TestMethod]
        public void StartProcessInstanceFaultTest()
        {
            try
            {
                using (var client = new WebEas.Services.Esb.BpmProxy(null))
                {
                    var response = client.StartProcessInstance(new Bpm.StartProcessInstanceRequest
                    {
                        ProcessDefinition = new Bpm.ProcessDefinition { Item = "dcom-iap", ItemElementName = Bpm.ItemChoiceType.id },
                        businessKey = "",
                        modulName = "ss",
                        Variables = new Bpm.Variable[]
                        { 
                            new Bpm.Variable { name = "assign", value = "1015e14d-2e03-486b-9a96-c5bc1e21aefa", type = "String" },
                            //new Bpm.Variable { name = "tenantid", value = "9ff9c399-6ff3-4efd-891a-7ffafc5c02b4", type = "String" },
                            new Bpm.Variable { name = "InfKanalPortal", value = "false", type = "Boolean" },
                            new Bpm.Variable { name = "InfKanalTlac", value = "false", type = "Boolean" },
                            new Bpm.Variable { name = "InfKanalUrTabula", value = "false", type = "Boolean" },
                            new Bpm.Variable { name = "InfKanalFacebook", value = "true", type = "Boolean" },
                            new Bpm.Variable { name = "InfKanalRozhlas", value = "false", type = "Boolean" },
                            new Bpm.Variable { name = "PredmetOznamu", value = "popis test", type = "String" },
                            new Bpm.Variable { name = "DatumPublikovania", value = "2014-11-27T10:48:46", type = "String" },
                            new Bpm.Variable { name = "D_Databuffer_Id", value = "29", type = "String" },
                            new Bpm.Variable { name = "D_Pohlad_Id", value = "", type = "String" },
                            new Bpm.Variable { name = "Zaznam_Id", value = "", type = "String" }
                        }
                    });
                }
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null && ex.InnerException is FaultException<Bpm.BpmFault>)
                {

                }
                else throw;
            }
        }
    }
}
