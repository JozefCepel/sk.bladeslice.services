using Funq;
using ServiceStack;
using ServiceStack.Auth;
using ServiceStack.Data;
using ServiceStack.DataAnnotations;
using ServiceStack.OrmLite;
using ServiceStack.Testing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using WebEas.Core.Ninject;
using WebEas.Esam.ServiceInterface.Office.Dap;
using WebEas.Esam.ServiceInterface.Office.Osa;
using WebEas.Esam.ServiceModel.Office;
using WebEas.Esam.ServiceModel.Office.Dap.Dto;
using WebEas.Esam.ServiceModel.Office.Dap.Types;
using WebEas.Esam.ServiceModel.Office.Osa.Dto;
using WebEas.ServiceInterface;
using WebEas.UnitTest.IsoDap;
using Xunit;
using Xunit.Priority;

namespace WebEas.UnitTest
{
    [TestCaseOrderer(PriorityOrderer.Name, PriorityOrderer.Assembly)]
    public class IsoTest : BaseTest
    {
        public IsoTest() : base()
        {
            System.Net.ServicePointManager.ServerCertificateValidationCallback = (sender, cert, chain, errors) =>
            {
                return cert.Subject.Contains("isodatalan.intra.dcom.sk");
            };
        }

        #region OSA

        //[Fact(Skip = "Nateraz vypnute")]
        public void NotifyPersonDataChange()
        {
            using (var client = new JsonServiceClient("http://localhost:82/esam/api/office/osa"))
            {
                var authResponse = client.Post(new Authenticate
                {
                    provider = CredentialsAuthProvider.Name,
                    UserName = "IsoNotifyTechUser",
                    Password = "p@55w0rd222",
                    Meta = new Dictionary<string, string> { { "TenantId", "4099558D-3D12-4D50-A101-489C7E7B1317" } }
                });

                var notifyPersonDataChangeDto = new NotifyPersonDataChangeDto
                {
                    IsoId = "request.DcmHeader.isoId",
                    OfficeId = "request.DcmHeader.officeId",
                    RequestId = "request.DcmHeader.requestId",
                    TenantId = "4099558D-3D12-4D50-A101-489C7E7B1317",
                    PersonDataChange = new PersonDataChangeDto[] { new PersonDataChangeDto { ChangeCode  = "NEW", DCOMPersonId = "059A6D3D-E0FD-4755-B788-1B79A0F553CC" } }
                };
                client.Post<NotifyPersonDataChangeDto>(notifyPersonDataChangeDto);
                client.Post(new Authenticate { provider = "logout" });
            }

            /*using (var appHost = GetBasicAppHost(System.Configuration.ConfigurationManager.ConnectionStrings["EsamOsaConnString"].ConnectionString, "osa"))
            {
                var service = appHost.Container.Resolve<OsaService>();
                service.Post(new NotifyPersonDataChangeDto { TenantId = "12121212121" });
            }*/
        }

        //[Fact(Skip = "Iba s vytocenou DCOM vpn")]
        public void OsaIntegration()
        {
            using (var appHost = GetBasicAppHost(System.Configuration.ConfigurationManager.ConnectionStrings["EsamOsaConnString"].ConnectionString, "osa"))
            {
                var dcmHeader = new IsoOsa.DcmHeader
                {
                    tenantId = "4099558d-3d12-4d50-a101-489c7e7b1317",
                    isoId =  "BF153004-33EB-41E0-8669-5386D672CA01",
                    requestId = Guid.NewGuid().ToString()
                };


                var req = new IsoOsa.ReqReplicatePerson
                {
                    replicate = (IsoOsa.ReplicateFilter[])Enum.GetValues(typeof(IsoOsa.ReplicateFilter)),
                    startWith = new IsoOsa.PersonCursor
                    {
                        personId = new IsoOsa.DCOMPersonId[]
                        {
                        new IsoOsa.DCOMPersonId
                        {
                            DCOMPersonId1 = string.Empty
                        }
                        }
                    }
                };

                var service = appHost.Container.Resolve<OsaService>();
                var repository = service.Repository as WebEasRepositoryBase;
                var persons = new List<IsoOsa.ReplicatedPerson>();
            }
        }

        #endregion

