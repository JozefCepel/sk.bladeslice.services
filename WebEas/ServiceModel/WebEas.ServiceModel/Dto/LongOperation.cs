using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using ServiceStack;

namespace WebEas.ServiceModel.Dto
{
    //[Route("/long/{OperationName}/_start", "POST")]
    [DataContract]
    public class LongOperationStartDtoBase
    {
        [DataMember]
        //[ApiMember(Name = "OperationParameters", Description = "Parametre operacie", IsRequired = true, ParameterType = "body")]
        public string OperationParameters { get; set; }

        [DataMember]
        //[ApiMember(Name = "OperationName", Description = "Nazov operacie", DataType = "string", ParameterType = "path", IsRequired = true)]
        public string OperationName { get; set; }

        [DataMember]
        //[ApiMember(Name = "OperationInfo", Description = "Info o operacii", DataType = "string")]
        public string OperationInfo { get; set; }

        [DataMember]
        public string SessionId { get; set; }

        [DataMember]
        public string ProcessKey { get; set; }
    }

    //[Route("/long/restart/{ProcessKey}", "GET")]
    [DataContract]
    public class LongOperationRestartDtoBase
    {
        [DataMember]
        [ApiMember(Name = "ProcessKey", Description = "Identifikator operacie", DataType = "string", ParameterType = "path", IsRequired = true)]
        public string ProcessKey { get; set; }
    }

    //[Route("/long/{OperationName}/_progress/{ProcessKey}", "GET")]
    [DataContract]
    public class LongOperationProgressDtoBase
    {
        [DataMember]
        [ApiMember(Name = "ProcessKey", Description = "Identifikator operacie", DataType = "string", ParameterType = "path", IsRequired = true)]
        public string ProcessKey { get; set; }

        [DataMember]
        [ApiMember(Name = "OperationName", Description = "Nazov operacie", DataType = "string", ParameterType = "path", IsRequired = true)]
        public string OperationName { get; set; }
    }

    //[Route("/long/{OperationName}/_result/{ProcessKey}", "GET")]
    [DataContract]
    public class LongOperationResultDtoBase
    {
        [DataMember]
        [ApiMember(Name = "ProcessKey", Description = "Identifikator operacie", DataType = "string", ParameterType = "path", IsRequired = true)]
        public string ProcessKey { get; set; }

        [DataMember]
        [ApiMember(Name = "OperationName", Description = "Nazov operacie", DataType = "string", ParameterType = "path", IsRequired = true)]
        public string OperationName { get; set; }
    }

    //[Route("/long/{OperationName}/_cancel/{ProcessKey}", "POST")]
    [DataContract]
    public class LongOperationCancelDtoBase
    {
        [DataMember]
        [ApiMember(Name = "ProcessKey", Description = "Identifikator operacie", DataType = "string", ParameterType = "path", IsRequired = true)]
        public string ProcessKey { get; set; }

        [DataMember]
        [ApiMember(Name = "OperationName", Description = "Nazov operacie", DataType = "string", ParameterType = "path", IsRequired = true)]
        public string OperationName { get; set; }

        [DataMember]
        [ApiMember(Name = "NotRemove", Description = "Urcuje ci sa ma operacia odstranit, alebo iba nastavit priznak LongOperationState.Canceled", DataType = "bool")]
        public bool NotRemove { get; set; }
    }

    //[Route("/long/list", "GET")]
    [DataContract]
    public class LongOperationListDtoBase
    {
        [DataMember]
        [ApiMember(Name = "PerTenant", Description = "Ci sa maju nacitat long operacie za celu obec alebo iba daneho uzivatela (default za uziv.)", DataType = "bool")]
        public bool PerTenant { get; set; }

        [DataMember]
        [ApiMember(Name = "Skip", DataType = "int")]
        public int Skip { get; set; }

        [DataMember]
        [ApiMember(Name = "Take", DataType = "int")]
        public int Take { get; set; }
    }

