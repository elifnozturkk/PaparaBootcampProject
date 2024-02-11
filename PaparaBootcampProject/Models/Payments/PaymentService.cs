using AutoMapper;
using PaparaApp.Project.API.Enums;
using PaparaApp.Project.API.Helper;
using PaparaApp.Project.API.Models.Flats;
using PaparaApp.Project.API.Models.Payments.DTOs;
using PaparaApp.Project.API.Models.Shared;
using PaparaApp.Project.API.Models.UnitOfWorks;
using PaparaApp.Project.API.Models.UserDiscountStatuses;
using PaparaApp.Project.API.Models.UserTimelyPaymentDetails;
using System.Net;
using System.Net.Mail;

namespace PaparaApp.Project.API.Models.Payments
{
    public class PaymentService : IPaymentService
    {
        IPaymentRepository _paymentRepository;
        IFlatRepository _flatRepository;
        IUserPaymentDetailRepository _userPaymentDetailRepository;
        IUserDiscountStatusRepository _userDiscountStatusRepository;
        UserContextHelper  _userContextHelper;
        IMapper _mapper;
        IUnitOfWork _unitOfWork;

        public PaymentService(IPaymentRepository paymentRepository,IFlatRepository flatRepository, 
            IMapper mapper, UserContextHelper userContextHelper, 
            PaymentDetailHelper paymentDetailHelper,
            IUnitOfWork unitOfWork, IUserPaymentDetailRepository userPaymentDetailRepository,
            IUserDiscountStatusRepository userDiscountStatusRepository)
        {
            _paymentRepository = paymentRepository;
            _flatRepository = flatRepository;
            _userPaymentDetailRepository = userPaymentDetailRepository;
            _userDiscountStatusRepository = userDiscountStatusRepository;
            _mapper = mapper;
            _userContextHelper = userContextHelper;
            _unitOfWork = unitOfWork;
        }
        public ResponseDto<List<Guid>> AddDueAll(ManagerPaymentAddRequestDto request)
        {
            var duesIds = new List<Guid>();
            var flats = _flatRepository.GetAllFlatsWithTenants();

            using (var transaction = _unitOfWork.BeginTransaction())
            {
                foreach (var flat in flats)
                {

                    var tenantId = flat.TenantId.Value;
                    var userPaymentDetail = _userPaymentDetailRepository.GetUserPaymentDetailRepository(tenantId);

                    decimal amount = request.Amount;

                    if (userPaymentDetail != null)
                    {
                        var duePaymentFreq = userPaymentDetail.TimelyPaidDue;

                        if (duePaymentFreq % 12 == 0)
                        {
                            var existingDiscount = _userDiscountStatusRepository.GetUserDiscountStatusById(tenantId, request.Year).FirstOrDefault();
                            if (existingDiscount == null || !existingDiscount.IsDiscountActive)
                            {
                                var userDiscountStatus = new UserDiscountStatus
                                {
                                    Id = Guid.NewGuid(),
                                    TenantId = tenantId,
                                    FlatId = flat.Id,
                                    IsDiscountActive = true,
                                    DiscountStartYear = request.Year,
                                    DiscountEndYear = request.Year + 1
                                };
                                _userDiscountStatusRepository.AddDiscountStatus(userDiscountStatus);
                                _unitOfWork.Commit();  

                                amount *= 0.9m; 
                            }
                        }
                    }
                    if(_paymentRepository.IsPaymentExist(tenantId, request.Year, request.Month, PaymentType.Dues))
                    {
                        return ResponseDto<List<Guid>>.Fail("Dues for the specified month and year already exist.", HttpStatusCode.BadRequest);
                    }   
                    var due = new Payment
                    {
                        Id = Guid.NewGuid(),
                        Amount = amount,
                        PaymentType = PaymentType.Dues,
                        Month = request.Month,
                        Year = request.Year,
                        FlatId = flat.Id,
                        TenantId = tenantId
                    };

                    _paymentRepository.AddDueAll(due);
                    duesIds.Add(due.Id);
                }
                _unitOfWork.Commit();
                transaction.Commit();
            }

            return new ResponseDto<List<Guid>> { Data = duesIds };
        }



