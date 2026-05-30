using Microsoft.EntityFrameworkCore;
using TNTTBD.Services.Application.Auth.Interfaces;
using TNTTBD.Services.Infrastructure.Persistence.Catechism;
using TNTTBD.Services.Infrastructure.Persistence.Catechism.Entities;

namespace TNTTBD.Services.Infrastructure.Auth;

public class ExternalLoginRepository : IExternalLoginRepository
{
    private readonly CatechismDbContext _dbContext;

    public ExternalLoginRepository(CatechismDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Guid?> FindUserIdByExternalLoginAsync(string provider, string subjectId, CancellationToken cancellationToken)
    {
        var login = await _dbContext.UserExternalLogins
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Provider == provider && x.SubjectId == subjectId, cancellationToken);

        return login?.UserId;
    }

    public async Task LinkExternalLoginAsync(Guid userId, string provider, string subjectId, CancellationToken cancellationToken)
    {
        var entity = new UserExternalLoginEntity
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            Provider = provider,
            SubjectId = subjectId,
            LinkedAt = DateTimeOffset.UtcNow
        };

        _dbContext.UserExternalLogins.Add(entity);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<List<string>> GetLinkedProvidersAsync(Guid userId, CancellationToken cancellationToken)
    {
        return await _dbContext.UserExternalLogins
            .AsNoTracking()
            .Where(x => x.UserId == userId)
            .Select(x => x.Provider)
            .ToListAsync(cancellationToken);
    }

    public async Task<bool> UnlinkExternalLoginAsync(Guid userId, string provider, CancellationToken cancellationToken)
    {
        var login = await _dbContext.UserExternalLogins
            .FirstOrDefaultAsync(x => x.UserId == userId && x.Provider == provider, cancellationToken);

        if (login == null)
        {
            return false;
        }

        _dbContext.UserExternalLogins.Remove(login);
        await _dbContext.SaveChangesAsync(cancellationToken);
        return true;
    }
}
