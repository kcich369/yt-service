using Domain.Entities;
using Domain.EntityIds;
using Domain.Helpers;
using Domain.Repositories;
using Domain.Results;
using Domain.Services;
using Domain.UnitOfWork;
using ExternalServices.Interfaces;
using Hangfire;
using Infrastructure.Helpers.Interfaces;
using ServiceBus.Producer.Messages;
using ServiceBus.Producer.Publisher;

namespace Infrastructure.Services;

public class TranscriptionDataService : ITranscriptionDataService
{
    private readonly IYtVideoTranscriptionRepository _transcriptionRepository;
    private readonly IChatGptService _chatGptService;
    private readonly ITxtFileHelper _txtFileHelper;
    private readonly IUnitOfWork _unitOfWork;

    public TranscriptionDataService(IYtVideoTranscriptionRepository transcriptionRepository,
        IChatGptService chatGptService,
        ITxtFileHelper txtFileHelper,
        IUnitOfWork unitOfWork)
    {
        _transcriptionRepository = transcriptionRepository;
        _chatGptService = chatGptService;
        _txtFileHelper = txtFileHelper;
        _unitOfWork = unitOfWork;
    }

    [DisableConcurrentExecution(timeoutInSeconds: 60)]
    public async Task<IResult<bool>> Create(YtVideoTranscriptionId ytVideoTranscriptionId, CancellationToken token)
    {
        var transcription = await _transcriptionRepository.GetToVideoDescription(ytVideoTranscriptionId, token);
        if (transcription is null)
            return Result<bool>.Success(true);

        var tagsResult = await (await _chatGptService.Ask(
                await _txtFileHelper.GetContents(transcription.PathData.FullValue, token), token))
            .ReturnOut(out var descriptionResult)
            .Next((des) => _chatGptService.Ask("Tags", token));
        if (tagsResult.IsError) // todo: log errors
            return Result<bool>.Error(tagsResult);

        transcription.AddData(
            YtVideoDescription.Create(descriptionResult.Data),
            YtVideoTag.Create(tagsResult.Data));
        await _unitOfWork.SaveChangesAsync(token);
        return Result<bool>.Success(true);
    }
}