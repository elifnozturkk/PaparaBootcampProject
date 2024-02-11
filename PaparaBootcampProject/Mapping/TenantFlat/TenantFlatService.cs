using AutoMapper;
using Azure;
using PaparaApp.Project.API.Mapping.TenantFlat.Dtos;
using PaparaApp.Project.API.Migrations;
using PaparaApp.Project.API.Models.Flats;
using PaparaApp.Project.API.Models.Shared;
using PaparaApp.Project.API.Models.UnitOfWorks;
using PaparaApp.Project.API.Models.UserTimelyPaymentDetails;
using System.Net;
using System.Net.NetworkInformation;

namespace PaparaApp.Project.API.Mapping.TenantFlat
{
    public class TenantFlatService : ITenantFlatService
    {
        private readonly ITenantFlatRepository _tenantFlatRepository;
        private readonly IFlatRepository _flatRepository;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;


        public TenantFlatService(ITenantFlatRepository tenantFlatRepository,IFlatRepository flatRepository, IMapper mapper, IUnitOfWork unitOfWork, IUserPaymentDetailRepository userPaymentDetailRepository)
        {
            _tenantFlatRepository = tenantFlatRepository;
            _flatRepository = flatRepository;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public ResponseDto<Guid?> AssignTenantToFlat(TenantFlatAddRequestDto request)
        {

            var tenantCandidate = _tenantFlatRepository.GetCurrentTenantByTenantId(request.TenantId);
            if (tenantCandidate == null)
            {
                return ResponseDto<Guid?>.Fail("Tenant not found.", HttpStatusCode.BadRequest);

            }

            var flatCandidate = _tenantFlatRepository.GetCurrentFlatByFlatId(request.FlatId);
            if (flatCandidate == null || flatCandidate.Status)
            {
                return ResponseDto<Guid?>.Fail("Flat not found or occupied.", HttpStatusCode.BadRequest);
            }

            var isAssigned = _tenantFlatRepository.IsTenantAssignedToFlat(request.TenantId);
            if (isAssigned)
            {
                return ResponseDto<Guid?>.Fail("Tenant already assigned to flat.", HttpStatusCode.BadRequest);
            }

            using var transaction = _unitOfWork.BeginTransaction();

            var tenantFlatToAdd = new TenantFlat
            {
                        Id = Guid.NewGuid(),
                        TenantId = request.TenantId,
                        FlatId = request.FlatId,
                        StartDate = DateTime.Now,
                        EndDate = request.EndDate ?? DateTime.MaxValue
            };

            _tenantFlatRepository.Add(tenantFlatToAdd);

            flatCandidate.Status = true;
            flatCandidate.TenantId = request.TenantId;
            _flatRepository.Update(flatCandidate);

            _unitOfWork.Commit();
            transaction.Commit();

            return new ResponseDto<Guid?> { Data = tenantCandidate.Id };

        }

        public ResponseDto<Guid?> UnassignTenantFromFlat(TenantFlatUpdateRequestDto request)
        {

            using var transaction = _unitOfWork.BeginTransaction();
            {
                var tenantFlatToUpdate = _tenantFlatRepository.GetTenantFlatById(request.Id);
                if (tenantFlatToUpdate == null)
                {
                    transaction.Rollback();
                    return ResponseDto<Guid?>.Fail("Tenant flat not found.", HttpStatusCode.BadRequest);
                }

                var flat = _flatRepository.GetById(tenantFlatToUpdate.FlatId);
                if (flat == null)
                {
                    transaction.Rollback();
                    return ResponseDto<Guid?>.Fail("Flat not found.", HttpStatusCode.BadRequest);
                }

                tenantFlatToUpdate.EndDate = DateTime.Now;
                _tenantFlatRepository.Update(tenantFlatToUpdate);

                flat.Status = false;
                flat.TenantId = null;
                _flatRepository.Update(flat);

                _unitOfWork.Commit();
                transaction.Commit();

                return new ResponseDto<Guid?> { Data = tenantFlatToUpdate.Id };
            }

           
        }

        public List<TenantFlatDto> GetAll() => _mapper.Map<List<TenantFlatDto>>(_tenantFlatRepository.GetAll());


        public TenantFlatDto GetTenantFlat(Guid id) => _mapper.Map<TenantFlatDto>(_tenantFlatRepository.GetTenantFlatById(id));


        public List<TenantFlatDto> GetTenantFlatHistoryByFlatId(Guid flatId) => _mapper.Map<List<TenantFlatDto>>(_tenantFlatRepository.GetTenantFlatHistoryByFlatId(flatId));

        public List<TenantFlatDto> GetTenantFlatHistoryByTenantId(Guid tenantId) => _mapper.Map<List<TenantFlatDto>>(_tenantFlatRepository.GetTenantFlatHistoryByTenantId(tenantId));

    }
}
