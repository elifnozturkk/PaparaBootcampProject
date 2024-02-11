using AutoMapper;
using PaparaApp.Project.API.Models.Flats.DTOs;
using PaparaApp.Project.API.Models.Shared;
using PaparaApp.Project.API.Models.UnitOfWorks;
using System.Net;

namespace PaparaApp.Project.API.Models.Flats
{
    public class FlatService : IFlatService
    {
        private readonly IFlatRepository _flatRepository;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public FlatService(IFlatRepository flatRepository, IMapper mapper, IUnitOfWork unitOfWork)
        {
            _flatRepository = flatRepository;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public List<FlatDto> GetAll() => _mapper.Map<List<FlatDto>>(_flatRepository.GetAll());

        public FlatDto GetById(Guid id) => _mapper.Map<FlatDto>(_flatRepository.GetById(id));

        public ResponseDto<Guid> Add(FlatAddRequestDto request)
        {
                var flatToAdd = new Flat
                {
                    Id = Guid.NewGuid(),
                    Floor = request.Floor,
                    BlockInfo = request.BlockInfo,
                    FlatNumber = request.FlatNumber,
                    Type = request.Type,
                };
            _flatRepository.Add(flatToAdd);
            _unitOfWork.Commit();
            return new ResponseDto<Guid> { Data = flatToAdd.Id, StatusCode = HttpStatusCode.Created };

        }

        public ResponseDto<Guid> Delete(Guid id)
        {
            var flatToDelete = _flatRepository.GetById(id);
             if (flatToDelete == null)
             {
                return ResponseDto<Guid>.Fail("Flat with ID not found.", HttpStatusCode.BadRequest);
             }
            _flatRepository.Delete(flatToDelete.Id);
            _unitOfWork.Commit();
            return new ResponseDto<Guid> { Data = flatToDelete.Id };
        }

  

        public ResponseDto<Guid> Update(FlatUpdateRequestDto request)
        {
            var flatToUpdate = _flatRepository.GetById(request.Id);

            if (flatToUpdate is null)
            {
                return ResponseDto<Guid>.Fail("Flat with ID not found.", HttpStatusCode.BadRequest);
            }
            flatToUpdate.Status = request.Status;

            _flatRepository.Update(flatToUpdate);
            _unitOfWork.Commit();
            return new ResponseDto<Guid> { Data = flatToUpdate.Id, StatusCode = HttpStatusCode.Created };



        }
    }
}
