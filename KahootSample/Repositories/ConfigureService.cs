﻿using DAO;
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




            //============================================================//
            services.AddScoped<TeamDAO>();

            return services;
        }
    }
}
