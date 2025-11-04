# Integration Testing Strategies for SOAP Clients in .NET
*How to Test Code Using SOAP Clients Without Live Services*

## Assumptions

- You have code that consumes a SOAP Web Service through a SOAP client.
- You need to test this code's request/response handling.
- The target SOAP Web Service is unreliable for testing purposes—either unavailable, intermittently accessible, or not guaranteed to be online.

## Options

**1. Mock the SOAP Client**  
Replace the SOAP client with a test double. This is the pure unit testing approach that isolates your code from external dependencies.

**2. Mock the SOAP Service**  
Create a fake SOAP service that responds to your client's requests. This integration testing approach validates the entire request-response pipeline. You can implement this using:
- **WebApplicationFactory**: Hosts the mock service in-memory for fast, isolated tests
- **IHost with Kestrel**: Runs the mock service as a real HTTP server on localhost
- **Testcontainers**: Runs the mock service in a Docker container for production-like testing

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

## Prerequisites

- .NET 8.0 or later

## License
This project is provided as-is for educational purposes.