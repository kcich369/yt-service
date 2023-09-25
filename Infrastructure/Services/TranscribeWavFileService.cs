using Domain.Entities;
using Domain.EntityIds;
using Domain.Helpers;
using Domain.Repositories;
using Domain.Results;
using Domain.Services;
using Domain.UnitOfWork;
using ExternalServices.Interfaces;
using Hangfire;
using ServiceBus.Producer.Messages;
using ServiceBus.Producer.Publisher;

namespace Infrastructure.Services;

[DisableConcurrentExecution(timeoutInSeconds: 60)]
public sealed class TranscribeWavFileService : ITranscribeWavFileService
{
    private readonly IYtVideoFileWavRepository _videoFileWavRepository;
    private readonly ISpeechToTextService _speechToTextService;
    private readonly ITranscriptionHelper _transcriptionHelper;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMessagePublisher _messagePublisher;

    public TranscribeWavFileService(IYtVideoFileWavRepository videoFileWavRepository,
        ISpeechToTextService speechToTextService,
        ITranscriptionHelper transcriptionHelper,
        IUnitOfWork unitOfWork,
        IMessagePublisher messagePublisher)
    {
        _videoFileWavRepository = videoFileWavRepository;
        _speechToTextService = speechToTextService;
        _transcriptionHelper = transcriptionHelper;
        _unitOfWork = unitOfWork;
        _messagePublisher = messagePublisher;
    }

    public async Task<IResult<bool>> Transcribe(YtVideoFileWavId videoFileWavId, CancellationToken token)
    {
        var ytVideoFileWav = await _videoFileWavRepository.GetToTranscription(videoFileWavId, token);
        if (ytVideoFileWav == null)
            return Result<bool>.Success(true);

        var transcriptionPathResult = await TranscriptionPathResult(ytVideoFileWav.PathData.FullValue,
            ytVideoFileWav.PathData.FileName, ytVideoFileWav.Language, token);
        if (transcriptionPathResult.IsError) //todo: log error
            return Result<bool>.Error(transcriptionPathResult);

        ytVideoFileWav.AddTranscription(YtVideoTranscription
            .Create(ytVideoFileWav.PathData.MainPath)
            .SetFileName(transcriptionPathResult.Data));
        await _unitOfWork.SaveChangesAsync(token);
        await _messagePublisher.Send(new VideoTranscribed(ytVideoFileWav.YtVideoTranscription.Id));

        return transcriptionPathResult.IsError
            ? Result<bool>.Error(transcriptionPathResult)
            : Result<bool>.Success(true);
    }

    private async Task<IResult<string>> TranscriptionPathResult(string path, string fileName, string language,
        CancellationToken token)
    {
        var transcriptionResult =
            await _speechToTextService.TranscribeFromWavFile(path, language, token);
        return transcriptionResult.IsError
            ? Result<string>.Error(transcriptionResult)
            : Result<string>.Success(
                await _transcriptionHelper.SaveTranscription(path, fileName, transcriptionResult.Data, token));
    }
}