using PaparaApp.Project.API.Enums;

namespace PaparaApp.Project.API.Models.UserTimelyPaymentDetails
{
    public interface IUserPaymentDetailRepository
    {
        void UpdateUserPaymentFrequency(Guid tenantId, PaymentType paymentType,PaymentStatus paymentStatus);

        UserTimelyPaymentDetail? GetUserPaymentDetailRepository(Guid tenantId);

    }
}
