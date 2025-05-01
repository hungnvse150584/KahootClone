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
        private readonly IQuestionInGameService _questionInGameService;
        private readonly IMapper _mapper;

        public QuestionService(IQuestionRepository questionRepository, IResponseRepository responseRepository, IQuestionInGameService questionInGameService, IMapper mapper)
        {
            _questionRepository = questionRepository;
            _responseRepository = responseRepository;
            _questionInGameService = questionInGameService;
            _mapper = mapper;
        }

        public async Task<BaseResponse<QuestionResponse>> CreateQuestionAsync(CreateQuestionRequest request)
        {
            try
            {
                if (request.CorrectOption == 2 && string.IsNullOrEmpty(request.Option2))
                {
                    return new BaseResponse<QuestionResponse>(
                        "Invalid CorrectOption: Option2 is null or empty.",
                        StatusCodeEnum.BadRequest_400,
                        null
                    );
                }
                if (request.CorrectOption == 3 && string.IsNullOrEmpty(request.Option3))
                {
                    return new BaseResponse<QuestionResponse>(
                        "Invalid CorrectOption: Option3 is null or empty.",
                        StatusCodeEnum.BadRequest_400,
                        null
                    );
                }
                if (request.CorrectOption == 4 && string.IsNullOrEmpty(request.Option4))
                {
                    return new BaseResponse<QuestionResponse>(
                        "Invalid CorrectOption: Option4 is null or empty.",
                        StatusCodeEnum.BadRequest_400,
                        null
                    );
                }
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
                var existing = await _questionRepository.GetQuestionByIdAsync(request.QuestionId);
                if (existing == null)
                {
                    return new BaseResponse<QuestionResponse>("Question not found", StatusCodeEnum.NotFound_404, null);
                }

                // Update fields
                existing.Text = request.Text;
                existing.TimeLimit = request.TimeLimit;
                existing.ImageUrl = request.ImageUrl;
                existing.Option1 = request.Option1;
                existing.Option2 = request.Option2;
                existing.Option3 = request.Option3;
                existing.Option4 = request.Option4;
                existing.CorrectOption = request.CorrectOption;
                existing.OrderIndex = request.OrderIndex;
                existing.QuizId = request.QuizId;
                existing.Status = request.Status;

                var updated = await _questionRepository.UpdateQuestionAsync(existing);
                var response = _mapper.Map<QuestionResponse>(updated);

                return new BaseResponse<QuestionResponse>("Question updated successfully", StatusCodeEnum.OK_200, response);
            }
            catch (Exception ex)
            {
                return new BaseResponse<QuestionResponse>($"An error occurred: {ex.Message}", StatusCodeEnum.InternalServerError_500, null);
            }
        }

        public async Task<BaseResponse<IEnumerable<QuestionResponse>>> GetAllQuestionsAsync()
        {
            try
            {
                var questions = await _questionRepository.GetAllAsync();
                if (questions == null || !questions.Any())
                {
                    return new BaseResponse<IEnumerable<QuestionResponse>>("No questions found", StatusCodeEnum.NotFound_404, null);
                }

                var response = _mapper.Map<IEnumerable<QuestionResponse>>(questions);
                return new BaseResponse<IEnumerable<QuestionResponse>>("Successfully retrieved questions list", StatusCodeEnum.OK_200, response);
            }
            catch (Exception ex)
            {
                return new BaseResponse<IEnumerable<QuestionResponse>>($"An error occurred: {ex.Message}", StatusCodeEnum.InternalServerError_500, null);
            }
        }

        public async Task<BaseResponse<QuestionResponse>> GetQuestionByIdAsync(int questionId)
        {
            try
            {
                var question = await _questionRepository.GetQuestionByIdAsync(questionId);
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
                var questions = await _questionRepository.GetQuestionsByQuizIdAsync(quizId);
                if (questions == null || !questions.Any())
                {
                    return new BaseResponse<IEnumerable<QuestionResponse>>("No questions found for this quiz", StatusCodeEnum.NotFound_404, null);
                }

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
                var question = await _questionRepository.GetQuestionByIdAsync(questionId);
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
                    return new BaseResponse<IEnumerable<ResponseResponse>>("No responses found for this question in game", StatusCodeEnum.NotFound_404, null);
                }

                var responseDtos = _mapper.Map<IEnumerable<ResponseResponse>>(responses);
                return new BaseResponse<IEnumerable<ResponseResponse>>("Successfully retrieved responses", StatusCodeEnum.OK_200, responseDtos);
            }
            catch (Exception ex)
            {
                return new BaseResponse<IEnumerable<ResponseResponse>>($"An error occurred: {ex.Message}", StatusCodeEnum.InternalServerError_500, null);
            }
        }

        public async Task<BaseResponse<ResponseResponse>> GetLastResponseByPlayerIdAndQuizIdAsync(int playerId, int quizId)
        {
            try
            {
                var response = await _questionRepository.GetLastResponseByPlayerIdAndQuizIdAsync(playerId, quizId);
                if (response == null)
                {
                    return new BaseResponse<ResponseResponse>("No response found for this player and quiz", StatusCodeEnum.NotFound_404, null);
                }

                var responseDto = _mapper.Map<ResponseResponse>(response);
                return new BaseResponse<ResponseResponse>("Successfully retrieved last response", StatusCodeEnum.OK_200, responseDto);
            }
            catch (Exception ex)
            {
                return new BaseResponse<ResponseResponse>($"An error occurred: {ex.Message}", StatusCodeEnum.InternalServerError_500, null);
            }
        }
        public async Task<BaseResponse<IEnumerable<QuestionResponse>>> SearchQuestionsAsync(int? quizId, int? sessionId, string searchTerm)
        {
            try
            {
                // Validate input: at least one of quizId or sessionId must be provided
                if (!quizId.HasValue && !sessionId.HasValue)
                {
                    return new BaseResponse<IEnumerable<QuestionResponse>>(
                        "Either QuizId or SessionId must be provided",
                        StatusCodeEnum.BadRequest_400,
                        null);
                }

                // Search within a session (via QuestionInGame)
                if (sessionId.HasValue)
                {
                    var questionsInGameResponse = await _questionInGameService.GetQuestionsInGameBySessionIdAsync(sessionId.Value);
                    if (questionsInGameResponse.StatusCode != StatusCodeEnum.OK_200 || questionsInGameResponse.Data == null)
                    {
                        return new BaseResponse<IEnumerable<QuestionResponse>>(
                            "No questions found for this session",
                            StatusCodeEnum.NotFound_404,
                            null);
                    }

                    // Fetch Question details for each QuestionInGame
                    var questionResponses = new List<QuestionResponse>();
                    foreach (var qig in questionsInGameResponse.Data)
                    {
                        var question = await _questionRepository.GetQuestionByIdAsync(qig.QuestionId);
                        if (question == null)
                        {
                            continue; // Skip if question not found
                        }

                        // Apply search filters
                        if (string.IsNullOrEmpty(searchTerm) ||
                            question.Text.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                            question.QuestionId.ToString() == searchTerm)
                        {
                            var questionResponse = _mapper.Map<QuestionResponse>(question);
                            questionResponses.Add(questionResponse);
                        }
                    }

                    return new BaseResponse<IEnumerable<QuestionResponse>>(
                        "Successfully retrieved questions for session",
                        StatusCodeEnum.OK_200,
                        questionResponses);
                }

                // Search within a quiz
                var questions = await _questionRepository.GetQuestionsByQuizIdAsync(quizId.Value);
                if (questions == null || !questions.Any())
                {
                    return new BaseResponse<IEnumerable<QuestionResponse>>(
                        "No questions found for this quiz",
                        StatusCodeEnum.NotFound_404,
                        null);
                }

                // Apply search filters
                var filteredQuestions = questions
                    .Where(q => string.IsNullOrEmpty(searchTerm) ||
                                q.Text.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                                q.QuestionId.ToString() == searchTerm)
                    .ToList();

                var response = _mapper.Map<IEnumerable<QuestionResponse>>(filteredQuestions);
                return new BaseResponse<IEnumerable<QuestionResponse>>(
                    "Successfully retrieved questions for quiz",
                    StatusCodeEnum.OK_200,
                    response);
            }
            catch (Exception ex)
            {
                return new BaseResponse<IEnumerable<QuestionResponse>>(
                    $"An error occurred: {ex.Message}",
                    StatusCodeEnum.InternalServerError_500,
                    null);
            }
        }
    }
}
