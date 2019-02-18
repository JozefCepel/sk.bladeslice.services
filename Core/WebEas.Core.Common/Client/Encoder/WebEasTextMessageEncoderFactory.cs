using System;
using System.Linq;
using System.ServiceModel.Channels;

namespace WebEas.Client.Encoder
{
    public class WebEasTextMessageEncoderFactory : MessageEncoderFactory
    {
        private readonly MessageEncoder encoder;
        private readonly MessageVersion version;

        /// <summary>
        /// Initializes a new instance of the <see cref="CustomTextMessageEncoderFactory" /> class.
        /// </summary>
        /// <param name="mediaType">Type of the media.</param>
        /// <param name="charSet">The char set.</param>
        /// <param name="version">The version.</param>
        internal WebEasTextMessageEncoderFactory(string mediaType, string charSet,
            MessageVersion version)
        {
            this.version = version;
            this.MediaType = mediaType;
            this.CharSet = charSet;            
            this.encoder = new WebEasTextMessageEncoder(this);
        }

        /// <summary>
        /// When overridden in a derived class, gets the message encoder that is
        /// produced by the factory.
        /// </summary>
        /// <returns>The <see cref="T:System.ServiceModel.Channels.MessageEncoder" /> used
        /// by the factory.</returns>
        /// <value></value>
        public override MessageEncoder Encoder
        {
            get
            {
                return this.encoder;
            }
        }

        /// <summary>
        /// When overridden in a derived class, gets the message version that is
        /// used by the encoders produced by the factory to encode messages.
        /// </summary>
        /// <returns>The <see cref="T:System.ServiceModel.Channels.MessageVersion" /> used
        /// by the factory.</returns>
        /// <value></value>
        public override MessageVersion MessageVersion
        {
            get
            {
                return this.version;
            }
        }

        /// <summary>
        /// Gets the type of the media.
        /// </summary>
        /// <value>The type of the media.</value>
        internal string MediaType { get; private set; }

        /// <summary>
        /// Gets the char set.
        /// </summary>
        /// <value>The char set.</value>
        internal string CharSet { get; private set; }
    }
}