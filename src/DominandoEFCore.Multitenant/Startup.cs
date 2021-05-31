using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DominandoEFCore.Multitenant.Data;
using DominandoEFCore.Multitenant.Domain;
using DominandoEFCore.Multitenant.Extensions;
using DominandoEFCore.Multitenant.Middlewares;
using DominandoEFCore.Multitenant.Provider;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;

namespace DominandoEFCore.Multitenant
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
            services.AddScoped<TenantData>();
            
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo {Title = "DominandoEFCore.Multitenant", Version = "v1"});
            });
            
            

            // Estrategia 1
            // const string stringDeConexao =
            //     @"Server=localhost, 1433;Database=EFCoreMultitenant;User Id=sa;Password=!123Senha;Application Name='CursoEFCore';pooling=true;";
            //
            // services.AddDbContext<ApplicationContext>(p => p
            //     .UseSqlServer(stringDeConexao)
            //     .LogTo(Console.WriteLine)
            //     .EnableSensitiveDataLogging()
            // );
            
            //Estrategia 3 -- Banco de dados
            services.AddHttpContextAccessor();

            // Toda vez que requisição precisar de um context 
            services.AddScoped<ApplicationContext>(provider =>
            {
                var optionBuilder = new DbContextOptionsBuilder<ApplicationContext>();

                var httpContext = provider.GetService<IHttpContextAccessor>()?.HttpContext;
                var tenantId = httpContext.GetTenantId();

                var stringDeConexao = Configuration.GetConnectionString(tenantId);

                optionBuilder
                    .UseSqlServer(stringDeConexao)
                    .LogTo(Console.WriteLine)
                    .EnableSensitiveDataLogging();

                return new ApplicationContext(optionBuilder.Options);
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "DominandoEFCore.Multitenant v1"));
            }

            //DatabaseInitialize(app);
            
            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            // app.UseMiddleware<TenantMiddleware>();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }

        // private void DatabaseInitialize(IApplicationBuilder app)
        // {
        //     using var db = app.ApplicationServices
        //         .CreateScope()
        //         .ServiceProvider
        //         .GetRequiredService<ApplicationContext>();
        //
        //     db.Database.EnsureDeleted();
        //     db.Database.EnsureCreated();
        //
        //     for (int index = 0; index <= 5; index++)
        //     {
        //         db.People.Add(new Person {Name = $"Person {index}"});
        //         db.Products.Add(new Product {Description = $"Product {index}"});
        //     }
        //
        //     db.SaveChanges();
        // }
    }
}