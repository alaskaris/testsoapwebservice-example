using System;
using DemoWebServiceProxy; // or the namespace you used with -n flag
using System.ServiceModel;
using System.Threading.Tasks;

namespace DemoWebServiceClient;

public class Calculator
{
    private readonly string _serviceUrl;

    public Calculator(string serviceUrl)
    {
        _serviceUrl = serviceUrl;
    }

    public async Task<string> Add(int a, int b)
    {
        var binding = new BasicHttpBinding();
        var endpoint = new EndpointAddress(_serviceUrl);
        var client = new CalculatorSoapServiceClient(binding, endpoint);
    
        try
        {
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