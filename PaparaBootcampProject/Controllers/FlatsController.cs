using Azure.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PaparaApp.Project.API.Filters;
using PaparaApp.Project.API.Models.Flats;
using PaparaApp.Project.API.Models.Flats.DTOs;
using System.Data;

namespace PaparaApp.Project.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FlatsController(IFlatService flatService) : ControllerBase
    {
        private readonly IFlatService _flatService = flatService;

        [HttpPost]
        [Route("add")]
        [Authorize (Roles = "Manager")]
        public IActionResult Add(FlatAddRequestDto flatAddRequestDto)
        {
            var response = _flatService.Add(flatAddRequestDto);
            if (response.Errors != null && response.Errors.Any())
            {
                return StatusCode((int)response.StatusCode, response);
            }
            return Created("", response);
        }


        [HttpPut]
        [Route("update")]
        [Authorize(Roles = "Manager")]
        [ServiceFilter(typeof(NotFoundActionFilter))]
        public IActionResult Update(FlatUpdateRequestDto flatUpdateRequestDto)
        {

            var response = _flatService.Update(flatUpdateRequestDto);
            if (response.Errors != null && response.Errors.Any())
            {
                return StatusCode((int)response.StatusCode, response);
            }
            return Created("", response);
        }


        [HttpDelete("{id}")]
        [Route("delete")]
        [Authorize(Roles = "Manager")]
        public IActionResult Delete(Guid id)
        {
            var response = _flatService.Delete(id);
            if (response.Errors != null && response.Errors.Any())
            {
                return StatusCode((int)response.StatusCode, response);
            }
            return Ok();
        }
    }
}
