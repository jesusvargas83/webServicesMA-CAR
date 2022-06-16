using datos.implementacion;
using datos.Implementacion;
using datos.interfaz;
using datos.Interfaz;
using logica.Implementacion;
using logica.Interfaz;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System;
using System.IO;
using System.Reflection;

namespace ccd_minagricultura
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
            // Configuraci�n para no mostrar campos con valor nulo
            services.AddMvc().AddJsonOptions(options =>
               options.JsonSerializerOptions.IgnoreNullValues = true
                );

            services.AddControllers();

            // Inyecci�n de dependencias
            services.AddScoped<IDapperConnector, DapperConnector>();
            services.AddScoped<IConsultaInformacion, ConsultaInformacion>();
            services.AddScoped<IConsultaInformacionRepository, ConsultaInformacionRepository>();

            // Agregar swagger a la configuraci�n de la soluci�n
            services.AddSwaggerGen(options =>
            {
                // Configuraci�n de la documentaci�n usando swagger
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "INTEGRACI�N CARPETA CIUDADANA DIGITAL",
                    Description = "Integraci�n Carpeta Ciudadana Digital MinAgricultura - Tr�mite Mi Registo Rural Y Carnet - (Asp.Net core 3.1)",
                    Contact = new OpenApiContact()
                    {
                        Name = "AGENCIA NACIONAL DIGITAL",
                        Url = new Uri("https://and.gov.co/"),
                        Email = "agencianacionaldigital@and.gov.co"
                    },
                    TermsOfService = new Uri("https://and.gov.co/servicios-ciudadanos-digitales/")
                });

                // Configuraci�n del uso de comentarios para la documentaci�n del endpoint
                var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();

                // Indicar que se usar� swagger y su punto de entrada de acuerdo a configuraci�n
                // creada en el m�todo ConfigureServices
                app.UseSwagger();
                app.UseSwaggerUI(options =>
                {
                    options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
                });
            }

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
