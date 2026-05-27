using System.ComponentModel.DataAnnotations;
using UserManagementAPI.Validation;

namespace UserManagementAPI.Contracts;

public sealed class CreateUserRequest
{
    [Required]
    [StringLength(50)]
    [NotWhiteSpace(ErrorMessage = "First name is required.")]
    public string FirstName { get; init; } = string.Empty;

    [Required]
    [StringLength(50)]
    [NotWhiteSpace(ErrorMessage = "Last name is required.")]
    public string LastName { get; init; } = string.Empty;

    [Required]
    [EmailAddress]
    [StringLength(100)]
    public string Email { get; init; } = string.Empty;

    [Required]
    [StringLength(100)]
    [NotWhiteSpace(ErrorMessage = "Department is required.")]
    public string Department { get; init; } = string.Empty;

    public bool IsActive { get; init; } = true;
}
