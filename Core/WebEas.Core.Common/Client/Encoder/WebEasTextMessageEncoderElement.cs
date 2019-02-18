using System;
using System.ComponentModel;
using System.Configuration;
using System.Linq;
using System.ServiceModel.Channels;
using System.ServiceModel.Configuration;
using System.Xml;

namespace WebEas.Client.Encoder
{
    public class WebEasTextMessageEncoderElement : BindingElementExtensionElement
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CustomTextMessageEncodingElement" /> class.
        /// </summary>
        public WebEasTextMessageEncoderElement()
        {
        }

        /// <summary>
        /// When overridden in a derived class, gets the <see cref="T:System.Type" />
        /// object that represents the custom binding element.
        /// </summary>
        /// <returns>A <see cref="T:System.Type" /> object that represents the custom binding
        /// type.</returns>
        /// <value></value>
        public override Type BindingElementType
        {
            get 
            {
                return typeof(WebEasTextMessageEncoderElement); 
            }
        }

        /// <summary>
        /// Gets or sets the message version.
        /// </summary>
        /// <value>The message version.</value>
        [ConfigurationProperty(ConfigurationStrings.MessageVersion,
            DefaultValue = ConfigurationStrings.DefaultMessageVersion)]
        [TypeConverter(typeof(MessageVersionConverter))]
        public MessageVersion MessageVersion
        {
            get
            {
                return (MessageVersion)base[ConfigurationStrings.MessageVersion];
            }
            set
            {
                base[ConfigurationStrings.MessageVersion] = value;
            }
        }

        /// <summary>
        /// Gets or sets the type of the media.
        /// </summary>
        /// <value>The type of the media.</value>
        [ConfigurationProperty(ConfigurationStrings.MediaType,
            DefaultValue = ConfigurationStrings.DefaultMediaType)]
        public string MediaType
        {
            get
            {
                return (string)base[ConfigurationStrings.MediaType];
            }

            set
            {
                base[ConfigurationStrings.MediaType] = value;
            }
        }

        /// <summary>
        /// Gets or sets the encoding.
        /// </summary>
        /// <value>The encoding.</value>
        [ConfigurationProperty(ConfigurationStrings.Encoding,
            DefaultValue = ConfigurationStrings.DefaultEncoding)]
        public string Encoding
        {
            get
            {
                return (string)base[ConfigurationStrings.Encoding];
            }

            set
            {
                base[ConfigurationStrings.Encoding] = value;
            }
        }

        /// <summary>
        /// Gets or sets the add encoding.
        /// </summary>
        /// <value>The add encoding.</value>
        [ConfigurationProperty(ConfigurationStrings.AddEncoding,
            DefaultValue = true)]
        public bool AddEncoding
        {
            get
            {
                return (bool)base[ConfigurationStrings.AddEncoding];
            }

            set
            {
                base[ConfigurationStrings.AddEncoding] = value;
            }
        }

        /// <summary>
        /// Gets the reader quotas element.
        /// </summary>
        /// <value>The reader quotas element.</value>
        [ConfigurationProperty(ConfigurationStrings.ReaderQuotas)]
        public XmlDictionaryReaderQuotasElement ReaderQuotasElement
        {
            get 
            { 
                return (XmlDictionaryReaderQuotasElement)base[ConfigurationStrings.ReaderQuotas]; 
            }
        }

        /// <summary>
        /// Applies the content of a specified binding element to this binding configuration
        /// element.
        /// </summary>
        /// <param name="bindingElement">A binding element.</param>
        /// <exception cref="T:System.ArgumentNullException">
        /// <paramref name="bindingElement" /> is null.</exception>
        public override void ApplyConfiguration(BindingElement bindingElement)
        {
            base.ApplyConfiguration(bindingElement);
            WebEasTextMessageBindingElement binding = (WebEasTextMessageBindingElement)bindingElement;
            binding.MessageVersion = this.MessageVersion;
            binding.MediaType = this.MediaType;
            binding.Encoding = this.Encoding;            
            this.ApplyConfiguration(binding.ReaderQuotas);
        }

        /// <summary>
        /// When overridden in a derived class, returns a custom binding element
        /// object.
        /// </summary>
        /// <returns>
        /// A custom <see cref="T:System.ServiceModel.Channels.BindingElement" />
        /// object.
        /// </returns>
        protected override BindingElement CreateBindingElement()
        {
            WebEasTextMessageBindingElement binding = new WebEasTextMessageBindingElement();
            this.ApplyConfiguration(binding);
            return binding;
        }

        /// <summary>
        /// Applies the configuration.
        /// </summary>
        /// <param name="readerQuotas">The reader quotas.</param>
        private void ApplyConfiguration(XmlDictionaryReaderQuotas readerQuotas)
        {
            if (readerQuotas == null)
            {
                throw new ArgumentNullException("readerQuotas");
            }
            
            if (this.ReaderQuotasElement.MaxDepth != 0)
            {
                readerQuotas.MaxDepth = this.ReaderQuotasElement.MaxDepth;
            }
            if (this.ReaderQuotasElement.MaxStringContentLength != 0)
            {
                readerQuotas.MaxStringContentLength = this.ReaderQuotasElement.MaxStringContentLength;
            }
            if (this.ReaderQuotasElement.MaxArrayLength != 0)
            {
                readerQuotas.MaxArrayLength = this.ReaderQuotasElement.MaxArrayLength;
            }
            if (this.ReaderQuotasElement.MaxBytesPerRead != 0)
            {
                readerQuotas.MaxBytesPerRead = this.ReaderQuotasElement.MaxBytesPerRead;
            }
            if (this.ReaderQuotasElement.MaxNameTableCharCount != 0)
            {
                readerQuotas.MaxNameTableCharCount = this.ReaderQuotasElement.MaxNameTableCharCount;
            }
        }
    }
}