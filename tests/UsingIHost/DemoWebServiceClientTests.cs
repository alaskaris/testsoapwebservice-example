using System.ServiceModel;
using DemoWebService;
using DemoWebService.WebServices;
using DemoWebServiceClient;
using DemoWebServiceProxy;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.Extensions.DependencyInjection;

namespace UsingIHost;

public class DemoWebServiceClientTests : IAsyncLifetime
{
    private IHost? _host;
    private string _serverUrl = "http://127.0.0.1:5050";

    public async ValueTask InitializeAsync()
    {
        var builder = WebApplication.CreateBuilder();

        // Either a static port can be used or a dynamic port can be assigned by using port 0
        builder.WebHost.UseUrls("http://127.0.0.1:0");

        // Configure services
        Program.ConfigureServices(builder.Services);
        
        var app = builder.Build();
        
        // Configure middleware
        Program.ConfigureMiddleware(app);
        
        _host = app;
        await _host.StartAsync();
        
        // Get the actual assigned port
        var address = _host.Services.GetRequiredService<IServer>().Features.Get<IServerAddressesFeature>();
        _serverUrl = address?.Addresses.FirstOrDefault() ?? throw new InvalidOperationException("Server address not found");
        
        // Give the server time to start
        await Task.Delay(500);
    }

    public async ValueTask DisposeAsync()
    {
        if (_host != null)
        {
            await _host.StopAsync();
            _host.Dispose();
        }
    }

    [Fact]
    public async Task ServiceEndpoint_IsReachable()
    {
        using var client = new HttpClient();
        var response = await client.GetAsync($"{_serverUrl}/demo.svc?wsdl", TestContext.Current.CancellationToken);
        var content = await response.Content.ReadAsStringAsync(TestContext.Current.CancellationToken);

        Assert.True(response.IsSuccessStatusCode);
        Assert.Contains("wsdl", content);
    }

    [Fact]
    public async Task ProxyClient_Add_ReturnsCorrectSum()
    {
        var binding = new BasicHttpBinding();
        var endpoint = new EndpointAddress($"{_serverUrl}/demo.svc");
        var calculatorSoapServiceClient = new CalculatorSoapServiceClient(binding, endpoint);

        var response = await calculatorSoapServiceClient.SumAsync(5, 3);
        var result = response.Body.SumResult;

        Assert.Equal("Sum of two number is: 8", result);
    }

    [Fact]
    public async Task Calculator_Add_ReturnsCorrectSum()
    {
        // Use the production constructor since we're making real HTTP calls
        var calculator = new Calculator($"{_serverUrl}/demo.svc");
        var result = await calculator.Add(5, 3);

        Assert.Equal("Sum of two number is: 8", result);
    }
}