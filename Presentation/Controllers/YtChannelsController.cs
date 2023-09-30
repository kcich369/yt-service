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
    private readonly IAppDbContext _dbContext;
    private readonly IDownloadYtVideoFilesService _downloadYtVideoFilesService;
    private readonly IAddChannelVideosService _channelVideosService;

    public YtChannelsController(IMediator mediator,
        IAppDbContext dbContext,
        IDownloadYtVideoFilesService downloadYtVideoFilesService,
        IAddChannelVideosService channelVideosService) : base(mediator)
    {
        _dbContext = dbContext;
        _downloadYtVideoFilesService = downloadYtVideoFilesService;
        _channelVideosService = channelVideosService;
    }


    [HttpPost]
    [SwaggerOperation(Summary = "Create yt channel", Description = "As response list of videos")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create(CreateYtChannelDto createYtChannelDto, CancellationToken token) =>
        await Mediator.Send(new CreateYtChannelCommand(createYtChannelDto), token).ToActionResult();

    [HttpPost("save/{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> SaveAll(string id, CancellationToken token)
    {
        await _downloadYtVideoFilesService.Download(new YtVideoId(id),token);
        return Ok("Ok");
    }

    [HttpPost("save-all")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> SaveAll(CancellationToken token)
    {
        var stopWatch = new Stopwatch();
        stopWatch.Start();
        stopWatch.Stop();
        return Ok(new { Time = stopWatch.Elapsed });
    }

    [HttpPost("apply-new-videos/{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> ApplyNewVideos(string id, CancellationToken token)
    {
        await _channelVideosService.ApplyNewVideos(new YtChannelId(id), token);
        return Ok("Ok");
    }

    [HttpGet("{id}/video-names")]
    [SwaggerOperation(Summary = "Create yt channel", Description = "As response list of videos")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetWithVideoNames(string id, CancellationToken token) => await Mediator
        .Send(new GetYtChannelAllWithVideoNamesQuery(new YtChannelId(id)), token)
        .ToActionResult();
}