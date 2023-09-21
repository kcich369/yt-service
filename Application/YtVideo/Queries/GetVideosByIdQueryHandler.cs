using Domain.Dtos.YtVideo;
using Domain.EntityIds;
using Domain.Enumerations;
using Domain.Repositories;
using Domain.Results;
using MediatR;

namespace Application.YtVideo.Queries;

public record GetVideosByIdQuery(YtVideoId Id) : IRequest<IResult<YtVideoDetailsDto>>;

public sealed class GetVideosByIdQueryHandler : IRequestHandler<GetVideosByIdQuery, IResult<YtVideoDetailsDto>>
{
    private readonly IYtVideoRepository _ytVideoRepository;

    public GetVideosByIdQueryHandler(IYtVideoRepository ytVideoRepository)
    {
        _ytVideoRepository = ytVideoRepository;
    }

    public async Task<IResult<YtVideoDetailsDto>> Handle(GetVideosByIdQuery request,
        CancellationToken cancellationToken)
    {
        var video = await _ytVideoRepository.GetById(request.Id, cancellationToken);
        return video is null
            ? Result<YtVideoDetailsDto>.Error(ErrorTypesEnums.NotFound)
            : Result<YtVideoDetailsDto>.Success(video);
    }
}