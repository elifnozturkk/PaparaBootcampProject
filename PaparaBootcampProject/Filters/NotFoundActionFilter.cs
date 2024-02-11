using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using PaparaApp.Project.API.Models.Flats;
using PaparaApp.Project.API.Models.Flats.DTOs;

namespace PaparaApp.Project.API.Filters
{
    public class NotFoundActionFilter(IFlatRepository flatRepository) : Attribute, IActionFilter
    {
        private readonly IFlatRepository _flatRepository = flatRepository;
        public void OnActionExecuted(ActionExecutedContext context)
        {
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            var requestDtoArgument = context.ActionArguments.FirstOrDefault(x => x.Value is FlatUpdateRequestDto).Value;

            if (requestDtoArgument is null) return;

            FlatUpdateRequestDto? requestDto = requestDtoArgument as FlatUpdateRequestDto;
            if (requestDto is null) return;


            if (!Guid.TryParse(requestDto.Id.ToString(), out var id)) return;

            var hasFlat = _flatRepository.GetById(id);

            if (hasFlat is null) context.Result = new NotFoundObjectResult($"Flat not found with id {id}");

        }
    }
}