        public ResponseDto<Guid> AddDueById(ManagerPaymentAddByTenantRequestDto request, Guid id)
        {
            using (var transaction = _unitOfWork.BeginTransaction())
            {
                var userPaymentDetail = _userPaymentDetailRepository.GetUserPaymentDetailRepository(id);

                var duePaymentFreq = userPaymentDetail?.TimelyPaidDue ?? 0;

                if (duePaymentFreq % 12 == 0 && duePaymentFreq != 0) 
                {
                    var existingDiscounts = _userDiscountStatusRepository.GetUserDiscountStatusById(id, request.Year);
                    _userDiscountStatusRepository.DeactivateDiscounts(id); 

                    var userDiscountStatus = new UserDiscountStatus
                    {
                        Id = Guid.NewGuid(),
                        TenantId = id,
                        FlatId = request.FlatId.Value, 
                        IsDiscountActive = true,
                        DiscountStartYear = request.Year,
                        DiscountEndYear = request.Year + 1
                    };

                    _userDiscountStatusRepository.AddDiscountStatus(userDiscountStatus);
                    request.Amount *= 0.9m; 
                }
                if (_paymentRepository.IsPaymentExist(id, request.Year, request.Month, PaymentType.Dues))
                {
                    return ResponseDto<Guid>.Fail("Dues for the specified month and year already exist.", HttpStatusCode.BadRequest);
                }
                var due = new Payment
                {
                    Id = Guid.NewGuid(),
                    Amount = request.Amount,
                    PaymentType = PaymentType.Dues,
                    Month = request.Month,
                    Year = request.Year,
                    FlatId = request.FlatId.Value, 
                    TenantId = id
                };

                _paymentRepository.AddDueById(due);
                _unitOfWork.Commit();
                transaction.Commit(); 

                return new ResponseDto<Guid> { Data = due.Id };
            }
        }

        public ResponseDto<List<Guid>> AddInvoiceByBlockNumber(ManagerPaymentAddRequestDto request, string blockNumber)
        {
            var invoiceIds = new List<Guid>();
            var flats = _flatRepository.GetFlatsWihtinBlock(blockNumber).ToList();
            decimal totalTenants = Convert.ToDecimal(flats.Count);
            if(totalTenants == 0)
            {
                return ResponseDto<List<Guid>>.Fail("No flats found in the specified block.", HttpStatusCode.NotFound);
            }
            using (var transaction = _unitOfWork.BeginTransaction())
            {
                foreach (var flat in flats)
                {
                    var tenantId = flat.TenantId.Value;
                    var userPaymentDetail = _userPaymentDetailRepository.GetUserPaymentDetailRepository(tenantId);
                    decimal amount = request.Amount / totalTenants;
                    int timelyPaymentStreak = 0;

                    switch (request.PaymentType)
                    {
                        case PaymentType.Electricity:
                            timelyPaymentStreak = userPaymentDetail?.TimelyPaidElectricity ?? 0;
                            break;
                        case PaymentType.NaturalGas:
                            timelyPaymentStreak = userPaymentDetail?.TimelyPaidGas ?? 0;
                            break;
                        case PaymentType.Water:
                            timelyPaymentStreak = userPaymentDetail?.TimelyPaidWater ?? 0;
                            break;
                    }

                    if (timelyPaymentStreak % 12 == 0 && timelyPaymentStreak != 0)
                    {
                        var existingDiscounts = _userDiscountStatusRepository.GetUserDiscountStatusById(tenantId, request.Year);
                        _userDiscountStatusRepository.DeactivateDiscounts(tenantId);

                        var userDiscountStatus = new UserDiscountStatus
                        {
                            Id = Guid.NewGuid(),
                            TenantId = tenantId,
                            FlatId = flat.Id,
                            IsDiscountActive = true,
                            DiscountStartYear = request.Year,
                            DiscountEndYear = request.Year + 1
                        };

                        _userDiscountStatusRepository.AddDiscountStatus(userDiscountStatus);
                        request.Amount *= 0.9m;
                    }
                    if (_paymentRepository.IsPaymentExist(tenantId, request.Year, request.Month, PaymentType.Dues))
                    {
                        return ResponseDto<List<Guid>>.Fail("Dues for the specified month and year already exist.", HttpStatusCode.BadRequest);
                    }
                    var invoice = new Payment
                    {
                        Id = Guid.NewGuid(),
                        Amount = amount,
                        PaymentType = request.PaymentType, 
                        Month = request.Month,
                        Year = request.Year,
                        FlatId = flat.Id,
                        TenantId = tenantId,
                    };

                    _paymentRepository.AddInvoiceById(invoice);

                    invoiceIds.Add(invoice.Id);
                }
                _unitOfWork.Commit();
                transaction.Commit();

                return new ResponseDto<List<Guid>> { Data = invoiceIds };

            }
        }




