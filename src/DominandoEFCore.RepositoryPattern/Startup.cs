using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using DominandoEFCore.RepositoryPattern.Data;
using DominandoEFCore.RepositoryPattern.Data.Repositories;
using DominandoEFCore.RepositoryPattern.Domain;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;

namespace DominandoEFCore.RepositoryPattern
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
            services.AddControllers()
                .AddJsonOptions(p =>
                {
                    p.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve;
                    //p.JsonSerializerOptions.WriteIndented = true;
                });

            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IDepartamentoRepository, DepartamentoRepository>();
            
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo {Title = "DominandoEFCore.RepositoryPattern", Version = "v1"});
            });
            
            const string stringDeConexao =
                @"Server=localhost, 1433;Database=UoW;User Id=sa;Password=!123Senha;Application Name='CursoEFCore';pooling=true;";

            services.AddDbContext<ApplicationContext>(p =>
                p.UseSqlServer(stringDeConexao)
            );
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "DominandoEFCore.RepositoryPattern v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();
            
            CargaInicial(app);

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }

        private void CargaInicial(IApplicationBuilder app)
        {
            using var db = app
                .ApplicationServices
                .CreateScope()
                .ServiceProvider
                .GetRequiredService<ApplicationContext>();

            if (db.Database.EnsureCreated())
            {
                db.Departamentos.AddRange(Enumerable.Range(1, 10)
                    .Select(p => new Departamento
                    {
                        Descricao = $"Departamento - {p}",
                        Colaboradores = Enumerable.Range(1, 10)
                            .Select(x => new Colaborador
                            {
                                Nome = $"Colaborador: {x}/{p}"
                            }).ToList()
                    }));

                db.SaveChanges();
            }
        }
    }
}