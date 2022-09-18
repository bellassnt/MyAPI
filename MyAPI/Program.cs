using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using MyAPI.AuthorizationAndAuthentication;
using MyAPI.Context;
using MyAPI.Filters;
using MyAPI.Interfaces;
using MyAPI.Repositories;
using System.Net;
using System.Text;

namespace MyAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            #region Add filters
            builder.Services.AddControllers(options =>
            {
                options.Filters.Add(typeof(CustomExceptionFilter));
                options.Filters.Add(typeof(CustomLogsFilter));
            });
            #endregion

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();

            #region Configure Swagger for authorization 
            builder.Services.AddSwaggerGen(options =>
            {
                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "Enter your token in the text input below.",
                });
                options.AddSecurityRequirement(new OpenApiSecurityRequirement
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
                    Array.Empty<string>()
                    }
                });
            });
            #endregion

            builder.Services.AddDbContext<InMemoryContext>(options => options.UseInMemoryDatabase("MyAPI"));

            var tokenConfiguration = new TokenConfiguration();
            new ConfigureFromConfigurationOptions<TokenConfiguration>(builder.Configuration.GetSection("TokenConfiguration")).Configure(tokenConfiguration);
            builder.Services.AddSingleton(tokenConfiguration);
            var tokenService = new TokenGenerator(tokenConfiguration);

            #region Add dependency injection
            builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>))
                .AddScoped(typeof(IUserRepository), typeof(UserRepository))
                .AddScoped(typeof(TokenGenerator));
            #endregion

            #region Add middleware
            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ClockSkew = TimeSpan.Zero,                    
                    ValidateIssuer = true,
                    ValidateLifetime = true,
                    ValidateAudience = true,
                    ValidateIssuerSigningKey = true,

                    ValidIssuer = tokenConfiguration.Issuer,
                    ValidAudience = tokenConfiguration.Audience,                    
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenConfiguration.Secret!))
                };

                options.Events = new JwtBearerEvents
                {
                    OnAuthenticationFailed = context =>
                    {
                        if (context.Exception.GetType() == typeof(SecurityTokenException))
                        {
                            context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;                            
                        }

                        return Task.CompletedTask;
                    }
                };
            });
            #endregion

            builder.Services.AddScoped<DataGenerator>();

            var app = builder.Build();

            // Configure the HTTP request pipeline
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            #region Redirect Http requests to Https ones
            app.UseHttpsRedirection();
            #endregion

            app.UseAuthentication();

            app.UseAuthorization();

            app.MapControllers();

            #region Generate data
            var scopedFactory = app.Services.GetService<IServiceScopeFactory>();

            using (var scope = scopedFactory.CreateScope())
            {
                var service = scope.ServiceProvider.GetService<DataGenerator>();
                service.Generate();
            }
            #endregion

            app.Run();
        }
    }
}