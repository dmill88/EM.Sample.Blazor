using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using EM.Sample.DomainLogic;
using EM.Sample.DataRepository.Context;
using Microsoft.EntityFrameworkCore;
using IdentityServer4.AccessTokenValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using EM.Sample.DomainModels.Enums;
using IdentityModel;

namespace EM.Sample.WebApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            string dbConnString = Configuration.GetConnectionString("DefaultConnection");
            //services.AddDbContext<ContentleverageContext>(options => options.UseSqlServer(dbConnString));
            DbContextOptionsBuilder<ContentleverageContext> dbCntxOptsBuilder = new DbContextOptionsBuilder<ContentleverageContext>();
            var dbCnxOptions = dbCntxOptsBuilder.UseSqlServer(dbConnString).Options;

            services.AddTransient<IAuthorCommands, AuthorCommands>((scope) =>
            {
                return new AuthorCommands(new ContentleverageContext(dbCnxOptions));
            });
            services.AddTransient<IAuthorQueries, AuthorQueries>((scope) =>
            {
                return new AuthorQueries(new ContentleverageContext(dbCnxOptions));
            });
            services.AddTransient<IBlogCommands, BlogCommands>((scope) =>
            {
                return new BlogCommands(new ContentleverageContext(dbCnxOptions));
            });
            services.AddTransient<IBlogQueries, BlogQueries>((scope) =>
            {
                return new BlogQueries(new ContentleverageContext(dbCnxOptions));
            });

            string[] localHostOriginsDev = GetUrisFromConfig("CORS:Development");
            string[] localHostOriginsStaging = GetUrisFromConfig("CORS:Staging");
            string[] localHostOriginsProd = GetUrisFromConfig("CORS:Production");

            services.AddCors(options => {
                options.AddPolicy("Development", config =>
                {
                    config.WithOrigins(localHostOriginsDev).AllowAnyHeader().AllowAnyMethod().AllowCredentials();
                });
                options.AddPolicy("Staging", config =>
                {
                    config.WithOrigins(localHostOriginsStaging).AllowAnyHeader().AllowAnyMethod().AllowCredentials();
                });
                options.AddPolicy("Production", config =>
                {
                    config.WithOrigins(localHostOriginsProd).AllowAnyHeader().AllowAnyMethod().AllowCredentials();
                });
            });

            services.AddControllers();
            
            // Register the Swagger generator, defining 1 or more Swagger documents
            services.AddSwaggerGen();
        }


        private string[] ConvertToListOfRoleNames(params UserRoles[] roles)
        {
            string[] rolesList = roles.Select(i => i.ToString()).ToArray();
            return rolesList;
        }


        private string[] GetUrisFromConfig(string key)
        {
            IEnumerable<string> stringArray = Configuration.GetSection(key).GetChildren().Select(i => {
                string uriBase = i.Value;
                if (uriBase.EndsWith("/"))
                    uriBase = uriBase.Substring(0, uriBase.LastIndexOf("/"));
                return uriBase;
            });
            return stringArray.ToArray();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseExceptionHandler("/error-local-development");
            }
            else
            {
                app.UseExceptionHandler("/error");
            }

            app.UseCors(env.EnvironmentName);

            app.UseHttpsRedirection();

            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Eight Monkeys Test Blog API");
                c.RoutePrefix = string.Empty; // To serve the Swagger UI at the app's root (http://localhost:<port>/), set the RoutePrefix property to an empty string:
            });

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
