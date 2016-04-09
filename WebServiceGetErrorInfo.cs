ShipPortTypeClient shpSvc = new ShipPortTypeClient();
ConsoleOutputBehavior consoleOutputBehavior = new ConsoleOutputBehavior();
shpSvc.Endpoint.Behaviors.Add(consoleOutputBehavior);


try
{
    shipmentResponse = shpSvc.ProcessShipConfirm(upss, shipmentRequest);
    var resultJson = ServiceStack.Text.JsonSerializer.SerializeToString(shipmentResponse);
    Console.WriteLine(resultJson);
}
catch (Exception e)
{
    if (!string.IsNullOrEmpty(consoleOutputBehavior.ConsoleOutputInspector.ResponseXML))
    {
        XElement element = XElement.Parse(consoleOutputBehavior.ConsoleOutputInspector.ResponseXML);
        IEnumerable<XElement> elements = element.Descendants(XName.Get("Description", "http://www.ups.com/XMLSchema/XOLTWS/Error/v1.1"));
        if (elements != null && elements.Count() > 0)
        {
            string message = elements.First().Value;
            Console.WriteLine(message);
        }
    }
}

----------------------

public class ConsoleOutputBehavior : IEndpointBehavior
    {
        public ConsoleOutputInspector ConsoleOutputInspector { get; private set; }
        public void AddBindingParameters(ServiceEndpoint endpoint, BindingParameterCollection bindingParameters)
        {
        }

        public void ApplyClientBehavior(ServiceEndpoint endpoint, ClientRuntime clientRuntime)
        {
            ConsoleOutputInspector = new ConsoleOutputInspector();
            clientRuntime.MessageInspectors.Add(ConsoleOutputInspector);
        }

        public void ApplyDispatchBehavior(ServiceEndpoint endpoint, EndpointDispatcher endpointDispatcher)
        {
            throw new Exception("Behavior not supported on the server side!");
        }

        public void Validate(ServiceEndpoint endpoint)
        {
        }
    }
    ----------------------------------
    public class ConsoleOutputInspector : IClientMessageInspector
    {
        public string ResponseXML = string.Empty;

        public void AfterReceiveReply(ref Message reply, object correlationState)
        {
            ResponseXML = reply.ToString();
        }

        public object BeforeSendRequest(ref Message request, IClientChannel channel)
        {
            return null;
        }
    }
    
