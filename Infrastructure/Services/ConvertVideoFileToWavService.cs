using Domain.Entities;
using Domain.EntityIds;
using Domain.Enumerations;
using Domain.Repositories;
using Domain.Results;
using Domain.Services;
using Domain.UnitOfWork;
using Hangfire;
using Infrastructure.Dtos;
using Infrastructure.Extensions;
using Infrastructure.Helpers.Interfaces;
using Microsoft.Extensions.Logging;
using ServiceBus.Producer.Messages;
using ServiceBus.Producer.Publisher;

namespace Infrastructure.Services;

public sealed class ConvertVideoFileToWavService : IConvertVideoFileToWavService
{
    private readonly IYtVideoFileRepository _ytVideoFileRepository;
    private readonly IConvertFileToWavHelper _convertFileToWavHelper;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<ConvertVideoFileToWavService> _logger;
    private readonly IMessagePublisher _messagePublisher;

    public ConvertVideoFileToWavService(IYtVideoFileRepository ytVideoFileRepository,
        IConvertFileToWavHelper convertFileToWavHelper,
        IUnitOfWork unitOfWork,
        ILogger<ConvertVideoFileToWavService> logger,
        IMessagePublisher messagePublisher)
    {
        _ytVideoFileRepository = ytVideoFileRepository;
        _convertFileToWavHelper = convertFileToWavHelper;
        _unitOfWork = unitOfWork;
        _logger = logger;
        _messagePublisher = messagePublisher;
    }

    [DisableConcurrentExecution(timeoutInSeconds: 60)]
    public async Task<IResult<bool>> Convert(YtVideoFileId ytVideoFileId, CancellationToken token)
    {
        var ytVideoFile = await _ytVideoFileRepository.GetById(ytVideoFileId, token);
        if (ytVideoFile == null)
            return Result<bool>.Success(true);
        if (ytVideoFile.Quality != VideoQualityEnum.High)
            return Result<bool>.Error(ErrorTypesEnums.Validation,
                $"Given yt video file with id {ytVideoFileId} contains improper quality value.");

        var convertingResult = await _convertFileToWavHelper.ConvertFileToWav(new PathDataDto(ytVideoFile.PathData), token);
        if (convertingResult.IsError)
            return Result<bool>.Error(convertingResult).LogErrorMessage(_logger);

        ytVideoFile.AddWavFile(YtVideoFileWav.Create(ytVideoFile.PathData.MainPath)
            .SetFileName(ytVideoFile.PathData.FileName));
        await _unitOfWork.SaveChangesAsync(token);
        await _messagePublisher.Send(new VideoConverted(ytVideoFile.WavFile.Id, ytVideoFile.Id));
        return Result<bool>.Success(true);
    }
}