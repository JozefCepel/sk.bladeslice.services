using System;
using System.Runtime.Serialization;

namespace WebEas.Esam.ServiceModel.Office
{
    [DataContract]
    public class ReportGenerationStatus
    {
        private DateTime? start = null;

        private long? percent = null;

        private string message;

        /// <summary>
        /// Initializes a new instance of the <see cref="ReportGenerationStatus" /> class.
        /// </summary>
        public ReportGenerationStatus()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ReportGenerationStatus" /> class.
        /// </summary>
        /// <param name="processKey">The process key.</param>
        public ReportGenerationStatus(string processKey)
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
                return this.Result != null;
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
        public string Message
        {
            get
            {
                if (this.HasError && string.IsNullOrEmpty(this.message))
                {
                    return this.Error.Message;
                }
                return this.message;
            }
            set
            {
                this.message = value;
            }
        }

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
        [DataMember]
        public Exception Error { get; set; }

        /// <summary>
        /// Gets or sets the result.
        /// </summary>
        /// <value>The result.</value>
        [DataMember]
        public RendererResult Result { get; set; }

        /// <summary>
        /// Gets or sets the corr id.
        /// </summary>
        /// <value>The corr id.</value>
        [DataMember(Name = "x-corr-id")]
        public Guid CorrId { get; set; }

        [DataMember]
        public bool CanCancel { get; set; }

        [DataMember]
        public string ErrorReportProcessKey { get; set; }

        /// <summary>
        /// Shoulds the serialize result.
        /// </summary>
        /// <returns></returns>
        public bool ShouldSerializeResult()
        {
            return this.Result != null;
        }

        /// <summary>
        /// Shoulds the serialize error.
        /// </summary>
        /// <returns></returns>
        public bool ShouldSerializeError()
        {
            return this.Error != null;
        }
    }
}
