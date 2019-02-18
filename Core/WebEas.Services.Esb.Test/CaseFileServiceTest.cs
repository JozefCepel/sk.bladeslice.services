using System;
using System.ServiceModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ServiceStack;
using WebEas.Services.Esb.Epod.CaseFile;

namespace WebEas.Services.Esb.Test
{
    [TestClass]
    public class CaseFileServiceTest : TestBaseProxy
    {
        [TestMethod]
        public void GetRecordDetailsTest()
        {
            using (var client = new WebEas.Services.Esb.EpodCaseFileProxy(null))
            {
                var data = client.GetRecordDetails(new RecordDetailRequestDto
                {
                    recordId = 15,
                    formDataOnly = true
                });
            }
        }

        [TestMethod]
        public void GetRecordDetailsFaultTest()
        {
            try
            {
                using (var client = new WebEas.Services.Esb.EpodCaseFileProxy(null))
                {
                    var data = client.GetRecordDetails(new RecordDetailRequestDto
                    {
                        recordId = -1,
                        formDataOnly = true
                    });
                }
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null && ex.InnerException is FaultException<WebEas.Services.Esb.Epod.CaseFile.ERegistryException>)
                {

                }
                else throw;
            }
        }

        [TestMethod]
        public void InsertBulkRecordTest()
        {
            var req = WebEas.Services.Esb.Test.Requests.InsertBulkRecordTest.FromJson<InsertBulkRecordDtoList>();

            using (var client = new WebEas.Services.Esb.EpodCaseFileProxy(null))
            {
                var data = client.InsertBulkRecord(req);
                client.DiscardRecordBulk(data.bulkId);
                
            }
        }

