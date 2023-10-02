using Domain.Entities;
using Domain.EntityIds;
using Domain.Enumerations;
using Domain.Repositories;
using Domain.Results;
using Domain.Services;
using Domain.UnitOfWork;
using ExternalServices.Interfaces;
using Hangfire;
using Infrastructure.Dtos;
using Infrastructure.Extensions;
using Infrastructure.Helpers.Interfaces;
using Microsoft.Extensions.Logging;
using ServiceBus.Producer.Messages;
using ServiceBus.Producer.Publisher;

namespace Infrastructure.Services;

public sealed class TranscribeWavFileService : ITranscribeWavFileService
{
    private readonly IYtVideoFileWavRepository _videoFileWavRepository;
    private readonly ITranscriptionService _transcriptionService;
    private readonly ITxtFileHelper _txtFileHelper;
    private readonly ILogger<TranscribeWavFileService> _logger;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMessagePublisher _messagePublisher;

    public TranscribeWavFileService(IYtVideoFileWavRepository videoFileWavRepository,
        ITranscriptionService transcriptionService,
        ITxtFileHelper txtFileHelper,
        ILogger<TranscribeWavFileService> logger,
        IUnitOfWork unitOfWork,
        IMessagePublisher messagePublisher)
    {
        _videoFileWavRepository = videoFileWavRepository;
        _transcriptionService = transcriptionService;
        _txtFileHelper = txtFileHelper;
        _logger = logger;
        _unitOfWork = unitOfWork;
        _messagePublisher = messagePublisher;
    }

    [DisableConcurrentExecution(timeoutInSeconds: 60)]
    public async Task<IResult<bool>> Transcribe(YtVideoFileWavId videoFileWavId, CancellationToken token)
    {
        var ytVideoFileWav = await _videoFileWavRepository.GetToTranscription(videoFileWavId, token);
        if (ytVideoFileWav == null)
            return Result<bool>.Error(
                    ErrorTypesEnums.NotFound, $"Yt video file wav with given id: {videoFileWavId} does not exist")
                .LogErrorMessage(_logger);

        var transcriptionPathResult =
            await TranscriptionPathResult(new PathDataDto(ytVideoFileWav.PathData), ytVideoFileWav.Language, token);
        if (transcriptionPathResult.IsError)
            return Result<bool>.Error(transcriptionPathResult).LogErrorMessage(_logger);

        ytVideoFileWav.AddTranscription(YtVideoTranscription
            .Create(transcriptionPathResult.Data.MainPath)
            .SetFileName(transcriptionPathResult.Data.FileName));
        await _unitOfWork.SaveChangesAsync(token);
        await _messagePublisher.Send(new VideoTranscribed(ytVideoFileWav.YtVideoTranscription.Id, ytVideoFileWav.Id));

        return transcriptionPathResult.IsError
            ? Result<bool>.Error(transcriptionPathResult)
            : Result<bool>.Success(true);
    }

    private async Task<IResult<PathDataDto>> TranscriptionPathResult(PathDataDto wavFileData, string language,
        CancellationToken token)
    {
        var transcriptionResult =
            await _transcriptionService.TranscribeFromWavFile(wavFileData.FullValue, language, token);
        if (transcriptionResult.IsError)
            return Result<PathDataDto>.Error(transcriptionResult);
        var transcriptionPath = new PathDataDto($"{wavFileData.MainPath}\\Transcription", wavFileData.FileName, "txt");
        await _txtFileHelper.Save(transcriptionPath.FullValue, transcriptionResult.Data, token);
        return Result<PathDataDto>.Success(transcriptionPath);
    }
}