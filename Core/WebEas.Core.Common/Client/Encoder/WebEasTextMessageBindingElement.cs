using System;
using System.Linq;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.Xml;

namespace WebEas.Client.Encoder
{
    public class WebEasTextMessageBindingElement : MessageEncodingBindingElement, IWsdlExportExtension 
    {
        private MessageVersion msgVersion;
        private string mediaType;
        private string encoding;        

        /// <summary>
        /// Initializes a new instance of the <see cref="CustomTextMessageBindingElement" /> class.
        /// </summary>
        /// <param name="binding">The binding.</param>
        public WebEasTextMessageBindingElement(WebEasTextMessageBindingElement binding) : this(binding.Encoding, binding.MediaType, binding.MessageVersion)
        {
            this.ReaderQuotas = new XmlDictionaryReaderQuotas();
            binding.ReaderQuotas.CopyTo(this.ReaderQuotas);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CustomTextMessageBindingElement" /> class.
        /// </summary>
        /// <param name="encoding">The encoding.</param>
        /// <param name="mediaType">Type of the media.</param>
        /// <param name="msgVersion">The MSG version.</param>
        public WebEasTextMessageBindingElement(string encoding, string mediaType,
            MessageVersion msgVersion)
        {
            if (encoding == null)
            {
                throw new ArgumentNullException("encoding");
            }

            if (mediaType == null)
            {
                throw new ArgumentNullException("mediaType");
            }

            if (msgVersion == null)
            {
                throw new ArgumentNullException("msgVersion");
            }

            this.msgVersion = msgVersion;
            this.mediaType = mediaType;
            this.encoding = encoding;            
            this.ReaderQuotas = new XmlDictionaryReaderQuotas();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CustomTextMessageBindingElement" /> class.
        /// </summary>
        /// <param name="encoding">The encoding.</param>
        /// <param name="mediaType">Type of the media.</param>
        public WebEasTextMessageBindingElement(string encoding, string mediaType) : this(encoding, mediaType, MessageVersion.Soap12)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CustomTextMessageBindingElement" /> class.
        /// </summary>
        /// <param name="encoding">The encoding.</param>
        public WebEasTextMessageBindingElement(string encoding) : this(encoding, "application/soap+xml")
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CustomTextMessageBindingElement" /> class.
        /// </summary>
        public WebEasTextMessageBindingElement() : this("UTF-8")
        {
        }

        /// <summary>
        /// When overridden in a derived class, gets or sets the message version
        /// that can be handled by the message encoders produced by the message encoder factory.
        /// </summary>
        /// <returns>The <see cref="T:System.ServiceModel.Channels.MessageVersion" /> used
        /// by the encoders produced by the message encoder factory.</returns>
        /// <value></value>
        public override MessageVersion MessageVersion
        {
            get
            {
                return this.msgVersion;
            }

            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("value");
                }
                this.msgVersion = value;
            }
        }

