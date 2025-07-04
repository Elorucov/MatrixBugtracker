using MatrixBugtracker.Abstractions;
using MatrixBugtracker.API.Middlewares;
using MatrixBugtracker.API.ProviderImpls;
using MatrixBugtracker.BL.Converters;
using MatrixBugtracker.BL.Extensions;
using MatrixBugtracker.DAL.Extensions;
using MatrixBugtracker.Domain.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http.Json;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using System.Text;
using System.Text.Json.Serialization;

namespace MatrixBugtracker.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.Configure<EmailConfig>(builder.Configuration.GetSection("EmailConfig"));
            builder.Services.AddTransient<ExceptionHandlerMiddleware>();
            builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            builder.Services.AddScoped<IUserIdProvider, UserIdProvider>();
            builder.Services.AddRepositories(builder.Configuration);
            builder.Services.AddServices();

            builder.Services.Configure<JsonOptions>(options =>
                options.SerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingDefault | JsonIgnoreCondition.WhenWritingNull);

            builder.Services.AddControllers().ConfigureApiBehaviorOptions(options =>
            {
                options.SuppressModelStateInvalidFilter = true;
            }).AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingDefault | JsonIgnoreCondition.WhenWritingNull;
                options.JsonSerializerOptions.Converters.Add(new CustomEnumConverter());
            });

            // Logger
            builder.Host.UseSerilog((context, configuration) =>
                configuration.ReadFrom.Configuration(context.Configuration));

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(opt =>
            {
                opt.SwaggerDoc("v1", new OpenApiInfo { Title = "Matrix Bugtracker", Version = "v1" });
                opt.DescribeAllParametersInCamelCase();
                opt.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Please enter token",
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    BearerFormat = "JWT",
                    Scheme = "bearer"
                });
                opt.AddSecurityRequirement(new OpenApiSecurityRequirement
                    {
                        {
                            new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" }
                            },
                            new List<string>()
                        }
                    });
            });

            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(opt =>
            {
                opt.TokenValidationParameters = new TokenValidationParameters
                {
                    RequireExpirationTime = true,
                    ValidateIssuer = true,
                    ValidateAudience = false,
                    ValidateIssuerSigningKey = true,
                    ValidateLifetime = true,
                    ValidIssuer = builder.Configuration["Jwt:Issuer"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:SecretKey"])),
                    ClockSkew = TimeSpan.Zero
                };
            });

            var app = builder.Build();
            app.UseMiddleware<ExceptionHandlerMiddleware>();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();
            app.Run();
        }
    }
}
