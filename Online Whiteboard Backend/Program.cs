
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.BearerToken;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.OpenApi.Models;
using Online_Whiteboard_Backend.Hubs;
using Online_Whiteboard_Backend.Models;

namespace Online_Whiteboard_Backend
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            string hubPath = "/socket";


            // Add services to the container.


            var connectionString = builder.Configuration["DefaultConnection"];

            builder.Services.AddDbContext<ApplicationDbContext>(options =>
               options.UseSqlServer(connectionString));

            builder.Services.AddDbContext<WhiteboardContext>(options =>
               options.UseSqlServer(connectionString));


            //builder.Services.AddIdentityCore<IdentityUser>(options =>
            //{
            //    options.Password.RequireDigit = true;
            //    options.Password.RequireLowercase = true;
            //    options.Password.RequireUppercase = true;
            //    options.Password.RequireNonAlphanumeric = true;
            //    options.Password.RequiredLength = 6;
            //}).AddEntityFrameworkStores<ApplicationDbContext>();
            builder.Services.AddHttpContextAccessor();

            



            


            builder.Services.AddAuthentication(options =>
            {
                options.DefaultSignInScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                //options.SchemeMap["Bearer"]
                //options.DefaultAuthenticateScheme = "Bearer";
            }).AddJwtBearer(options =>
            {

                //options.Authority = "local auth";

                //options.ForwardSignIn = 
                //options.ForwardDefault = "BearerToken";

                //options.Events = new JwtBearerEvents
                //{
                //    OnMessageReceived = context =>
                //    {
                //        for (int i = 0; i < 10; i++)
                //        {
                //            Console.WriteLine("jbe");
                //        }
                //        var accessToken = context.Request.Query["access_token"];
                //        if (!string.IsNullOrEmpty(accessToken) && context.HttpContext.Request.Path.StartsWithSegments(hubPath))
                //        {
                //            context.Token = accessToken;
                //        }
                //        return Task.CompletedTask;
                //    }
                //};
                options.Validate();

            });


            builder.Services.AddIdentityApiEndpoints<IdentityUser>(options => {
                options.User.RequireUniqueEmail = true;
            })
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddSignInManager<CustomSignInManager<IdentityUser>>();

            builder.Services.AddAuthorization(options =>
            {
                options.AddPolicy("optionalAuthentication", policy =>
                {
                    policy.Requirements.Add(new OptionalAuthenticationRequirement());
                });
            });

            builder.Services.AddSingleton<IAuthorizationHandler, OptionalAuthenticationHandler>();


            //builder.Services.AddAuthenticationCore();

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(options =>
            {
                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Please enter token",
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    BearerFormat = "JWT",
                    Scheme = "bearer"
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
                    new string[] { }
                }
            });
            }
            );

            builder.Services.AddSignalR(options =>
            {
                Console.WriteLine(options.SupportedProtocols[0]);
            });


            builder.Services.AddSingleton<WhiteboardStatemachine>();

            builder.Services.AddLogging();

            var app = builder.Build();


            app.MapCustomIdentityApi<IdentityUser>();
            //app.MapIdentityApi<IdentityUser>();

            app.MapPost("/logout", async (SignInManager<IdentityUser> signInManager, [FromBody] object empty) =>
            {
                if (empty != null)
                {
                    await signInManager.SignOutAsync();
                    return Results.Ok();
                }
                return Results.Unauthorized();
            }).RequireAuthorization();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();


            app.UseAuthentication();
            app.UseAuthorization();

            app.UseFileServer();

            app.MapControllers();

            app.MapHub<WhiteboardHub>(hubPath);

            app.Run();
        }
    }
}
