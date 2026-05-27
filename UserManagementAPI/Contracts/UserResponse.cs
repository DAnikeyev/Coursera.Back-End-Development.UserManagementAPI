using UserManagementAPI.Models;

namespace UserManagementAPI.Contracts;

public sealed record UserResponse(
    Guid Id,
    string FirstName,
    string LastName,
    string Email,
    string Department,
    bool IsActive,
    DateTime CreatedAtUtc)
{
    public static UserResponse FromModel(User user) =>
        new(
            user.Id,
            user.FirstName,
            user.LastName,
            user.Email,
            user.Department,
            user.IsActive,
            user.CreatedAtUtc);
}
