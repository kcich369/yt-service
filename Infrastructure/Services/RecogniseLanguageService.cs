using Domain.EntityIds;
using Domain.Enumerations;
using Domain.Enumerations.Base;
using Domain.Repositories;
using Domain.Results;
using Domain.Services;
using Domain.UnitOfWork;
using ExternalServices.Interfaces;
using Hangfire;
using Infrastructure.Extensions;
using Microsoft.Extensions.Logging;
using ServiceBus.Producer.Messages;
using ServiceBus.Producer.Publisher;

namespace Infrastructure.Services;

public sealed class RecogniseLanguageService : IRecogniseLanguageService
{
    private readonly IYtVideoFileWavRepository _videoFileWavRepository;
    private readonly ILanguageRecognitionService _languageRecognitionService;
    private readonly ILogger<RecogniseLanguageService> _logger;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMessagePublisher _messagePublisher;

    public RecogniseLanguageService(IYtVideoFileWavRepository videoFileWavRepository,
        ILanguageRecognitionService languageRecognitionService,
        ILogger<RecogniseLanguageService> logger,
        IUnitOfWork unitOfWork,
        IMessagePublisher messagePublisher)
    {
        _videoFileWavRepository = videoFileWavRepository;
        _languageRecognitionService = languageRecognitionService;
        _logger = logger;
        _unitOfWork = unitOfWork;
        _messagePublisher = messagePublisher;
    }

    [DisableConcurrentExecution(timeoutInSeconds: 60)]
    public async Task<IResult<bool>> Recognise(YtVideoFileWavId videoFileWavId, CancellationToken token)
    {
        var ytVideoWav = await _videoFileWavRepository.GetToLanguageRecognition(videoFileWavId, token);
        if (ytVideoWav is null)
            return Result<bool>.Success(true);
        var recogniseLanguageResult =
            await _languageRecognitionService.FromWavFile(ytVideoWav.PathData.FullValue, token);
        if (recogniseLanguageResult.IsError)
            return Result<bool>.Error(recogniseLanguageResult).LogErrorMessage(_logger);
        var language = Enumeration.GetAll<SupportedLanguagesEnum>()
            .FirstOrDefault(x => x.CultureValue == recogniseLanguageResult.Data);
        if(language is null)
            return Result<bool>.Error(ErrorTypesEnums.Validation, "Language is not supported").LogErrorMessage(_logger);
        ytVideoWav.SetLanguage(language);
        await _messagePublisher.Send(new LanguageRecognised(ytVideoWav.Id, ytVideoWav.Id));
        await _unitOfWork.SaveChangesAsync(token);
        return Result<bool>.Success(true);
    }
}