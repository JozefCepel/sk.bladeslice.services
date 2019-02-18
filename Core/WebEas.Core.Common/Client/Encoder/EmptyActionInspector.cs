using System;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Dispatcher;

namespace WebEas.Client.Encoder
{
    /// <summary>
    /// Inspector wcf napojenie - nastavenie prazdnej action
    /// </summary>
    internal class EmptyActionInspector : IClientMessageInspector
    {
        #region IClientMessageInspector Members
        
        /// <summary>
        /// Enables inspection or modification of a message after a reply message
        /// is received but prior to passing it back to the client application.
        /// </summary>
        /// <param name="reply">The message to be transformed into types and handed back
        /// to the client application.</param>
        /// <param name="correlationState">Correlation state data.</param>
        public void AfterReceiveReply(ref System.ServiceModel.Channels.Message reply, object correlationState)
        {
        }
        
        /// <summary>
        /// Enables inspection or modification of a message before a request message
        /// is sent to a service.
        /// </summary>
        /// <param name="request">The message to be sent to the service.</param>
        /// <param name="channel">The WCF client object channel.</param>
        /// <returns>
        /// The object that is returned as the <paramref name="correlationState " />argument
        /// of the <see cref="M:System.ServiceModel.Dispatcher.IClientMessageInspector.AfterReceiveReply(System.ServiceModel.Channels.Message@,System.Object)" />
        /// method. This is null if no correlation state is used.The best practice is to
        /// make this a <see cref="T:System.Guid" /> to ensure that no two <paramref name="correlationState" />
        /// objects are the same.
        /// </returns>
        public object BeforeSendRequest(ref System.ServiceModel.Channels.Message request, IClientChannel channel)
        {
            request.Headers.Action = String.Empty;
            return null;
        }
        
        #endregion
    }
}