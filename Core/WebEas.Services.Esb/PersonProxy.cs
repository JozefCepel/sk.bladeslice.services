using System;
using System.Diagnostics;
using System.Linq;
using System.ServiceModel;
using WebEas.Services.Esb.Person;

namespace WebEas.Services.Esb
{
    /// <summary>
    /// Person proxy
    /// </summary>
    /// <example>
    /// using(PersonProxy proxy = new PersonProxy())
    /// {
    /// }
    /// </example>
    public class PersonProxy : ProxyBase, IDisposable
    {
        public const string ServiceUrl = "https://lbsoa.intra.dcom.sk/soa/person/2.0/personService";

        private Person.PersonServicesChannel proxy;

        /// <summary>
        /// Initializes a new instance of the <see cref="PersonProxy" /> class.
        /// </summary>
        public PersonProxy(string stsThumbprint) : base(stsThumbprint)
        {
            this.proxy = WebEas.Core.Sts.SecurityTokenServiceHelper.CreateChannelProxy<Person.PersonServicesChannel>(ServiceUrl, stsThumbprint);
        }

        /// <summary>
        /// Adds the person bank account.
        /// </summary>
        /// <param name="personId">The person id.</param>
        /// <param name="bankoveSpojenie">The bankove spojenie.</param>
        /// <returns></returns>
        public long? AddPersonBankAccount(BankoveSpojenieNaZapis bankoveSpojenie)
        { 
            try
            {
                WebEas.Core.Sts.SecurityTokenServiceHelper.CheckService(ref proxy, ServiceUrl, stsThumbPrint);

                var request = new AddPersonBankAccount(bankoveSpojenie);
                var stopwatch = new Stopwatch();
                stopwatch.Start();
                AddPersonBankAccountResponse response = this.proxy.AddPersonBankAccount(request);
                stopwatch.Stop();
                LogRequestDuration(ServiceUrl, stopwatch);
                return response.AddPersonBankAccountResponse1;
            }
            catch (FaultException<DcomFaultType> ex)
            {
                var msg = ex.Detail == null || string.IsNullOrEmpty(ex.Detail.faultMessage) ? ex.Message : ex.Detail.faultMessage;
                throw new WebEasValidationException(null, msg, ex);
            }
            catch (Exception ex)
            {
                this.CheckGlobalExceptions(ex);
                throw new WebEasProxyException("Error in AddPersonBankAccount", ex, bankoveSpojenie);
            }
        }

        /// <summary>
        /// Finds the person.
        /// </summary>
        /// <param name="vyhladavacieKriteria">The vyhladavacie kriteria.</param>
        /// <returns></returns>
        public OsobaList FindPerson(VyhladavacieKriteria vyhladavacieKriteria)
        {
            try
            {
                WebEas.Core.Sts.SecurityTokenServiceHelper.CheckService(ref proxy, ServiceUrl, stsThumbPrint);

                var request = new FindPerson(vyhladavacieKriteria);
                var stopwatch = new Stopwatch();
                stopwatch.Start();
                FindPersonResponse response = this.proxy.FindPerson(request);
                stopwatch.Stop();
                LogRequestDuration(ServiceUrl, stopwatch);
                return response.OsobaList;
            }
            catch (FaultException<DcomFaultType> ex)
            {
                var msg = ex.Detail == null || string.IsNullOrEmpty(ex.Detail.faultMessage) ? ex.Message : ex.Detail.faultMessage;
                throw new WebEasValidationException(null, msg, ex);
            }
            catch (Exception ex)
            {
                this.CheckGlobalExceptions(ex);
                throw new WebEasProxyException("Error in FindPerson", ex, vyhladavacieKriteria);
            }
        }

        /// <summary>
        /// Vyhľadá kolekciu FO aj PO podľa zadanáho reťazca.
        /// Ak je zadaný číselný reťazec s menej ako 4 znakmi alebo ľubovolný
        /// reťazec s menej ako 2 znakmi, vráti validačnú chybu.Pravidlá
        /// vyhľadávania sú popísané v algoritme.Fyzické osoby radí vzostupne
        /// podľa priezviska, právnické vzostupne podľa názvu.
        /// </summary>
        /// <param name="findPersonComboReq">The vyhladavacie kriteria.</param>
        /// <returns></returns>
        public findPersonComboResponse FindPersonCombo(findPersonComboRequest findPersonComboReq)
        {
            try
            {
                WebEas.Core.Sts.SecurityTokenServiceHelper.CheckService(ref proxy, ServiceUrl, stsThumbPrint);

                var request = new FindPersonCombo(findPersonComboReq);
                var stopwatch = new Stopwatch();
                stopwatch.Start();
                FindPersonComboResponse1 response = this.proxy.FindPersonCombo(request);
                stopwatch.Stop();
                LogRequestDuration(ServiceUrl, stopwatch);
                return response.FindPersonComboResponse;
            }
            catch (FaultException<DcomFaultType> ex)
            {
                var msg = ex.Detail == null || string.IsNullOrEmpty(ex.Detail.faultMessage) ? ex.Message : ex.Detail.faultMessage;
                throw new WebEasValidationException(null, msg, ex);
            }
            catch (Exception ex)
            {
                this.CheckGlobalExceptions(ex);
                throw new WebEasProxyException("Error in FindPersonCombo", ex, findPersonComboReq);
            }
        }

