using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VinClean.Service.DTO.Type;
using VinClean.Service.DTO;
using VinClean.Repo.Models;
using AutoMapper;
using VinClean.Repo.Repository;

namespace VinClean.Service.Service
{
    public interface ISlotService
    {
        Task<ServiceResponse<List<Slot>>> GetSlotList();
        Task<ServiceResponse<Slot>> GetSlotById(int id);
        Task<ServiceResponse<Slot>> DeleteSlot(int id);
        Task<ServiceResponse<Slot>> AddSlot(Slot request);
        Task<ServiceResponse<Slot>> UpdateSlot(Slot request);
    }
    public class SlotService : ISlotService
    {
        private readonly ISlotRepository _repository;
        private readonly IMapper _mapper;
        public SlotService(ISlotRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<ServiceResponse<List<Slot>>> GetSlotList()
        {
            ServiceResponse<List<Slot>> _response = new();
            try
            {
                var ListSlot = await _repository.GetSlotList();
                var ListSlotDTO = new List<Slot>();
                foreach (var slot in ListSlot)
                {
                    ListSlotDTO.Add(slot);
                }
                _response.Success = true;
                _response.Message = "OK";
                _response.Data = ListSlotDTO;
            }
            catch (Exception ex)
            {
                _response.Success = false;
                _response.Message = "Error";
                _response.Data = null;
                _response.ErrorMessages = new List<string> { Convert.ToString(ex.Message) };
            }
            return _response;

        }

        public async Task<ServiceResponse<Slot>> GetSlotById(int id)
        {
            ServiceResponse<Slot> _response = new();
            try
            {
                var slot = await _repository.GetSlotById(id);
                if (slot == null)
                {
                    _response.Success = false;
                    _response.Message = "NotFound";
                    return _response;
                }
                var slotDTO = _mapper.Map<Slot>(slot);
                _response.Success = true;
                _response.Message = "OK";
                _response.Data = slotDTO;

            }
            catch (Exception ex)
            {
                _response.Success = false;
                _response.Message = "Error";
                _response.Data = null;
                _response.ErrorMessages = new List<string> { Convert.ToString(ex.Message) };
            }
            return _response;

        }

        public async Task<ServiceResponse<Slot>> UpdateSlot(Slot request)
        {
            ServiceResponse<Slot> _response = new();
            try
            {
                var existingSlot = await _repository.GetSlotById(request.SlotId);
                if (existingSlot == null)
                {
                    _response.Success = false;
                    _response.Message = "NotFound";
                    _response.Data = null;
                    return _response;
                }
                existingSlot.SlotName = request.SlotName;
                existingSlot.DayOfweek = request.DayOfweek;
                existingSlot.StartTime = request.StartTime;
                existingSlot.EndTime = request.EndTime;


                if (!await _repository.UpdateSlot(existingSlot))
                {
                    _response.Success = false;
                    _response.Message = "RepoError";
                    _response.Data = null;
                    return _response;
                }

               /* var _TypetDTO = _mapper.Map<Slot>(existingType);*/
                _response.Success = true;
                _response.Data = existingSlot;
                _response.Message = "Updated";

            }
            catch (Exception ex)
            {
                _response.Success = false;
                _response.Data = null;
                _response.Message = "Error";
                _response.ErrorMessages = new List<string> { Convert.ToString(ex.Message) };
            }
            return _response;
        }

        public async Task<ServiceResponse<Slot>> AddSlot(Slot request)
        {
            ServiceResponse<Slot> _response = new();
            try
            {
                Slot _newSlot = new Slot()
                {
                    SlotName = request.SlotName,
                    DayOfweek = request.DayOfweek,
                    StartTime = request.StartTime,
                    EndTime = request.EndTime,

                };

                if (!await _repository.AddSlot(_newSlot))
                {
                    _response.Error = "RepoError";
                    _response.Success = false;
                    _response.Data = null;
                    return _response;
                }

                _response.Success = true;
                _response.Data = _newSlot;
                _response.Message = "Created";

            }
            catch (Exception ex)
            {
                _response.Success = false;
                _response.Data = null;
                _response.Message = "Error";
                _response.ErrorMessages = new List<string> { Convert.ToString(ex.Message) };
            }

            return _response;
        }

        public async Task<ServiceResponse<Slot>> DeleteSlot(int id)

        {
            ServiceResponse<Slot> _response = new();
            try
            {
                var existingSlot = await _repository.GetSlotById(id);
                if (existingSlot == null)
                {
                    _response.Success = false;
                    _response.Message = "NotFound";
                    _response.Data = null;
                    return _response;
                }


                if (!await _repository.DeleteSlot(id))
                {
                    _response.Success = false;
                    _response.Message = "RepoError";
                    return _response;
                }

                _response.Success = true;
                _response.Message = "SoftDeleted";

            }
            catch (Exception ex)
            {
                _response.Success = false;
                _response.Message = "Error";
                _response.ErrorMessages = new List<string> { Convert.ToString(ex.Message)
    };
            }
            return _response;
        }
    }
}
