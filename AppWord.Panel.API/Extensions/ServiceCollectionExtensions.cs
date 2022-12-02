using AppWord.Core.Helpers.JWT;
using AppWord.Core.IServices;
using AppWord.Core.Mapping;
using AppWord.Core.Services;
using AppWord.Data;
using AppWord.Model.Models.Options;
using AutoMapper;
using FluentValidation.AspNetCore;
using FM.Project.BaseLibrary.BaseGenericException;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Globalization;
using System.Text;

namespace AppWord.Panel.API.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection ServiceCollectionExtension(this IServiceCollection services, IConfiguration configuration)
        {

            #region Services
            services.AddScoped<IAuthenticationService, AuthenticationService>();
            services.AddScoped<ITokenHelper, JwtHelper>();
            services.AddScoped<IUserService, UserService>();
            #endregion

            #region AutoMapper
            var mapperConfig = new MapperConfiguration(mc =>
            {
                mc.AllowNullCollections = true;
                mc.AddProfile(new AppWordMapping());
            });

            IMapper mapper = mapperConfig.CreateMapper();
            services.AddSingleton(mapper);
            #endregion

            #region MemoryCache
            services.AddMemoryCache();
            #endregion

            #region Default
            services.AddControllers(options =>
            {
                options.Filters.Add(typeof(ValidateModelStateAttribute));
            })
            .AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<Program>());
            services.AddEndpointsApiExplorer();
            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.SuppressModelStateInvalidFilter = true;
            });
            #endregion

            #region FluentValidation
            services.AddFluentValidation(conf =>
            {
                conf.RegisterValidatorsFromAssembly(typeof(CacheSettings).Assembly);
            });
            #endregion

            #region Multi-Language
            services.AddLocalization();

            services.Configure<RequestLocalizationOptions>(
                options =>
                {
                    var supportedCultures = new List<CultureInfo>
                    {
                        new CultureInfo("en"),
                        new CultureInfo("tr")
                    };

                    options.DefaultRequestCulture = new RequestCulture(culture: "tr", uiCulture: "tr");
                    options.SupportedCultures = supportedCultures;
                    options.SupportedUICultures = supportedCultures;
                    options.RequestCultureProviders = new[] { new RouteDataRequestCultureProvider { IndexOfCulture = 1, IndexofUICulture = 1 } };
                });

            services.Configure<RouteOptions>(options =>
            {
                options.ConstraintMap.Add("culture", typeof(LanguageRouteConstraint));
            });
            #endregion

            #region Cors
            services.AddCors(p => p.AddPolicy("corsapp", builder =>
            {
                builder.WithOrigins("*").AllowAnyMethod().AllowAnyHeader();
            }));
            #endregion

            #region PostgreSql
            services.AddDbContext<AppWordDbContext>(options => options.UseNpgsql(configuration.GetConnectionString("SqlConnection"), npgOptions =>
                npgOptions.MigrationsAssembly("AppWord.Data")
            ));
            #endregion

            #region Swagger
            services.AddSwaggerGen();
            var securityScheme = new OpenApiSecurityScheme()
            {
                Name = "Authorization",
                Type = SecuritySchemeType.ApiKey,
                Scheme = "Bearer",
                BearerFormat = "JWT",
                In = ParameterLocation.Header,
                Description = "JSON Web Token based security",
            };

            var securityReq = new OpenApiSecurityRequirement()
{
    {
        new OpenApiSecurityScheme
        {
            Reference = new OpenApiReference
            {
                Type = ReferenceType.SecurityScheme,
                Id = "Bearer"
            }
        },
        new string[] {}
    }
};

            var contact = new OpenApiContact()
            {
                Name = "AppWord",
                Email = "fatih.mandirali@hotmail.com",
                Url = new Uri("http://www.mohamadlawand.com")
            };

            var license = new OpenApiLicense()
            {
                Name = "Free License",
                Url = new Uri("http://www.mohamadlawand.com")
            };

            var info = new OpenApiInfo()
            {
                Version = "v1",
                Title = "AppWord Panel - JWT Authentication with Swagger",
                Description = "AppWord Panel - JWT Authentication with Swagger",
                TermsOfService = new Uri("http://www.example.com"),
                Contact = contact,
                License = license
            };

            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(o =>
            {
                o.SwaggerDoc("v1", info);
                o.AddSecurityDefinition("Bearer", securityScheme);
                o.AddSecurityRequirement(securityReq);
            });
            #endregion

            #region ExceptionService
            services.AddScoped<FMExceptionCatcherMiddleware>();
            #endregion

            #region Authentication
            services.AddAuthentication(o =>
            {
                o.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                o.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                o.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(o =>
            {
                o.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidIssuer = configuration["Jwt:Issuer"],
                    ValidAudience = configuration["Jwt:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey
                        (Encoding.UTF8.GetBytes(configuration["Jwt:Key"])),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true, //expariotion for isrequired
                    ValidateIssuerSigningKey = true, //expariotion for isrequired
                    RequireExpirationTime = true, //expariotion for isrequired
                    ClockSkew = TimeSpan.Zero //expariotion for isrequired
                };
            });

            services.AddAuthorization();
            #endregion

            return services;
        }
    }
}
