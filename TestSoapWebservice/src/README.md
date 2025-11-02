# How to create a  SOAP web service in ASP.NET Core

This guide will walk you through the steps to create a SOAP web service using ASP.NET Core. SOAP (Simple Object Access Protocol) is a protocol for exchanging structured information in web services.

1. **Set Up Your ASP.NET Core WebAPI Project**

   First, create a new ASP.NET Core Web API. You can do this using the .NET CLI, e.g.:

   ```bash
   dotnet new webapi -n DemoWebService
   cd DemoWebService
   ```
2. **Add RequiredNuGet Packages**

   You will need to add the `SoapCore` package to your project. You can do this via the NuGet Package Manager or by running the following command:

   ```bash
   dotnet add package SoapCore
   ``` 
3. **Create a Service Contract**
    
    Define an interface that represents your SOAP service contract. For example, create a file named `IMySoapService.cs`:
    
    ```csharp
    using System.ServiceModel;
    using System.Threading.Tasks;
    
    [ServiceContract]
    public interface ICalculatorSoapService
    {
         [OperationContract]
         string Sum(int num1,int num2);
    }
    ```
4. **Implement the Service**
   
    Create a class that implements the service contract. For example, create a file named `MySoapService.cs`:
   ```csharp
      using System.Threading.Tasks;
     
      public class CalculatorSoapService : ICalculatorSoapService
      {
           public string Sum(int num1, int num2)
           {
               return $"Sum of two number is: {num1+ num2}";
           }
      }
   ```
5. **Configure the SOAP Endpoint**
    
    In your `Startup.cs` or `Program.cs` (depending on your ASP.NET Core version), configure the SOAP endpoint:
    
    ```csharp
    using Microsoft.AspNetCore.Builder;
    using Microsoft.Extensions.DependencyInjection;
    using SoapCore;
    
    var builder = WebApplication.CreateBuilder(args);
    
    // Add services to the container.
    builder.Services.AddSoapCore();
    builder.Services.AddSingleton<ICalculatorSoapService, CalculatorSoapService>();
    
    var app = builder.Build();
    
    // Configure the HTTP request pipeline.
    app.UseRouting();
    
    app.UseEndpoints(endpoints =>
    {
         endpoints.UseSoapEndpoint<ICalculatorSoapService>("/Service.svc", new SoapEncoderOptions(), SoapSerializer.DataContractSerializer);
    });
    
    app.Run();
    ```
6. **Run the Application**

    Now, run your application using the following command:
    
    ```bash
    dotnet run
    ```
    
    Your SOAP web service should now be running at `http://localhost:5000/Service.svc`.
7. **Test the SOAP Service**
    
    You can test your SOAP service using tools like Postman, SoapUI, or by creating a simple client application that consumes the SOAP service.
8. **Create a SOAP Client (Optional)**
    
    If you want to create a client to consume the SOAP service, you can use the `Connected Services` feature in Visual Studio or use the `dotnet-svcutil` tool to generate the client proxy classes.
    ```bash
    dotnet tool install --global dotnet-svcutil
    dotnet-svcutil http://localhost:5000/Service.svc
    ```
    This will generate the necessary client code to call your SOAP service.
    Alternatively, you can download the WSDL from `http://localhost:5000/Service.svc?wsdl` and use it to generate client code.
    ```bash
    wget http://localhost:5000/Service.svc?wsdl -o service.wsdl
    dotnet-svcutil service.wsdl
    ```
    Note: The dotnet-svcutil generates asynchronous methods by default. To force the generation of synchronous methods, use the following:
    ```bash
    dotnet-svcutil service.wsdl --sync
    ```
9. **Use the SOAP Client (Optional)**

    The generated client can now be used to call the SOAP service methods defined in your service contract.
    ```csharp
    var binding = new BasicHttpBinding();
    var endpoint = new EndpointAddress("http://localhost:5000/Service.svc");
    
    var client = new CalculatorSoapServiceClient(binding, endpoint);
    var response = await client.SumAsync(a, b);
    return response.Body.SumResult;
    ```
10. **Conclusion**
    
    You have successfully created a SOAP web service using ASP.NET Core. You can now expand upon this basic example by adding more operations, handling complex data types, and implementing additional features as needed.

## **References**
- [SOAP Web Service in .NET Core (C# Corner)](https://www.c-sharpcorner.com/article/soap-web-service-in-net-core/)
- [asammad48/SOAPAPIDotNetCore (github)](https://github.com/asammad48/SOAPAPIDotNetCore)