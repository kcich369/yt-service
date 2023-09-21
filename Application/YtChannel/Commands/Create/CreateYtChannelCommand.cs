using Domain.Dtos.YtChannel;
using Domain.Results;
using MediatR;

namespace Application.YtChannel.Commands.Create;

public record CreateYtChannelCommand
    (CreateYtChannelDto CreateYtChannelDto) : IRequest<IResult<YtChannelVideosDto>>;