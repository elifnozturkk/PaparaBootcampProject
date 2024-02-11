
using Microsoft.EntityFrameworkCore;
using PaparaApp.Project.API.Models;
using PaparaApp.Project.API.Models.Flats;
using PaparaApp.Project.API.Models.Users.Tenants;
using PaparaApp.Project.API.Models.Users;
using PaparaApp.Project.API.Mapping.TenantFlat.Dtos;

namespace PaparaApp.Project.API.Mapping.TenantFlat
{
    public class TenantFlatRepositrory(AppDbContext dbContext) : ITenantFlatRepository
    {
        private readonly AppDbContext _dbContext = dbContext;
        public TenantFlat Add(TenantFlat tenantFlat)
        {
            _dbContext.TenantFlats.Add(tenantFlat);
            return tenantFlat;
        }

        public void Delete(Guid id) => _dbContext.TenantFlats.Remove(_dbContext.TenantFlats.First(tf => tf.Id == id));


        public List<TenantFlat> GetAll() => _dbContext.TenantFlats.ToList();

        public Flat? GetCurrentFlatByFlatId(Guid flatId) => _dbContext.Flats.FirstOrDefault(tf => tf.Id == flatId);


        public AppUser? GetCurrentTenantByTenantId(Guid tenantId) => _dbContext.Users.FirstOrDefault(tf => tf.Id == tenantId);

        public List<TenantFlat> GetStartDate(Guid tenantId)
        {
            throw new NotImplementedException();
        }

        public List<TenantFlatInfo> GetStartDateAndTenantId()
        {
            var tenantFlats = _dbContext.TenantFlats
                    .Select(tf => new TenantFlatInfo { StartDate = tf.StartDate, TenantId = tf.TenantId })
                    .ToList();
            return tenantFlats;
        }


        public TenantFlat? GetTenantFlatById(Guid id) => _dbContext.TenantFlats.FirstOrDefault(tf => tf.Id == id);



        public List<TenantFlat> GetTenantFlatHistoryByFlatId(Guid flatId)
        {
           return  _dbContext.TenantFlats.Where(tf => tf.FlatId == flatId)
               .OrderByDescending(tf => tf.StartDate).ToList();
        }

        public List<TenantFlat> GetTenantFlatHistoryByTenantId(Guid tenantId)
        {
                return _dbContext.TenantFlats.Where(tf => tf.TenantId == tenantId)
                    .OrderByDescending(tf => tf.StartDate).ToList();
        }

        public bool IsTenantAssignedToFlat(Guid tenantId)
        {
            var isAssigned = _dbContext.TenantFlats
                .Include(tf => tf.Flat) 
                .Any(tf => tf.TenantId == tenantId && tf.Flat.Status == true); 

            return isAssigned;
        }


        public TenantFlat Update(TenantFlat tenantFlat)
        {
           _dbContext.TenantFlats.Update(tenantFlat);
            return tenantFlat;
        }
    }
}
