using Domain.Entities;
using Domain.EntityIds;
using Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using Persistence.Context;

namespace Persistence.Repositories;

public class YtVideoTranscriptionRepository : IYtVideoTranscriptionRepository
{
    private readonly IAppDbContext _context;

    public YtVideoTranscriptionRepository(IAppDbContext context)
    {
        _context = context;
    }

    public async Task<YtVideoTranscription> GetToVideoDescription(YtVideoTranscriptionId ytVideoTranscriptionId,
        CancellationToken token) => await _context
        .Set<YtVideoTranscription>()
        .Include(x => x.Description)
        .FirstOrDefaultAsync(x => x.Id == ytVideoTranscriptionId, token);
}