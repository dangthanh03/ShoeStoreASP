using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ShoeStoreASP.Models.Domain;
using ShoeStoreASP.Models.ViewModel;
using ShoeStoreASP.Service.Abstract;

namespace ShoeStoreASP.Controllers
{
    public class UserAuthenticateController : Controller
    {

        private readonly UserManager<ApplicationUser> userManager;
        private IUserAuthenticate authService;
        public UserAuthenticateController(IUserAuthenticate authService, UserManager<ApplicationUser> _userManager)
        {
            this.userManager = _userManager;
            this.authService = authService;
        }

          public async Task<IActionResult> Register11()
          {
              var model = new RegistrationModel{

                Email= "thanh@gmail.com",
                Username = "thanh",
                Name = "Jake",
                Password = "Thanh@123",
                PasswordConfirm = "Thanh@123",
                Role = "User"

              };

              var result = await authService.RegisterAsync(model);
              return Ok(result.Message);
          }
        public async Task<IActionResult> Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var result = await authService.LoginAsync(model);

            if (result.IsSuccess)
            {
                TempData["msg"] = "Login success";
                return RedirectToAction("Index", "Home");
            }
            else
            {
                TempData["msg"] = $"Could not log in: {result.Data}";
                return RedirectToAction(nameof(Login));
            }
        }

        public async Task<IActionResult> Logout()
        {
            await authService.LogoutAsync();
            return RedirectToAction(nameof(Login));
        }

        [HttpGet]
        public async Task<IActionResult> Register()
        {
            var registration = new RegistrationModel();
            registration.AvailableRoles = await authService.GetAvailableRoles();

            return View(registration);
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegistrationModel model)
        {
            if (!ModelState.IsValid)
            {
                model.AvailableRoles = await authService.GetAvailableRoles();
                return View(model);
            }

            var result = await authService.RegisterAsync(model);

            if (result.IsSuccess)
            {
                TempData["msg"] = "Register success";
                return RedirectToAction("Index", "Home");
            }
            else
            {
                model.AvailableRoles = await authService.GetAvailableRoles();
                TempData["msg"] = "Could not register...";
                return View(model);
            }
        }

        [HttpGet]
        public async Task<IActionResult> ChangePass()
        {
            var user = await userManager.GetUserAsync(User);

            // Check if the user is signed in
            if (user == null)
            {
                // Handle the case where the user is not signed in
                return RedirectToAction("Login"); // Redirect to your login action
            }

            var userProfileModel = new UserProfileViewModel
            {
                // Transfer necessary information from ApplicationUser to UserProfileModel
                Name = user.UserName,
                Email = user.Email
                // Add other properties as needed
            };

            return View(userProfileModel);
        }

        [HttpPost]
        public async Task<IActionResult> ChangePass(UserProfileViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Get the current user information
                var user = await userManager.GetUserAsync(User);

                if (user == null)
                {
                    // Handle the case where the user does not exist
                    return RedirectToAction("Login");
                }

                // Update username and email information
                user.UserName = model.Name;
                user.Email = model.Email;

                // Change the password if the user enters the current and new passwords
                if (!string.IsNullOrEmpty(model.CurrentPassword) && !string.IsNullOrEmpty(model.NewPassword))
                {
                    // Check the current password
                    var changePasswordResult = await userManager.ChangePasswordAsync(user, model.CurrentPassword, model.NewPassword);

                    if (!changePasswordResult.Succeeded)
                    {
                        // Handle the case where changing the password is unsuccessful
                        foreach (var error in changePasswordResult.Errors)
                        {
                            ModelState.AddModelError(string.Empty, error.Description);
                        }

                        TempData["ErrorMessage"] = "Failed to update";
                        return View(model);
                    }
                }

                // Save the changes
                var updateResult = await userManager.UpdateAsync(user);

                if (updateResult.Succeeded)
                {
                    // Handle the case where the update is successful
                    return RedirectToAction("Logout", "UserAuthentication"); // Redirect to the home page or a page indicating successful update
                }
                else
                {
                    // Handle the case where the update is unsuccessful
                    foreach (var error in updateResult.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }

                    TempData["ErrorMessage"] = "Failed to update";
                }
            }

            // Return to the view with the model if there is an error
            return View(model);
        }

    }
}
