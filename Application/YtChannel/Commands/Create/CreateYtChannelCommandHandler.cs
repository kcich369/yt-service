using Domain.Dtos.YtChannel;
using Domain.Enumerations;
using Domain.Repositories;
using Domain.Results;
using Domain.Services;
using MediatR;

namespace Application.YtChannel.Commands.Create;

public sealed class
    CreateYtChannelCommandHandler : IRequestHandler<CreateYtChannelCommand, IResult<YtChannelVideosDto>>
{
    private readonly IYtChannelRepository _ytChannelRepository;
    private readonly ICreateYtChannelWithVideosService _createYtChannelWithVideosService;

    public CreateYtChannelCommandHandler(IYtChannelRepository ytChannelRepository,
        ICreateYtChannelWithVideosService createYtChannelWithVideosService)
    {
        _ytChannelRepository = ytChannelRepository;
        _createYtChannelWithVideosService = createYtChannelWithVideosService;
    }

    public async Task<IResult<YtChannelVideosDto>> Handle(CreateYtChannelCommand request,
        CancellationToken cancellationToken)
    {
        var createChannelResult =
            await _createYtChannelWithVideosService.Execute(request.CreateYtChannelDto.Name, cancellationToken);

        return createChannelResult.IsError
            ? Result<YtChannelVideosDto>.Error(createChannelResult)
            : Result<YtChannelVideosDto>.Success(createChannelResult.Data);
    }
}