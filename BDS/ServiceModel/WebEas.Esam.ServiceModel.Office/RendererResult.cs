using System.Runtime.Serialization;

namespace WebEas.Esam.ServiceModel.Office
{
    [DataContract]
    public class RendererResult
    {
        /// <summary>
        /// Gets the name of the created document.
        /// </summary>
        [DataMember]
        public string DocumentName { get; set; }

        /// <summary>
        /// Gets an array of exceptions that has occurred during the report processing.
        /// </summary>
        [DataMember]
        public System.Exception[] Errors { get; set; }

        /// <summary>
        /// Gets the the MIME type of the document.
        /// </summary>
        [DataMember]
        public string MimeType { get; set; }

        /// <summary>
        /// Gets the character encoding of the document.
        /// </summary>
        [DataMember]
        public string Encoding { get; set; }

        /// <summary>
        /// Gets the file extension of the document.
        /// </summary>
        [DataMember]
        public string Extension { get; set; }

        /// <summary>
        /// Gets a value that indicates whether the collection contains errors.
        /// </summary>
        [DataMember]
        public bool HasErrors
        {
            get
            {
                return this.Errors != null && this.Errors.Length > 0;
            }
        }

        /// <summary>
        /// Gets a byte array that contains the rendered report.
        /// </summary>
        [DataMember]
        public byte[] DocumentBytes { get; set; }
    }
}
