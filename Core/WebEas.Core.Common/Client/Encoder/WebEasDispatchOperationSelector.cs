using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Channels;
using System.ServiceModel.Dispatcher;
using System.Xml;

namespace WebEas.Client.Encoder
{
    /// <summary>
    /// 
    /// </summary>
    public class WebEasDispatchOperationSelector : IDispatchOperationSelector
    {
        private readonly List<string> dispatchDictionary;
        private readonly string defaultOperationName;

        /// <summary>
        /// Initializes a new instance of the <see cref="WebEasDispatchOperationSelector" /> class.
        /// </summary>
        /// <param name="dispatchDictionary">The dispatch dictionary.</param>
        /// <param name="defaultOperationName">Default name of the operation.</param>
        public WebEasDispatchOperationSelector(List<string> dispatchDictionary, string defaultOperationName)
        {
            this.dispatchDictionary = dispatchDictionary;
            this.defaultOperationName = defaultOperationName;
        }
 
        #region IDispatchOperationSelector Members
 
        /// <summary>
        /// Creates the message copy.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="body">The body.</param>
        /// <returns></returns>
        private Message CreateMessageCopy(Message message, XmlDictionaryReader body)
        {
            Message copy = Message.CreateMessage(message.Version, message.Headers.Action, body);
            copy.Headers.CopyHeaderFrom(message, 0);
            copy.Properties.CopyProperties(message.Properties);
            return copy;
        }
 
        /// <summary>
        /// Associates a local operation with the incoming method.
        /// </summary>
        /// <param name="message">The incoming <see cref="T:System.ServiceModel.Channels.Message" />
        /// to be associated with an operation.</param>
        /// <returns>
        /// The name of the operation to be associated with the <paramref name="message" />.
        /// </returns>
        public string SelectOperation(ref System.ServiceModel.Channels.Message message)
        {
            XmlDictionaryReader bodyReader = message.GetReaderAtBodyContents();
            XmlQualifiedName lookupQName = new XmlQualifiedName(bodyReader.LocalName, bodyReader.NamespaceURI);
            message = this.CreateMessageCopy(message, bodyReader);
 
            var operationName = this.dispatchDictionary.FirstOrDefault(e => lookupQName.Name.ToLower().Contains(e.ToLower()));            
 
            if (operationName != null)
            {
                return operationName;
            }
            
            return this.defaultOperationName;            
        }
        #endregion
    }
}