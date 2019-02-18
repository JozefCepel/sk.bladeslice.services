using System;
using System.Linq;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;
using System.Xml;

namespace WebEas.Core.Sts
{
    public class CustomMessageFormatter : IClientMessageFormatter 
    {
        private readonly IClientMessageFormatter formatter;

        public CustomMessageFormatter(IClientMessageFormatter formatter)
        {
            this.formatter = formatter;
        }
 
        public object DeserializeReply(Message message, object[] parameters)
        {
            return this.formatter.DeserializeReply(message, parameters);
        }

        public Message SerializeRequest(MessageVersion messageVersion, object[] parameters)
        {
            Message message = this.formatter.SerializeRequest(messageVersion, parameters);
            return new CustomMessage(message);
        }
    }

    public class CustomMessage : Message
    {
        private readonly Message message;

        public CustomMessage(Message message)
        {
            this.message = message;
        }

        public override MessageHeaders Headers
        {
            get
            {
                return this.message.Headers;
            }
        }

        public override MessageProperties Properties
        {
            get
            {
                return this.message.Properties;
            }
        }

        public override MessageVersion Version
        {
            get
            {
                return this.message.Version;
            }
        }

        protected override void OnWriteStartBody(XmlDictionaryWriter writer)
        {
            writer.WriteStartElement("Body", "http://schemas.xmlsoap.org/soap/envelope/");
        }

        protected override void OnWriteBodyContents(XmlDictionaryWriter writer)
        {
            this.message.WriteBodyContents(writer);
        }

        protected override void OnWriteStartEnvelope(XmlDictionaryWriter writer)
        {
            writer.WriteStartElement("SOAP-ENV", "Envelope", "http://schemas.xmlsoap.org/soap/envelope/");
            writer.WriteAttributeString("xmlns", "ns1", null, "https://vanacosmin.ro/WebService/soap/");
        }
    }

    [AttributeUsage(AttributeTargets.Method)]
    public class MobilityProviderFormatMessageAttribute : Attribute, IOperationBehavior
    {
        public void AddBindingParameters(OperationDescription operationDescription, BindingParameterCollection bindingParameters)
        {
        }

        public void ApplyClientBehavior(OperationDescription operationDescription, ClientOperation clientOperation)
        {
        }

        public void ApplyDispatchBehavior(OperationDescription operationDescription, DispatchOperation dispatchOperation)
        {
            DataContractSerializerOperationBehavior serializerBehavior = operationDescription.Behaviors.Find<DataContractSerializerOperationBehavior>();

            if (dispatchOperation.Formatter == null)
            {
                ((IOperationBehavior)serializerBehavior).ApplyDispatchBehavior(operationDescription, dispatchOperation);
            }

            IDispatchMessageFormatter innerDispatchFormatter = dispatchOperation.Formatter;
            //dispatchOperation.Formatter = new CustomMessageFormatter(innerDispatchFormatter);
        }

        public void Validate(OperationDescription operationDescription)
        {
        }
    }

    public class CustomOperationBehavior : IOperationBehavior
    {
        public void AddBindingParameters(OperationDescription operationDescription, BindingParameterCollection bindingParameters)
        {
            throw new NotImplementedException();
        }

        public void ApplyClientBehavior(OperationDescription operationDescription, ClientOperation clientOperation)
        {
            DataContractSerializerOperationBehavior serializerBehavior = operationDescription.Behaviors.Find<DataContractSerializerOperationBehavior>();

            if (clientOperation.Formatter == null)
            {
                ((IOperationBehavior)serializerBehavior).ApplyClientBehavior(operationDescription, clientOperation);
            }

            IClientMessageFormatter innerClientFormatter = clientOperation.Formatter;

            clientOperation.Formatter = new CustomMessageFormatter(innerClientFormatter);
        }

        public void ApplyDispatchBehavior(OperationDescription operationDescription, DispatchOperation dispatchOperation)
        {
            throw new NotImplementedException();
        }

        public void Validate(OperationDescription operationDescription)
        {
            throw new NotImplementedException();
        }
    }
}
