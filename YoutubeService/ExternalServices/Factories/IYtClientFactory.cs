using YoutubeReExplode;

namespace ExternalServices.Factories;

internal interface IYtClientFactory
{
    YoutubeClient GetYtClient();
}