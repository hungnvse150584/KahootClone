using AutoMapper;
using BOs.Model;
using Services.RequestAndResponse.Request.AnswerRequest;
using Services.RequestAndResponse.Request.QuestionRequest;
using Services.RequestAndResponse.Request.ScoreRequest;
using Services.RequestAndResponse.Request.TeamRequest;
using Services.RequestAndResponse.Response;
using Services.RequestAndResponse.TeamResponse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Services.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Team mappings
            CreateMap<CreateTeamRequest, Team>();
            CreateMap<UpdateTeamRequest, Team>();
            CreateMap<Team, TeamResponse>()
                .ForMember(dest => dest.TotalScore, opt => opt.MapFrom(src => src.TeamMembers != null ? src.TeamMembers.Sum(tm => tm.Score) : 0))
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.UtcNow))
                .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => DateTime.UtcNow));

            // TeamMember mappings
            CreateMap<TeamMember, TeamMemberResponse>();

            // Player mappings
            CreateMap<Player, PlayerResponse>();

            //Question mappings
            CreateMap<CreateQuestionRequest, Question>();
            CreateMap<UpdateQuestionRequest, Question>(); 
            CreateMap<Question, QuestionResponse>(); 

            //Answer mappings
            CreateMap<CreateAnswerRequest, Answer>();
            CreateMap<UpdateAnswerRequest, Answer>();
            CreateMap<Answer, AnswerResponse>();

            //Score mappings
            CreateMap<CreateScoreRequest, Score>();
            CreateMap<UpdateScoreRequest, Score>();
            CreateMap<Score, ScoreResponse>();
        }
    }
}
