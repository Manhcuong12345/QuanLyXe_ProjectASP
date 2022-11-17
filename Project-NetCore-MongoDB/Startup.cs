﻿using Microsoft.OpenApi.Models;
using Project_NetCore_MongoDB.Models;
using Project_NetCore_MongoDB.Repository;
using Project_NetCore_MongoDB.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Project_NetCore_MongoDB.Repository.Interface;
using Project_NetCore_MongoDB.Services.Interface;
using Project_NetCore_MongoDB.Common;
using Project_NetCore_MongoDB.Configuration;
using Abp.Dependency;
using Newtonsoft.Json.Serialization;
using Project_NetCore_MongoDB.Middleware; 

namespace Project_NetCore_MongoDB
{
    public class Startup
    {
        private const string _defaultCorsPolicyName = "localhost";

       // public Startup(IConfiguration configuration)
       // {
            //Configuration = configuration;
       // }

        public Startup(IWebHostEnvironment env)
        {
            Configuration = env.GetAppConfiguration();
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            // Default Policy
            //services.AddCors(options =>
            //{
            //    options.AddDefaultPolicy(
            //        builder =>
            //        {
            //            builder.WithOrigins("https://localhost:44351", "http://localhost:4200")
            //                                .AllowAnyHeader()
            //                                .AllowAnyMethod();
            //        });
            //});

            if (Configuration.GetSection("App") != null && Configuration.GetSection("App").GetSection("CorsOrigins") != null)
            {
                // Configure CORS for angular2 UI
                services.AddCors(
                    options => options.AddPolicy(
                        _defaultCorsPolicyName,
                        builder => builder
                            .WithOrigins(
                                // App:CorsOrigins in appsettings.json can contain more than one address separated by comma.
                                Configuration
                                    .GetSection("App")
                                    .GetSection("CorsOrigins")
                                    .GetChildren()
                                    .Select(s => s.Value)
                                    .ToArray()
                            )
                            .AllowAnyHeader()
                            .AllowAnyMethod()
                            .AllowCredentials()
                    )
                );
            }

            services.AddSwaggerGen(swagger =>
            {
                swagger.SwaggerDoc("v1", new OpenApiInfo { Title = "Blog .Net Core MongoDB", Version = "v1" });
                // To Enable authorization using Swagger (JWT)    
                swagger.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "Enter 'Bearer' [space] and then your valid token in the text input below.\r\n\r\nExample: \"Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9\"",
                });
                swagger.AddSecurityRequirement(new OpenApiSecurityRequirement
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
                             new List<string>()
                    }
                });
            });

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                    .AddJwtBearer(options => {
                        options.TokenValidationParameters = new TokenValidationParameters
                        {
                            ValidateIssuer = true,
                            ValidateAudience = true,
                            ValidateLifetime = true,
                            ValidateIssuerSigningKey = true,

                            ValidIssuer = Configuration.GetSection("Authentication:Issuer").Value,
                            ValidAudience = Configuration.GetSection("Authentication:Audience").Value,
                            IssuerSigningKey = JwtSecurityKey.Create(Configuration.GetSection("Authentication:SecurityKey").Value)
                        };

                        options.Events = new JwtBearerEvents
                        {
                            OnAuthenticationFailed = context =>
                            {
                                Console.WriteLine("OnAuthenticationFailed: " + context.Exception.Message);
                                return Task.CompletedTask;
                            },
                            OnTokenValidated = context =>
                            {
                                Console.WriteLine("OnTokenValidated: " + context.SecurityToken);
                                return Task.CompletedTask;
                            }
                        };
                    });

            //Midderware
            services.Configure<DbConfiguration>(Configuration.GetSection("MongoDbConnection"));

            //services.AddTransient<IProductsService, ProductsService>();
            //services.AddTransient<IProductsRepository, ProductsRepository>();

            //services.AddTransient<ICategoriesService, CategoriesService>();
            //services.AddTransient<ICategoriesRepository, CategoriesRepository>();

            services.AddTransient<IUsersService, UsersService>();
            services.AddTransient<IUsersRepository, UsersRepository>();

            //services.AddTransient<IRolesService, RolesService>();
            //services.AddTransient<IRolesRepository, RolesRepository>();

            //services.AddTransient<ICommentsService, CommentsService>();
            //services.AddTransient<ICommentsRepository, CommentsRepository>();

            //services.AddTransient<IArticlesService, ArticlesService>();
            //services.AddTransient<IArticlesRepository, ArticlesRepository>();

            services.AddTransient<ICarsService, CarsService>();
            services.AddTransient<ICarsRepository, CarsRepository>();

            services.AddTransient<IDriversService, DriversService>();
            services.AddTransient<IDriversRepository, DriversRepository>();

            services.AddControllers();

            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            //Roles Policy
            services.AddAuthorization(options =>
            {
                //options.AddPolicy("AdminPolicy",
                //policy => policy.RequireRole("Admin"));
                //To include multiple roles in the policy
                options.AddPolicy("AdminPolicy",
                   policy => {
                       policy.RequireAuthenticatedUser();
                       policy.RequireClaim("Admin");

                   });

                options.AddPolicy("UserPolicy",
                  policy => {
                      policy.RequireAuthenticatedUser();
                      policy.RequireClaim("User");

                  });
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

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseCors(_defaultCorsPolicyName); // Enable CORS!

            //app.UseCors(builder =>
            //{
            //    builder
            //    .AllowAnyOrigin()
            //    .AllowAnyMethod()
            //    .AllowAnyHeader();
            //});

            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "MongoDB CRUD API V1");
            });

            //app.UseMiddleware<JwtAuth>();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            
            // app.UseMvc();
        }
    }
}
