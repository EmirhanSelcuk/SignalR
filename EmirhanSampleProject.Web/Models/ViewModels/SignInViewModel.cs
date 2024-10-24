using System.ComponentModel.DataAnnotations;

namespace EmirhanSampleProject.Web.Models.ViewModels
{
    public record SignInViewModel([Required] string Email,[Required] string Password)    
    {
    }
}
