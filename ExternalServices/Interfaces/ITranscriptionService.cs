using System.Threading;
using System.Threading.Tasks;
using Domain.Results;

namespace ExternalServices.Interfaces;

public interface ITranscriptionService
{
    Task<IResult<string>> TranscribeFromWavFile(string path, string language, CancellationToken token);
}