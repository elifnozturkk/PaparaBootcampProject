using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PaparaApp.Project.API.Models.Shared;
using PaparaApp.Project.API.Models.Users.Tenants;
using PaparaApp.Project.API.Models.Tokens;
using PaparaApp.Project.API.Models.Tokens.DTOs;
using PaparaApp.Project.API.Models.Users.ApartmanManagers.DTOs;
using PaparaApp.Project.API.Models.Users.Tenants.DTOs;
using System.Net;

namespace PaparaApp.Project.API.Models.Users
{
    public class AppUserService(
        UserManager<AppUser> userManager,
        SignInManager<AppUser> signInManager,
        RoleManager<AppRole> roleManager,
        TokenService tokenService) 
        : IAppUserService
    {
        public UserManager<AppUser> UserManager { get; set; } = userManager;
        public SignInManager<AppUser> SignInManager { get; set; } = signInManager;
        public RoleManager<AppRole> RoleManager { get; set; } = roleManager;
        public TokenService TokenService { get; set; } = tokenService;

        public async Task<ResponseDto<Guid?>> CreateUser(TenantCreateRequestDto request)
        {
            var existingUser = await UserManager.FindByNameAsync(request.NationalityId);
            if (existingUser != null)
            {
                return ResponseDto<Guid?>.Fail(new List<string> { "A user with the given nationality id already exists." }, HttpStatusCode.BadRequest);
            }

            var tenantUser = new TenantUser
            {
                UserName = request.NationalityId,
                Name = request.Name,
                Surname = request.Surname,
                Email = request.Email,
                PhoneNumber = request.PhoneNumber,
                NationalityId = request.NationalityId
            };

            var result = await UserManager.CreateAsync(tenantUser);
            if (!result.Succeeded)
            {
                var errors = result.Errors.Select(e => e.Description).ToList();
                return ResponseDto<Guid?>.Fail(errors);
            }

            const string tenantRoleName = "Tenant";

            var isRoleExist = await RoleManager.RoleExistsAsync(tenantRoleName);
            if (!isRoleExist)
            {
                var roleResult = await RoleManager.CreateAsync(new AppRole { Name = tenantRoleName });
                if (!roleResult.Succeeded)
                {
                    var errors = roleResult.Errors.Select(x => x.Description).ToList();
                    return ResponseDto<Guid?>.Fail(errors);
                }
            }
            var roleAssignResult = await userManager.AddToRoleAsync(tenantUser, tenantRoleName);
            if (!roleAssignResult.Succeeded)
            {
                var errors = roleAssignResult.Errors.Select(x => x.Description).ToList();
                return ResponseDto<Guid?>.Fail(errors);
            }

            return ResponseDto<Guid?>.Success(tenantUser.Id);
        }


        public async Task<ResponseDto<TokenCreateResponseDto>> LoginTenantUser(TenantLoginRequestDto request)
        {
            var tenantUser = await userManager.Users
                .FirstOrDefaultAsync(u => u.PhoneNumber == request.PhoneNumber && u.UserName == request.NationalityId);

            if (tenantUser == null)
            {
                return ResponseDto<TokenCreateResponseDto>.Fail("National ID or phone number is wrong.", HttpStatusCode.BadRequest);
            }

            return await tokenService.GenerateJwtTokenForTenant(tenantUser);
        }


        public async Task<ResponseDto<TokenCreateResponseDto>> LoginManagerUser(ApartmanManagerLoginRequestDto request)
        {
            var apartmanManagerUser = await userManager.FindByNameAsync(request.Username);

            if (apartmanManagerUser == null || !await userManager.CheckPasswordAsync(apartmanManagerUser, request.Password))
            {
                return ResponseDto<TokenCreateResponseDto>.Fail("Username or password is wrong.", HttpStatusCode.BadRequest);
            }

            return await tokenService.GenerateJwtTokenForApartmanManager(apartmanManagerUser);
        }

    }
}
