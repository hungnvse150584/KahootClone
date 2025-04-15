using Services.RequestAndResponse.BaseResponse;
using Services.RequestAndResponse.Request.TeamMemberRequest;
using Services.RequestAndResponse.Response;
using Services.RequestAndResponse.Response.TeamMemberResponses;

public interface ITeamMemberService
{
    Task<BaseResponse<TeamMemberResponse>> CreateTeamMemberAsync(CreateTeamMemberRequest request);
    Task<BaseResponse<TeamMemberResponse>> UpdateTeamMemberAsync(int teamMemberId, UpdateTeamMemberRequest request);
    Task<BaseResponse<TeamMemberResponse>> GetTeamMemberByIdAsync(int teamMemberId);
    Task<BaseResponse<IEnumerable<TeamMemberResponse>>> GetTeamMembersByTeamIdAsync(int teamId);
    Task<BaseResponse<IEnumerable<TeamMemberResponse>>> GetTeamMembersByPlayerIdAsync(int playerId);
    Task<BaseResponse<string>> DeleteTeamMemberAsync(int teamMemberId);
    Task<BaseResponse<IEnumerable<TeamMemberResponse>>> GetAllTeamMembersAsync();
}