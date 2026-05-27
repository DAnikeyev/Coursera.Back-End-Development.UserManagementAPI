using System.Collections.Concurrent;
using UserManagementAPI.Contracts;
using UserManagementAPI.Models;

namespace UserManagementAPI.Repositories;

public sealed class InMemoryUserRepository : IUserRepository
{
    private readonly ConcurrentDictionary<Guid, User> _users = new();

    public InMemoryUserRepository()
    {
        SeedUsers().ToList().ForEach(user => _users[user.Id] = user);
    }

    public Task<IReadOnlyCollection<User>> GetAllAsync(CancellationToken cancellationToken)
    {
        IReadOnlyCollection<User> users = _users.Values
            .OrderBy(user => user.LastName)
            .ThenBy(user => user.FirstName)
            .ToArray();

        return Task.FromResult(users);
    }

    public Task<User?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        _users.TryGetValue(id, out var user);
        return Task.FromResult(user);
    }

    public Task<User> AddAsync(CreateUserRequest request, CancellationToken cancellationToken)
    {
        var user = new User(
            Guid.NewGuid(),
            request.FirstName.Trim(),
            request.LastName.Trim(),
            request.Email.Trim(),
            request.Department.Trim(),
            request.IsActive,
            DateTime.UtcNow);

        _users[user.Id] = user;

        return Task.FromResult(user);
    }

    public Task<User?> UpdateAsync(Guid id, UpdateUserRequest request, CancellationToken cancellationToken)
    {
        if (!_users.TryGetValue(id, out var existingUser))
        {
            return Task.FromResult<User?>(null);
        }

        var updatedUser = existingUser with
        {
            FirstName = request.FirstName.Trim(),
            LastName = request.LastName.Trim(),
            Email = request.Email.Trim(),
            Department = request.Department.Trim(),
            IsActive = request.IsActive
        };

        _users[id] = updatedUser;

        return Task.FromResult<User?>(updatedUser);
    }

    public Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken)
    {
        var deleted = _users.TryRemove(id, out _);
        return Task.FromResult(deleted);
    }

    private static IEnumerable<User> SeedUsers()
    {
        yield return new User(
            Guid.Parse("b4358f6f-8c58-49f1-977b-1cb8af31ac10"),
            "Ava",
            "Patel",
            "ava.patel@techhive.local",
            "Human Resources",
            true,
            DateTime.UtcNow.AddDays(-10));

        yield return new User(
            Guid.Parse("68d9f10d-c926-485e-aa3d-6e776778fd7d"),
            "Marcus",
            "Lee",
            "marcus.lee@techhive.local",
            "IT Operations",
            true,
            DateTime.UtcNow.AddDays(-7));
    }
}
