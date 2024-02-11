using PaparaApp.Project.API.Models.Flats.DTOs;
using PaparaApp.Project.API.Models.Shared;

namespace PaparaApp.Project.API.Models.Flats
{
    public interface IFlatService
    {
        List<FlatDto> GetAll();
        ResponseDto<Guid> Add(FlatAddRequestDto flatAddRequestDto);
        ResponseDto<Guid> Update(FlatUpdateRequestDto flatUpdateRequestDto);
        FlatDto GetById(Guid id);
        ResponseDto<Guid> Delete(Guid id);

    }
}