        public ResponseDto<Guid> AddTenantPayment(TenantPaymentAddRequestDto request)
        {
            var statuses = new List<PaymentStatus> { PaymentStatus.Pending }; 

            var tenantId = _userContextHelper.GetTenantIdFromToken();
            if (tenantId == Guid.Empty)
            {
                return ResponseDto<Guid>.Fail("Invalid tenant ID.", HttpStatusCode.BadRequest);
            }

            var paymentToUpdate = _paymentRepository.GetDebt(tenantId, request.DebtYear, request.DebtMonth, request.PaymentType, statuses).FirstOrDefault();
            if (paymentToUpdate == null)
            {
                return ResponseDto<Guid>.Fail("Payment record for the specified debt not found.", HttpStatusCode.NotFound);
            }

            if (paymentToUpdate.PaymentStatus != PaymentStatus.Pending)
            {
                return ResponseDto<Guid>.Fail("Payment for the specified debt has already been made.", HttpStatusCode.BadRequest);
            }
            var dueDate = new DateTime(request.DebtYear, request.DebtMonth, 1).AddMonths(1).AddDays(-1);
            var isPaymentOverdue = request.PaymentDate > dueDate;

            paymentToUpdate.PaymentDate = request.PaymentDate;
            paymentToUpdate.PaymentMethod = request.PaymentMethod;
            paymentToUpdate.PaymentStatus = isPaymentOverdue ? PaymentStatus.Overdue : PaymentStatus.Paid;

            _paymentRepository.UpdatePayment(paymentToUpdate);

            _userPaymentDetailRepository.UpdateUserPaymentFrequency(tenantId, request.PaymentType, paymentToUpdate.PaymentStatus); 


       
            _unitOfWork.Commit();

            return new ResponseDto<Guid> { Data = tenantId }; 
        }

        public ResponseDto<Guid> AddBulkTenantPaymentForMonth(TenantBulkPaymentMonthAddRequestDto request)
        {
            var statuses = new List<PaymentStatus> { PaymentStatus.Pending }; ;
            var tenantId = _userContextHelper.GetTenantIdFromToken();
            if (tenantId == Guid.Empty)
            {
                return ResponseDto<Guid>.Fail("Invalid tenant ID.", HttpStatusCode.BadRequest);
            }

            var unpaidPayments = _paymentRepository.GetDebt(tenantId, request.Year, request.Month, statuses);
            if (!unpaidPayments.Any())
            {
                return ResponseDto<Guid>.Fail("No unpaid debts found for the specified month.", HttpStatusCode.NotFound);
            }

            var dueDate = new DateTime(unpaidPayments.First().Year, unpaidPayments.First().Month, 1).AddMonths(1).AddDays(-1);
            var isPaymentOverdue = request.PaymentDate > dueDate;

            using (var transaction = _unitOfWork.BeginTransaction())
            {
                foreach (var payment in unpaidPayments)
                {
                    PaymentType paymentType = payment.PaymentType.Value;
                    payment.PaymentDate = request.PaymentDate;
                    payment.PaymentMethod = request.PaymentMethod;
                    payment.PaymentStatus = isPaymentOverdue ? PaymentStatus.Overdue : PaymentStatus.Paid;

                    _paymentRepository.UpdatePayment(payment);
                    _userPaymentDetailRepository.UpdateUserPaymentFrequency(tenantId, paymentType, payment.PaymentStatus); 
                }

                _unitOfWork.Commit();
                transaction.Commit();
            }

            return new ResponseDto<Guid> { Data = tenantId };
        }

