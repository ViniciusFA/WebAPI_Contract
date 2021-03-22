using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.PlatformAbstractions;
using Microsoft.FeatureManagement;
using Microsoft.OpenApi.Models;
using System.IO;
using WebAPI.Data;
using WebAPI.Controllers;
using WebAPI.Controllers.InterfaceController;

namespace WebAPI
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
            services.AddDbContext<DataContext>(opt => opt.UseInMemoryDatabase("db_contracts"));
            services.AddScoped<DataContext, DataContext>();
            services.AddScoped<IContractController, ContractController>();
            services.AddControllers();
            services.AddFeatureManagement(Configuration.GetSection("FeatureManagement"));

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1",
                    new OpenApiInfo
                    {
                        Title = "Web API Contracts",
                        Version = "v1",
                        Description = "Web API to process Contracts.",
                        Contact = new OpenApiContact
                        {
                            Name = "Vinicius Andrade",
                            Email = "vinicius.fontesdeandrade@gmail.com"
                        }
                    });

                string caminhoAplicacao =
                    PlatformServices.Default.Application.ApplicationBasePath;
                string nomeAplicacao =
                    PlatformServices.Default.Application.ApplicationName;
                string caminhoXmlDoc =
                    Path.Combine(caminhoAplicacao, $"{nomeAplicacao}.xml");

                c.IncludeXmlComments(caminhoXmlDoc);
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            // Ativando middlewares para uso do Swagger
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Web API Contracts");
            });

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
