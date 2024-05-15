using System.ComponentModel.DataAnnotations;

namespace Application.Models.Identity;

public class RegistrationModel
{
    [Required(ErrorMessage = "User Name is required")]
    public required string Username { get; set; }

    [Required(ErrorMessage = "Password is required")]
    public required string Password { get; set; }

    [Required(ErrorMessage = "Firstname is required")]
    public required string Firstname { get; set; }

    [Required(ErrorMessage = "Lastname is required")]
    public required string Lastname { get; set; }

    [EmailAddress]
    [Required(ErrorMessage = "Email is required")]
    public required string Email { get; set; }
}