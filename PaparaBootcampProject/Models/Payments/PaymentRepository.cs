
using PaparaApp.Project.API.Enums;
using PaparaApp.Project.API.Models.Flats;
using System;
using static System.Net.Mime.MediaTypeNames;

namespace PaparaApp.Project.API.Models.Payments
{
    public class PaymentRepository(AppDbContext dbContext) : IPaymentRepository
    {
        private readonly AppDbContext _dbContext = dbContext;
        public Payment AddDueAll(Payment payment)
        {
            _dbContext.Payments.Add(payment);
            return payment;
        }

        public Payment AddDueById(Payment payment)
        {
            _dbContext.Payments.Add(payment);
            return payment;
        }


        public Payment AddInvoiceById(Payment payment)
        {
            _dbContext.Payments.Add(payment);
            return payment;

        }

        public IEnumerable<Payment> GetDebt(Guid tenantId, IEnumerable<PaymentStatus> paymentStatuses)
        {
            return _dbContext.Payments.Where(p => p.TenantId == tenantId && paymentStatuses.Contains(p.PaymentStatus));
        }

        public IEnumerable<Payment> GetDebt(Guid tenantId, int year, IEnumerable<PaymentStatus> paymentStatuses)
        {
            return _dbContext.Payments.Where(p => p.TenantId == tenantId && p.Year == year && paymentStatuses.Contains(p.PaymentStatus));
        }

        public IEnumerable<Payment> GetDebt(Guid tenantId, int year, PaymentType paymentType, IEnumerable<PaymentStatus> paymentStatuses)
        {
            return _dbContext.Payments.Where(p => p.TenantId == tenantId && p.Year == year && p.PaymentType == paymentType && paymentStatuses.Contains(p.PaymentStatus));
        }

        public IEnumerable<Payment> GetDebt(Guid tenantId, int year, int month, IEnumerable<PaymentStatus> paymentStatuses)
        {
            return _dbContext.Payments.Where(p => p.TenantId == tenantId && p.Year == year && p.Month == month && paymentStatuses.Contains(p.PaymentStatus));
        }

        public IEnumerable<Payment> GetDebt(Guid tenantId, int year, int month, PaymentType paymentType, IEnumerable<PaymentStatus> paymentStatuses)
        {
            return _dbContext.Payments.Where(p => p.TenantId == tenantId && p.Year == year && p.Month == month && p.PaymentType == paymentType && paymentStatuses.Contains(p.PaymentStatus)) ;
        }

        public IEnumerable<Payment> GetDebtOfBlock(Guid flatId, int year, int month, IEnumerable<PaymentStatus> paymentStatuses)
        {
            return _dbContext.Payments.Where(p => p.FlatId == flatId && p.Year == year && p.Month == month && paymentStatuses.Contains(p.PaymentStatus));
        }

        public IEnumerable<Payment> GetDebtOfBlock(Guid flatId, int year, IEnumerable<PaymentStatus> paymentStatuses)
        {
            return _dbContext.Payments.Where(p => p.FlatId == flatId && p.Year == year && paymentStatuses.Contains(p.PaymentStatus));
        }

        public bool IsPaymentExist(Guid tenantId, int year, int month, PaymentType paymentType)
        {
            return _dbContext.Payments.Any(p => p.TenantId == tenantId && p.Year == year && p.Month == month && p.PaymentType == paymentType);
        }

        public Payment UpdatePayment(Payment payment)
        {
            _dbContext.Payments.Update(payment);
            return payment;
        }


    }
}
