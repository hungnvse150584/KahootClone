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
using Services.RequestAndResponse.Request.AnswerRequest;
using Services.RequestAndResponse.Response;

namespace Services.Service
{
    public class AnswerService : IAnswerService
    {
        private readonly IAnswerRepository _answerRepository;
        private readonly IMapper _mapper;

        public AnswerService(IAnswerRepository answerRepository, IMapper mapper)
        {
            _answerRepository = answerRepository;
            _mapper = mapper;
        }

        public async Task<BaseResponse<AnswerResponse>> CreateAnswerAsync(CreateAnswerRequest request)
        {
            try
            {
                var answer = _mapper.Map<Answer>(request);
                await _answerRepository.AddAsync(answer);
                await _answerRepository.SaveChangesAsync();

                var response = _mapper.Map<AnswerResponse>(answer);
                return new BaseResponse<AnswerResponse>("Answer created successfully", StatusCodeEnum.Created_201, response);
            }
            catch (Exception ex)
            {
                return new BaseResponse<AnswerResponse>($"An error occurred: {ex.Message}", StatusCodeEnum.InternalServerError_500, null);
            }
        }

        public async Task<BaseResponse<AnswerResponse>> UpdateAnswerAsync(UpdateAnswerRequest request)
        {
            try
            {
                var existing = await _answerRepository.GetByIdAsync(request.AnswerId);
                if (existing == null)
                {
                    return new BaseResponse<AnswerResponse>("Answer not found", StatusCodeEnum.NotFound_404, null);
                }

                // Cập nhật thông tin
                existing.Text = request.Text;
                existing.IsCorrect = request.IsCorrect;
                existing.QuestionId = request.QuestionId;

                await _answerRepository.UpdateAsync(existing);
                await _answerRepository.SaveChangesAsync();

                var response = _mapper.Map<AnswerResponse>(existing);
                return new BaseResponse<AnswerResponse>("Answer updated successfully", StatusCodeEnum.OK_200, response);
            }
            catch (Exception ex)
            {
                return new BaseResponse<AnswerResponse>($"An error occurred: {ex.Message}", StatusCodeEnum.InternalServerError_500, null);
            }
        }

        public async Task<BaseResponse<AnswerResponse>> GetAnswerByIdAsync(int answerId)
        {
            try
            {
                var answer = await _answerRepository.GetByIdAsync(answerId);
                if (answer == null)
                {
                    return new BaseResponse<AnswerResponse>("Answer not found", StatusCodeEnum.NotFound_404, null);
                }

                var response = _mapper.Map<AnswerResponse>(answer);
                return new BaseResponse<AnswerResponse>("Successfully retrieved answer", StatusCodeEnum.OK_200, response);
            }
            catch (Exception ex)
            {
                return new BaseResponse<AnswerResponse>($"An error occurred: {ex.Message}", StatusCodeEnum.InternalServerError_500, null);
            }
        }

        public async Task<BaseResponse<IEnumerable<AnswerResponse>>> GetAnswersByQuestionIdAsync(int questionId)
        {
            try
            {
                var answers = await _answerRepository.GetAnswersByQuestionIdAsync(questionId);
                var response = _mapper.Map<IEnumerable<AnswerResponse>>(answers);

                return new BaseResponse<IEnumerable<AnswerResponse>>("Successfully retrieved answers", StatusCodeEnum.OK_200, response);
            }
            catch (Exception ex)
            {
                return new BaseResponse<IEnumerable<AnswerResponse>>($"An error occurred: {ex.Message}", StatusCodeEnum.InternalServerError_500, null);
            }
        }

        public async Task<BaseResponse<string>> DeleteAnswerAsync(int answerId)
        {
            try
            {
                var answer = await _answerRepository.GetByIdAsync(answerId);
                if (answer == null)
                {
                    return new BaseResponse<string>("Answer not found", StatusCodeEnum.NotFound_404, null);
                }

                await _answerRepository.DeleteAsync(answer);
                await _answerRepository.SaveChangesAsync();

                return new BaseResponse<string>("Answer deleted successfully", StatusCodeEnum.OK_200, "Deleted");
            }
            catch (Exception ex)
            {
                return new BaseResponse<string>($"An error occurred: {ex.Message}", StatusCodeEnum.InternalServerError_500, null);
            }
        }
    }
}
