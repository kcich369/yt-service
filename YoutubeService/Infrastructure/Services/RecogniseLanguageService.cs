﻿using Domain.EntityIds;
using Domain.Repositories;
using Domain.Results;
using Domain.Services;
using Domain.UnitOfWork;
using ExternalServices.Interfaces;
using Hangfire;
using Infrastructure.Services.Base;
using ServiceBus.Producer.Messages;
using ServiceBus.Producer.Publisher;

namespace Infrastructure.Services;

[DisableConcurrentExecution(timeoutInSeconds: 60)]
public sealed class RecogniseLanguageService : MessagePublisherService<LanguageRecognised>, IRecogniseLanguageService
{
    private readonly IYtVideoFileWavRepository _videoFileWavRepository;
    private readonly ISpeechToTextService _speechToTextService;
    private readonly IUnitOfWork _unitOfWork;

    public RecogniseLanguageService(IYtVideoFileWavRepository videoFileWavRepository,
        ISpeechToTextService speechToTextService,
        IUnitOfWork unitOfWork,
        IMessagePublisher publisher) : base(publisher)
    {
        _videoFileWavRepository = videoFileWavRepository;
        _speechToTextService = speechToTextService;
        _unitOfWork = unitOfWork;
    }

    public async Task<IResult<bool>> Recognise(YtVideoFileWavId videoFileWavId, CancellationToken token)
    {
        var ytVideoWav = await _videoFileWavRepository.GetToLanguageRecognition(videoFileWavId, token);
        if (ytVideoWav is null)
            return Result<bool>.Success(true);
        var recogniseLanguageResult =
            await _speechToTextService.RecogniseLanguageFromWavFile(ytVideoWav.PathData.FullValue, token);
        if (recogniseLanguageResult.IsError) //todo: log error
            return Result<bool>.Error(recogniseLanguageResult);

        ytVideoWav.SetLanguage(recogniseLanguageResult.Data);
        
        await Publish(new LanguageRecognised(ytVideoWav.Id));
        await _unitOfWork.SaveChangesAsync(token);
        return Result<bool>.Success(true);
    }
}