        /// <summary>
        /// Gets the person.
        /// </summary>
        /// <param name="osobaRequest">The osoba request.</param>
        /// <returns></returns>
        public OsobaDetail GetPerson(OsobaRequest osobaRequest)
        {
            try
            {
                WebEas.Core.Sts.SecurityTokenServiceHelper.CheckService(ref proxy, ServiceUrl, stsThumbPrint);

                var request = new GetPerson(osobaRequest);
                var stopwatch = new Stopwatch();
                stopwatch.Start();
                GetPersonResponse response = this.proxy.GetPerson(request);
                stopwatch.Stop();
                LogRequestDuration(ServiceUrl, stopwatch);
                return response.OsobaDetail;
            }
            catch (FaultException<DcomFaultType> ex)
            {
                var msg = ex.Detail == null || string.IsNullOrEmpty(ex.Detail.faultMessage) ? ex.Message : ex.Detail.faultMessage;
                throw new WebEasValidationException(null, msg, ex);
            }
            catch (Exception ex)
            {
                this.CheckGlobalExceptions(ex);
                throw new WebEasProxyException("Error in GetPerson", ex, osobaRequest);
            }
        }

        /// <summary>
        /// Gets the person by upvs identifiers.
        /// </summary>
        /// <param name="actor">The actor.</param>
        /// <param name="subject">The subject.</param>
        /// <returns></returns>
        public GetPersonByUpvsIdentifiersResponse GetPersonByUpvsIdentifiers(UpvsIdentifiers actor, UpvsIdentifiers subject)
        {
            try
            {
                WebEas.Core.Sts.SecurityTokenServiceHelper.CheckService(ref proxy, ServiceUrl, stsThumbPrint);

                var request = new GetPersonByUpvsIdentifiers(new GetPersonByUpvsIdentifiersRequest { Actor = actor, Subject = subject });
                var stopwatch = new Stopwatch();
                stopwatch.Start();
                GetPersonByUpvsIdentifiersResponse1 response = this.proxy.GetPersonByUpvsIdentifiers(request);
                stopwatch.Stop();
                LogRequestDuration(ServiceUrl, stopwatch);
                return response.GetPersonByUpvsIdentifiersResponse;
            }
            catch (FaultException<DcomFaultType> ex)
            {
                var msg = ex.Detail == null || string.IsNullOrEmpty(ex.Detail.faultMessage) ? ex.Message : ex.Detail.faultMessage;
                throw new WebEasValidationException(null, msg, ex);
            }
            catch (Exception ex)
            {
                this.CheckGlobalExceptions(ex);
                throw new WebEasProxyException("Error in GetPersonByUpvsIdentifiers", ex, actor, subject);
            }
        }

        /// <summary>
        /// Gets the person duplicates.
        /// </summary>
        /// <param name="personId">The person id.</param>
        /// <returns></returns>
        public DuplicitnaOsoba[] GetPersonDuplicates(string personId)
        {
            try
            {
                WebEas.Core.Sts.SecurityTokenServiceHelper.CheckService(ref proxy, ServiceUrl, stsThumbPrint);

                var request = new GetPersonDuplicates(new DuplicitnaOsobaRequest { PersonId = personId });
                var stopwatch = new Stopwatch();
                stopwatch.Start();
                GetPersonDuplicatesResponse response = this.proxy.GetPersonDuplicates(request);
                stopwatch.Stop();
                LogRequestDuration(ServiceUrl, stopwatch);
                return response.DuplicitnaOsobaList;
            }
            catch (FaultException<DcomFaultType> ex)
            {
                var msg = ex.Detail == null || string.IsNullOrEmpty(ex.Detail.faultMessage) ? ex.Message : ex.Detail.faultMessage;
                throw new WebEasValidationException(null, msg, ex);
            }
            catch (Exception ex)
            {
                this.CheckGlobalExceptions(ex);
                throw new WebEasProxyException("Error in GetPersonDuplicates", ex, personId);
            }
        }

