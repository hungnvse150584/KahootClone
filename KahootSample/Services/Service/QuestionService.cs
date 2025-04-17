using AutoMapper;
using BOs.Model;
using Repositories.IRepository;
using Repositories.Repository;
using Services.IService;
using Services.RequestAndResponse.BaseResponse;
using Services.RequestAndResponse.Enum;
using Services.RequestAndResponse.Request.QuestionRequest;
using Services.RequestAndResponse.Response.QuestionResponses;
using Services.RequestAndResponse.Response.ResponseResponses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Services.Service
{
    public class QuestionService : IQuestionService
    {
        private readonly IQuestionRepository _questionRepository;
        private readonly IResponseRepository _responseRepository;
        private readonly IMapper _mapper;

        public QuestionService(IQuestionRepository questionRepository, IResponseRepository responseRepository, IMapper mapper)
        {
            _questionRepository = questionRepository;
            _responseRepository = responseRepository;
            _mapper = mapper;
        }


        public async Task<BaseResponse<QuestionResponse>> CreateQuestionAsync(CreateQuestionRequest request)
        {
            try
            {
                var question = _mapper.Map<Question>(request);
                var created = await _questionRepository.AddQuestionAsync(question);
                var response = _mapper.Map<QuestionResponse>(created);

                return new BaseResponse<QuestionResponse>("Question created successfully", StatusCodeEnum.Created_201, response);
            }
            catch (Exception ex)
            {
                return new BaseResponse<QuestionResponse>($"An error occurred: {ex.Message}", StatusCodeEnum.InternalServerError_500, null);
            }
        }

        public async Task<BaseResponse<QuestionResponse>> UpdateQuestionAsync(UpdateQuestionRequest request)
        {
            try
            {
                var existing = await _questionRepository.GetByIdAsync(request.QuestionId);
                if (existing == null)
                {
                    return new BaseResponse<QuestionResponse>("Question not found", StatusCodeEnum.NotFound_404, null);
                }

                // Cập nhật thông tin
                existing.Text = request.Text;
                existing.TimeLimit = request.TimeLimit;
                existing.OrderIndex = request.OrderIndex; // Sửa từ Order thành OrderIndex
                existing.QuizId = request.QuizId;

                var updated = await _questionRepository.UpdateQuestionAsync(existing);
                var response = _mapper.Map<QuestionResponse>(updated);

                return new BaseResponse<QuestionResponse>("Question updated successfully", StatusCodeEnum.OK_200, response);
            }
            catch (Exception ex)
            {
                return new BaseResponse<QuestionResponse>($"An error occurred: {ex.Message}", StatusCodeEnum.InternalServerError_500, null);
            }
        }
        public async Task<BaseResponse<QuestionResponse>> GetQuestionsAsync()
        {
            try
            {
                var questions = await _questionRepository.GetAllAsync();
                if (questions == null)
                {
                    return new BaseResponse<QuestionResponse>("Questions List not found", StatusCodeEnum.NotFound_404, null);
                }
                var response = _mapper.Map<QuestionResponse>(questions);
                return new BaseResponse<QuestionResponse>("Successfully retrieved questions list", StatusCodeEnum.OK_200, response);
            }
            catch(Exception ex)
            {
                return new BaseResponse<QuestionResponse>($"An error occured: {ex.Message}", StatusCodeEnum.InternalServerError_500, null);
            }
        }
        public async Task<BaseResponse<QuestionResponse>> GetQuestionByIdAsync(int questionId)
        {
            try
            {
                var question = await _questionRepository.GetByIdAsync(questionId);
                if (question == null)
                {
                    return new BaseResponse<QuestionResponse>("Question not found", StatusCodeEnum.NotFound_404, null);
                }

                var response = _mapper.Map<QuestionResponse>(question);
                return new BaseResponse<QuestionResponse>("Successfully retrieved question", StatusCodeEnum.OK_200, response);
            }
            catch (Exception ex)
            {
                return new BaseResponse<QuestionResponse>($"An error occurred: {ex.Message}", StatusCodeEnum.InternalServerError_500, null);
            }
        }

        public async Task<BaseResponse<IEnumerable<QuestionResponse>>> GetQuestionsByQuizIdAsync(int quizId)
        {
            try
            {
                var questions = await _questionRepository.GetByQuizIdAsync(quizId);
                var response = _mapper.Map<IEnumerable<QuestionResponse>>(questions);

                return new BaseResponse<IEnumerable<QuestionResponse>>("Successfully retrieved questions", StatusCodeEnum.OK_200, response);
            }
            catch (Exception ex)
            {
                return new BaseResponse<IEnumerable<QuestionResponse>>($"An error occurred: {ex.Message}", StatusCodeEnum.InternalServerError_500, null);
            }
        }

        public async Task<BaseResponse<string>> DeleteQuestionAsync(int questionId)
        {
            try
            {
                var question = await _questionRepository.GetByIdAsync(questionId);
                if (question == null)
                {
                    return new BaseResponse<string>("Question not found", StatusCodeEnum.NotFound_404, null);
                }

                await _questionRepository.DeleteQuestionAsync(question);
                return new BaseResponse<string>("Question deleted successfully", StatusCodeEnum.OK_200, "Deleted");
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
                    return new BaseResponse<IEnumerable<ResponseResponse>>("No Responses found for this question in game", StatusCodeEnum.NotFound_404, null);
                }

                var responseDtos = _mapper.Map<IEnumerable<ResponseResponse>>(responses);
                return new BaseResponse<IEnumerable<ResponseResponse>>("Successfully retrieved Responses", StatusCodeEnum.OK_200, responseDtos);
            }
            catch (Exception ex)
            {
                return new BaseResponse<IEnumerable<ResponseResponse>>($"An error occurred: {ex.Message}", StatusCodeEnum.InternalServerError_500, null);
            }
        }
    }
}
