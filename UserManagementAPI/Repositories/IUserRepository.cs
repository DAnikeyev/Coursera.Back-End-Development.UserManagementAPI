using UserManagementAPI.Contracts;
using UserManagementAPI.Models;

namespace UserManagementAPI.Repositories;

public interface IUserRepository
{
    Task<IReadOnlyCollection<User>> GetAllAsync(CancellationToken cancellationToken);

    Task<User?> GetByIdAsync(Guid id, CancellationToken cancellationToken);

    Task<User> AddAsync(CreateUserRequest request, CancellationToken cancellationToken);

    Task<User?> UpdateAsync(Guid id, UpdateUserRequest request, CancellationToken cancellationToken);

    Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken);
}
