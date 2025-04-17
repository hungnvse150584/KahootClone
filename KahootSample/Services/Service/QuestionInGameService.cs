using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using BOs.Model;
using Repositories.IRepository;
using Services.IService;
using Services.RequestAndResponse.BaseResponse;
using Services.RequestAndResponse.Enum;
using Services.RequestAndResponse.Request.QuestionInGameRequest;
using Services.RequestAndResponse.Response.QuestionInGameResponse;

namespace Services.Service
{
    public class QuestionInGameService : IQuestionInGameService
    {
        private readonly IQuestionInGameRepository _repo;
        private readonly IMapper _mapper;

        public QuestionInGameService(IQuestionInGameRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<BaseResponse<QuestionInGameResponse>> CreateAsync(CreateQuestionInGameRequest request)
        {
            var entity = _mapper.Map<QuestionInGame>(request);
            var created = await _repo.AddAsync(entity);
            return new BaseResponse<QuestionInGameResponse>("Created", StatusCodeEnum.Created_201, _mapper.Map<QuestionInGameResponse>(created));
        }

        public async Task<BaseResponse<QuestionInGameResponse>> UpdateAsync(UpdateQuestionInGameRequest request)
        {
            var existing = await _repo.GetByIdAsync(request.QuestionInGameId);
            if (existing == null)
                return new BaseResponse<QuestionInGameResponse>("Not found", StatusCodeEnum.NotFound_404, null);

            _mapper.Map(request, existing);
            var updated = await _repo.UpdateAsync(existing);
            return new BaseResponse<QuestionInGameResponse>("Updated", StatusCodeEnum.OK_200, _mapper.Map<QuestionInGameResponse>(updated));
        }

        public async Task<BaseResponse<QuestionInGameResponse>> GetByIdAsync(int id)
        {
            var data = await _repo.GetByIdAsync(id);
            if (data == null)
                return new BaseResponse<QuestionInGameResponse>("Not found", StatusCodeEnum.NotFound_404, null);

            return new BaseResponse<QuestionInGameResponse>("Success", StatusCodeEnum.OK_200, _mapper.Map<QuestionInGameResponse>(data));
        }

        public async Task<BaseResponse<IEnumerable<QuestionInGameResponse>>> GetBySessionIdAsync(int sessionId)
        {
            var data = await _repo.GetBySessionIdAsync(sessionId);
            return new BaseResponse<IEnumerable<QuestionInGameResponse>>("Success", StatusCodeEnum.OK_200, _mapper.Map<IEnumerable<QuestionInGameResponse>>(data));
        }

        public async Task<BaseResponse<string>> DeleteAsync(int id)
        {
            var existing = await _repo.GetByIdAsync(id);
            if (existing == null)
                return new BaseResponse<string>("Not found", StatusCodeEnum.NotFound_404, null);

            await _repo.DeleteAsync(existing);
            return new BaseResponse<string>("Deleted", StatusCodeEnum.OK_200, null);
        }
    }
}
