namespace UserManagementAPI.Models;

public sealed record User(
    Guid Id,
    string FirstName,
    string LastName,
    string Email,
    string Department,
    bool IsActive,
    DateTime CreatedAtUtc);
