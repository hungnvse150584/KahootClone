using BOs.Model;
using DAO.BaseDAO;
using DAO.IBaseDAO;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAO
{
    public static class ConfigureService
    {
        public static IServiceCollection ConfigureDataAccessObjectService(this IServiceCollection services, IConfiguration configuration)
        {

            services.AddScoped<Team>();

            services.AddScoped<User>();

            services.AddScoped<Quiz>();

            services.AddScoped<Question>();
            return services;
        }
    }
}
