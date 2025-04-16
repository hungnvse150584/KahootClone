using AutoMapper;
using BOs.Model;
using Repositories.IRepository;
using Services.IService;
using Services.RequestAndResponse.BaseResponse;
using Services.RequestAndResponse.Enum;
using Services.RequestAndResponse.Request;
using Services.RequestAndResponse.Response.QuizResponses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Services.Service
{
    public class QuizService : IQuizService
    {
        private readonly IQuizRepository _quizRepository;
        private readonly IMapper _mapper;

        public QuizService(IQuizRepository quizRepository, IMapper mapper)
        {
            _quizRepository = quizRepository;
            _mapper = mapper;
        }

        public async Task<BaseResponse<QuizResponse>> CreateQuizAsync(CreateQuizRequest request)
        {
            try
            {
                var quiz = _mapper.Map<Quiz>(request);
                var createdQuiz = await _quizRepository.AddQuizAsync(quiz);
                var response = _mapper.Map<QuizResponse>(createdQuiz);

                return new BaseResponse<QuizResponse>("Quiz created successfully", StatusCodeEnum.Created_201, response);
            }
            catch (Exception ex)
            {
                return new BaseResponse<QuizResponse>($"An error occurred: {ex.Message}", StatusCodeEnum.InternalServerError_500, null);
            }
        }

        public async Task<BaseResponse<QuizResponse>> UpdateQuizAsync(UpdateQuizRequest request)
        {
            try
            {
                var existingQuiz = await _quizRepository.GetByIdAsync(request.QuizId);
                if (existingQuiz == null)
                {
                    return new BaseResponse<QuizResponse>("Quiz not found", StatusCodeEnum.NotFound_404, null);
                }

                existingQuiz.Title = request.Title;
                existingQuiz.Pin = request.Pin;
                existingQuiz.QrCode = request.QrCode;

                var updatedQuiz = await _quizRepository.UpdateQuizAsync(existingQuiz);
                var response = _mapper.Map<QuizResponse>(updatedQuiz);

                return new BaseResponse<QuizResponse>("Quiz updated successfully", StatusCodeEnum.OK_200, response);
            }
            catch (Exception ex)
            {
                return new BaseResponse<QuizResponse>($"An error occurred: {ex.Message}", StatusCodeEnum.InternalServerError_500, null);
            }
        }

        public async Task<BaseResponse<QuizResponse>> GetQuizByIdAsync(int quizId)
        {
            try
            {
                var quiz = await _quizRepository.GetByIdAsync(quizId);
                if (quiz == null)
                {
                    return new BaseResponse<QuizResponse>("Quiz not found", StatusCodeEnum.NotFound_404, null);
                }

                var response = _mapper.Map<QuizResponse>(quiz);
                return new BaseResponse<QuizResponse>("Successfully retrieved quiz", StatusCodeEnum.OK_200, response);
            }
            catch (Exception ex)
            {
                return new BaseResponse<QuizResponse>($"An error occurred: {ex.Message}", StatusCodeEnum.InternalServerError_500, null);
            }
        }

        public async Task<BaseResponse<IEnumerable<QuizResponse>>> GetQuizzesByUserIdAsync(int userId)
        {
            try
            {
                var quizzes = await _quizRepository.GetQuizzesByUserIdAsync(userId);
                if (quizzes == null || !quizzes.Any())
                {
                    return new BaseResponse<IEnumerable<QuizResponse>>("No quizzes found for this user", StatusCodeEnum.NotFound_404, null);
                }

                var response = _mapper.Map<IEnumerable<QuizResponse>>(quizzes);
                return new BaseResponse<IEnumerable<QuizResponse>>("Successfully retrieved quizzes", StatusCodeEnum.OK_200, response);
            }
            catch (Exception ex)
            {
                return new BaseResponse<IEnumerable<QuizResponse>>($"An error occurred: {ex.Message}", StatusCodeEnum.InternalServerError_500, null);
            }
        }

        public async Task<BaseResponse<IEnumerable<QuizResponse>>> SearchQuizzesByTitleAsync(string title)
        {
            try
            {
                var quizzes = await _quizRepository.SearchQuizzesByTitleAsync(title);
                if (quizzes == null || !quizzes.Any())
                {
                    return new BaseResponse<IEnumerable<QuizResponse>>("No quizzes found matching the title", StatusCodeEnum.NotFound_404, null);
                }

                var response = _mapper.Map<IEnumerable<QuizResponse>>(quizzes);
                return new BaseResponse<IEnumerable<QuizResponse>>("Successfully retrieved quizzes", StatusCodeEnum.OK_200, response);
            }
            catch (Exception ex)
            {
                return new BaseResponse<IEnumerable<QuizResponse>>($"An error occurred: {ex.Message}", StatusCodeEnum.InternalServerError_500, null);
            }
        }

        public async Task<BaseResponse<string>> DeleteQuizAsync(int quizId)
        {
            try
            {
                var quiz = await _quizRepository.GetByIdAsync(quizId);
                if (quiz == null)
                {
                    return new BaseResponse<string>("Quiz not found", StatusCodeEnum.NotFound_404, null);
                }

                await _quizRepository.DeleteQuizAsync(quizId);
                return new BaseResponse<string>("Quiz deleted successfully", StatusCodeEnum.OK_200, "Deleted");
            }
            catch (Exception ex)
            {
                return new BaseResponse<string>($"An error occurred: {ex.Message}", StatusCodeEnum.InternalServerError_500, null);
            }
        }
    }
}
