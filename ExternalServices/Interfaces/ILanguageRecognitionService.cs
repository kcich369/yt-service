using System.Threading;
using System.Threading.Tasks;
using Domain.Results;

namespace ExternalServices.Interfaces;

public interface ILanguageRecognitionService
{
    Task<IResult<string>> FromWavFile(string path, CancellationToken token);

}