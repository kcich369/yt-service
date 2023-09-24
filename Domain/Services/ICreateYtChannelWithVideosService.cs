using Domain.Dtos.YtChannel;
using Domain.Results;

namespace Domain.Services;

public interface ICreateYtChannelWithVideosService
{
    Task<IResult<YtChannelVideosDto>> Execute(string name, bool handleName, CancellationToken token);
}