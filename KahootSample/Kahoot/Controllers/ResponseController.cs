using Microsoft.AspNetCore.Mvc;
using Services.IService;
using Services.RequestAndResponse.BaseResponse;
using Services.RequestAndResponse.Enum;
using Services.RequestAndResponse.Request.ResponseRequest;
using Services.RequestAndResponse.Response.ResponseResponses;

namespace Kahoot.Controllers
{
    [Route("api/response")]
    [ApiController]
    public class ResponseController : ControllerBase
    {
        private readonly IResponseService _responseService;

        public ResponseController(IResponseService responseService)
        {
            _responseService = responseService;
        }

        [HttpPost]
        [Route("CreateResponse")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<BaseResponse<ResponseResponse>>> CreateResponse([FromBody] CreateResponseRequest request)
        {
            try
            {
                if (request == null)
                {
                    return BadRequest(new BaseResponse<ResponseResponse>("Request body cannot be null!", StatusCodeEnum.BadRequest_400, null));
                }

                if (!ModelState.IsValid)
                {
                    return BadRequest(new BaseResponse<ResponseResponse>("Invalid request data!", StatusCodeEnum.BadRequest_400, null));
                }

                var result = await _responseService.CreateResponseAsync(request);
                return StatusCode((int)result.StatusCode, result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new BaseResponse<ResponseResponse>($"Something went wrong! Error: {ex.Message}", StatusCodeEnum.InternalServerError_500, null));
            }
        }

        [HttpGet]
        [Route("GetResponse/{responseId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<BaseResponse<ResponseResponse>>> GetResponseById(int responseId)
        {
            try
            {
                var result = await _responseService.GetResponseByIdAsync(responseId);
                return StatusCode((int)result.StatusCode, result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new BaseResponse<ResponseResponse>($"Something went wrong! Error: {ex.Message}", StatusCodeEnum.InternalServerError_500, null));
            }
        }

        [HttpGet]
        [Route("GetAllResponses")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<BaseResponse<IEnumerable<ResponseResponse>>>> GetAllResponses()
        {
            try
            {
                var result = await _responseService.GetAllResponsesAsync();
                return StatusCode((int)result.StatusCode, result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new BaseResponse<IEnumerable<ResponseResponse>>($"Something went wrong! Error: {ex.Message}", StatusCodeEnum.InternalServerError_500, null));
            }
        }

        [HttpPut]
        [Route("UpdateResponse/{responseId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<BaseResponse<ResponseResponse>>> UpdateResponse(int responseId, [FromBody] UpdateResponseRequest request)
        {
            try
            {
                if (request == null)
                {
                    return BadRequest(new BaseResponse<ResponseResponse>("Request body cannot be null!", StatusCodeEnum.BadRequest_400, null));
                }

                if (!ModelState.IsValid)
                {
                    return BadRequest(new BaseResponse<ResponseResponse>("Invalid request data!", StatusCodeEnum.BadRequest_400, null));
                }

                var result = await _responseService.UpdateResponseAsync(responseId, request);
                return StatusCode((int)result.StatusCode, result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new BaseResponse<ResponseResponse>($"Something went wrong! Error: {ex.Message}", StatusCodeEnum.InternalServerError_500, null));
            }
        }

        [HttpDelete]
        [Route("DeleteResponse/{responseId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<BaseResponse<string>>> DeleteResponse(int responseId)
        {
            try
            {
                var result = await _responseService.DeleteResponseAsync(responseId);
                return StatusCode((int)result.StatusCode, result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new BaseResponse<string>($"Something went wrong! Error: {ex.Message}", StatusCodeEnum.InternalServerError_500, null));
            }
        }

        [HttpGet]
        [Route("GetResponsesByPlayer/{playerId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<BaseResponse<IEnumerable<ResponseResponse>>>> GetResponsesByPlayerId(int playerId)
        {
            try
            {
                var result = await _responseService.GetResponsesByPlayerIdAsync(playerId);
                return StatusCode((int)result.StatusCode, result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new BaseResponse<IEnumerable<ResponseResponse>>($"Something went wrong! Error: {ex.Message}", StatusCodeEnum.InternalServerError_500, null));
            }
        }

        [HttpGet]
        [Route("GetResponsesByQuestion/{questionId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<BaseResponse<IEnumerable<ResponseResponse>>>> GetResponsesByQuestionId(int questionId)
        {
            try
            {
                var result = await _responseService.GetResponsesByQuestionIdAsync(questionId);
                return StatusCode((int)result.StatusCode, result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new BaseResponse<IEnumerable<ResponseResponse>>($"Something went wrong! Error: {ex.Message}", StatusCodeEnum.InternalServerError_500, null));
            }
        }
    }
}
