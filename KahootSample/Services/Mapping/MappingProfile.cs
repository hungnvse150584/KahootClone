using AutoMapper;
using BOs.Model;

using Services.RequestAndResponse.Request.QuestionRequest;
using Services.RequestAndResponse.Request.GameSessionRequest;
using Services.RequestAndResponse.Request.QuestionRequest;
using Services.RequestAndResponse.Request.ResponseRequest;

using Services.RequestAndResponse.Request.TeamRequest;
using Services.RequestAndResponse.Response.GameSessionResponses;
using Services.RequestAndResponse.Response.ResponseResponses;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Services.RequestAndResponse.Response.QuestionResponses;
using Services.RequestAndResponse.Response.TeamResponse;
using Services.RequestAndResponse.PlayerRequest;
using Services.RequestAndResponse.Response.PlayerResponse;
using Services.RequestAndResponse.Request.QuestionInGameRequest;
using Services.RequestAndResponse.Response.QuestionInGameResponse;

namespace Services.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Ánh xạ cho Response
            CreateMap<CreateResponseRequest, Response>().ReverseMap();
            CreateMap<UpdateResponseRequest, Response>().ReverseMap();
            CreateMap<Response, ResponseResponse>().ReverseMap();

            //GameSession 
            CreateMap<CreateGameSessionRequest, GameSession>().ReverseMap();
            CreateMap<UpdateGameSessionRequest, GameSession>().ReverseMap();
            CreateMap<GameSession, GameSessionResponse>().ReverseMap();
            // Team mappings
            CreateMap<CreateTeamRequest, Team>().ReverseMap();
            CreateMap<UpdateTeamRequest, Team>().ReverseMap();
            CreateMap<Team, TeamResponse>()          
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.UtcNow))
                .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => DateTime.UtcNow));

           
            // Player mappings
            CreateMap<CreatePlayerRequest, Player>().ReverseMap();
            CreateMap<Player, PlayerResponse>().ReverseMap();

            //Question mappings
            CreateMap<CreateQuestionRequest, Question>();
            CreateMap<UpdateQuestionRequest, Question>(); 
            CreateMap<Question, QuestionResponse>();
            CreateMap<Response, ResponseResponse>();

            CreateMap<CreateQuestionRequest, Question>().ReverseMap();

            CreateMap<UpdateQuestionRequest, Question>().ReverseMap(); 
            CreateMap<Question, QuestionResponse>().ReverseMap();
            CreateMap<Response, ResponseResponse>().ReverseMap();

            //QuestionInGame mappings
            CreateMap<CreateQuestionInGameRequest, QuestionInGame>();
            CreateMap<UpdateQuestionInGameRequest, QuestionInGame>();
            CreateMap<QuestionInGame, QuestionInGameResponse>();
            CreateMap<Response, ResponseResponse>();
        }
    }
}