        public ResponseDto<Guid> AddBulkTenantPaymentForYear(TenantBulkPaymentYearRequestDto request)
        {
            var tenantId = _userContextHelper.GetTenantIdFromToken();
            var statuses = new List<PaymentStatus> { PaymentStatus.Pending };
            if (tenantId == Guid.Empty)
            {
                return ResponseDto<Guid>.Fail("Invalid tenant ID.", HttpStatusCode.BadRequest);
            }

            var unpaidPayments = _paymentRepository.GetDebt(tenantId, request.Year,statuses);

            if (!unpaidPayments.Any())
            {
                return ResponseDto<Guid>.Fail("No unpaid debts found for the specified year.", HttpStatusCode.NotFound);
            }

            using (var transaction = _unitOfWork.BeginTransaction())
            {
                foreach (var payment in unpaidPayments)
                {
                    var dueDate = new DateTime(payment.Year, payment.Month, 1).AddMonths(1).AddDays(-1);
                    var isPaymentOverdue = request.PaymentDate > dueDate;

                    PaymentType paymentType = payment.PaymentType.Value;
                    payment.PaymentDate = request.PaymentDate;
                    payment.PaymentMethod = request.PaymentMethod;
                    payment.PaymentStatus = isPaymentOverdue ? PaymentStatus.Overdue : PaymentStatus.Paid;
                    _paymentRepository.UpdatePayment(payment);
                }

                _unitOfWork.Commit();
                transaction.Commit();
            }

            return new ResponseDto<Guid> { Data = tenantId };
        }


       

        public ResponseDto<Guid> AddInvoiceById(ManagerPaymentAddByTenantRequestDto request, Guid id)
        {
            var invoice = new Payment
            {
                Id = Guid.NewGuid(),
                Amount = request.Amount,
                PaymentType = request.PaymentType,
                Month = request.Month,
                Year = request.Year,
                FlatId = request.FlatId,
                TenantId = id
            };
            _paymentRepository.AddInvoiceById(invoice);
            _unitOfWork.Commit();
            return new ResponseDto<Guid> { Data = invoice.Id };

        }
        public ResponseDto<List<PaymentDetailDto>> GetAllPaidDebt(int year, int month)
        {
            var flats = _flatRepository.GetAllFlatsWithTenants();
            var statuses = new List<PaymentStatus> { PaymentStatus.Paid, PaymentStatus.Overdue };
            List<Payment> allPaidDebts = new List<Payment>();

            foreach (var flat in flats)
            {
                var tenantId = flat.TenantId.Value;
                allPaidDebts.AddRange(_paymentRepository.GetDebt(tenantId, year, month, statuses));
            }

            if (!allPaidDebts.Any())
            {
                return ResponseDto<List<PaymentDetailDto>>.Fail("No paid debts found for the specified year and month.", HttpStatusCode.NotFound);
            }

            var formattedPayments = PaymentDetailHelper.FormatPayments(allPaidDebts.ToList());

            return new ResponseDto<List<PaymentDetailDto>> { Data = formattedPayments };
        }
        public ResponseDto<List<PaymentDetailDto>> GetTenantPaidDebt(Guid? tenantId, int year, int month)
        {
            Guid actualTenantId = _userContextHelper.GetCurrentTenantId(tenantId);
            var statuses = new List<PaymentStatus> { PaymentStatus.Paid, PaymentStatus.Overdue };
            var tenantsPaidDebt = _paymentRepository.GetDebt(actualTenantId, year, month, statuses);
            if (!tenantsPaidDebt.Any())
            {
                return ResponseDto<List<PaymentDetailDto>>.Fail("No paid  debts found for the specified year and month.", HttpStatusCode.NotFound);
            }

            var formattedPayments = PaymentDetailHelper.FormatPayments(tenantsPaidDebt.ToList());

            return new ResponseDto<List<PaymentDetailDto>> { Data = formattedPayments };
        }


        public ResponseDto<List<PaymentDetailDto>> GetTenantPaidDebt(Guid? tenantId, int year)
        {
            Guid actualTenantId = _userContextHelper.GetCurrentTenantId(tenantId);
            var statuses = new List<PaymentStatus> { PaymentStatus.Paid, PaymentStatus.Overdue };
            var tenantsPaidDebt = _paymentRepository.GetDebt(actualTenantId, year, statuses);


            if (!tenantsPaidDebt.Any())
            {
                return ResponseDto<List<PaymentDetailDto>>.Fail("No paid debts found for the specified year.", HttpStatusCode.NotFound);
            }

            var formattedPayments = PaymentDetailHelper.FormatPayments(tenantsPaidDebt.ToList());

            return new ResponseDto<List<PaymentDetailDto>> { Data = formattedPayments };
        }

