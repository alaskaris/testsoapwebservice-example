# Integration Testing with IHost in ASP.NET Core

This guide demonstrates how to set up integration tests for an ASP.NET Core application using `IHost`. 
This is particularly useful for testing SOAP services implemented with SoapCore.

## Background
The `IHost` interface allows us to build and start the host, enabling us to make actual HTTP requests and verify 
responses in a controlled environment. This server is run in memory, making it very efficient 
for testing. It uses actual TCP ports instead of in-memory test server.

The same code that is used to configure the services and the middleware pipeline in your application is reused in 
the test host. To avoid code duplication, we can extract the configuration logic into a separate method or class that 
can be called from both the application startup and the test setup.