using System.Threading;
using System.Threading.Tasks;
using Domain.Results;
using ExternalServices.Dto;

namespace ExternalServices.Interfaces;

public interface IYtChannelService
{
    Task<IResult<YtChannelData>> Get(string ytChannelName, bool getByHandleName, CancellationToken token);

}