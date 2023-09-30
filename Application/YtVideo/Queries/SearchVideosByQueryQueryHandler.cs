using Domain.Dtos.YtVideo;
using Domain.Repositories;
using MediatR;

namespace Application.YtVideo.Queries;

public record SearchVideosByQueryQuery(SearchVideosDto SearchVideosDto) : IRequest<YtVideoSearchResult>;

public sealed class SearchVideosByQueryQueryHandler : IRequestHandler<SearchVideosByQueryQuery, YtVideoSearchResult>
{
    private readonly IYtVideoRepository _ytVideoRepository;

    public SearchVideosByQueryQueryHandler(IYtVideoRepository ytVideoRepository)
    {
        _ytVideoRepository = ytVideoRepository;
    }

    public async Task<YtVideoSearchResult> Handle(SearchVideosByQueryQuery request,
        CancellationToken cancellationToken) =>
        new()
        {
            SearchQuery = request.SearchVideosDto.Query,
            Results = await _ytVideoRepository.SearchByQuery(request.SearchVideosDto.Query,
                request.SearchVideosDto.Amount, cancellationToken)
        };
}