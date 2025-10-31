using DemoWebService.WebServices;
using SoapCore;

namespace DemoWebService;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add SOAP services to the container.
        builder.Services.AddSoapCore();
        builder.Services.AddScoped<WebServices.CalculatorSoapService>();

        var app = builder.Build();

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

        app.Run();
    }
}