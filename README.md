# How to test code that is using a SOAP client

## Assumptions

- You have a SOAP client that is targeting a SOAP Web Service.
- The SOAP client is used by some code that is making a request and gets a response, and you want to test this code.
- The target SOAP Web Service is not available for testing or it is available only during a certain period of time, making it difficult to assume that it is always online

## Options

- Mock the SOAP Client: this is the true unit testing approach
- Use a mock service: build a mock service that the SOAP client can call, using the wsdl file. This is an integration test approach where the request-response pipeline is also tested.
  - Use `WebApplicationFactory` to create an in-memory instance of the mock service
  - Use `IHost` and `IHostBuilder` to run the mock service using Kestrel
  - Use `TestContainers` to run a containerized mock service, which requires the creation of a docker image, as well as running Docker
 

## Comparison
### Host vs WebApplicationFactory
|Feature|IHost|WebApplicationFactory|
|:------|:----|:--------------------|
|Setup|More manual control|Simpler, less code|
|Server|Real Kestrel server|In-memory TestServer|
|Port|Needs explicit port|Random available port|
|Speed|Slightly slower (real TCP)|Faster (in-memory)|
|Debugging|Can test with external tools|Cannot access externally|
|Port conflicts|Can happen|Never happens|

### Host vs WebApplicationFactory vs TestContainers
|Approach|Pros|Cons|
|:-------|:---|:---|
|WebApplicationFactory|Fast, in-memory, no Docker needed|Not testing real deployment|
|IHost|Fast, more control than WAF|Still not real deployment|
|Testcontainers|Tests real container, closest to production|Slower, requires Docker|

**Sources**
- [How do I integration test a SoapCore endpoint? (Stack Overflow)](https://stackoverflow.com/questions/66338805/how-do-i-integration-test-a-soapcore-endpoint)
- [Integration tests in ASP.NET Core (Microsoft Learn)](https://learn.microsoft.com/en-us/aspnet/core/test/integration-tests?view=aspnetcore-9.0&pivots=xunit)
- [SOAP Web Service in .NET Core (C# Corner)](https://www.c-sharpcorner.com/article/soap-web-service-in-net-core/)
- [asammad48/SOAPAPIDotNetCore (github)](https://github.com/asammad48/SOAPAPIDotNetCore)
