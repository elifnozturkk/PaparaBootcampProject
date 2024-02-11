using PaparaApp.Project.API.Models.Shared;
using PaparaApp.Project.API.Models.Tokens.DTOs;
using PaparaApp.Project.API.Models.Users.ApartmanManagers.DTOs;
using PaparaApp.Project.API.Models.Users.Tenants.DTOs;

namespace PaparaApp.Project.API.Models.Users
{
    public interface IAppUserService 
    {
        Task<ResponseDto<Guid?>> CreateUser(TenantCreateRequestDto request);

        Task<ResponseDto<TokenCreateResponseDto>> LoginTenantUser(TenantLoginRequestDto request);

        Task<ResponseDto<TokenCreateResponseDto>> LoginManagerUser(ApartmanManagerLoginRequestDto request);


    }
}
