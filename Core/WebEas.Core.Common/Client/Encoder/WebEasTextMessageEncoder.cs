using System;
using System.IO;
using System.Linq;
using System.ServiceModel.Channels;
using System.Text;
using System.Xml;

namespace WebEas.Client.Encoder
{
    /// <summary>
    /// Custom Web Eas Text Message Encoder
    /// </summary>
    public class WebEasTextMessageEncoder : MessageEncoder
    {
        private readonly WebEasTextMessageEncoderFactory factory;
        private readonly XmlWriterSettings writerSettings;
        private readonly string contentType;

        /// <summary>
        /// Initializes a new instance of the <see cref="CustomTextMessageEncoder" /> class.
        /// </summary>
        /// <param name="factory">The factory.</param>
        public WebEasTextMessageEncoder(WebEasTextMessageEncoderFactory factory)
        {
            this.factory = factory;

            this.writerSettings = new XmlWriterSettings();
            this.writerSettings.Encoding = Encoding.GetEncoding(factory.CharSet);
            this.contentType = string.Format("{0};charset={1}", this.factory.MediaType, this.writerSettings.Encoding.HeaderName.ToUpper());
        }

        /// <summary>
        /// When overridden in a derived class, gets the MIME content type used
        /// by the encoder.
        /// </summary>
        /// <returns>The content type that is supported by the message encoder.</returns>
        /// <value></value>
        public override string ContentType
        {
            get
            {
                return this.contentType;
            }
        }

        /// <summary>
        /// When overridden in a derived class, gets the media type value that is
        /// used by the encoder.
        /// </summary>
        /// <returns>The media type that is supported by the message encoder.</returns>
        /// <value></value>
        public override string MediaType
        {
            get
            {
                return this.factory.MediaType;
            }
        }

        /// <summary>
        /// When overridden in a derived class, gets the message version value that
        /// is used by the encoder.
        /// </summary>
        /// <returns>The <see cref="T:System.ServiceModel.Channels.MessageVersion" /> that
        /// is used by the encoder.</returns>
        /// <value></value>
        public override MessageVersion MessageVersion
        {
            get
            {
                return this.factory.MessageVersion;
            }
        }

        /// <summary>
        /// Returns a value that indicates whether a specified message-level content-type
        /// value is supported by the message encoder.
        /// </summary>
        /// <param name="contentType">The message-level content-type being tested.</param>
        /// <returns>
        /// true if the message-level content-type specified is supported; otherwise
        /// false.
        /// </returns>
        public override bool IsContentTypeSupported(string contentType)
        {
            return true;
            //TODO doplnit validaciu
            //if (contentType == null)
            //{
            //    throw new ArgumentNullException("contentType");
            //}
            //return this.IsContentTypeSupported(contentType, this.ContentType, this.MediaType);
        }

        /// <summary>
        /// Reads the message.
        /// </summary>
        /// <param name="buffer">The buffer.</param>
        /// <param name="bufferManager">The buffer manager.</param>
        /// <param name="contentType">Type of the content.</param>
        /// <returns></returns>
        public override Message ReadMessage(ArraySegment<byte> buffer, BufferManager bufferManager, string contentType)
        {
            byte[] msgContents = new byte[buffer.Count];
            Array.Copy(buffer.Array, buffer.Offset, msgContents, 0, msgContents.Length);
            bufferManager.ReturnBuffer(buffer.Array);

            MemoryStream stream = new MemoryStream(msgContents);
            var message = this.ReadMessage(stream, int.MaxValue);
            return message;
        }

        /// <summary>
        /// When overridden in a derived class, reads a message from a specified
        /// stream.
        /// </summary>
        /// <param name="stream">The <see cref="T:System.IO.Stream" /> object from which the
        /// message is read.</param>
        /// <param name="maxSizeOfHeaders">The maximum size of the headers that can be read
        /// from the message.</param>
        /// <param name="contentType">The Multipurpose Internet Mail Extensions (MIME) message-level
        /// content-type.</param>
        /// <returns>
        /// The <see cref="T:System.ServiceModel.Channels.Message" /> that is read
        /// from the stream specified.
        /// </returns>
        public override Message ReadMessage(Stream stream, int maxSizeOfHeaders, string contentType)
        {
            XmlReader reader = XmlReader.Create(new StreamReader(stream, Encoding.GetEncoding(this.factory.CharSet)));
            return Message.CreateMessage(reader, maxSizeOfHeaders, this.MessageVersion);
        }

