using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using ServiceStack;
using ServiceStack.Web;

namespace WebEas.Util
{
    /// <summary>
    /// 
    /// </summary>
    public class StreamResult : IHasOptions, IStreamWriter
    {
        private readonly Stream stream;
        private readonly FileInfo fileInfo;

        /// <summary>
        /// Initializes a new instance of the <see cref="StreamResult" /> class.
        /// </summary>
        /// <param name="responseStream">The response stream.</param>
        /// <param name="contentType">Type of the content.</param>
        /// <param name="attachment">The attachment.</param>
        public StreamResult(Stream responseStream, string contentType = "application/octet-stream", string attachment = null)
        {
            this.stream = responseStream;

            this.Options = new Dictionary<string, string>
            {
                { "Content-Type", contentType },
            };

            if (!string.IsNullOrEmpty(attachment))
            {
                this.Options.Add("Content-Disposition", string.Format("attachment; filename*=UTF-8''{0}", HttpUtility.UrlPathEncode(attachment)));
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="StreamResult" /> class.
        /// </summary>
        /// <param name="fileInfo">The file info.</param>
        /// <param name="contentType">Type of the content.</param>
        /// <param name="attachment">The attachment.</param>
        public StreamResult(FileInfo fileInfo, string contentType = "application/octet-stream", string attachment = null)
        {
            this.fileInfo = fileInfo;

            this.Options = new Dictionary<string, string>
            {
                { "Content-Type", contentType },
            };

            if (!string.IsNullOrEmpty(attachment))
            {
                this.Options.Add("Content-Disposition", string.Format("attachment; filename=\"{0}\";", attachment));
            }
        }

        public IDictionary<string, string> Options { get; private set; }

        /// <summary>
        /// Writes to.
        /// </summary>
        /// <param name="responseStream">The response stream.</param>
        public void WriteTo(Stream responseStream)
        {
            if (this.stream == null && this.fileInfo == null)
            {
                return;
            }
            else if (this.fileInfo != null)
            {
                using (FileStream str = this.fileInfo.Open(FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                {
                    str.WriteTo(responseStream);
                    responseStream.Flush();
                }
            }
            else
            {
                this.stream.WriteTo(responseStream);
                responseStream.Flush();
            }
        }
    }
}