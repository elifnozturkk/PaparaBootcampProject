using PaparaApp.Project.API.Enums;
using PaparaApp.Project.API.Models.UnitOfWorks;

namespace PaparaApp.Project.API.Models.UserTimelyPaymentDetails
{
    public class UserPaymentDetailRepository(AppDbContext dbContext, IUnitOfWork unitOfWork) : IUserPaymentDetailRepository
    {
        private readonly AppDbContext _dbContext = dbContext;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public UserTimelyPaymentDetail? GetUserPaymentDetailRepository(Guid tenantId)
        {
            return _dbContext.UserTimelyPaymentDetails.FirstOrDefault(x => x.TenantId == tenantId);
        }


        public void UpdateUserPaymentFrequency(Guid tenantId, PaymentType paymentType, PaymentStatus paymentStatus)
        {
            var userPaymentDetail = _dbContext.UserTimelyPaymentDetails.FirstOrDefault(x => x.TenantId == tenantId );
            if (userPaymentDetail == null)
            {
                userPaymentDetail = new UserTimelyPaymentDetail
                {
                    Id = Guid.NewGuid(),
                    TenantId = tenantId,
                };
                _dbContext.UserTimelyPaymentDetails.Add(userPaymentDetail);
            }

            if (paymentStatus == PaymentStatus.Paid)
            {
                switch (paymentType)
                {
                    case PaymentType.Water:
                        userPaymentDetail.TimelyPaidWater++;
                        break;
                    case PaymentType.Electricity:
                        userPaymentDetail.TimelyPaidElectricity++;
                        break;
                    case PaymentType.NaturalGas:
                        userPaymentDetail.TimelyPaidGas++;
                        break;
                    case PaymentType.Dues:
                        userPaymentDetail.TimelyPaidDue++;
                        break;
                }
            }
            if (paymentStatus == PaymentStatus.Overdue)
            {
                switch (paymentType)
                {
                    case PaymentType.Water:
                        userPaymentDetail.TimelyPaidWater = 0;
                        break;
                    case PaymentType.Electricity:
                        userPaymentDetail.TimelyPaidElectricity = 0;
                        break;
                    case PaymentType.NaturalGas:
                        userPaymentDetail.TimelyPaidGas = 0;
                        break;
                    case PaymentType.Dues:
                        userPaymentDetail.TimelyPaidDue = 0;
                        break;
                }
            }


            _dbContext.SaveChanges();
        }
    }
}
