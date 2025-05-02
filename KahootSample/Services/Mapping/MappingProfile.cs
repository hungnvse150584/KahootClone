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
using Services.RequestAndResponse.Request.TeamResultRequest;
using Services.RequestAndResponse.Response.TeamResultResponses;
using Services.RequestAndResponse.Request.PlayerRequest;
using Services.RequestAndResponse.Response.QuizResponses;
using Services.RequestAndResponse.Request;
using Services.RequestAndResponse.Response.UserResponse;
using Services.RequestAndResponse.Request.UserRequest;

namespace Services.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            //User
            CreateMap<CreateUserRequest, User>();
            CreateMap<UpdateUserRequest, User>();
            CreateMap<LoginRequest, User>();
            CreateMap<User, LoginResponse>();
            CreateMap<User, UserResponse>()
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId))
                .ForMember(dest => dest.Username, opt => opt.MapFrom(src => src.Username))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.CreatedAt))
                .ForMember(dest => dest.Role, opt => opt.MapFrom(src => src.Role.ToString()));



            //TeamResult
            CreateMap<CreateTeamResultRequest, TeamResultInGame>();
            CreateMap<UpdateTeamResultRequest, TeamResultInGame>();
            CreateMap<TeamResultInGame, TeamResultResponse>();
            CreateMap<TeamResultInGame, TeamRankingResponse>();

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
            //CreateMap<CreatePlayerRequest, Player>().ReverseMap();
            //    //.ForMember(dest => dest.SessionId, opt => opt.MapFrom(src => src.SessionId.Value)); // Handle nullable int;
            //CreateMap<Player, PlayerResponse>().ReverseMap();

            CreateMap<CreatePlayerRequest, Player>()
                .ForMember(dest => dest.SessionId, opt => opt.MapFrom(src => src.SessionId.Value));
            CreateMap<UpdatePlayerRequest, Player>()
                .ForMember(dest => dest.JoinedAt, opt => opt.Ignore()); // Ignore fields not in the request
            CreateMap<Player, PlayerResponse>();

            //Question mappings
            CreateMap<CreateQuestionRequest, Question>()
                 .ForMember(dest => dest.ImageUrl, opt => opt.Ignore())
                 .ForMember(dest => dest.CreatedTime, opt => opt.MapFrom(src => DateTime.UtcNow))
                 .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status ?? "Active"));

            CreateMap<UpdateQuestionRequest, Question>()
                .ForMember(dest => dest.ImageUrl, opt => opt.Ignore());

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

            //Quizz
            CreateMap<Quiz, QuizResponse>();
            CreateMap<CreateQuizRequest, Quiz>();
            CreateMap<UpdateQuizRequest, Quiz>();
            CreateMap<Response, ResponseResponse>();

        }
    }
}