        /// <summary>
        /// When overridden in a derived class, writes a message of less than a
        /// specified size to a byte array buffer at the specified offset.
        /// </summary>
        /// <param name="message">The <see cref="T:System.ServiceModel.Channels.Message" />
        /// to write to the message buffer.</param>
        /// <param name="maxMessageSize">The maximum message size that can be written.</param>
        /// <param name="bufferManager">The <see cref="T:System.ServiceModel.Channels.BufferManager" />
        /// that manages the buffer to which the message is written.</param>
        /// <param name="messageOffset">The offset of the segment that begins from the start
        /// of the byte array that provides the buffer.</param>
        /// <returns>
        /// A <see cref="T:System.ArraySegment`1" /> of type byte that provides the
        /// buffer to which the message is serialized.
        /// </returns>
        public override ArraySegment<byte> WriteMessage(Message message, int maxMessageSize, BufferManager bufferManager, int messageOffset)
        {
            MemoryStream stream = new MemoryStream();
            XmlWriter writer = XmlWriter.Create(stream, this.writerSettings);
            message.WriteMessage(writer);
            writer.Close();

            byte[] messageBytes = stream.GetBuffer();
            int messageLength = (int)stream.Position;
            stream.Close();

            int totalLength = messageLength + messageOffset;
            byte[] totalBytes = bufferManager.TakeBuffer(totalLength);
            Array.Copy(messageBytes, 0, totalBytes, messageOffset, messageLength);

            ArraySegment<byte> byteArray = new ArraySegment<byte>(totalBytes, messageOffset, messageLength);
            return byteArray;
        }

        /// <summary>
        /// When overridden in a derived class, writes a message to a specified
        /// stream.
        /// </summary>
        /// <param name="message">The <see cref="T:System.ServiceModel.Channels.Message" />
        /// to write to the <paramref name="stream" />.</param>
        /// <param name="stream">The <see cref="T:System.IO.Stream" /> object to which the
        /// <paramref name="message" /> is written.</param>
        public override void WriteMessage(Message message, Stream stream)
        {
            XmlWriter writer = XmlWriter.Create(stream, this.writerSettings);
            message.WriteMessage(writer);
            writer.Close();
        }

        /// <summary>
        /// Determines whether [is content type supported] [the specified content type].
        /// </summary>
        /// <param name="contentType">Type of the content.</param>
        /// <param name="supportedContentType">Type of the supported content.</param>
        /// <param name="supportedMediaType">Type of the supported media.</param>
        /// <returns></returns>
        internal bool IsContentTypeSupported(string contentType, string supportedContentType, string supportedMediaType)
        {
            if (supportedContentType == contentType)
            {
                return true;
            }
            if (contentType.Length > supportedContentType.Length && contentType.StartsWith(supportedContentType, StringComparison.Ordinal) && contentType[supportedContentType.Length] == ';')
            {
                return true;
            }
            if (contentType.StartsWith(supportedContentType, StringComparison.OrdinalIgnoreCase))
            {
                if (contentType.Length == supportedContentType.Length)
                {
                    return true;
                }
                if (contentType.Length > supportedContentType.Length)
                {
                    char chr = contentType[supportedContentType.Length];
                    if (chr == ';')
                    {
                        return true;
                    }
                    int length = supportedContentType.Length;
                    if (chr == '\r' && contentType.Length > supportedContentType.Length + 1 && contentType[length + 1] == '\n')
                    {
                        length = length + 2;
                        chr = contentType[length];
                    }
                    if (chr == ' ' || chr == '\t')
                    {
                        for (length++; length < contentType.Length; length++)
                        {
                            chr = contentType[length];
                            if (chr != ' ' && chr != '\t')
                            {
                                break;
                            }
                        }
                    }
                    if (chr == ';' || length == contentType.Length)
                    {
                        return true;
                    }
                }
            }

            if (contentType.Replace(" ", "") == supportedContentType.Replace(" ", ""))
            {
                return true;
            }

            return contentType.ToLower() == this.factory.MediaType.ToLower();
        }
    }
}