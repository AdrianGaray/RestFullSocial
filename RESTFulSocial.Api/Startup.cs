using AutoMapper;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using RESTFulSocial.Core.Interfaces;
using RESTFulSocial.Core.Options;
using RESTFulSocial.Core.Services;
using RESTFulSocial.Infrastructure.Data;
using RESTFulSocial.Infrastructure.Filters;
using RESTFulSocial.Infrastructure.Interfaces;
using RESTFulSocial.Infrastructure.Repositories;
using RESTFulSocial.Infrastructure.Services;
using System;
using System.IO;
using System.Reflection;
using System.Text;

namespace RESTFulSocial.Api
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
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            services.AddControllers(options => 
            {
                options.Filters.Add<GlobalExceptionFilter>();
            }).AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
                options.SerializerSettings.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore;
            })
            .ConfigureApiBehaviorOptions( opctions =>
            {
                //opctions.SuppressModelStateInvalidFilter = true;
            });

            services.Configure<PaginationOptions>(Configuration.GetSection("Pagination"));

            services.AddDbContext<DB_SocialMediaContext>(options =>
                           options.UseSqlServer(Configuration.GetConnectionString("ConnSocialMedia"))
                       );

            services.AddTransient<IPostService, PostService>();
            /*services.AddTransient<IPostRepository, PostRepository>(); 
            services.AddTransient<IUserRepository, UserRepository>();*/
            services.AddScoped(typeof(IRepository<>), typeof(BaseRepository<>));
            services.AddTransient<IUnitOfWork, UnitOfWork>();

            services.AddSingleton<IUriService>(provider =>
           {
               var accesor = provider.GetRequiredService<IHttpContextAccessor>();
               var request = accesor.HttpContext.Request;
               var absoluteUri = string.Concat(request.Scheme, "://", request.Host.ToUriComponent());

               return new UriService(absoluteUri);
           });

            services.AddSwaggerGen(doc =>
            {
                doc.SwaggerDoc("v1", new OpenApiInfo { Title = "Social Media API", Version = "v1"});

                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml" ;
                var xmlPath = Path.Combine(AppContext.BaseDirectory,xmlFile);
                doc.IncludeXmlComments(xmlPath);
            });

            // CONFIGURACIÓN DEL SERVICIO DE AUTENTICACIÓN JWT
            services.AddAuthentication(options => 
            {
                // se define el esquemas
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                // se define el Challenge
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options => 
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    // valida el emisor
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    // Valida el tiempo del token. Aceptar los tokens que no se hayan vencido
                    ValidateLifetime = true,
                    // valida la firma del emisor
                    ValidateIssuerSigningKey = true,
                    // se lee del archivo de configuracion
                    // emisor valido
                    ValidIssuer = Configuration["Authentication:Issuer"],
                    ValidAudience = Configuration["Authentication:Audience"],
                    // especificar cual es la llave con la que nosotros estamos firmando.
                    // cual es el secretkey que estamos utilizando para firmar nuestros tokens
                    // Encoding, lo que hace cuando recibe el string(secretkey) lo convierte en un string de bytes
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Authentication:SecretKey"]))
                };
            });

            services.AddMvc(options =>
            {
                options.Filters.Add<ValidationFilter>();
            }).AddFluentValidation(options => {
                options.RegisterValidatorsFromAssemblies(AppDomain.CurrentDomain.GetAssemblies());
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseSwagger();
            app.UseSwaggerUI(options => 
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "Social Media API V1");
                options.RoutePrefix = string.Empty;
            });

            app.UseRouting();

            // AÑADIMOS EL MIDDLEWARE DE AUTENTICACIÓN
            // DE USUARIOS AL PIPELINE DE ASP.NET CORE
            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
