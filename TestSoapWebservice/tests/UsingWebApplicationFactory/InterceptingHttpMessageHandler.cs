namespace UsingWebApplicationFactory;

internal class InterceptingHttpMessageHandler : DelegatingHandler
{
    private readonly HttpClient _client;

    public InterceptingHttpMessageHandler(HttpMessageHandler innerHandler, HttpClient client)
    {
        InnerHandler = innerHandler;
        _client = client;
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        return await _client.PostAsync(request.RequestUri, request.Content, cancellationToken);
    }
}