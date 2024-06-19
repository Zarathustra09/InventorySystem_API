using System.ComponentModel.DataAnnotations;

namespace InventorySystem_API.Models
{
  public class RegisterDto
  {
    [Required]
    [StringLength(50)]
    public string Username { get; set; }

    [Required]
    [StringLength(255)]
    public string Password { get; set; }

    [Required]
    [Compare("Password", ErrorMessage = "Passwords do not match.")]
    public string ConfirmPassword { get; set; }
  }
}
