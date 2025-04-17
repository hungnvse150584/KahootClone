using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Services.IService;
using Services.Mapping;
using Services.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public static class ConfigureService
    {
        public static IServiceCollection ConfigureServiceService(this IServiceCollection services, IConfiguration configuration)
        {

            services.AddAutoMapper(typeof(MappingProfile));

            services.AddScoped<ITeamService, TeamService>();

            services.AddScoped<IQuestionService, QuestionService>();

            services.AddScoped<IUserService, UserService>();

   
            services.AddScoped<IQuizService, QuizService>();

            services.AddScoped<IGameSessionService, GameSessionService>();

            services.AddScoped<IResponseService, ResponseService>();



            services.AddScoped<IPlayerService, PlayerService>();


            services.AddScoped<IScoreService, ScoreService>();

            return services;
        }
    }
}
