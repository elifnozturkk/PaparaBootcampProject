using PaparaApp.Project.API.Enums;
using System;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace PaparaApp.Project.API.Models.Payments
{
    public interface IPaymentRepository
    {
        Payment AddDueById(Payment payment); 
        Payment AddDueAll(Payment payment); 
        Payment AddInvoiceById(Payment payment); 

        IEnumerable<Payment> GetDebt(Guid tenantId, IEnumerable<PaymentStatus> paymentStatuses);
        IEnumerable<Payment> GetDebt(Guid tenantId, int year, IEnumerable<PaymentStatus> paymentStatuses);
        IEnumerable<Payment> GetDebt(Guid tenantId, int year, PaymentType paymentType, IEnumerable<PaymentStatus> paymentStatuses);
        IEnumerable<Payment> GetDebt(Guid tenantId, int year, int month, IEnumerable<PaymentStatus> paymentStatuses); 
        IEnumerable<Payment> GetDebt(Guid tenantId, int year, int month, PaymentType paymentType, IEnumerable<PaymentStatus> paymentStatuses); 

        IEnumerable<Payment> GetDebtOfBlock(Guid flatId, int year, int month, IEnumerable<PaymentStatus> paymentStatuses); 
        IEnumerable<Payment> GetDebtOfBlock(Guid flatId, int year, IEnumerable<PaymentStatus> paymentStatuses); 

        bool IsPaymentExist(Guid tenantId, int year, int month, PaymentType paymentType);

        Payment UpdatePayment(Payment payment); //make payment for tenant






    }
}
