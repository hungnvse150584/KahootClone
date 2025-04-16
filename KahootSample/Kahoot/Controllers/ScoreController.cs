using Microsoft.AspNetCore.Mvc;
using Services.IService;
using Services.RequestAndResponse.BaseResponse;
using Services.RequestAndResponse.Enum;
using Services.RequestAndResponse.Request.ScoreRequest;
using Services.RequestAndResponse.Response.ScoreResponse;
using System.Threading.Tasks;

namespace Kahoot.Controllers
{
    [Route("api/score")]
    [ApiController]
    public class ScoreController : ControllerBase
    {
        private readonly IScoreService _scoreService;

        public ScoreController(IScoreService scoreService)
        {
            _scoreService = scoreService;
        }

        [HttpPost]
        [Route("CreateScore")]
        public async Task<ActionResult<BaseResponse<ScoreResponse>>> CreateScore([FromBody] CreateScoreRequest request)
        {
            if (request == null)
                return BadRequest(new BaseResponse<ScoreResponse>("Request body cannot be null", StatusCodeEnum.BadRequest_400, null));

            if (!ModelState.IsValid)
                return BadRequest(new BaseResponse<ScoreResponse>("Invalid request data", StatusCodeEnum.BadRequest_400, null));

            var result = await _scoreService.CreateScoreAsync(request);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpPut]
        [Route("UpdateScore/{scoreId}")]
        public async Task<ActionResult<BaseResponse<ScoreResponse>>> UpdateScore(int scoreId, [FromBody] UpdateScoreRequest request)
        {
            if (scoreId <= 0)
                return BadRequest(new BaseResponse<ScoreResponse>("Invalid Score ID", StatusCodeEnum.BadRequest_400, null));

            if (request == null)
                return BadRequest(new BaseResponse<ScoreResponse>("Request body cannot be null", StatusCodeEnum.BadRequest_400, null));

            if (!ModelState.IsValid)
                return BadRequest(new BaseResponse<ScoreResponse>("Invalid request data", StatusCodeEnum.BadRequest_400, null));

            var result = await _scoreService.UpdateScoreAsync(request);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpGet]
        [Route("GetScoreById/{scoreId}")]
        public async Task<ActionResult<BaseResponse<ScoreResponse>>> GetScoreById(int scoreId)
        {
            if (scoreId <= 0)
                return BadRequest(new BaseResponse<ScoreResponse>("Invalid Score ID", StatusCodeEnum.BadRequest_400, null));

            var result = await _scoreService.GetScoreByIdAsync(scoreId);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpGet]
        [Route("GetScoresByPlayerId/{playerId}")]
        public async Task<ActionResult<BaseResponse<IEnumerable<ScoreResponse>>>> GetScoresByPlayerId(int playerId)
        {
            if (playerId <= 0)
                return BadRequest(new BaseResponse<IEnumerable<ScoreResponse>>("Invalid Player ID", StatusCodeEnum.BadRequest_400, null));

            var result = await _scoreService.GetScoresByPlayerIdAsync(playerId);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpGet]
        [Route("GetScoresBySessionId/{sessionId}")]
        public async Task<ActionResult<BaseResponse<IEnumerable<ScoreResponse>>>> GetScoresBySessionId(int sessionId)
        {
            if (sessionId <= 0)
                return BadRequest(new BaseResponse<IEnumerable<ScoreResponse>>("Invalid Session ID", StatusCodeEnum.BadRequest_400, null));

            var result = await _scoreService.GetScoresBySessionIdAsync(sessionId);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpDelete]
        [Route("DeleteScore/{scoreId}")]
        public async Task<ActionResult<BaseResponse<string>>> DeleteScore(int scoreId)
        {
            if (scoreId <= 0)
                return BadRequest(new BaseResponse<string>("Invalid Score ID", StatusCodeEnum.BadRequest_400, null));

            var result = await _scoreService.DeleteScoreAsync(scoreId);
            return StatusCode((int)result.StatusCode, result);
        }
        [HttpGet]
[Route("TopScores/{topN}")]
public async Task<ActionResult<BaseResponse<IEnumerable<ScoreResponse>>>> GetTopScores(int topN)
{
    if (topN <= 0)
        return BadRequest(new BaseResponse<IEnumerable<ScoreResponse>>("Invalid number requested", StatusCodeEnum.BadRequest_400, null));

    var result = await _scoreService.GetTopScoresAsync(topN);
    return StatusCode((int)result.StatusCode, result);
}

[HttpGet]
[Route("TotalPointsByPlayer/{playerId}")]
public async Task<ActionResult<BaseResponse<int>>> GetTotalPointsByPlayer(int playerId)
{
    if (playerId <= 0)
        return BadRequest(new BaseResponse<int>("Invalid Player ID", StatusCodeEnum.BadRequest_400, 0));

    var result = await _scoreService.GetTotalPointsByPlayerAsync(playerId);
    return StatusCode((int)result.StatusCode, result);
}
    }
}