    //[Route("/RunningLongOperationsWithRecords/{OperationName}", "GET")]
    [DataContract]
    public class RunningLongOperationsWithRecordsDtoBase
    {
        [DataMember]
        [ApiMember(Name = "OperationName", Description = "Nazov operacie", DataType = "string", ParameterType = "path", IsRequired = true)]
        public string OperationName { get; set; }
    }

    [DataContract]
    public class LongOperationStatus
    {
        private DateTime? start = null;
        private long? percent = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="LongOperationStatus" /> class.
        /// </summary>
        public LongOperationStatus()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LongOperationStatus" /> class.
        /// </summary>
        /// <param name="processKey">The process key.</param>
        public LongOperationStatus(string processKey)
        {
            this.ProcessKey = processKey;
        }

        /// <summary>
        /// Gets or sets the process key.
        /// </summary>
        /// <value>The process key.</value>
        [DataMember]
        public string ProcessKey { get; set; }

        /// <summary>
        /// Gets the is created.
        /// </summary>
        /// <value>The is created.</value>
        [DataMember]
        public bool IsFinished
        {
            get
            {
                return this.Result != null || this.HasError; // || this.Percents >= 100;
            }
        }

        /// <summary>
        /// Gets the has error.
        /// </summary>
        /// <value>The has error.</value>
        [DataMember]
        public bool HasError
        {
            get
            {
                return this.Error != null;
            }
        }

        /// <summary>
        /// Ci Result tejto operacie je subor (typeof(RenderResult))
        /// ..kedze tu nemame k dispozicii tuto triedu (referenciu) tak sa bude naplnat v RepositoryBase...
        /// </summary>
        [DataMember]
        public bool HasAttachement { get; set; }

        /// <summary>
        /// Gets or sets the not close.
        /// </summary>
        /// <value>The not close.</value>
        [DataMember]
        public bool NotClose { get; set; }

        /// <summary>
        /// Gets or sets the message.
        /// </summary>
        /// <value>The message.</value>
        [DataMember]
        public string Message { get; set; }

        /// <summary>
        /// Gets the percents.
        /// </summary>
        /// <value>The percents.</value>
        [DataMember]
        public long Percents
        {
            get
            {
                if (this.Count == 0)
                {
                    return 0;
                }

                long perPlus = 0;
                if (this.start.HasValue)
                {
                    perPlus += DateTime.Now.Subtract(this.start.Value).Seconds;
                }

                if (this.percent.HasValue)
                {
                    perPlus += this.percent.Value;
                }

                long per = (this.Current / this.Count) * 100;

                if (per < 100 && per + perPlus < 100)
                {
                    return per + perPlus;
                }
                else if (perPlus < 100 && perPlus > per)
                {
                    return perPlus;
                }
                return per;
            }
            set
            {
                this.percent = value;
            }
        }

        /// <summary>
        /// Gets or sets the start.
        /// </summary>
        /// <value>The start.</value>
        [DataMember]
        public DateTime Start
        {
            get
            {
                if (this.start.HasValue)
                {
                    return this.start.Value;
                }
                return DateTime.Now;
            }
            set
            {
                this.start = value;
            }
        }

        /// <summary>
        /// Changed time in UnixTimeMilliseconds
        /// </summary>
        [DataMember]
        public long Changed { get; set; }

        /// <summary>
        /// Gets or sets the current.
        /// </summary>
        /// <value>The current.</value>
        [DataMember]
        public long Current { get; set; }

        /// <summary>
        /// Gets or sets the count.
        /// </summary>
        /// <value>The count.</value>
        [DataMember]
        public long Count { get; set; }

        /// <summary>
        /// Gets or sets the error.
        /// </summary>
        /// <value>The error.</value>
        [DataMember(EmitDefaultValue = false)]
        public Exception Error { get; set; }

        /// <summary>
        /// Gets or sets the result.
        /// </summary>
        /// <value>The result.</value>
        [DataMember(EmitDefaultValue = false)]
        public object Result { get; set; }

