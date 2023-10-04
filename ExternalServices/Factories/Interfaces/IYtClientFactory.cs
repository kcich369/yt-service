using YoutubeReExplode;

namespace ExternalServices.Factories.Interfaces;

internal interface IYtClientFactory
{
    YoutubeClient GetYtClient();
}