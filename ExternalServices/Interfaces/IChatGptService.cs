using System.Threading;
using System.Threading.Tasks;
using Domain.Results;

namespace ExternalServices.Interfaces;

public interface IChatGptService
{
    Task<IResult<string>> Ask(string message, CancellationToken token);
}