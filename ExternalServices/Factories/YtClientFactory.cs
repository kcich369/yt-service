using YoutubeReExplode;

namespace ExternalServices.Factories;

internal sealed class YtClientFactory : IYtClientFactory
{
    private YoutubeClient _ytClient;

    public YoutubeClient GetYtClient() => _ytClient ??= new YoutubeClient();
}