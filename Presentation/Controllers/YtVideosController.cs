using System.Text.Json;
using Application.YtVideo.Queries;
using Domain.Dtos;
using Domain.Dtos.YtVideo;
using Domain.Entities;
using Domain.EntityIds;
using Domain.UnitOfWork;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Persistence.Context;
using Presentation.Abstractions;
using ServiceBus.Producer.Messages;
using ServiceBus.Producer.Publisher;
using Swashbuckle.AspNetCore.Annotations;

namespace Presentation.Controllers;

[Route("yt-videos")]
public sealed class YtVideosController : ApiController
{
    public YtVideosController(IMediator mediator, IMessagePublisher publisher) : base(mediator)
    {
    }

    [HttpPost("search")]
    [SwaggerOperation(Summary = "Data of searching yt videos names", Description = "Search yt video by name")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetWithVideoNames(SearchVideosDto searchVideosDto, CancellationToken token) => Ok(
        await Mediator.Send(new SearchVideosByQueryQuery(searchVideosDto), token));

    [HttpGet("id")]
    [SwaggerOperation(Summary = "Data of searching yt videos names",
        Description = "Search yt video names by given value")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Get(string id, CancellationToken token) =>
        Ok(await Mediator.Send(new GetVideosByIdQuery(new YtVideoId(id)), token));
}