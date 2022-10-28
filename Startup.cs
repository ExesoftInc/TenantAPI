using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Swagger;
using NLog;
using System.IO;
using TenantAPI.Entities;
using TenantAPI.Services;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TenantAPI.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Net;
using Microsoft.AspNetCore.Diagnostics;
using TenantAPI.Helper;
using Microsoft.AspNetCore.Http;
using TenantAPI.Models;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.FileProviders;

namespace TenantAPI
{
    public class Startup
    {
        readonly string MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
        private CorsPolicy GenerateCorsPolicy()
        {
            var builder = new CorsPolicyBuilder();
            builder.AllowAnyHeader();
            builder.AllowAnyMethod();
            //builder.AllowAnyOrigin(); // For anyone access.
            builder.WithOrigins("https://localhost"
                , "http://localhost:4200"
                , "https://localhost:4200"
                , "https://instantwebapi.com");
            builder.AllowCredentials();

            return builder.Build();
        }
        public Startup(IConfiguration configuration)
        {
            LogManager.LoadConfiguration(String.Concat(Directory.GetCurrentDirectory(), "/nlog.config"));
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<AppSettings>(Configuration);
            //added for Swagger
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Tenant API", Version = "v1" });
                c.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "value should be \"bearer <client token>\"",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement()
                {
                    [new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        },
                        Scheme = "oauth2",
                        Name = "Bearer",
                        In = ParameterLocation.Header
                    }] = new List<string>()
                });
            });


            services.AddDbContext<EntitiesContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("EntitiesConnection")
            ));

            //email services
            services.AddTransient<IEmailSender, SendGridEmailSender>();
            services.Configure<SendGridEmailSenderOptions>(options =>
            {
                options.ApiKey = Configuration["AuthorizeClient:SendGrid:ClientSecret"];
                options.SenderEmail = Configuration["AuthorizeClient:SendGrid:ClientId"];
                options.SenderName = Configuration["AuthorizeClient:SendGrid:ClientName"];
            });

            services.ConfigureServices();

            services.AddCors(options =>
            {
                options.AddPolicy(name: MyAllowSpecificOrigins
                                  , GenerateCorsPolicy());
            });
            //services.AddCors();
            services.AddControllers();

            services.AddScoped<IAuthenticate, Authenticate>();
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                .AddJwtBearer(options =>
                {
                    options.SaveToken = true;
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(
                            Encoding.ASCII.GetBytes(Configuration.GetSection("Token").Value)),
                        ValidateIssuer = false,
                        ValidateAudience = false
                    };
                });

            services.AddAuthorization(config =>
            {
                config.AddPolicy(Policies.Master, Policies.MasterPolicy());
                config.AddPolicy(Policies.Manager, Policies.AdminPolicy());
                config.AddPolicy(Policies.Crew, Policies.CrewPolicy());
                config.AddPolicy(Policies.User, Policies.UserPolicy());
            });
            services.AddHttpContextAccessor();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            //added for Swagger
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                if (env.IsDevelopment())
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Tenant API");
                }
                else
                {
                    //c.RoutePrefix = "/eStoreAPI";// add your virtual path here.
                    c.SwaggerEndpoint("/TenantAPI/swagger/v1/swagger.json", "Tenant API");
                }
            });

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler(builder =>
                {
                    builder.Run(async context =>
                    {
                        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

                        var error = context.Features.Get<IExceptionHandlerFeature>();
                        if (error != null)
                        {
                            context.Response.AddApplicationError(error.Error.Message);
                            await context.Response.WriteAsync(error.Error.Message);
                        }
                    });
                });
            }

            app.UseHsts(hsts => hsts.MaxAge(365));
            app.UseXContentTypeOptions();
            app.UseReferrerPolicy(opts => opts.NoReferrer());

            const string cacheMaxAge = "604800"; //7 days
            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(
                    Path.Combine(env.ContentRootPath, Globals.STATIC_FOLDER)),
                RequestPath = "/StaticFiles"
                , //For caching
                OnPrepareResponse = ctx =>
                {
                    ctx.Context.Response.Headers.Append("Cache-Control", $"public, max-age={cacheMaxAge}");
                }
            });
            //Registered after static files, to set headers for dynamic content.
            app.UseXfo(xfo => xfo.Deny());

            app.UseRedirectValidation(opts =>
            {
                opts.AllowedDestinations("https://localhost:4200/#/");
            });
            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();
            app.UseCors(MyAllowSpecificOrigins);

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}