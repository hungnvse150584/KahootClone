using AutoMapper;
using BOs.Model;
using Repositories.IRepository;
using Services.IService;
using Services.RequestAndResponse.BaseResponse;
using Services.RequestAndResponse.Enum;
using Services.RequestAndResponse.Request.ResponseRequest;
using Services.RequestAndResponse.Response;
using Services.RequestAndResponse.Response.ResponseResponses;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Services.Service
{
    public class ResponseService : IResponseService
    {
        private readonly IResponseRepository _responseRepository;
        private readonly IMapper _mapper;

        public ResponseService(IResponseRepository responseRepository, IMapper mapper)
        {
            _responseRepository = responseRepository;
            _mapper = mapper;
        }

        public async Task<BaseResponse<ResponseResponse>> CreateResponseAsync(CreateResponseRequest request)
        {
            try
            {
                var response = _mapper.Map<Response>(request);
                var createdResponse = await _responseRepository.AddAsync(response);
                var responseDto = _mapper.Map<ResponseResponse>(createdResponse);
                return new BaseResponse<ResponseResponse>("Response created successfully", StatusCodeEnum.Created_201, responseDto);
            }
            catch (Exception ex)
            {
                return new BaseResponse<ResponseResponse>($"An error occurred: {ex.Message}", StatusCodeEnum.InternalServerError_500, null);
            }
        }

        public async Task<BaseResponse<ResponseResponse>> GetResponseByIdAsync(int responseId)
        {
            try
            {
                var response = await _responseRepository.GetByIdAsync(responseId);
                if (response == null)
                {
                    return new BaseResponse<ResponseResponse>("Response not found", StatusCodeEnum.NotFound_404, null);
                }

                var responseDto = _mapper.Map<ResponseResponse>(response);
                return new BaseResponse<ResponseResponse>("Response retrieved successfully", StatusCodeEnum.OK_200, responseDto);
            }
            catch (Exception ex)
            {
                return new BaseResponse<ResponseResponse>($"An error occurred: {ex.Message}", StatusCodeEnum.InternalServerError_500, null);
            }
        }

        public async Task<BaseResponse<IEnumerable<ResponseResponse>>> GetAllResponsesAsync()
        {
            try
            {
                var responses = await _responseRepository.GetAllAsync();
                if (responses == null || !responses.Any())
                {
                    return new BaseResponse<IEnumerable<ResponseResponse>>("No Responses found", StatusCodeEnum.NotFound_404, null);
                }

                var responseDtos = _mapper.Map<IEnumerable<ResponseResponse>>(responses);
                return new BaseResponse<IEnumerable<ResponseResponse>>("Successfully retrieved Responses", StatusCodeEnum.OK_200, responseDtos);
            }
            catch (Exception ex)
            {
                return new BaseResponse<IEnumerable<ResponseResponse>>($"An error occurred: {ex.Message}", StatusCodeEnum.InternalServerError_500, null);
            }
        }

        public async Task<BaseResponse<ResponseResponse>> UpdateResponseAsync(int responseId, UpdateResponseRequest request)
        {
            try
            {
                var response = await _responseRepository.GetByIdAsync(responseId);
                if (response == null)
                {
                    return new BaseResponse<ResponseResponse>($"Response with id {responseId} not found", StatusCodeEnum.NotFound_404, null);
                }

                response.PlayerId = request.PlayerId;
                response.QuestionInGameId = request.QuestionInGameId;
                response.SelectedOption = request.SelectedOption;
                response.ResponseTime = request.ResponseTime;
                response.Score = request.Score;
                response.Streak = request.Streak;

                var updatedResponse = await _responseRepository.UpdateAsync(response);
                var responseDto = _mapper.Map<ResponseResponse>(updatedResponse);

                return new BaseResponse<ResponseResponse>("Response updated successfully", StatusCodeEnum.OK_200, responseDto);
            }
            catch (ArgumentNullException ex)
            {
                return new BaseResponse<ResponseResponse>(ex.Message, StatusCodeEnum.NotFound_404, null);
            }
            catch (Exception ex)
            {
                return new BaseResponse<ResponseResponse>($"An error occurred while updating Response: {ex.Message}", StatusCodeEnum.InternalServerError_500, null);
            }
        }

        public async Task<BaseResponse<string>> DeleteResponseAsync(int responseId)
        {
            try
            {
                var response = await _responseRepository.GetByIdAsync(responseId);
                if (response == null)
                {
                    return new BaseResponse<string>("Response not found", StatusCodeEnum.NotFound_404, null);
                }

                await _responseRepository.DeleteAsync(response);
                return new BaseResponse<string>("Response deleted successfully", StatusCodeEnum.OK_200, null);
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

        public async Task<BaseResponse<IEnumerable<ResponseResponse>>> GetResponsesByPlayerIdAsync(int playerId)
        {
            try
            {
                var responses = await _responseRepository.GetResponsesByPlayerIdAsync(playerId);
                if (responses == null || !responses.Any())
                {
                    return new BaseResponse<IEnumerable<ResponseResponse>>("No Responses found for this player", StatusCodeEnum.NotFound_404, null);
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