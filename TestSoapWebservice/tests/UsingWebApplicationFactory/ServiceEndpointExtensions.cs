using System.ServiceModel.Description;

namespace UsingWebApplicationFactory;

public static class ServiceEndpointExtensions {
    public static void InterceptRequestsWithHttpClient(this ServiceEndpoint endpoint, HttpClient httpClient)
    {
        endpoint.EndpointBehaviors.Add(new HttpMessageHandlerBehavior(httpClient));
    }
}