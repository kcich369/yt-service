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
    private readonly IMessagePublisher _publisher;
    private readonly ILogger<YtVideosController> _logger;
    private readonly IAppDbContext _dbContext;
    private readonly IUnitOfWork _unitOfWork;

    public YtVideosController(IMediator mediator, IMessagePublisher publisher,
        ILogger<YtVideosController> logger,
        IAppDbContext dbContext,
        IUnitOfWork unitOfWork) : base(mediator)
    {
        _publisher = publisher;
        _logger = logger;
        _dbContext = dbContext;
        _unitOfWork = unitOfWork;
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

    [HttpPost]
    [SwaggerOperation(Summary = "Data of searching yt videos names",
        Description = "Search yt video names by given value")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> ClosedCaptions(CancellationToken token)
    {
        // _logger.LogError("Error from yt videos controller");
        var message = new ChannelCreated(new YtChannelId("01HBH0EAFX616XN8G2AQWBZHA8"));
        // var message = new NewVideoCreated(new YtVideoId("01HBH0GR0X79X0J1HMGMGAQKX8"),new YtChannelId("01HBH0EAFX616XN8G2AQWBZHA8"));

        await _publisher.Send(message);

        // var channel = await _dbContext.Set<YtChannel>().FirstOrDefaultAsync(x => x.Id == new YtChannelId("01HB0X4DRJJCRYYE8H9BD71EMW"));
        // var channel = await _dbContext.Set<YtChannel>().FirstOrDefaultAsync();
        return Ok("OK!");
    }
}