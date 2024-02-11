

namespace PaparaApp.Project.API.Models.UserDiscountStatuses
{
    public class UserDiscountStatusRepository(AppDbContext dbcontext) : IUserDiscountStatusRepository
    {
        private readonly AppDbContext _dbContext = dbcontext;

        public UserDiscountStatus AddDiscountStatus(UserDiscountStatus userDiscountStatus)
        {
           _dbContext.UserDiscountStatuses.Add(userDiscountStatus);
            return userDiscountStatus;
        }

        public void DeactivateDiscounts(Guid tenantId)
        {
            var activeDiscounts = _dbContext.UserDiscountStatuses
                                            .Where(uds => uds.TenantId == tenantId && uds.IsDiscountActive);
            foreach (var discount in activeDiscounts)
            {
                discount.IsDiscountActive = false;
            }
        }


        public List<UserDiscountStatus> GetUserDiscountStatusById(Guid id, int year)
        {
             return _dbContext.UserDiscountStatuses.Where(x => x.TenantId == id && x.DiscountStartYear == year).ToList();
        }

    }
}
