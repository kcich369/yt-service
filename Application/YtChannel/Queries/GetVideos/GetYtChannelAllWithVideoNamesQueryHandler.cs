using Domain.Dtos;
using Domain.Dtos.YtChannel;
using Domain.EntityIds;
using Domain.Enumerations;
using Domain.Repositories;
using Domain.Results;
using MediatR;

namespace Application.YtChannel.Queries.GetVideos;

public record GetYtChannelAllWithVideoNamesQuery(YtChannelId YtChannelId) : IRequest<IResult<YtChannelVideosDto>>;

public class
    GetYtChannelAllWithVideoNamesQueryHandler : IRequestHandler<GetYtChannelAllWithVideoNamesQuery,
        IResult<YtChannelVideosDto>>
{
    private readonly IYtChannelRepository _ytChannelRepository;

    public GetYtChannelAllWithVideoNamesQueryHandler(IYtChannelRepository ytChannelRepository)
    {
        _ytChannelRepository = ytChannelRepository;
    }

    public async Task<IResult<YtChannelVideosDto>> Handle(GetYtChannelAllWithVideoNamesQuery request,
        CancellationToken cancellationToken)
    {
        var channel =
            await _ytChannelRepository.GetYtVideoChannelWithDownloadedVideoNames(request.YtChannelId, cancellationToken);
        return channel is null
            ? Result<YtChannelVideosDto>.Error(ErrorTypesEnums.NotFound, "Channel with given id does not exist")
            : Result<YtChannelVideosDto>.Success(channel);
    }
}