        /// <summary>
        /// List natural persons for address
        /// </summary>
        /// <param name="PobytyNaAdreseRequest">request</param>
        /// <returns></returns>
        public PobytNaAdrese[] ListNaturalPersonsForAddress(PobytyNaAdreseRequest request)
        {
            try
            {
                WebEas.Core.Sts.SecurityTokenServiceHelper.CheckService(ref proxy, ServiceUrl, stsThumbPrint);

                var stopwatch = new Stopwatch();
                stopwatch.Start();
                var response = proxy.ListNaturalPersonsForAddress(new ListNaturalPersonsForAddress(request));
                stopwatch.Stop();
                LogRequestDuration(ServiceUrl, stopwatch);
                return response.ListNaturalPersonsForAddressResponse1;
            }
            catch (FaultException<DcomFaultType> ex)
            {
                var msg = ex.Detail == null || string.IsNullOrEmpty(ex.Detail.faultMessage) ? ex.Message : ex.Detail.faultMessage;
                throw new WebEasValidationException(null, msg, ex);
            }
            catch (Exception ex)
            {
                this.CheckGlobalExceptions(ex);
                throw new WebEasProxyException("Error in ListNaturalPersonsForAddress", ex, request);
            }
        }

        /// <summary>
        /// Gets the person for public.
        /// </summary>
        /// <param name="osobaRequest">The osoba request.</param>
        /// <returns></returns>
        public OsobaDetail GetPersonForPublic(OsobaRequest osobaRequest)
        {
            try
            {
                WebEas.Core.Sts.SecurityTokenServiceHelper.CheckService(ref proxy, ServiceUrl, stsThumbPrint);

                var request = new GetPersonForPublic(osobaRequest);
                var stopwatch = new Stopwatch();
                stopwatch.Start();
                GetPersonForPublicResponse response = this.proxy.GetPersonForPublic(request);
                stopwatch.Stop();
                LogRequestDuration(ServiceUrl, stopwatch);
                return response.OsobaDetail;
            }
            catch (FaultException<DcomFaultType> ex)
            {
                var msg = ex.Detail == null || string.IsNullOrEmpty(ex.Detail.faultMessage) ? ex.Message : ex.Detail.faultMessage;
                throw new WebEasValidationException(null, msg, ex);
            }
            catch (Exception ex)
            {
                this.CheckGlobalExceptions(ex);
                throw new WebEasProxyException("Error in GetPersonForPublic", ex, osobaRequest);
            }
        }

        /// <summary>
        /// Gets the person identifiers.
        /// </summary>
        /// <param name="identifikatoryOsobyRequest">The identifikatory osoby request.</param>
        /// <returns></returns>
        public IdentifikatoryOsobyResponse GetPersonIdentifiers(IdentifikatoryOsobyRequest identifikatoryOsobyRequest)
        {
            try
            {
                WebEas.Core.Sts.SecurityTokenServiceHelper.CheckService(ref proxy, ServiceUrl, stsThumbPrint);

                var request = new GetPersonIdentifiers(identifikatoryOsobyRequest);
                var stopwatch = new Stopwatch();
                stopwatch.Start();
                GetPersonIdentifiersResponse response = this.proxy.GetPersonIdentifiers(request);
                stopwatch.Stop();
                LogRequestDuration(ServiceUrl, stopwatch);
                return response.IdentifikatoryOsobyResponse;
            }
            catch (FaultException<DcomFaultType> ex)
            {
                var msg = ex.Detail == null || string.IsNullOrEmpty(ex.Detail.faultMessage) ? ex.Message : ex.Detail.faultMessage;
                throw new WebEasValidationException(null, msg, ex);
            }
            catch (Exception ex)
            {
                this.CheckGlobalExceptions(ex);
                throw new WebEasProxyException("Error in GetPersonIdentifiers", ex, identifikatoryOsobyRequest);
            }
        }

        /// <summary>
        /// Checks the electronic delivery possible.
        /// </summary>
        /// <param name="personId">The person id.</param>
        /// <returns></returns>
        public CheckElectronicDeliveryPossibleResponse CheckElectronicDeliveryPossible(string personId)
        {
            try
            {
                WebEas.Core.Sts.SecurityTokenServiceHelper.CheckService(ref proxy, ServiceUrl, stsThumbPrint);

                var request = new CheckElectronicDeliveryPossible
                {
                    CheckElectronicDeliveryPossibleRequest = new CheckElectronicDeliveryPossibleRequest { PersonId = personId }
                };
                var stopwatch = new Stopwatch();
                stopwatch.Start();
                CheckElectronicDeliveryPossibleResponse1 response = this.proxy.CheckElectronicDeliveryPossible(request);
                stopwatch.Stop();
                LogRequestDuration(ServiceUrl, stopwatch);
                return response.CheckElectronicDeliveryPossibleResponse;
            }
            catch (FaultException<DcomFaultType> ex)
            {
                var msg = ex.Detail == null || string.IsNullOrEmpty(ex.Detail.faultMessage) ? ex.Message : ex.Detail.faultMessage;
                throw new WebEasValidationException(null, msg, ex);
            }
            catch (Exception ex)
            {
                this.CheckGlobalExceptions(ex);
                throw new WebEasProxyException("Error in CheckElectronicDeliveryPossible", ex, personId);
            }
        }

