using System.ServiceModel;
using DemoWebServiceClient;
using DemoWebServiceProxy;
using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Images;

namespace UsingTestContainers;

public class DemoWebServiceClientTests : IAsyncLifetime
{
    private DotNet.Testcontainers.Containers.IContainer? _container;
    private string? _serverUrl;

    public async ValueTask InitializeAsync()
    {
        var projectDirectory = Directory.GetParent(Directory.GetCurrentDirectory())!.Parent!.Parent!.Parent!.Parent!.FullName;
        var imageName = "demowebservice:latest";
        
        // Equivalent of running in a terminal from directory src:
        // docker build -f DemoWebService\Dockerfile -t demowebservice:latest . 
        var imageFromDockerfile = new ImageFromDockerfileBuilder()
            .WithDockerfileDirectory(Path.Combine(projectDirectory,"src")) // Docker context
            .WithDockerfile("DemoWebService/Dockerfile") // Path to Dockerfile
            .WithName(imageName)
            .WithCleanUp(true) // Cleanup intermediate containers
            .Build();
        
        // Build the image
        await imageFromDockerfile.CreateAsync();
        
        // Create the container
        _container = new ContainerBuilder()
            .WithImage(imageName)
            .WithPortBinding(8080, true)
            .WithWaitStrategy(Wait.ForUnixContainer()
                .UntilInternalTcpPortIsAvailable(8080)
                .UntilHttpRequestIsSucceeded(r => r.ForPort(8080)
                    .ForPath("/demo.svc")
                    .ForStatusCode(System.Net.HttpStatusCode.OK)))
            .WithOutputConsumer(Consume.RedirectStdoutAndStderrToConsole())
            .Build();

        // Start the container
        await _container.StartAsync();

        var port = _container.GetMappedPublicPort(8080);
        _serverUrl = $"http://localhost:{port}";
    }
    

    public async ValueTask DisposeAsync()
    {
        if (_container != null)
        {
            await _container.DisposeAsync();
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
        var calculator = new Calculator($"{_serverUrl}/demo.svc");
        var result = await calculator.Add(5, 3);

        Assert.Equal("Sum of two number is: 8", result);
    }
}