        /// <summary>
        /// Gets or sets the type of the media.
        /// </summary>
        /// <value>The type of the media.</value>
        public string MediaType
        {
            get
            {
                return this.mediaType;
            }

            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("value");
                }
                this.mediaType = value;
            }
        }

        /// <summary>
        /// Gets or sets the encoding.
        /// </summary>
        /// <value>The encoding.</value>
        public string Encoding
        {
            get
            {
                return this.encoding;
            }

            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("value");
                }
                this.encoding = value;
            }
        }

        // This encoder does not enforces any quotas for the unsecure messages. The 
        // quotas are enforced for the secure portions of messages when this encoder
        // is used in a binding that is configured with security. 
        /// <summary>
        /// Gets the reader quotas.
        /// </summary>
        /// <value>The reader quotas.</value>
        public XmlDictionaryReaderQuotas ReaderQuotas { get; private set; }

        /// <summary>
        /// When overridden in a derived class, creates a factory for producing
        /// message encoders.
        /// </summary>
        /// <returns>
        /// The <see cref="T:System.ServiceModel.Channels.MessageEncoderFactory" />
        /// used to produce message encoders.
        /// </returns>
        public override MessageEncoderFactory CreateMessageEncoderFactory()
        {
            return new WebEasTextMessageEncoderFactory(this.MediaType,
                this.Encoding, this.MessageVersion);
        }

        /// <summary>
        /// When overridden in a derived class, returns a copy of the binding element
        /// object.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.ServiceModel.Channels.BindingElement" /> object
        /// that is a deep clone of the original.
        /// </returns>
        public override BindingElement Clone()
        {
            return new WebEasTextMessageBindingElement(this);
        }

        /// <summary>
        /// Builds the channel factory.
        /// </summary>
        /// <typeparam name="TChannel">The type of the T channel.</typeparam>
        /// <param name="context">The context.</param>
        /// <returns></returns>
        public override IChannelFactory<TChannel> BuildChannelFactory<TChannel>(BindingContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            context.BindingParameters.Add(this);
            return context.BuildInnerChannelFactory<TChannel>();
        }

        /// <summary>
        /// Determines whether this instance [can build channel factory] the specified context.
        /// </summary>
        /// <typeparam name="TChannel">The type of the T channel.</typeparam>
        /// <param name="context">The context.</param>
        /// <returns></returns>
        public override bool CanBuildChannelFactory<TChannel>(BindingContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            return context.CanBuildInnerChannelFactory<TChannel>();
        }

        /// <summary>
        /// Builds the channel listener.
        /// </summary>
        /// <typeparam name="TChannel">The type of the T channel.</typeparam>
        /// <param name="context">The context.</param>
        /// <returns></returns>
        public override IChannelListener<TChannel> BuildChannelListener<TChannel>(BindingContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            context.BindingParameters.Add(this);
            return context.BuildInnerChannelListener<TChannel>();
        }

        /// <summary>
        /// Determines whether this instance [can build channel listener] the specified context.
        /// </summary>
        /// <typeparam name="TChannel">The type of the T channel.</typeparam>
        /// <param name="context">The context.</param>
        /// <returns></returns>
        public override bool CanBuildChannelListener<TChannel>(BindingContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            context.BindingParameters.Add(this);
            return context.CanBuildInnerChannelListener<TChannel>();
        }

        /// <summary>
        /// Gets the property.
        /// </summary>
        /// <typeparam name="T">The type of the T.</typeparam>
        /// <param name="context">The context.</param>
        /// <returns></returns>
        public override T GetProperty<T>(BindingContext context)
        {
            if (typeof(T) == typeof(XmlDictionaryReaderQuotas))
            {
                return (T)(object)this.ReaderQuotas;
            }
            else
            {
                return base.GetProperty<T>(context);
            }
        }

        /// <summary>
        /// Writes custom Web Services Description Language (WSDL) elements into
        /// the generated WSDL for a contract.
        /// </summary>
        /// <param name="exporter">The <see cref="T:System.ServiceModel.Description.WsdlExporter" />
        /// that exports the contract information.</param>
        /// <param name="context">Provides mappings from exported WSDL elements to the contract
        /// description.</param>
        void IWsdlExportExtension.ExportContract(WsdlExporter exporter, WsdlContractConversionContext context)
        {
        }

        /// <summary>
        /// Writes custom Web Services Description Language (WSDL) elements into
        /// the generated WSDL for an endpoint.
        /// </summary>
        /// <param name="exporter">The <see cref="T:System.ServiceModel.Description.WsdlExporter" />
        /// that exports the endpoint information.</param>
        /// <param name="context">Provides mappings from exported WSDL elements to the endpoint
        /// description.</param>
        void IWsdlExportExtension.ExportEndpoint(WsdlExporter exporter, WsdlEndpointConversionContext context)
        {
            // The MessageEncodingBindingElement is responsible for ensuring that the WSDL has the correct
            // SOAP version. We can delegate to the WCF implementation of TextMessageEncodingBindingElement for this.
            TextMessageEncodingBindingElement mebe = new TextMessageEncodingBindingElement();
            mebe.MessageVersion = this.msgVersion;
            ((IWsdlExportExtension)mebe).ExportEndpoint(exporter, context);
        }
    }
}