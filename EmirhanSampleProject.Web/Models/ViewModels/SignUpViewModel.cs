using System.ComponentModel.DataAnnotations;

namespace EmirhanSampleProject.Web.Models.ViewModels
{
    public record SignUpViewModel([Required]string Email, [Required] string Password, [Required] string ConfirmPassword);
    
}
