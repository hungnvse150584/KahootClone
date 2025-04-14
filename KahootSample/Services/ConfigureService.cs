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
<<<<<<< HEAD
            services.AddScoped<IQuestionService, QuestionService>();
=======

            services.AddScoped<IUserService, UserService>();

>>>>>>> main

            return services;
        }
    }
}
