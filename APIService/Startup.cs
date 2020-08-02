using APIService.Model;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using System;
using System.Linq;

namespace APIService
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
            services.AddDbContext<MySqlDbContext>(options => options.UseMySql(Configuration["ConnectionStrings:ConexaoMySql"]));
            services.AddMvc(options =>
            {
                options.Filters.Add(new HeaderFilter());
            }).SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            
            services.Configure<IISOptions>(options =>
            {
                options.AutomaticAuthentication = false;
            });
            
            services.AddSwaggerGen(c =>
                {
                    c.SwaggerDoc("s1", new OpenApiInfo
                    {
                        Version = Extender.AssemblyVersion,
                        Title = "APIService",
                        Description = "ASP.NET Core Web API",
                        //TermsOfService = new Uri("https://example.com/terms"),
                        Contact = new OpenApiContact
                        {
                            Name = "Raythan Padovani",
                            Email = "raythan100@yahoo.com.br",
                            Url = new Uri("https://www.linkedin.com/in/raythan-padovani-8723a3a8/"),
                        },
                        //License = new OpenApiLicense
                        //{
                        //    Name = "Use under LICX",
                        //    Url = new Uri("https://example.com/license"),
                        //}
                    });
                    // Set the comments path for the Swagger JSON and UI.
                    c.IncludeXmlComments(Extender.PathCombinedXml);
                    c.OperationFilter<HeaderFilter>();
                    c.ResolveConflictingActions(apiDescriptions => apiDescriptions.First()); 
                });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger(c =>
            {
                c.SerializeAsV2 = true;
            });
            
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("../swagger/s1/swagger.json", "APIService");
                c.DefaultModelExpandDepth(0);
                c.DefaultModelsExpandDepth(-1);
            });

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseMvc();
        }

    }
}
