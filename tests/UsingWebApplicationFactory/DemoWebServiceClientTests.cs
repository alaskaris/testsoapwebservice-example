using System.ServiceModel;
using Microsoft.AspNetCore.Mvc.Testing;
using DemoWebService;
using DemoWebServiceClient;
using DemoWebServiceProxy;

namespace UsingWebApplicationFactory;

public class DemoWebServiceClientTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;

    public DemoWebServiceClientTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task ServiceEndpoint_IsReachable()
    {
        var client = _factory.CreateClient();
        var response = await client.GetAsync("/demo.svc?wsdl", TestContext.Current.CancellationToken);
        var content = await response.Content.ReadAsStringAsync(TestContext.Current.CancellationToken);

        Assert.True(response.IsSuccessStatusCode);
        Assert.Contains("wsdl", content);
    }

    [Fact]
    public async Task ProxyClient_Add_ReturnsCorrectSum()
    {
        // Get the base address of the in-memory service
        var client = _factory.CreateClient();
        var serviceUrl = client.BaseAddress + "demo.svc";

        var binding = new BasicHttpBinding();
        var endpoint = new EndpointAddress(serviceUrl);
        var calculatorSoapServiceClient = new CalculatorSoapServiceClient(binding, endpoint);
        calculatorSoapServiceClient.Endpoint.InterceptRequestsWithHttpClient(client);

        var response = await calculatorSoapServiceClient.SumAsync(5, 3);
        var result = response.Body.SumResult;

        Assert.Equal($"Sum of two number is: 8", result);
    }

    [Fact]
    public async Task Calculator_Add_ReturnsCorrectSum()
    {
        var client = _factory.CreateClient();
        var serviceUrl = client.BaseAddress + "demo.svc";

        var calculator = new Calculator(client, serviceUrl.ToString());
        var result = await calculator.Add(5, 3);

        Assert.Equal("Sum of two number is: 8", result);
    }
}