﻿using DAO;
using DAOs;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Repositories.BaseRepository;
using Repositories.IBaseRepository;
using Repositories.IRepository;
using Repositories.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories
{
    public static class ConfigureService
    {
        public static IServiceCollection ConfigureRepositoryService(this IServiceCollection services, IConfiguration configuration)
        {
            
            services.AddScoped<ITeamRepository, TeamRepository>();

            services.AddScoped<IQuestionRepository, QuestionRepository>();

            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IAnswerRepository, AnswerRepository>();

            services.AddScoped<IQuizRepository, QuizRepository>();

            services.AddScoped<IGameSessionRepository, GameSessionRepository>();

            services.AddScoped<IResponseRepository, ResponseRepository>();

            services.AddScoped<ITeamMemberRepository, TeamMemberRepository>();

            services.AddScoped<IPlayerRepository, PlayerRepository>();

            //============================================================//
            services.AddScoped<TeamDAO>();

            services.AddScoped<QuestionDAO>();

            services.AddScoped<UserDAO>();

            services.AddScoped<AnswerDAO>();

            services.AddScoped<ScoreDAO>();
            services.AddScoped<QuizDAO>();

            services.AddScoped<GameSessionDAO>();

            services.AddScoped<ResponseDAO>();

            services.AddScoped<TeamMemberDAO>();

            services.AddScoped<PlayerDAO>();
            return services;
        }
    }
}
