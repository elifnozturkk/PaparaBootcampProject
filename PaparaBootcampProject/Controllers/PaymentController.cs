using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PaparaApp.Project.API.Models.Payments;
using PaparaApp.Project.API.Models.Payments.DTOs;

namespace PaparaApp.Project.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController(IPaymentService paymentService) : ControllerBase
    {
        private readonly IPaymentService _paymentService = paymentService;

        [HttpGet]
        [Route("tenants/{tenantId}/debts/due/{year}")]
        [Authorize(Roles = "Manager")]
        public IActionResult GetDueDebtForTenantWithId(Guid tenantId, int year)
        {
            var response = _paymentService.GetTenantPaidDebt(tenantId, year);
            if (response.Errors != null && response.Errors.Any())
            {
                return StatusCode((int)response.StatusCode, response);
            }
            return Ok(response);
        }

        [HttpGet]
        [Route("debts/due/{year}")]
        [Authorize(Roles = "Tenant")]
        public IActionResult GetDueDebtForTenantWithoutId(int year)
        {
            Guid? tenantId = null;
            var response = _paymentService.GetTenantPaidDebt(tenantId, year);
            if (response.Errors != null && response.Errors.Any())
            {
                return StatusCode((int)response.StatusCode, response);
            }
            return Ok(response);
        }

        [HttpGet]
        [Route("debts/due/{year}/{month}")]
        [Authorize(Roles = "Tenant")]
        public IActionResult GetDueDebtForTenantWithoutId(int year, int month)
        {
            Guid? tenantId = null; 
            var response = _paymentService.GetTenantPaidDebt(tenantId, year, month);
            if (response.Errors != null && response.Errors.Any())
            {
                return StatusCode((int)response.StatusCode, response);
            }
            return Ok(response);
        }

        [HttpGet]
        [Route("tenants/{tenantId}/debts/due/{year}/{month}")]
        [Authorize(Roles = "Manager")]

        public IActionResult GetDueDebtForTenantWithId(Guid tenantId, int year, int month)
        {
            var response = _paymentService.GetTenantPaidDebt(tenantId, year, month);
            if (response.Errors != null && response.Errors.Any())
            {
                return StatusCode((int)response.StatusCode, response);
            }
            return Ok(response);
        }

        [HttpGet]
        [Route("tenants/{tenantId}/debts/unpaid/{year}")]
        [Authorize(Roles = "Manager")]
        public IActionResult GetUnpiadDebtForTenantWithId(Guid tenantId, int year)
        {
            var response = _paymentService.GetTenantUnpaidDebt(tenantId, year);
            if (response.Errors != null && response.Errors.Any())
            {
                return StatusCode((int)response.StatusCode, response);
            }
            return Ok(response);
        }

        [HttpGet]
        [Route("debts/unpaid/{year}")]
        [Authorize(Roles = "Tenant")]
        public IActionResult GetUnpiadDebtForTenantWithoutId(int year)
        {
            Guid? tenantId = null;
            var response = _paymentService.GetTenantUnpaidDebt(tenantId, year);
            if (response.Errors != null && response.Errors.Any())
            {
                return StatusCode((int)response.StatusCode, response);
            }
            return Ok(response);
        }

        [HttpGet]
        [Route("debts/unpaid/{year}/{month}")]
        [Authorize(Roles = "Tenant")]
        public IActionResult GetUnpiadDebtForTenantWithoutId(int year, int month)
        {
            Guid? tenantId = null;
            var response = _paymentService.GetTenantUnpaidDebt(tenantId, year, month);
            if (response.Errors != null && response.Errors.Any())
            {
                return StatusCode((int)response.StatusCode, response);
            }
            return Ok(response);
        }

        [HttpGet]
        [Route("tenants/{tenantId}/debts/unpaid/{year}/{month}")]
        [Authorize(Roles = "Manager")]
        public IActionResult GetUnpiadDebtForTenantWithId(Guid tenantId, int year, int month)
        {
            var response = _paymentService.GetTenantUnpaidDebt(tenantId, year, month);
            if (response.Errors != null && response.Errors.Any())
            {
                return StatusCode((int)response.StatusCode, response);
            }
            return Ok(response);
        }

        [HttpGet]
        [Route("blocks/{blockNumber}/debts/due/{year}")]
        [Authorize(Roles = "Manager")]
        public IActionResult GetDueDebtForBlock(string blockNumber, int year)
        {
            var response = _paymentService.GetBlockPaidDebt(blockNumber, year);
            if (response.Errors != null && response.Errors.Any())
            {
                return StatusCode((int)response.StatusCode, response);
            }
            return Ok(response);
        }

        [HttpGet]
        [Route("blocks/{blockNumber}/debts/unpaid/{year}/{month}")]
        [Authorize(Roles = "Manager")]
        public IActionResult GetUnpaidDebtForBlock(string blockNumber, int year, int month)
        {
            var response = _paymentService.GetBlockUnpaidDebt(blockNumber, year, month);
            if (response.Errors != null && response.Errors.Any())
            {
                return StatusCode((int)response.StatusCode, response);
            }
            return Ok(response);
        }

        [HttpPut]
        [Authorize(Roles = "Tenant")]
        [Route("payments/single")]
        public IActionResult MakePayment(TenantPaymentAddRequestDto request)
        {
            var response = _paymentService.AddTenantPayment(request);
            if (response.Errors != null && response.Errors.Any())
            {
                return StatusCode((int)response.StatusCode, response);
            }
            return Created("", response);
        }

        [HttpPut]
        [Authorize(Roles = "Tenant")]
        [Route("payments/bulk/month")]
        public IActionResult MakeBulkPaymentForMonth(TenantBulkPaymentMonthAddRequestDto request)
        {
            var response = _paymentService.AddBulkTenantPaymentForMonth(request);
            if (response.Errors != null && response.Errors.Any())
            {
                return StatusCode((int)response.StatusCode, response);
            }
            return Created("", response);
        }


        [HttpPut]
        [Authorize(Roles = "Tenant")]
        [Route("payments/bulk/year")]
        public IActionResult MakeBulkPaymentForYear(TenantBulkPaymentYearRequestDto request)
        {
            var response = _paymentService.AddBulkTenantPaymentForYear(request);
            if (response.Errors != null && response.Errors.Any())
            {
                return StatusCode((int)response.StatusCode, response);
            }
            return Created("", response);
        }

        [HttpPost]
        [Route("tenants/{id}/dues/add")]
        [Authorize(Roles = "Manager")]
        public IActionResult AddTenantDue(ManagerPaymentAddByTenantRequestDto request, Guid id)
        {
            var response = _paymentService.AddDueById(request, id);
            if (response.Errors != null && response.Errors.Any())
            {
                return StatusCode((int)response.StatusCode, response);
            }
            return Created("",response);



        }

        [HttpPost]
        [Route("tenants/dues/add-to-all")]
        [Authorize(Roles = "Manager")]
        public IActionResult AddDueToAllTenants(ManagerPaymentAddRequestDto request)
        {
            var response = _paymentService.AddDueAll(request);
            if (response.Errors != null && response.Errors.Any())
            {
                return StatusCode((int)response.StatusCode, response);
            }
            return Created("", response);
        }

        [HttpPost]
        [Route("blocks/{blockNumber}/invoices/add")]
        [Authorize(Roles = "Manager")]
        public IActionResult AddTenantInvoice(ManagerPaymentAddRequestDto request, string blockNumber)
        {
            var response = _paymentService.AddInvoiceByBlockNumber(request, blockNumber);
            if (response.Errors != null && response.Errors.Any())
            {
                return StatusCode((int)response.StatusCode, response);
            }
            return Created("", response);
        }

        [HttpPost] 
        [Route("payments/single/add")]
        [Authorize(Roles = "Tenant")]
        public IActionResult MakeSinglePayment(TenantPaymentAddRequestDto request)
        {
            var response = _paymentService.AddTenantPayment(request);
            if (response.Errors != null && response.Errors.Any())
            {
                return StatusCode((int)response.StatusCode, response);
            }
            return Created("", response);
        }

    }
}
