using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using PaparaApp.Project.API.Models.Users.ApartmanManagers;
using PaparaApp.Project.API.Models.Shared;
using PaparaApp.Project.API.Models.Users.Tenants;
using PaparaApp.Project.API.Models.Tokens.DTOs;
using PaparaApp.Project.API.Models.Users;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace PaparaApp.Project.API.Models.Tokens
{
    public class TokenService(UserManager<AppUser> userManager, IConfiguration configuration) 
    {
        public async Task<ResponseDto<TokenCreateResponseDto>> GenerateJwtTokenForApartmanManager(AppUser apartmanManagerUser)
        {
            if (apartmanManagerUser == null)
            {
                return ResponseDto<TokenCreateResponseDto>.Fail("User not found with the given credentials.");
            }

            // Continue with the rest of the token generation process
            var signatureKey = configuration.GetSection("TokenOptions")["SignatureKey"]!;
            var tokenExpireAsHour = configuration.GetSection("TokenOptions")["Expire"]!;

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(signatureKey));
            SigningCredentials signingCredentials =
                new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);

            var claimList = new List<Claim>();
            var managerIdAsClaim = new Claim(ClaimTypes.NameIdentifier, apartmanManagerUser.Id.ToString());

            var userRoles = await userManager.GetRolesAsync(apartmanManagerUser);
            foreach (var role in userRoles)
            {
                claimList.Add(new Claim(ClaimTypes.Role, role));
            }
            var idAsClaim = new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString());
            claimList.Add(managerIdAsClaim);
            claimList.Add(idAsClaim);

            var token = new JwtSecurityToken(
                expires: DateTime.Now.AddHours(Convert.ToDouble(tokenExpireAsHour)),
                signingCredentials: signingCredentials,
                claims: claimList
            );

            var responseDto = new TokenCreateResponseDto
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token),
            };

            return ResponseDto<TokenCreateResponseDto>.Success(responseDto);
        }

        public async Task<ResponseDto<TokenCreateResponseDto>> GenerateJwtTokenForTenant(AppUser tenantUser)
        {
            if (tenantUser == null)
            {
                return ResponseDto<TokenCreateResponseDto>.Fail("User not found with the given credentials.");
            }

            // Continue with the rest of the token generation process
            var signatureKey = configuration.GetSection("TokenOptions")["SignatureKey"]!;
            var tokenExpireAsHour = configuration.GetSection("TokenOptions")["Expire"]!;

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(signatureKey));
            SigningCredentials signingCredentials =
                new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);

            var claimList = new List<Claim>();
            var userIdAsClaim = new Claim(ClaimTypes.NameIdentifier, tenantUser.Id.ToString());

            var userRoles = await userManager.GetRolesAsync(tenantUser);
            foreach (var role in userRoles)
            {
                claimList.Add(new Claim(ClaimTypes.Role, role));
            }

            var userPhoneAsClaim = new Claim(ClaimTypes.MobilePhone, tenantUser.PhoneNumber!);
            var idAsClaim = new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString());
            claimList.Add(userPhoneAsClaim);
            claimList.Add(idAsClaim);
            claimList.Add(userIdAsClaim);

            var token = new JwtSecurityToken(
                expires: DateTime.Now.AddHours(Convert.ToDouble(tokenExpireAsHour)),
                signingCredentials: signingCredentials,
                claims: claimList
            );

            var responseDto = new TokenCreateResponseDto
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token),
            };

            return ResponseDto<TokenCreateResponseDto>.Success(responseDto);
        }

    }
}
