using EmirhanSampleProject.Web.Models;
using EmirhanSampleProject.Web.Models.ViewModels;
using EmirhanSampleProject.Web.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace EmirhanSampleProject.Web.Controllers
{
    public class HomeController(ILogger<HomeController> logger,UserManager<IdentityUser> userManager,
        SignInManager<IdentityUser> signInManager,AppDbContext context,FileService fileService) : Controller
    {
      

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }
        public IActionResult SignUp()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SignUp(SignUpViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var userToCreate = new IdentityUser()
            {
                UserName = model.Email,
                Email = model.Email
            };
            var result = await userManager.CreateAsync(userToCreate, model.Password);
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            return RedirectToAction(nameof(SignIn));
        }
        public IActionResult SignIn()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SignIn(SignInViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var hasUser = await userManager.FindByEmailAsync(model.Email);
            if(hasUser is null)
            {
                ModelState.AddModelError(string.Empty, "Email or Password wrong");
            }

            var result = await signInManager.PasswordSignInAsync(hasUser,model.Password, true,false);

            if (result.Succeeded)
            {
                ModelState.AddModelError(string.Empty, "Email or Password wrong");
            }

            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> ProductList()
        {
            var user = await userManager.FindByEmailAsync("emirhanselcuk73@gmail.com");

            if (context.Products.Any(x => x.UserId == user!.Id))
            {
                var products = context.Products.Where(x => x.UserId == user!.Id).ToList();
                return View(products);
            }
            var productList = new List<Product>()
            {
             new Product() { Name = "Phone 1", Description = "Test 1", Price = 100, UserId = user!.Id },
             new Product() { Name = "Phone 2", Description = "Test 2", Price = 200, UserId = user!.Id },
             new Product() { Name = "Phone 3", Description = "Test 3", Price = 300, UserId = user!.Id },
             new Product() { Name = "Phone 4", Description = "Test 4", Price = 400, UserId = user!.Id },
             new Product() { Name = "Phone 5", Description = "Test 5", Price = 500, UserId = user!.Id }

        };

            context.Products.AddRange(productList);
            await context.SaveChangesAsync();
           
       

            return View(productList);
        }


        [Authorize]
        [HttpGet]
        public async Task<IActionResult> CreateExcel()
        {
            var response = new
            {
                Status = await fileService.AddMessageToQueue()
            };
            return Json(response);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
