using System.ServiceModel.Channels;
using System.ServiceModel.Dispatcher;
using System.Text;
using System.Xml;

namespace WebEas.Core.Sts
{
    internal class SamlMessageInspector : IClientMessageInspector
    {
        public SamlMessageInspector()
        {
            this.DisplayClaims = new DisplayClaimCollection();
            this.HTTPHeaders = new HTTPHeaderCollection();
        }

        public DisplayClaimCollection DisplayClaims
        {
            get;
            private set;
        }

        internal HTTPHeaderCollection HTTPHeaders
        {
            get;
            set;
        }

        public void AfterReceiveReply(ref Message reply, object correlationState)
        {
            // Create the buffer / copy reply contents
            var buffer = reply.CreateBufferedCopy(13000);
            // Inspect the response (for example, extract the body contents)
            var thisReply = buffer.CreateMessage();
            var reader = thisReply.GetReaderAtBodyContents();
            var info = new StringBuilder();
            var writer = XmlWriter.Create(info);
            writer.WriteNode(reader, true);
            writer.Close();

            var str = info.ToString();

            // You can close the buffer after the message has been recreated.
            reply = buffer.CreateMessage();
            buffer.Close();

            var responseMessage = new XmlDocument();
            responseMessage.LoadXml(str);

            var nsMan = new XmlNamespaceManager(responseMessage.NameTable);
            nsMan.AddNamespace("t", "http://docs.oasis-open.org/ws-sx/ws-trust/200512");
            nsMan.AddNamespace("i", "http://schemas.xmlsoap.org/ws/2005/05/identity");

            var xpath = "//t:RequestSecurityTokenResponseCollection/t:RequestSecurityTokenResponse/i:RequestedDisplayToken/i:DisplayToken";

            // Get display claims
            this.DisplayClaims = new DisplayClaimCollection();
            var requestedDisplayTokens = responseMessage.DocumentElement.SelectSingleNode(xpath, nsMan);
            if (requestedDisplayTokens == null) return;

            foreach (XmlNode requestDisplayToken in requestedDisplayTokens.ChildNodes)
            {
                var dc = new DisplayClaim();
                dc.ClaimType = requestDisplayToken.Attributes["Uri"].Value;
                foreach (XmlNode requestDisplayTokenValue in requestDisplayToken.ChildNodes)
                {
                    switch (requestDisplayTokenValue.LocalName)
                    {
                        case "DisplayTag":
                            dc.DisplayTag = requestDisplayTokenValue.InnerText;
                            break;
                        case "Description":
                            dc.Description = requestDisplayTokenValue.InnerText;
                            break;
                        case "DisplayValue":
                            dc.DisplayValue = requestDisplayTokenValue.InnerText;
                            break;
                            //ignore other
                        default:
                            break;
                    }
                }

                this.DisplayClaims.Add(dc);
            }
        }

        public object BeforeSendRequest(ref Message request, System.ServiceModel.IClientChannel channel)
        {
            // Add HTTP Headers
            HttpRequestMessageProperty httpRequestMessage;
            object httpRequestMessageObject;

            foreach (var h in HTTPHeaders)
            {
                if (request.Properties.TryGetValue(HttpRequestMessageProperty.Name, out httpRequestMessageObject))
                {
                    httpRequestMessage = httpRequestMessageObject as HttpRequestMessageProperty;
                    httpRequestMessage.Headers[h.Name] = h.Value;
                }
                else
                {
                    httpRequestMessage = new HttpRequestMessageProperty();
                    httpRequestMessage.Headers.Add(h.Name, h.Value);
                    request.Properties.Add(HttpRequestMessageProperty.Name, httpRequestMessage);
                }
            }

            return null;
        }
    }
}