        [TestMethod]
        public void InsertRecordTest()
        {
            //int ePodatelnaId = 4358;
            //string xml = "<GenericRuling xmlns=\"http://schemas.gov.sk/form/DCOM_Rozhodnutia_GenerickeRozhodnutie_sk/1.0\"><MunicipalityHeader><NameOfOffice><TitleSk>Test Office</TitleSk></NameOfOffice></MunicipalityHeader><RulingHeader><RulingCode>001</RulingCode><ResponsiblePerson><PersonData><PhysicalPerson><PersonName><GivenName>FirstName</GivenName><FamilyName>Surname</FamilyName></PersonName></PhysicalPerson></PersonData></ResponsiblePerson></RulingHeader><RulingBody><Text>Vy\u017eiadanie dopl\u0148uj\u00facich inform\u00e1ci\u00ed bolo zamietnut\u00e9.</Text><Reasoning>Nie je mo\u017en\u00e9 poskytn\u00fa\u0165 \u0161irokej verejnosti v\u00fdpis bankov\u00e9ho \u00fa\u010dtu p\u00e1na premiera.</Reasoning><Charge>Poskytovanie osobn\u00fdch inform\u00e1ci\u00ed verejn\u00fdch \u010dinite\u013eov s\u00fa zak\u00e1zan\u00e9</Charge></RulingBody><SignatoryPerson><PersonData><PhysicalPerson><PersonName><GivenName>FirstName</GivenName><FamilyName>Surname</FamilyName></PersonName></PhysicalPerson></PersonData></SignatoryPerson><DeliveryTo><PersonData><PhysicalPerson><PersonName><GivenName>FirstName</GivenName><FamilyName>Surname</FamilyName></PersonName></PhysicalPerson><PhysicalAddress><Country><TitleSk>Slovensko</TitleSk></Country><Area>Kosicky</Area><County><TitleSk>Kosicky</TitleSk></County><Municipality><TitleSk>Kosice</TitleSk></Municipality><District>Barca</District><StreetName>Ulica</StreetName><BuildingNumber>5</BuildingNumber><PropertyRegistrationNumber>15</PropertyRegistrationNumber><Unit> /2</Unit><DeliveryAddress><PostalCode>04414</PostalCode></DeliveryAddress></PhysicalAddress></PersonData></DeliveryTo></GenericRuling>";
            //  int ePodatelnaId = 4404;
            //string xml = "<GenericRuling xmlns=\"http://schemas.gov.sk/form/DCOM_Rozhodnutia_GenerickeRozhodnutie_sk/1.0\"><MunicipalityHeader><NameOfOffice><TitleSk>Test Office</TitleSk></NameOfOffice></MunicipalityHeader><RulingHeader><RulingCode>001</RulingCode><ResponsiblePerson><PersonData><PhysicalPerson><PersonName><GivenName>FirstName</GivenName><FamilyName>Surname</FamilyName></PersonName></PhysicalPerson></PersonData></ResponsiblePerson></RulingHeader><RulingBody><Text>Vyžiadanie doplnujúcich informácií bolo zamietnuté.</Text><Reasoning>Nie je možné poskytnút širokej verejnosti výpis bankového úctu pána premiera.</Reasoning><Charge>Poskytovanie osobných informácií verejných cinitelov sú zakázané</Charge></RulingBody><SignatoryPerson><PersonData><PhysicalPerson><PersonName><GivenName>FirstName</GivenName><FamilyName>Surname</FamilyName></PersonName></PhysicalPerson></PersonData></SignatoryPerson><DeliveryTo><PersonData><PhysicalPerson><PersonName><GivenName>FirstName</GivenName><FamilyName>Surname</FamilyName></PersonName></PhysicalPerson><PhysicalAddress><Country><TitleSk>Slovensko</TitleSk></Country><Area>Kosicky</Area><County><TitleSk>Kosicky</TitleSk></County><Municipality><TitleSk>Kosice</TitleSk></Municipality><District>Barca</District><StreetName>Ulica</StreetName><BuildingNumber>5</BuildingNumber><PropertyRegistrationNumber>15</PropertyRegistrationNumber><Unit> /2</Unit><DeliveryAddress><PostalCode>04414</PostalCode></DeliveryAddress></PhysicalAddress></PersonData></DeliveryTo></GenericRuling>";
            //string xml = "<TaxDemandNote xmlns=\"http://schemas.gov.sk/form/DCOM_Rozhodnutia_RozhodnutieDorucovaciFormular_sk/1.0\"><DecisionCode>49</DecisionCode><TaxPayerPP><PersonData><PhysicalPerson><PersonName><GivenName>Petra</GivenName><FamilyName>Odlišná</FamilyName><Affix position=\"prefix\"></Affix><Affix position=\"postfix\"></Affix></PersonName></PhysicalPerson><ID><IdentifierType><TitleSk>Rodné číslo</TitleSk></IdentifierType><IdentifierValue>9662276085</IdentifierValue></ID><PhysicalAddress><Country><TitleSk>Slovenská republika</TitleSk></Country><County /><Municipality><TitleSk>Santovka</TitleSk></Municipality><StreetName>Parková</StreetName><BuildingNumber>2</BuildingNumber><PropertyRegistrationNumber>179</PropertyRegistrationNumber><DeliveryAddress><PostalCode>93587</PostalCode></DeliveryAddress></PhysicalAddress><TelephoneAddress><TelephoneType /><Number /></TelephoneAddress><InternetAddress /></PersonData></TaxPayerPP><Decision><Period>2014</Period><TotalAmount>19.950</TotalAmount><Instruction>Default poučenie</Instruction><Reasoning /><Tax><TypeOfTax>TKO</TypeOfTax><DueDate>2014-07-31</DueDate></Tax><Instalments><TotalAmount>19.950</TotalAmount><Instalment><DueDate>2014-05-07</DueDate><Amount>9.980</Amount></Instalment><Instalment><DueDate>2014-07-31</DueDate><Amount>9.970</Amount></Instalment><Instalment><DueDate>2099-12-31</DueDate><Amount>99999999999</Amount></Instalment><Instalment><DueDate>2099-12-31</DueDate><Amount>99999999999</Amount></Instalment><Instalment><DueDate>2099-12-31</DueDate><Amount>99999999999</Amount></Instalment><Instalment><DueDate>2099-12-31</DueDate><Amount>99999999999</Amount></Instalment><Instalment><DueDate>2099-12-31</DueDate><Amount>99999999999</Amount></Instalment><Instalment><DueDate>2099-12-31</DueDate><Amount>99999999999</Amount></Instalment><Instalment><DueDate>2099-12-31</DueDate><Amount>99999999999</Amount></Instalment><Instalment><DueDate>2099-12-31</DueDate><Amount>99999999999</Amount></Instalment><Instalment><DueDate>2099-12-31</DueDate><Amount>99999999999</Amount></Instalment><Instalment><DueDate>2099-12-31</DueDate><Amount>99999999999</Amount></Instalment></Instalments><BankAccountOfMunicipality><PersonData><BankConnection><Holder /><BankName>Všeobecná Úverová Banka Banka, a.s.</BankName><InternationalBankConnection><IBAN>SK4502000000000018728382</IBAN><BIC>SK4502000000000018728382</BIC></InternationalBankConnection></BankConnection></PersonData><PayerReference>218</PayerReference></BankAccountOfMunicipality></Decision></TaxDemandNote>";
            //try
            //{
            //    using (var client = new WebEas.Services.Esb.EpodCaseFileProxy(null))
            //    {
            //        var record =
            //                new WebEas.Services.Esb.Epod.CaseFile.InsertRecordDraftDto
            //                {

            //                    recordType = WebEas.Services.Esb.Epod.CaseFile.RecordType.RozhodnutieMeritorne,
            //                    subject = "Rozhodnutie",
            //                    relatedRecordId = 8201,
            //                    relatedRecordIdSpecified = true,
            //                    caseFileId = 1750,
            //                    caseFileIdSpecified = true,
            //                    //recordSource = Services.Esb.Epod.CaseFile.RecordSource.Vytvoreny,
            //                    confidentiality = Services.Esb.Epod.CaseFile.RecordConfidentiality.Pristupne,
            //                    confidentialitySpecified = false,
            //                    isMonitored = false,
            //                    InsertRecordPartDto = new WebEas.Services.Esb.Epod.CaseFile.InsertRecordPartDto[]
            //                    {
            //                        new WebEas.Services.Esb.Epod.CaseFile.InsertRecordPartDto
            //                        {
            //                            mimetype = "text/xml",
            //                            data = xml,
            //                            encoding = WebEas.Services.Esb.Epod.CaseFile.Encoding.XML,
            //                            description = "Rozhodnutie - doručovací formulár",
            //                            //formId = "http://schemas.gov.sk/form/DCOM_Rozhodnutia_GenerickeRozhodnutie_sk/1.0",
            //                            formId = "http://schemas.gov.sk/form/DCOM_Rozhodnutia_RozhodnutieDorucovaciFormular_sk/1.0",                                                                        
            //                            name = "Rozhodnutie",
            //                            partClass = WebEas.Services.Esb.Epod.CaseFile.MessagePartClass.FORM,                                
            //                        }
            //                    }

            //                };

            //        //var record = new WebEas.Services.Esb.Epod.CaseFile.InsertRecordDto
            //        // {
            //        //     isMonitored = true,
            //        //     isComplete = false,
            //        //     //correlationId = ePodatelnaId,
            //        //     //correlationIdSpecified = true,
            //        //     RecordDto = new WebEas.Services.Esb.Epod.CaseFile.RecordDto
            //        //     {
            //        //         recordType = WebEas.Services.Esb.Epod.CaseFile.RecordType.RozhodnutieOPreruseni,
            //        //         subject = "Generické rozhodnutie",
            //        //         relatedRecordId = ePodatelnaId,
            //        //         relatedRecordIdSpecified = true,
            //        //         recordSource = RecordSource.Vytvoreny,
            //        //         RecordPartDto = new WebEas.Services.Esb.Epod.CaseFile.RecordPartDto[]
            //        //        {
            //        //            new WebEas.Services.Esb.Epod.CaseFile.RecordPartDto
            //        //            {
            //        //                mimetype = "text/xml",
            //        //                data = xml,
            //        //                encoding = WebEas.Services.Esb.Epod.CaseFile.Encoding.XML,
            //        //                description = "Generické rozhodnutie",
            //        //                formId = "http://schemas.gov.sk/form/DCOM_Rozhodnutia_GenerickeRozhodnutie_sk/1.0",
            //        //                isSigned = false,
            //        //                name = "Generické rozhodnutie",
            //        //                partClass = WebEas.Services.Esb.Epod.CaseFile.MessagePartClass.FORM
            //        //            }
            //        //        }
            //        //     }
            //        // };

            //        var id = client.InsertRecord(record);
            //    }
            //}
            //catch (Exception ex)
            //{
            //    throw ex;
            //}
        }
    }
}