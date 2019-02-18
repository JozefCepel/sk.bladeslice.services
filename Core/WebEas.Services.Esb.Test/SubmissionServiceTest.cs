using System;
using System.ServiceModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace WebEas.Services.Esb.Test
{
    [TestClass]
    public class SubmissionServiceTest : TestBaseProxy
    {
        [TestMethod]
        public void CreateSubmissionFaultTest()
        {
            string xml = "";
            try
            {
                using (WebEas.Services.Esb.EpodSubmissionProxy proxy = new WebEas.Services.Esb.EpodSubmissionProxy(null))
                {
                    proxy.CreateSubmission(new WebEas.Services.Esb.Epod.Submission.CreateSubmissionDto
                    {
                        dateReceived = DateTime.Now,
                        //senderId = string.Empty,
                       senderId = "94bd82d8-e579-428c-9325-1c49c189a612",
                        //recipientId = "9FF9C399-6FF3-4EFD-891A-7FFAFC5C02B4",
                        subject = "Asistovane podanie",
                        RecordPartDto = new WebEas.Services.Esb.Epod.Submission.RecordPartDto[]
                            {
                                new WebEas.Services.Esb.Epod.Submission.RecordPartDto
                                {
                                    mimetype = "text/xml",
                                    data = xml,
                                    encoding = WebEas.Services.Esb.Epod.Submission.Encoding.XML,
                                    description = "Asistované podanie",
                                    //formId = nameSpace,
                                    isSigned = false,
                                    name = "test",
                                    partClass = WebEas.Services.Esb.Epod.Submission.MessagePartClass.FORM
                                }
                            },
                        submissionChannel = WebEas.Services.Esb.Epod.Submission.SubmissionChannel.ListinneBezDorucenky
                    });
                }
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null && ex.InnerException is FaultException<WebEas.Services.Esb.Epod.Submission.DcomFault>)
                {

                }
                else throw;
            }
        }

        [TestMethod]
        public void CreateSubmissionTest()
        {
            string xml = "<RequestForInformationByLawAboutFreeInformationAccessCB xmlns=\"http://schemas.gov.sk/form/DCOM_eDemokracia_ZiadostOPoskytnutieInformaciePO_sk/1.0\"><MunicipalityHeader><NameOfOffice><TitleSk>Litava</TitleSk></NameOfOffice><PersonData><PhysicalAddress><Municipality><TitleSk>Bratislava 1</TitleSk></Municipality><StreetName>Ra\u010dianska 100</StreetName><BuildingNumber>11 B</BuildingNumber><PropertyRegistrationNumber>20</PropertyRegistrationNumber><DeliveryAddress><PostalCode>80000</PostalCode></DeliveryAddress></PhysicalAddress></PersonData></MunicipalityHeader><Applicant><PersonData><CorporateBody><CorporateBodyFullName>Zvolensk\u00e1 Slatina</CorporateBodyFullName><LegalForm><TitleSk>Obec (obecn\u00fd \u00farad), mesto (mestsk\u00fd \u00farad)</TitleSk></LegalForm></CorporateBody><PhysicalAddress type=\"streetAddress\"><Country><TitleSk>Slovensk\u00e1 republika</TitleSk></Country><Municipality><TitleSk>Zvolen</TitleSk></Municipality><PropertyRegistrationNumber>66</PropertyRegistrationNumber><DeliveryAddress><PostalCode>04001</PostalCode></DeliveryAddress></PhysicalAddress><ID><IdentifierType><TitleSk>I\u010cO (Identifika\u010dn\u00e9 \u010d\u00edslo organiz\u00e1cie)</TitleSk></IdentifierType><IdentifierValue>00320447</IdentifierValue></ID></PersonData></Applicant><ResponsiblePerson><PersonData><PhysicalPerson><PersonName><GivenName>Jozko</GivenName><FamilyName>Pisko</FamilyName></PersonName></PhysicalPerson></PersonData></ResponsiblePerson><Request><Subject>dhgdf</Subject><Text>fghhhhhhhhhhhhhhhhhhhhhhhhhhhhh</Text></Request><DeliveryMethod>Po\u0161ta</DeliveryMethod></RequestForInformationByLawAboutFreeInformationAccessCB>";
            using (WebEas.Services.Esb.EpodSubmissionProxy proxy = new WebEas.Services.Esb.EpodSubmissionProxy(null))
            {
                proxy.CreateSubmission(new WebEas.Services.Esb.Epod.Submission.CreateSubmissionDto
                {
                    dateReceived = DateTime.Now,
                    //senderId = string.Empty,
                    senderId = "A6B0D1E0-112F-440B-B228-5BCD068535F3",
                    recipientId = "9FF9C399-6FF3-4EFD-891A-7FFAFC5C02B4",
                    //subject = "Asistovane podanie",
                    subject = "Žiadosť o poskytnutie informácie podľa zákona č. 211/2000 Z.z. o slobodnom prístupe k informáciám (PO)",
                    RecordPartDto = new WebEas.Services.Esb.Epod.Submission.RecordPartDto[]
                        {
                            new WebEas.Services.Esb.Epod.Submission.RecordPartDto
                            {
                                mimetype = "text/xml",
                                data = xml,
                                encoding = WebEas.Services.Esb.Epod.Submission.Encoding.XML,
                                description = "Asistované podanie",
                                //formId = nameSpace,
                                isSigned = false,
                                //name = "Žiadosť o poskytnutie informácie podľa zákona č. 211/2000 Z.z. o slobodnom prístupe k informáciám (PO)",
                                name = "test",
                                partClass = WebEas.Services.Esb.Epod.Submission.MessagePartClass.FORM
                            }
                        },
                    submissionChannel = WebEas.Services.Esb.Epod.Submission.SubmissionChannel.PodanieSObsluhou
                });
            }
        }
    }
}
