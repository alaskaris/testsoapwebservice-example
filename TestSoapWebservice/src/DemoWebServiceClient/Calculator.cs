using System;
using System.Net.Http;
using DemoWebServiceProxy; // or the namespace you used with -n flag
using System.ServiceModel;
using System.Threading.Tasks;
using DemoWebServiceClient.Testing;

namespace DemoWebServiceClient;

public class Calculator
{
    private readonly string _serviceUrl;
    private readonly HttpClient? _httpClient;

    // Constructor for production use
    public Calculator(string serviceUrl)
    {
        _serviceUrl = serviceUrl;
    }

    // Constructor for testing with HttpClient
    public Calculator(HttpClient httpClient, string serviceUrl)
    {
        _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        _serviceUrl = serviceUrl;
    }

    public async Task<string> Add(int a, int b)
    {
        var binding = new BasicHttpBinding();
        var endpoint = new EndpointAddress(_serviceUrl);
        var client = new CalculatorSoapServiceClient(binding, endpoint);

        try
        {
            // If HttpClient was provided (for testing), use it
            if (_httpClient != null)
            {
                client.Endpoint.InterceptRequestsWithHttpClient(_httpClient);
            }

            var response = await client.SumAsync(a, b);
            return response.Body.SumResult;
        }
        finally
        {
            if (client.State == CommunicationState.Faulted)
            {
                client.Abort();
            }
            else
            {
                await client.CloseAsync();
            }
        }
    }
}