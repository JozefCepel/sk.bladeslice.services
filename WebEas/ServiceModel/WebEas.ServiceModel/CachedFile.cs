using System;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;

namespace WebEas.ServiceModel
{
    /// <summary>
    /// 
    /// </summary>
    [DataContract]
    public class CachedFile
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CachedFile" /> class.
        /// </summary>
        public CachedFile()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CachedFile" /> class.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        /// <param name="data">The data.</param>
        public CachedFile(string fileName, byte[] data)
        {
            this.FileName = fileName;
            this.Data = data;
            this.TempId = Guid.NewGuid().ToString();
        }

        /// <summary>
        /// Gets or sets the name of the file.
        /// </summary>
        /// <value>The name of the file.</value>
        [DataMember]
        public string FileName { get; set; }

        /// <summary>
        /// Gets the file extension.
        /// </summary>
        /// <value>The file extension.</value>
        [IgnoreDataMember]
        public string FileExtension
        {
            get
            {
                if (string.IsNullOrEmpty(this.FileName))
                {
                    return null;
                }

                return Path.GetExtension(this.FileName).Replace(".", string.Empty);
            }
        }

        /// <summary>
        /// Gets or sets the temp id.
        /// </summary>
        /// <value>The temp id.</value>
        [DataMember]
        public string TempId { get; set; }

        ///// <summary>
        ///// Gets or sets the tenant id.
        ///// </summary>
        ///// <value>The tenant id.</value>
        //[DataMember]
        //public string TenantId { get; set; }

        ///// <summary>
        ///// Gets or sets the DCOM id.
        ///// </summary>
        ///// <value>The DCOM id.</value>
        //[DataMember]
        //public string UserId { get; set; }

        /// <summary>
        /// Gets or sets the data.
        /// </summary>
        /// <value>The data.</value>
        [DataMember]
        public byte[] Data { get; set; }

    }
}