using Domain.Entities;
using Domain.EntityIds;
using Domain.Enumerations;
using Domain.Helpers;
using Domain.Providers;
using Domain.Repositories;
using Domain.Results;
using Domain.Services;
using Domain.UnitOfWork;
using Hangfire;
using ServiceBus.Producer.Messages;
using ServiceBus.Producer.Publisher;

namespace Infrastructure.Services;

[DisableConcurrentExecution(timeoutInSeconds: 60)]
public sealed class ConvertVideoFileToWavService : IConvertVideoFileToWavService
{
    private readonly IYtVideoFileRepository _ytVideoFileRepository;
    private readonly IConvertFileToWavHelper _convertFileToWavHelper;
    private readonly IPathProvider _pathProvider;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMessagePublisher _messagePublisher;

    public ConvertVideoFileToWavService(IYtVideoFileRepository ytVideoFileRepository,
        IConvertFileToWavHelper convertFileToWavHelper,
        IPathProvider pathProvider,
        IUnitOfWork unitOfWork,
        IMessagePublisher messagePublisher)
    {
        _ytVideoFileRepository = ytVideoFileRepository;
        _convertFileToWavHelper = convertFileToWavHelper;
        _pathProvider = pathProvider;
        _unitOfWork = unitOfWork;
        _messagePublisher = messagePublisher;
    }

    public async Task<IResult<bool>> Convert(YtVideoFileId ytVideoFileId, CancellationToken token)
    {
        var ytVideoFile = await _ytVideoFileRepository.GetById(ytVideoFileId, token);
        if (ytVideoFile == null)
            return Result<bool>.Success(true);
        if (ytVideoFile.Quality != VideoQualityEnum.High.Name)
            return Result<bool>.Error(ErrorTypesEnums.Validation,
                "Given yt video file contains improper quality value.");

        var convertingResult = await _convertFileToWavHelper.ConvertFileToWav(ytVideoFile.PathData.ToString(),
            ytVideoFile.PathData.ToString(), token);
        if (convertingResult.IsError) //todo: log errors
            return Result<bool>.Error(convertingResult);

        ytVideoFile.AddWavFile(YtVideoFileWav.Create(ytVideoFile.PathData.MainPath)
            .SetFileName(ytVideoFile.PathData.FileName));
        await _unitOfWork.SaveChangesAsync(token);
        await _messagePublisher.Send(new VideoConverted(ytVideoFile.WavFile.Id, ytVideoFile.Id));
        return Result<bool>.Success(true);
    }
}