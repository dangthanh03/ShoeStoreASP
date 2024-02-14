using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ShoeStoreASP.Models.Domain;
using ShoeStoreASP.Models.ViewModel;
using ShoeStoreASP.Service.Abstract;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ShoeStoreASP.Service.Implementation
{
    public class UserAuthenticate : IUserAuthenticate
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly ICartService cartService;

        public UserAuthenticate(UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager, RoleManager<IdentityRole> roleManager, ICartService cartService)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.signInManager = signInManager;
            this.cartService = cartService;
        }

        public async Task<Result<string>> RegisterAsync(RegistrationModel model)
        {
            var userExists = await userManager.FindByNameAsync(model.Username);
            if (userExists != null)
            {
                return Result<string>.Fail("User already exists");
            }

            ApplicationUser user = new ApplicationUser()
            {
                Email = model.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = model.Username,
                Name = model.Name,
                EmailConfirmed = true,
                PhoneNumberConfirmed = true,
            };

            var result = await userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
            {
                return Result<string>.Fail("User creation fail");
            }
            var cartResult = await cartService.CreateCartAsync(user.Id);
            if (!cartResult.IsSuccess)
            {
                // Xử lý lỗi tạo giỏ hàng
                return Result<string>.Fail($"Error creating cart: {cartResult.Message}");
            }
            if (!await roleManager.RoleExistsAsync(model.Role))
                await roleManager.CreateAsync(new IdentityRole(model.Role));

            if (await roleManager.RoleExistsAsync(model.Role))
            {
                await userManager.AddToRoleAsync(user, model.Role);
            }

            return Result<string>.Success("You have registered successfully");
        }

        public async Task<Result<string>> LoginAsync(LoginModel model)
        {
            var user = await userManager.FindByNameAsync(model.Username);
            if (user == null)
            {
                return Result<string>.Fail("Invalid username");
            }

            if (!await userManager.CheckPasswordAsync(user, model.Password))
            {
                return Result<string>.Fail("Invalid Password");
            }

            var signInResult = await signInManager.PasswordSignInAsync(user, model.Password, true, true);
            if (signInResult.Succeeded)
            {
                var userRoles = await userManager.GetRolesAsync(user);
                var authClaims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName),
            };

                foreach (var userRole in userRoles)
                {
                    authClaims.Add(new Claim(ClaimTypes.Role, userRole));
                }

                return Result<string>.Success("Logged in successfully");
            }
            else if (signInResult.IsLockedOut)
            {
                return Result<string>.Fail("User is locked out");
            }
            else
            {
                return Result<string>.Fail("Error on logging in");
            }
        }

        public async Task LogoutAsync()
        {
            await signInManager.SignOutAsync();
        }

        public async Task<List<string>> GetAvailableRoles()
        {
            var roles = await roleManager.Roles.ToListAsync();
            return roles.Select(r => r.Name).ToList();
        }
    }
}
