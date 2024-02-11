using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PaparaApp.Project.API.Mapping.TenantFlat;
using PaparaApp.Project.API.Mapping.TenantFlat.Dtos;
using PaparaApp.Project.API.Models.Users;
using PaparaApp.Project.API.Models.Users.ApartmanManagers.DTOs;
using PaparaApp.Project.API.Models.Users.Tenants.DTOs;

namespace PaparaApp.Project.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController(IAppUserService appUserService, ITenantFlatService tenantFlatService) : ControllerBase
    {
        private readonly IAppUserService _appUserService = appUserService;
        private readonly ITenantFlatService _tenantFlatService = tenantFlatService;


        [HttpPost]
        [Route("manager-login")]
        public async Task<IActionResult> ManagerLogin(ApartmanManagerLoginRequestDto request)
        {
            var response = await _appUserService.LoginManagerUser(request);
            if (response.Errors != null && response.Errors.Any())
            {
                return StatusCode((int)response.StatusCode, response);
            }
            return Created("", response);
        }

        [HttpPost]
        [Route("create-tenant")]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> CreateTenantUser(TenantCreateRequestDto request)
        {
            var response = await _appUserService.CreateUser(request);
            if (response.Errors != null && response.Errors.Any())
            {
                return StatusCode((int)response.StatusCode, response);
            }
            return Created("", response);
        }

        [HttpPost("tenant-login")]
        public async Task<IActionResult> TenantLogin(TenantLoginRequestDto request)
        {
            var response = await _appUserService.LoginTenantUser(request);
            if (response.Errors != null && response.Errors.Any())
            {
                return StatusCode((int)response.StatusCode, response);
            }
            return Created("", response);
        }


        [HttpPost]
        [Route("assign-flat")]
        [Authorize(Roles = "Manager")]
        public IActionResult AssignTenant(TenantFlatAddRequestDto request)
        {
            var response = _tenantFlatService.AssignTenantToFlat(request);
            if (response.Errors != null && response.Errors.Any())
            {
                return StatusCode((int)response.StatusCode, response);
            }
            return Created("", response);

        }

        [HttpPut]
        [Route("unassign-flat")]
        [Authorize(Roles = "Manager")]
        public IActionResult UpdateTenantFlat(TenantFlatUpdateRequestDto request)
        {
            var response = _tenantFlatService.UnassignTenantFromFlat(request);
            if (response.Errors != null && response.Errors.Any())
            {
                return StatusCode((int)response.StatusCode, response);
            }
            return Created("", response);
        }


    }
}
