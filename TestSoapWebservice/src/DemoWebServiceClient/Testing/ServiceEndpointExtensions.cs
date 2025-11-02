using System.Net.Http;
using System.ServiceModel.Description;

namespace DemoWebServiceClient.Testing;

public static class ServiceEndpointExtensions
{
    public static void InterceptRequestsWithHttpClient(this ServiceEndpoint endpoint, HttpClient httpClient)
    {
        endpoint.EndpointBehaviors.Add(new HttpMessageHandlerBehavior(httpClient));
    }    
}