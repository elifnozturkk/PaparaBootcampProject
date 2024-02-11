using Azure;
using PaparaApp.Project.API.Mapping.TenantFlat.Dtos;
using PaparaApp.Project.API.Models.Shared;

namespace PaparaApp.Project.API.Mapping.TenantFlat
{
    public interface ITenantFlatService
    {
        TenantFlatDto GetTenantFlat(Guid id);
        List<TenantFlatDto> GetAll();
        List<TenantFlatDto> GetTenantFlatHistoryByFlatId(Guid flatId);
        List<TenantFlatDto> GetTenantFlatHistoryByTenantId(Guid tenantId);
        ResponseDto<Guid?> AssignTenantToFlat(TenantFlatAddRequestDto request);
        ResponseDto<Guid?> UnassignTenantFromFlat(TenantFlatUpdateRequestDto request);

        

    }
}
