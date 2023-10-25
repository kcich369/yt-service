using System.Diagnostics;
using Application.YtChannel.Commands.Create;
using Application.YtChannel.Queries.GetVideos;
using Domain.Entities;
using Domain.EntityIds;
using Domain.Services;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Persistence.Context;
using Presentation.Abstractions;
using Presentation.Extensions;
using Swashbuckle.AspNetCore.Annotations;

namespace Presentation.Controllers;

[Route("ytchannels")]
public class YtChannelsController : ApiController
{
    public YtChannelsController(IMediator mediator) : base(mediator)
    {
    }

    [HttpPost]
    [SwaggerOperation(Summary = "Create yt channel", Description = "As response list of videos")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create(CreateYtChannelDto createYtChannelDto, CancellationToken token) =>
        await Mediator.Send(new CreateYtChannelCommand(createYtChannelDto), token).ToActionResult();

   
    [HttpGet("{id}/video-names")]
    [SwaggerOperation(Summary = "Get yt channel with videos list", Description = "Get yt channel with videos list")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetWithVideoNames(string id, CancellationToken token) => await Mediator
        .Send(new GetYtChannelAllWithVideoNamesQuery(new YtChannelId(id)), token)
        .ToActionResult();
}