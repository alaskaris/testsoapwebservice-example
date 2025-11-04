using System;
using System.Net.Http;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;
using System.Threading;
using System.Threading.Tasks;

namespace DemoWebServiceClient.Testing;

internal class HttpMessageHandlerBehavior : IEndpointBehavior
{
    private readonly HttpClient _client;

    public HttpMessageHandlerBehavior(HttpClient client)
    {
        _client = client;
    }

    public void AddBindingParameters(ServiceEndpoint endpoint, BindingParameterCollection bindingParameters)
    {
        bindingParameters.Add(new Func<HttpClientHandler, HttpMessageHandler>(GetHttpMessageHandler));
    }

    public void ApplyClientBehavior(ServiceEndpoint endpoint, ClientRuntime clientRuntime) { }

    public void ApplyDispatchBehavior(ServiceEndpoint endpoint, EndpointDispatcher endpointDispatcher) { }

    public void Validate(ServiceEndpoint endpoint) { }

    public HttpMessageHandler GetHttpMessageHandler(HttpClientHandler httpClientHandler)
    {
        return new InterceptingHttpMessageHandler(httpClientHandler, _client);
    }

    public Func<HttpRequestMessage, CancellationToken, Task<HttpResponseMessage>> OnSendingAsync { get; set; }
    public Func<HttpResponseMessage, CancellationToken, Task<HttpResponseMessage>> OnSentAsync { get; set; }
}