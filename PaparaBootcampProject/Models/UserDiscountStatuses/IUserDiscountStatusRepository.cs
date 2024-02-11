namespace PaparaApp.Project.API.Models.UserDiscountStatuses
{
    public interface IUserDiscountStatusRepository
    {
        UserDiscountStatus AddDiscountStatus(UserDiscountStatus userDiscountStatus); 

        List<UserDiscountStatus> GetUserDiscountStatusById(Guid id, int year);

        void DeactivateDiscounts(Guid id);


    }
}
