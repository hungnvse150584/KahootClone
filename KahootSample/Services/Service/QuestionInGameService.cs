using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using BOs.Model;
using DAO;
using Repositories.IRepository;
using Services.IService;
using Services.RequestAndResponse.BaseResponse;
using Services.RequestAndResponse.Enum;
using Services.RequestAndResponse.Request.QuestionInGameRequest;
using Services.RequestAndResponse.Response.QuestionInGameResponse;
using Services.RequestAndResponse.Response.ResponseResponses;
using static System.Formats.Asn1.AsnWriter;

namespace Services.Service
{
    public class QuestionInGameService : IQuestionInGameService
    {
        private readonly IQuestionInGameRepository _questionInGameRepository;
        private readonly IResponseRepository _responseRepository;
        private readonly IMapper _mapper;

        public QuestionInGameService(IQuestionInGameRepository questionInGameRepository, IResponseRepository responseRepository, IMapper mapper)
        {
            _questionInGameRepository = questionInGameRepository;
            _responseRepository = responseRepository;
            _mapper = mapper;
        }

        public async Task<BaseResponse<QuestionInGameResponse>> CreateQuestionInGameAsync(CreateQuestionInGameRequest request)
        {
            try
            {
                var questionInGame = _mapper.Map<QuestionInGame>(request);
                var created = await _questionInGameRepository.AddQuestionInGameAsync(questionInGame);
                var response = _mapper.Map<QuestionInGameResponse>(created);

                return new BaseResponse<QuestionInGameResponse>("QuestionInGame created successfully", StatusCodeEnum.Created_201, response);
            }
            catch (Exception ex)
            {
                return new BaseResponse<QuestionInGameResponse>($"An error occurred: {ex.Message}", StatusCodeEnum.InternalServerError_500, null);
            }
        }

        public async Task<BaseResponse<QuestionInGameResponse>> UpdateQuestionInGameAsync(UpdateQuestionInGameRequest request)
        {
            try
            {
                var existing = await _questionInGameRepository.GetQuestionInGameByIdAsync(request.QuestionInGameId);
                if (existing == null)
                {
                    return new BaseResponse<QuestionInGameResponse>("QuestionInGame not found", StatusCodeEnum.NotFound_404, null);
                }

                // Update fields
                existing.SessionId = request.SessionId;
                existing.QuestionId = request.QuestionId;
                existing.OrderIndex = request.OrderIndex;
                existing.CreatedTime = request.CreatedTime;
                existing.TotalMembers = request.TotalMembers;

                var updated = await _questionInGameRepository.UpdateQuestionInGameAsync(existing);
                var response = _mapper.Map<QuestionInGameResponse>(updated);

                return new BaseResponse<QuestionInGameResponse>("QuestionInGame updated successfully", StatusCodeEnum.OK_200, response);
            }
            catch (Exception ex)
            {
                return new BaseResponse<QuestionInGameResponse>($"An error occurred: {ex.Message}", StatusCodeEnum.InternalServerError_500, null);
            }
        }

        public async Task<BaseResponse<QuestionInGameResponse>> GetQuestionInGameByIdAsync(int questionInGameId)
        {
            try
            {
                var questionInGame = await _questionInGameRepository.GetQuestionInGameByIdAsync(questionInGameId);
                if (questionInGame == null)
                {
                    return new BaseResponse<QuestionInGameResponse>("QuestionInGame not found", StatusCodeEnum.NotFound_404, null);
                }

                var response = _mapper.Map<QuestionInGameResponse>(questionInGame);
                return new BaseResponse<QuestionInGameResponse>("Successfully retrieved QuestionInGame", StatusCodeEnum.OK_200, response);
            }
            catch (Exception ex)
            {
                return new BaseResponse<QuestionInGameResponse>($"An error occurred: {ex.Message}", StatusCodeEnum.InternalServerError_500, null);
            }
        }

        public async Task<BaseResponse<IEnumerable<QuestionInGameResponse>>> GetQuestionsInGameBySessionIdAsync(int sessionId)
        {
            try
            {
                var questionsInGame = await _questionInGameRepository.GetBySessionIdAsync(sessionId);
                if (questionsInGame == null || !questionsInGame.Any())
                {
                    return new BaseResponse<IEnumerable<QuestionInGameResponse>>("No QuestionsInGame found for this session", StatusCodeEnum.NotFound_404, null);
                }

                var response = _mapper.Map<IEnumerable<QuestionInGameResponse>>(questionsInGame);
                return new BaseResponse<IEnumerable<QuestionInGameResponse>>("Successfully retrieved QuestionsInGame", StatusCodeEnum.OK_200, response);
            }
            catch (Exception ex)
            {
                return new BaseResponse<IEnumerable<QuestionInGameResponse>>($"An error occurred: {ex.Message}", StatusCodeEnum.InternalServerError_500, null);
            }
        }

        public async Task<BaseResponse<string>> DeleteQuestionInGameAsync(int questionInGameId)
        {
            try
            {
                var questionInGame = await _questionInGameRepository.GetQuestionInGameByIdAsync(questionInGameId);
                if (questionInGame == null)
                {
                    return new BaseResponse<string>("QuestionInGame not found", StatusCodeEnum.NotFound_404, null);
                }

                await _questionInGameRepository.DeleteQuestionInGameAsync(questionInGame);
                return new BaseResponse<string>("QuestionInGame deleted successfully", StatusCodeEnum.OK_200, "Deleted");
            }
            catch (Exception ex)
            {
                return new BaseResponse<string>($"An error occurred: {ex.Message}", StatusCodeEnum.InternalServerError_500, null);
            }
        }

        public async Task<BaseResponse<IEnumerable<ResponseResponse>>> GetResponsesByQuestionInGameIdAsync(int questionInGameId)
        {
            try
            {
                var responses = await _responseRepository.GetResponsesByQuestionInGameIdAsync(questionInGameId);
                if (responses == null || !responses.Any())
                {
                    return new BaseResponse<IEnumerable<ResponseResponse>>("No responses found for this QuestionInGame", StatusCodeEnum.NotFound_404, null);
                }

                var responseDtos = _mapper.Map<IEnumerable<ResponseResponse>>(responses);
                return new BaseResponse<IEnumerable<ResponseResponse>>("Successfully retrieved responses", StatusCodeEnum.OK_200, responseDtos);
            }
            catch (Exception ex)
            {
                return new BaseResponse<IEnumerable<ResponseResponse>>($"An error occurred: {ex.Message}", StatusCodeEnum.InternalServerError_500, null);
            }
        }

        public async Task<BaseResponse<ResponseResponse>> GetLastResponseByPlayerIdAndSessionIdAsync(int playerId, int sessionId)
        {
            try
            {
                var response = await _questionInGameRepository.GetLastResponseByPlayerIdAndSessionIdAsync(playerId, sessionId);
                if (response == null)
                {
                    return new BaseResponse<ResponseResponse>("No response found for this player and session", StatusCodeEnum.NotFound_404, null);
                }

                var responseDto = _mapper.Map<ResponseResponse>(response);
                return new BaseResponse<ResponseResponse>("Successfully retrieved last response", StatusCodeEnum.OK_200, responseDto);
            }
            catch (Exception ex)
            {
                return new BaseResponse<ResponseResponse>($"An error occurred: {ex.Message}", StatusCodeEnum.InternalServerError_500, null);
            }
        }
    }
}