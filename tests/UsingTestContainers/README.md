# Integration Testing with TestContainers in ASP.NET Core

This guide demonstrates how to set up integration tests for an ASP.NET Core application using TestContainers. 
This is particularly useful for testing SOAP services implemented with SoapCore. 

## Background
The `TestContainers` library allows us to create lightweight, throwaway instances of the SOAP Web Service a in Docker 
container for testing purposes.

## Solution

1. **Configure the Dockerfile build context**: The build context should be set to the `src` directory 
(not the project root), as the Dockerfile's `COPY` commands reference paths relative to this location.

2. **Create the Docker image using TestContainers**: Use `ImageFromDockerfileBuilder` to build the image from your Dockerfile:
    - Set `WithDockerfileDirectory()` to the `src` folder (build context)
    - Set `WithDockerfile()` to the relative path of your Dockerfile (e.g., `DemoWebService/Dockerfile`)
    - Assign a name with `WithName()`

3. **Configure the container**: Use `ContainerBuilder` to set up the container with:
    - Port binding for your service (e.g., port 8080)
    - A wait strategy that checks both port availability and HTTP endpoint readiness
    - Output consumer to redirect container logs to test output for debugging

4. **Start the container**: Call `CreateAsync()` on the image builder, then `StartAsync()` on the container. The wait 
strategy ensures the service is ready before tests execute.

5. **Access the service**: Retrieve the mapped public port and construct the service URL to interact with your SOAP 
endpoints in tests.

**Key consideration**: Ensure the Dockerfile's `COPY` commands use paths relative to the `src` directory build context.