        public ResponseDto<List<PaymentDetailDto>> GetTenantUnpaidDebt(Guid? tenantId, int year)
        {
            Guid actualTenantId = _userContextHelper.GetCurrentTenantId(tenantId);
            var statuses = new List<PaymentStatus> { PaymentStatus.Pending};
            var tenantsUnpaidDebt = _paymentRepository.GetDebt(actualTenantId, year, statuses);

            if (!tenantsUnpaidDebt.Any())
            {
                return ResponseDto<List<PaymentDetailDto>>.Fail("No unpaid debts found for the specified year.", HttpStatusCode.NotFound);
            }

            var formattedPayments = PaymentDetailHelper.FormatPayments(tenantsUnpaidDebt.ToList());

            return new ResponseDto<List<PaymentDetailDto>> { Data = formattedPayments };
        }

        public ResponseDto<List<PaymentDetailDto>> GetTenantUnpaidDebt(Guid? tenantId, int year, int month)
        {
            Guid actualTenantId = _userContextHelper.GetCurrentTenantId(tenantId);
            var statuses = new List<PaymentStatus> { PaymentStatus.Pending };
            var tenantsUnpaidDebt = _paymentRepository.GetDebt(actualTenantId, year, month, statuses);
            if(!tenantsUnpaidDebt.Any())
            {
                return ResponseDto<List<PaymentDetailDto>>.Fail("No unpaid debts found for the specified month and year.", HttpStatusCode.NotFound);
            }

            var formattedPayments = PaymentDetailHelper.FormatPayments(tenantsUnpaidDebt.ToList());

            return new ResponseDto<List<PaymentDetailDto>> { Data = formattedPayments };
        }

        public ResponseDto<List<PaymentDetailDto>> GetBlockUnpaidDebt(string blockNumber, int year, int month)
        {
            using (var transaction = _unitOfWork.BeginTransaction())
            {
                var flatIdsByBlock = _flatRepository.GetIdByBlock(blockNumber).ToList();

                var statuses = new List<PaymentStatus> { PaymentStatus.Pending};
                List<Payment> allUnpaidDebts = new List<Payment>();

                foreach (var flat in flatIdsByBlock)
                {
                    var debts = _paymentRepository.GetDebtOfBlock(flat, year,month, statuses)
                                   .ToList();
                    allUnpaidDebts.AddRange(debts);
                }
                if (!allUnpaidDebts.Any())
                {
                    return ResponseDto<List<PaymentDetailDto>>.Fail("No unpaid debts found for the specified block, month and year.", HttpStatusCode.NotFound);
                }

                var formattedPayments = PaymentDetailHelper.FormatPayments(allUnpaidDebts);
                _unitOfWork.Commit();
                transaction.Commit();

                return new ResponseDto<List<PaymentDetailDto>> { Data = formattedPayments };
            }
        }

        public ResponseDto<List<PaymentDetailDto>> GetBlockPaidDebt(string blockNumber, int year)
        {
            using (var transaction = _unitOfWork.BeginTransaction())
            {
                var flatIdsByBlock = _flatRepository.GetIdByBlock(blockNumber).ToList();

                var statuses = new List<PaymentStatus> { PaymentStatus.Overdue, PaymentStatus.Paid };
                List<Payment> allPaidDebts = new();

                foreach (var flat in flatIdsByBlock)
                {
                    var debts = _paymentRepository.GetDebtOfBlock(flat, year, statuses)
                                   .ToList();
                    allPaidDebts.AddRange(debts);
                }

                if (!allPaidDebts.Any())
                {
                    return ResponseDto<List<PaymentDetailDto>>.Fail("No paid debts found for the specified block and year.", HttpStatusCode.NotFound);
                }

                var formattedPayments = PaymentDetailHelper.FormatPayments(allPaidDebts);

                _unitOfWork.Commit();
                transaction.Commit();

                return new ResponseDto<List<PaymentDetailDto>> { Data = formattedPayments };
            }
        }

  
    }
}