        /// <summary>
        /// Get legal person
        /// </summary>
        /// <param name="personId">person id </param>
        public OsobaDetail GetLegalPerson(string personId)
        {
            try
            {
                WebEas.Core.Sts.SecurityTokenServiceHelper.CheckService(ref proxy, ServiceUrl, stsThumbPrint);

                var request = new getLegalPerson
                {
                    GetLegalPersonRequest = personId
                };

                var stopwatch = new Stopwatch();
                stopwatch.Start();
                var response = proxy.getLegalPerson(request);
                stopwatch.Stop();
                LogRequestDuration(ServiceUrl, stopwatch);
                return response.OsobaDetail;
            }
            catch (FaultException<DcomFaultType> ex)
            {
                var msg = ex.Detail == null || string.IsNullOrEmpty(ex.Detail.faultMessage) ? ex.Message : ex.Detail.faultMessage;
                throw new WebEasValidationException(null, msg, ex);
            }
            catch (Exception ex)
            {
                CheckGlobalExceptions(ex);
                throw new WebEasProxyException("Error in GetLegalPerson", ex, personId);
            }
        }

        /// <summary>
        /// Get legal persons
        /// </summary>
        /// <param name="personId">person ids </param>
        public OsobaDetail[] GetLegalPersons(string[] personId)
        {
            try
            {
                WebEas.Core.Sts.SecurityTokenServiceHelper.CheckService(ref proxy, ServiceUrl, stsThumbPrint);

                var request = new getLegalPersons
                {
                    GetLegalPersonsRequest = personId
                };

                var stopwatch = new Stopwatch();
                stopwatch.Start();
                var response = proxy.getLegalPersons(request);
                stopwatch.Stop();
                LogRequestDuration(ServiceUrl, stopwatch);
                return response?.GetLegalPersonsResponse1?.Osoba;
            }
            catch (FaultException<DcomFaultType> ex)
            {
                var msg = ex.Detail == null || string.IsNullOrEmpty(ex.Detail.faultMessage) ? ex.Message : ex.Detail.faultMessage;
                throw new WebEasValidationException(null, msg, ex);
            }
            catch (Exception ex)
            {
                CheckGlobalExceptions(ex);
                throw new WebEasProxyException("Error in GetLegalPersons", ex, personId);
            }
        }

        /// <summary>
        /// Get natural person
        /// </summary>
        /// <param name="personId">person id </param>
        public OsobaDetail GetNaturalPerson(string personId)
        {
            try
            {
                WebEas.Core.Sts.SecurityTokenServiceHelper.CheckService(ref proxy, ServiceUrl, stsThumbPrint);

                var request = new getNaturalPerson
                {
                    GetNaturalPersonRequest = personId
                };

                var stopwatch = new Stopwatch();
                stopwatch.Start();
                var response = proxy.getNaturalPerson(request);
                stopwatch.Stop();
                LogRequestDuration(ServiceUrl, stopwatch);
                return response.OsobaDetail;
            }
            catch (FaultException<DcomFaultType> ex)
            {
                var msg = ex.Detail == null || string.IsNullOrEmpty(ex.Detail.faultMessage) ? ex.Message : ex.Detail.faultMessage;
                throw new WebEasValidationException(null, msg, ex);
            }
            catch (Exception ex)
            {
                CheckGlobalExceptions(ex);
                throw new WebEasProxyException("Error in GetNaturalPerson", ex, personId);
            }
        }

        /// <summary>
        /// Get natural persons
        /// </summary>
        /// <param name="personId">person ids </param>
        public OsobaDetail[] GetNaturalPersons(string[] personId)
        {
            try
            {
                WebEas.Core.Sts.SecurityTokenServiceHelper.CheckService(ref proxy, ServiceUrl, stsThumbPrint);

                var request = new getNaturalPersons
                {
                    GetNaturalPersonsRequest = personId
                };

                var stopwatch = new Stopwatch();
                stopwatch.Start();
                var response = proxy.getNaturalPersons(request);
                stopwatch.Stop();
                LogRequestDuration(ServiceUrl, stopwatch);
                return response?.GetNaturalPersonsResponse1?.Osoba;
            }
            catch (FaultException<DcomFaultType> ex)
            {
                var msg = ex.Detail == null || string.IsNullOrEmpty(ex.Detail.faultMessage) ? ex.Message : ex.Detail.faultMessage;
                throw new WebEasValidationException(null, msg, ex);
            }
            catch (Exception ex)
            {
                CheckGlobalExceptions(ex);
                throw new WebEasProxyException("Error in GetNaturalPersons", ex, personId);
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