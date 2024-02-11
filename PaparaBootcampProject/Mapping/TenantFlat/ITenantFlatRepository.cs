using PaparaApp.Project.API.Models.Flats;
using PaparaApp.Project.API.Models.Users.Tenants;
using PaparaApp.Project.API.Models.Users;
using PaparaApp.Project.API.Mapping.TenantFlat.Dtos;

namespace PaparaApp.Project.API.Mapping.TenantFlat
{
    public interface ITenantFlatRepository
    {
        TenantFlat? GetTenantFlatById(Guid id);
        List<TenantFlat> GetAll();
        Flat? GetCurrentFlatByFlatId(Guid flatId);
        AppUser? GetCurrentTenantByTenantId(Guid tenantId);
        List<TenantFlat> GetTenantFlatHistoryByFlatId(Guid flatId);
        List<TenantFlat> GetTenantFlatHistoryByTenantId(Guid tenantId);

        bool IsTenantAssignedToFlat(Guid tenantId);
        TenantFlat Add(TenantFlat tenantFlat);
        TenantFlat Update(TenantFlat tenantFlat);
        void Delete(Guid id);

    }
}
