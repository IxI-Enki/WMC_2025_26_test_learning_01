using Application.Interfaces;
using Domain.Contracts;

namespace Application.Services;

/// <summary>
/// BookUniquenessChecker.
/// </summary>
public class AuthorUniquenessChecker( IUnitOfWork uow ) : IAuthorUniquenessChecker
{
    public async Task<bool> IsUniqueAsync( int id, string fullName, CancellationToken ct = default )
    {
        var existing = await uow.Authors.GetByFullName(fullName, ct);

        return existing is null || existing.Id == id;
    }
}