        /// <summary>
        /// Gets or sets the corr id.
        /// </summary>
        /// <value>The corr id.</value>
        [DataMember]
        public Guid CorrId { get; set; }

        [DataMember]
        public bool CanCancel { get; set; }

        /// <summary>
        /// Shoulds the serialize result.
        /// </summary>
        /// <returns></returns>
        //public bool ShouldSerializeResult()
        //{
        //	return this.Result != null;
        //}

        /// <summary>
        /// Shoulds the serialize error.
        /// </summary>
        /// <returns></returns>
        //public bool ShouldSerializeError()
        //{
        //	return this.Error != null;
        //}

        /// <summary>
        /// Operation name
        /// </summary>
        [DataMember]
        public string OperationName { get; set; }

        /// <summary>
        /// 'cakajuca/prebiehajuca/dokoncena/zrusena', //nastavenie ikony v 2 riadku
        /// </summary>
        [DataMember]
        public LongOperationState State { get; set; }

        /// <summary>
        /// ak existuje report k akcii, idcko do dmska		
        /// </summary>
        [DataMember]
        public string ReportId { get; set; }

        /// <summary>
        /// Parametre operacie
        /// </summary>
        [DataMember]
        public string OperationParameters { get; set; }

        /// <summary>
        /// Info o operacii
        /// </summary>
        [DataMember]
        public string OperationInfo { get; set; }

        /// <summary>
        /// Id uzivatela
        /// </summary>
        [DataMember]
        public string UserId { get; set; }

        /// <summary>
        /// Id Tenanta
        /// </summary>
        [DataMember]
        public string TenantId { get; set; }
    }

    [DataContract]
    public class LongOperationInfo
    {
        /// <summary>
        /// nastavovanie ikony podla typu akcie 
        /// </summary>
        [DataMember]
        public string Typ { get; set; }

        /// <summary>
        /// prvy riadok BOLD, to iste ako sa vola akcia aplikacii 
        /// </summary>
        [DataMember]
        public string Nazov { get; set; }

        /// <summary>
        /// prvy riadok, druh dane pre ktory bola akcia spustena
        /// </summary>
        [DataMember]
        public string Druh { get; set; }

        /// <summary>
        /// prvy riadok, pocet zaznamov pre ktore bola akcia spustena (ak ho vieme)
        /// </summary>
        [DataMember]
        public long? Pocet { get; set; }

        /// <summary>
        /// Icon
        /// </summary>
        [DataMember]
        public string Icon { get; set; }

        /// <summary>
        /// Url
        /// </summary>
        [DataMember]
        public string Url { get; set; }

        /// <summary>
        /// Modul z ktoreho bola long time operacia volana
        /// </summary>
        [DataMember(Name = "modul")]
        public string Modul { get; set; }

        /// <summary>
        /// Sem si FE uklada info o operacii (napr. odkial bola pustena...)
        /// </summary>
        [DataMember]
        public string ViewParam { get; set; }
    }

    [DataContract]
    public enum LongOperationState
    {
        /// <summary>
        /// Čakajúca vo fronte
        /// </summary>
        [EnumMember(Value = "1")]
        Waiting = 1,

        /// <summary>
        /// Prebiehajúca 
        /// </summary>
        [EnumMember(Value = "2")]
        Running,

        /// <summary>
        /// Dokončená
        /// </summary>
        [EnumMember(Value = "3")]
        Done,

        /// <summary>
        /// Zrušená
        /// </summary>
        [EnumMember(Value = "4")]
        Canceled,

        /// <summary>
        /// Zlyhaná
        /// </summary>
        [EnumMember(Value = "5")]
        Failed,

        /// <summary>
        /// Obnovená
        /// </summary>
        [EnumMember(Value = "6")]
        Restored
    }

    public class LongOperationStatusCount
    {
        public long Tenant { get; set; }
        public long User { get; set; }
    }
}
