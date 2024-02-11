namespace PaparaApp.Project.API.Models.UserDiscountStatuses
{
    public interface IUserDiscountStatusService
    {
        UserDiscountStatus AddDiscountStatus(UserDiscountStatus userDiscountStatus);
        List<UserDiscountStatus> GetUserDiscountStatusById(Guid id);
        UserDiscountStatus DeactivateDiscounts(UserDiscountStatus userDiscountStatus);
    }
}
