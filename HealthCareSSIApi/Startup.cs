using HealthSSI.Core;
using HealthSSI.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Swagger;
using System.Collections.Generic;

namespace HealthCareSSIApi
{
    public class Startup
    {
        public Startup(Microsoft.AspNetCore.Hosting.IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{GetEnvironmentName(env)}.json", optional: true)
                .AddJsonFile(@"C:\Program Files\Amazon\ElasticBeanstalk\config\containerconfiguration", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables();

            var config = builder.Build();

            builder.AddInMemoryCollection(ParseEbConfig(config));

            Configuration = builder.Build();
        }

        public IConfiguration Configuration { get; }

        private static Dictionary<string, string> ParseEbConfig(IConfiguration config)
        {
            Dictionary<string, string> dict = new Dictionary<string, string>();

            foreach (IConfigurationSection pair in config.GetSection("iis:env").GetChildren())
            {
                string[] keypair = pair.Value.Split(new[] { '=' }, 2);
                dict.Add(keypair[0], keypair[1]);
            }

            return dict;
        }

        /// <summary>
        /// Get the EnvironmentName whether running local or on an AWS EBS instance.
        /// In AWS, this would be an environment variable - ASPNETCORE_ENVIRONMENT
        /// </summary>
        /// <param name="env"></param>
        /// <returns>environment name (string) ex. "Development", "Production", etc...</returns>
        private static string GetEnvironmentName(Microsoft.AspNetCore.Hosting.IHostingEnvironment env)
        {
            const string awsContainerConfigLocation = @"C:\Program Files\Amazon\ElasticBeanstalk\config\containerconfiguration";
            const string aspNetCoreEnvironment = "ASPNETCORE_ENVIRONMENT";
            const string configSection = "iis:env";

            var config = new ConfigurationBuilder()
                .AddJsonFile(awsContainerConfigLocation, optional: true, reloadOnChange: true).Build();

            Dictionary<string, string> dict = new Dictionary<string, string>();
            foreach (IConfigurationSection pair in config.GetSection(configSection).GetChildren())
            {
                string[] keypair = pair.Value.Split(new[] { '=' }, 2);
                dict.Add(keypair[0], keypair[1]);
            }

            return dict.ContainsKey(aspNetCoreEnvironment)
                ? dict[aspNetCoreEnvironment]
                : env.EnvironmentName;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "Health SSI",
                    Description = "SSI HealthCare Platform",
                });
            });

            services.AddDbContext<SSIDbContext>(options => options
                .UseSqlServer(Configuration.GetConnectionString("DefaultConnection"), b => b.MigrationsAssembly("HealthCareSSIApi"))
            );

            services.AddScoped<IHospitalService, HospitalService>();
            services.AddScoped<IInsuraceCoService, InsuranceCoService>();
            services.AddScoped<IPatientService, PatientService>();
            services.AddScoped<IDocumentService, DocumentService>();
            services.AddScoped<ISignatureService, SignatureService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            });

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
