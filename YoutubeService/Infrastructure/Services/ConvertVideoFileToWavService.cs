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
using Infrastructure.Services.Base;
using ServiceBus.Producer.Messages;
using ServiceBus.Producer.Publisher;

namespace Infrastructure.Services;

[DisableConcurrentExecution(timeoutInSeconds: 60)]
public sealed class ConvertVideoFileToWavService : MessagePublisherService<VideoConverted>,
    IConvertVideoFileToWavService
{
    private readonly IYtVideoFileRepository _ytVideoFileRepository;
    private readonly IConvertFileToWavHelper _convertFileToWavHelper;
    private readonly IPathProvider _pathProvider;
    private readonly IUnitOfWork _unitOfWork;

    public ConvertVideoFileToWavService(IYtVideoFileRepository ytVideoFileRepository,
        IConvertFileToWavHelper convertFileToWavHelper,
        IPathProvider pathProvider,
        IUnitOfWork unitOfWork,
        IMessagePublisher publisher) : base(publisher)
    {
        _ytVideoFileRepository = ytVideoFileRepository;
        _convertFileToWavHelper = convertFileToWavHelper;
        _pathProvider = pathProvider;
        _unitOfWork = unitOfWork;
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

        ytVideoFile.AddWavFile(YtVideoFileWav.Create(ytVideoFile.PathData.MainPath, ytVideoFile.PathData.DirectoryName)
            .SetFileName(ytVideoFile.PathData.FileName));
        await _unitOfWork.SaveChangesAsync(token);
        await Publish(new VideoConverted(ytVideoFile.WavFile.Id));
        return Result<bool>.Success(true);
    }
}