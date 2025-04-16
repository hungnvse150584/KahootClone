﻿using AutoMapper;
using BOs.Model;
using Services.RequestAndResponse.Request.AnswerRequest;
using Services.RequestAndResponse.Request.QuestionRequest;
using Services.RequestAndResponse.Request.ScoreRequest;
using Services.RequestAndResponse.Request.GameSessionRequest;
using Services.RequestAndResponse.Request.QuestionRequest;
using Services.RequestAndResponse.Request.ResponseRequest;
using Services.RequestAndResponse.Request.TeamMemberRequest;
using Services.RequestAndResponse.Request.TeamRequest;
using Services.RequestAndResponse.Response.GameSessionResponses;
using Services.RequestAndResponse.Response.ResponseResponses;
using Services.RequestAndResponse.Response.TeamMemberResponses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Services.RequestAndResponse.Response.AnswerResponse;
using Services.RequestAndResponse.Response.QuestionResponses;
using Services.RequestAndResponse.Response.ScoreResponse;
using Services.RequestAndResponse.Response.TeamResponse;

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
                .ForMember(dest => dest.TotalScore, opt => opt.MapFrom(src => src.TeamMembers != null ? src.TeamMembers.Sum(tm => tm.Score) : 0))
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.UtcNow))
                .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => DateTime.UtcNow));

            // Ánh xạ cho TeamMember
            CreateMap<CreateTeamMemberRequest, TeamMember>().ReverseMap();
            CreateMap<UpdateTeamMemberRequest, TeamMember>().ReverseMap();
            CreateMap<TeamMember, TeamMemberResponse>().ReverseMap();

            // Player mappings
            

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
            CreateMap<CreateQuestionRequest, Question>().ReverseMap();

            CreateMap<UpdateQuestionRequest, Question>().ReverseMap(); 
            CreateMap<Question, QuestionResponse>().ReverseMap(); 
        }
    }
}
