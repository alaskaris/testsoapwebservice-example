using DemoWebService.WebServices;
using SoapCore;

namespace DemoWebService;

// Adding partial to make it accessible in tests
public partial class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        ConfigureServices(builder.Services);

        var app = builder.Build();

        ConfigureMiddleware(app);

        app.Run();
    }

    public static void ConfigureMiddleware(WebApplication app)
    {
        app.UseRouting();

#pragma warning disable ASP0014
        app.UseEndpoints(endpoints =>
        {
            endpoints.UseSoapEndpoint<ICalculatorSoapService>(
                path: "/demo.svc",
                encoder: new SoapEncoderOptions(),
                serializer: SoapSerializer.XmlSerializer);
        });
#pragma warning restore ASP0014
    }

    public static void ConfigureServices(IServiceCollection serviceCollection)
    {
        // Add SOAP services to the container.
        serviceCollection.AddSoapCore();
        serviceCollection.AddSingleton<ICalculatorSoapService, CalculatorSoapService>();
    }
}