        //[Fact, Xunit.Priority.Priority(1)]
        //[Fact(Skip = "Iba s vytocenou DCOM vpn")]
        public void DapIntegration()
        {
            using (var appHost = GetBasicAppHost(System.Configuration.ConfigurationManager.ConnectionStrings["EsamDapConnString"].ConnectionString, "dap"))
            {
                StringBuilder errorlog = new StringBuilder();

                var dcmHeader = new DcmHeader
                {
                    tenantId = "4099558D-3D12-4D50-A101-489C7E7B1317",
                    isoId = "BF153004-33EB-41E0-8669-5386D672CA01"
                };

                using (var client = new DanePoplatkyClient())
                {
                    dcmHeader.requestId = Guid.NewGuid().ToString();
                    var taxOrFeeTypes = client.getTaxOrFeeTypes(ref dcmHeader, new ReqBase());

                    foreach (var tax in taxOrFeeTypes.resultRecords)
                    {
                        if (errorlog.ToString().Split('\n').Length > 100)
                        {
                            break;
                        }

                        foreach (TaxAssessmentTypeEnum taxAssessmentType in Enum.GetValues(typeof(TaxAssessmentTypeEnum)))
                        {
                            if (errorlog.ToString().Split('\n').Length > 100)
                            {
                                break;
                            }

                            dcmHeader.requestId = Guid.NewGuid().ToString();
                            var rozhodnutia = client.getTaxAssessments(ref dcmHeader,
                            new ReqGetTaxAssessments
                            {
                                taxAssessmentTypeSpecified = true,
                                taxAssessmentType = taxAssessmentType,
                                taxOrFeeTypeCode = tax.typeCode,
                                dateFromSpecified = false,
                                dateToSpecified = false,
                                useAssessmentDate = true
                            });

                            foreach (var taxAssessment in rozhodnutia.resultRecords)
                            {
                                string vymerText = $"taxAssessment {taxAssessment.year}-{taxAssessment.taxOrFeeTypeCode}-{taxAssessment.taxAssessmentType} ({taxAssessment.decisionID}): ";

                                if (taxAssessment.amount01Specified && taxAssessment.amount01 == 0)
                                {
                                    errorlog.AppendLine(string.Concat(vymerText, "amount01 = ", taxAssessment.amount01));
                                }

                                if (taxAssessment.amount02Specified && taxAssessment.amount02 == 0)
                                {
                                    errorlog.AppendLine(string.Concat(vymerText, "amount02 = ", taxAssessment.amount02));
                                }

                                if (taxAssessment.amount03Specified && taxAssessment.amount03 == 0)
                                {
                                    errorlog.AppendLine(string.Concat(vymerText, "amount03 = ", taxAssessment.amount03));
                                }

                                if (taxAssessment.amount04Specified && taxAssessment.amount04 == 0)
                                {
                                    errorlog.AppendLine(string.Concat(vymerText, "amount04 = ", taxAssessment.amount04));
                                }

                                if (taxAssessment.amount05Specified && taxAssessment.amount05 == 0)
                                {
                                    errorlog.AppendLine(string.Concat(vymerText, "amount05 = ", taxAssessment.amount05));
                                }

                                if (taxAssessment.amount06Specified && taxAssessment.amount06 == 0)
                                {
                                    errorlog.AppendLine(string.Concat(vymerText, "amount06 = ", taxAssessment.amount06));
                                }

                                if (taxAssessment.amount07Specified && taxAssessment.amount07 == 0)
                                {
                                    errorlog.AppendLine(string.Concat(vymerText, "amount07 = ", taxAssessment.amount07));
                                }

                                if (taxAssessment.amount08Specified && taxAssessment.amount08 == 0)
                                {
                                    errorlog.AppendLine(string.Concat(vymerText, "amount08 = ", taxAssessment.amount08));
                                }

                                if (taxAssessment.amount09Specified && taxAssessment.amount09 == 0)
                                {
                                    errorlog.AppendLine(string.Concat(vymerText, "amount09 = ", taxAssessment.amount09));
                                }

                                if (taxAssessment.amount10Specified && taxAssessment.amount10 == 0)
                                {
                                    errorlog.AppendLine(string.Concat(vymerText, "amount10 = ", taxAssessment.amount10));
                                }

                                if (taxAssessment.amount11Specified && taxAssessment.amount11 == 0)
                                {
                                    errorlog.AppendLine(string.Concat(vymerText, "amount11 = ", taxAssessment.amount11));
                                }

                                if (taxAssessment.amount12Specified && taxAssessment.amount12 == 0)
                                {
                                    errorlog.AppendLine(string.Concat(vymerText, "amount12 = ", taxAssessment.amount12));
                                }

                                if (taxAssessment.dueDate01Specified && taxAssessment.dueDate01 == DateTime.MinValue)
                                {
                                    errorlog.AppendLine(string.Concat(vymerText, "Date01 = ", taxAssessment.dueDate01.ToString()));
                                }

                                if (taxAssessment.dueDate02Specified && taxAssessment.dueDate02 == DateTime.MinValue)
                                {
                                    errorlog.AppendLine(string.Concat(vymerText, "Date02 = ", taxAssessment.dueDate02.ToString()));
                                }

                                if (taxAssessment.dueDate03Specified && taxAssessment.dueDate03 == DateTime.MinValue)
                                {
                                    errorlog.AppendLine(string.Concat(vymerText, "Date03 = ", taxAssessment.dueDate03.ToString()));
                                }

                                if (taxAssessment.dueDate04Specified && taxAssessment.dueDate04 == DateTime.MinValue)
                                {
                                    errorlog.AppendLine(string.Concat(vymerText, "Date04 = ", taxAssessment.dueDate04.ToString()));
                                }

                                if (taxAssessment.dueDate05Specified && taxAssessment.dueDate05 == DateTime.MinValue)
                                {
                                    errorlog.AppendLine(string.Concat(vymerText, "Date05 = ", taxAssessment.dueDate05.ToString()));
                                }

                                if (taxAssessment.dueDate06Specified && taxAssessment.dueDate06 == DateTime.MinValue)
                                {
                                    errorlog.AppendLine(string.Concat(vymerText, "Date06 = ", taxAssessment.dueDate06.ToString()));
                                }

                                if (taxAssessment.dueDate07Specified && taxAssessment.dueDate07 == DateTime.MinValue)
                                {
                                    errorlog.AppendLine(string.Concat(vymerText, "Date07 = ", taxAssessment.dueDate07.ToString()));
                                }

                                if (taxAssessment.dueDate08Specified && taxAssessment.dueDate08 == DateTime.MinValue)
                                {
                                    errorlog.AppendLine(string.Concat(vymerText, "Date08 = ", taxAssessment.dueDate08.ToString()));
                                }

                                if (taxAssessment.dueDate09Specified && taxAssessment.dueDate09 == DateTime.MinValue)
                                {
                                    errorlog.AppendLine(string.Concat(vymerText, "Date09 = ", taxAssessment.dueDate09.ToString()));
                                }

                                if (taxAssessment.dueDate10Specified && taxAssessment.dueDate10 == DateTime.MinValue)
                                {
                                    errorlog.AppendLine(string.Concat(vymerText, "Date10 = ", taxAssessment.dueDate10.ToString()));
                                }

                                if (taxAssessment.dueDate11Specified && taxAssessment.dueDate11 == DateTime.MinValue)
                                {
                                    errorlog.AppendLine(string.Concat(vymerText, "Date11 = ", taxAssessment.dueDate11.ToString()));
                                }

                                if (taxAssessment.dueDate12Specified && taxAssessment.dueDate12 == DateTime.MinValue)
                                {
                                    errorlog.AppendLine(string.Concat(vymerText, "Date12 = ", taxAssessment.dueDate12.ToString()));
                                }

                                if (taxAssessment.assessmentDate == DateTime.MinValue)
                                {
                                    errorlog.AppendLine(string.Concat(vymerText, "assessmentDate = ", taxAssessment.assessmentDate.ToString()));
                                }

                                if (taxAssessment.maturityDate == DateTime.MinValue)
                                {
                                    //errorlog.AppendLine(string.Concat(vymerText, "maturityDate = ", taxAssessment.maturityDate.ToString()));
                                }

                                if (taxAssessment.validityDate == DateTime.MinValue)
                                {
                                    //errorlog.AppendLine(string.Concat(vymerText, "validityDate = ", taxAssessment.validityDate.ToString()));
                                }

                                if (taxAssessment.taxAssessmentType != taxAssessmentType)
                                {
                                    errorlog.AppendLine(string.Concat(vymerText, "taxAssessmentType = ", taxAssessment.taxAssessmentType.ToString()));
                                }

                                if (string.IsNullOrEmpty(taxAssessment.collectivePayment))
                                {
                                    //errorlog.AppendLine(string.Concat(vymerText, "collectivePayment = Empty"));
                                }

                                if (string.IsNullOrEmpty(taxAssessment.decisionNumber))
                                {
                                    errorlog.AppendLine(string.Concat(vymerText, "decisionNumber = Empty"));
                                }

                                if (taxAssessment.decisionID == 0)
                                {
                                    errorlog.AppendLine(string.Concat(vymerText, "decisionID = 0"));
                                }

                                if (string.IsNullOrEmpty(taxAssessment?.person?.dcomID))
                                {
                                    errorlog.AppendLine(string.Concat(vymerText, "person IsNullOrEmpty"));
                                }

                                var match = Regex.Match(taxAssessment.payersReference ?? string.Empty, @"VS\d+");
                                if (!match.Success)
                                {
                                    errorlog.AppendLine(string.Concat(vymerText, " Nepodarilo sa vyparsovat VS! ", "payersReference = ", taxAssessment.payersReference));
                                }

                                if (match.Success)
                                {
                                    var vs = match.Groups[0].Value.Remove(0, 2);
                                    var strLength = typeof(Vymer).GetProperty(nameof(Vymer.VS)).GetCustomAttribute<StringLengthAttribute>().MaximumLength;

                                    if (string.IsNullOrEmpty(vs))
                                    {
                                        errorlog.AppendLine(string.Concat(vymerText, " VS je prazdny! ", "payersReference = ", taxAssessment.payersReference));
                                    }

                                    if (vs.Length > strLength)
                                    {
                                        errorlog.AppendLine(string.Concat(vymerText, $" VS ma vacsiu dlzku ako {strLength}! ", "payersReference = ", taxAssessment.payersReference));
                                    }
                                }

                                if (errorlog.ToString().Split('\n').Length > 100)
                                {
                                    break;
                                }
                            }
                        }
                    }

                }


                int pocet = errorlog.ToString().Split('\n').Length - 1;
                if (pocet > 0)
                {
                    errorlog.Insert(0, string.Concat("Počet chýb: ", pocet, Environment.NewLine));
                }

                Assert.True(errorlog.Length == 0, errorlog.ToString());
            }
        }
    }
}
