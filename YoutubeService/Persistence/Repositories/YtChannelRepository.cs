using Domain.Dtos;
using Domain.Dtos.YtChannel;
using Domain.Entities;
using Domain.EntityIds;
using Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using Persistence.Context;
using Persistence.Specifications.Base;
using Persistence.Specifications.YtChannel;

namespace Persistence.Repositories;

public sealed class YtChannelRepository : IYtChannelRepository
{
    private readonly IAppDbContext _context;

    public YtChannelRepository(IAppDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(YtChannel ytChannel, CancellationToken token) =>
        await _context.Set<YtChannel>().AddAsync(ytChannel, token);

    public async Task Add(YtChannel ytChannel, CancellationToken token) =>
        await _context.Set<YtChannel>().AddAsync(ytChannel, token);

    public async Task<YtChannel> GetWithVideos(YtChannelId ytChannel, int amount, CancellationToken token) =>
        await _context.Set<YtChannel>()
            .Include(x => x.Videos)
            .FirstOrDefaultAsync(x => x.Id == ytChannel, token);

    public async Task<IEnumerable<YtChannelId>> GetAllIds(CancellationToken token) =>
        await _context.Set<YtChannel>().Select(x => x.Id).ToListAsync(token);

    public async Task<bool> Exists(string name, CancellationToken token) =>
        await _context.Set<YtChannel>().AnyAsync(x => x.Name == name, token);

    public async Task<YtChannelVideosDto> GetYtVideoChannelWithDownloadedVideoNames(YtChannelId ytChannelId,
        CancellationToken token)
        => await _context.Set<YtChannel>()
            .ApplySelectedSpecification(new GetWithVideoNamesSelectedSpecification(ytChannelId))
            .FirstOrDefaultAsync(token);
}