# Integration Testing with IHost in ASP.NET Core

This guide demonstrates how to set up integration tests for an ASP.NET Core application using `IHost`. 
This is particularly useful for testing SOAP services implemented with SoapCore.

Based on the context showing this README is specifically about IHost (not TestContainers), here's an appropriate **Background** section:

## Background

Integration testing ASP.NET Core SOAP services requires a testing approach that accurately reflects how the service 
behaves in a real hosting environment. Traditional unit testing approaches don't adequately test the full HTTP pipeline, 
middleware configuration, and SOAP endpoint routing.

`IHost` provides a lightweight yet comprehensive solution for integration testing by:

- **Running the actual application code** in a test environment
- **Using real TCP ports** for HTTP communication (not in-memory servers)
- **Executing the full middleware pipeline** exactly as configured in production
- **Reusing the same configuration logic** from your application startup

This approach is particularly valuable for SOAP services built with SoapCore because:
- SOAP endpoints require proper HTTP routing and middleware configuration
- XML serialization/deserialization needs to work end-to-end
- The WCF/SOAP message handling pipeline must be tested in full
- Request/response validation happens at the HTTP transport layer

Unlike `WebApplicationFactory` which creates a test server with in-memory HTTP handling, `IHost` spins up a real web server on localhost, making it ideal for scenarios where you need to test:
- External HTTP clients connecting to your service
- Complex middleware interactions
- Port-specific configurations
- Real network communication patterns

The result is a balance between test speed (faster than containerized tests) and realism (closer to production than pure unit tests).


## Solution
The `IHost` interface provides a lightweight yet comprehensive solution for integration testing by:

- **Running the actual application code** in a test environment
- **Using real TCP ports** for HTTP communication (not in-memory servers)
- **Executing the full middleware pipeline** exactly as configured in production<sup>1</sup>
- **Reusing the same configuration logic** from your application startup

This approach is particularly valuable for SOAP services built with SoapCore because:
- SOAP endpoints require proper HTTP routing and middleware configuration
- XML serialization/deserialization needs to work end-to-end
- The WCF/SOAP message handling pipeline must be tested in full
- Request/response validation happens at the HTTP transport layer

Unlike `WebApplicationFactory` which creates a test server with in-memory HTTP handling, `IHost` spins up a real web 
server on localhost, making it ideal for scenarios where you need to test:
- External HTTP clients connecting to your service
- Complex middleware interactions
- Port-specific configurations
- Real network communication patterns

The result is a balance between test speed (faster than containerized tests) and realism (closer to production than pure unit tests).

<sup>1</sup> The same code that is used to configure the services and the middleware pipeline in your application is reused in 
the test host. To avoid code duplication, we can extract the configuration logic into a separate method or class that 
can be called from both the application startup and the test setup.