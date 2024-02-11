using Azure;
using PaparaApp.Project.API.Enums;
using PaparaApp.Project.API.Models.Payments.DTOs;
using PaparaApp.Project.API.Models.Shared;

namespace PaparaApp.Project.API.Models.Payments
{
    public interface IPaymentService
    {
        ResponseDto<Guid> AddDueById(ManagerPaymentAddByTenantRequestDto request, Guid id);
        ResponseDto<List<Guid>> AddDueAll(ManagerPaymentAddRequestDto request);
        ResponseDto<List<Guid>> AddInvoiceByBlockNumber(ManagerPaymentAddRequestDto request, string blockNumber);

        ResponseDto<Guid> AddTenantPayment(TenantPaymentAddRequestDto request);
        ResponseDto<Guid> AddBulkTenantPaymentForMonth(TenantBulkPaymentMonthAddRequestDto request);

        ResponseDto<Guid> AddBulkTenantPaymentForYear(TenantBulkPaymentYearRequestDto request);





        ResponseDto<List<PaymentDetailDto>> GetAllPaidDebt(int year, int month ); //bütün kullancılar - ödenmişler 
        ResponseDto<List<PaymentDetailDto>> GetTenantPaidDebt(Guid? tenantId, int year, int month ); //tek kullancı - aylık ödenen   
        ResponseDto<List<PaymentDetailDto>> GetTenantPaidDebt(Guid? tenantId, int year); //tek kullancı - yıllık ödenen
        ResponseDto<List<PaymentDetailDto>> GetBlockPaidDebt(string blockNumber, int year); //bina bşına(blok) - yıllık ödenen






        ResponseDto<List<PaymentDetailDto>> GetTenantUnpaidDebt(Guid? tenantId, int year); //tek kullancı - ödenmemişler
        ResponseDto<List<PaymentDetailDto>> GetTenantUnpaidDebt(Guid? tenantId, int year, int month); //tek kullancı - ödenmemişler - aylı
        ResponseDto<List<PaymentDetailDto>> GetBlockUnpaidDebt(string blockNumber, int year, int month); //tek kullancı - ödenmemişler - aylı







    }
}
