using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookAPI.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer;
using BookAPI.Interface;
using BookAPI.Services;
using Swashbuckle.AspNetCore.Filters;
using NSwag.AspNetCore;
using System.Reflection;
using NJsonSchema;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Swashbuckle.AspNetCore.Swagger;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication;

namespace BookAPI
{
    public class Startup
    {
        //private object appSettingsSection;
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            var connection = Configuration.GetConnectionString("Book");

            var appSettings = Configuration.GetSection("AppSettings:Secret").Value;
            var key = Encoding.ASCII.GetBytes(appSettings);

            services.AddDbContext<BookApiDataContext>(options => options.UseSqlServer(connection));

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Swashbuckle.AspNetCore.Swagger.Info
                {
                    Version = "V1",
                    Title = "Book API",
                    Description = "Book application API",
                    TermsOfService = "None",
                    Contact = new Swashbuckle.AspNetCore.Swagger.Contact
                    {
                        Name = "Cyber Interns",
                        Email = "cyberinters@slack.com",
                        Url = "cyberinterns.slack.com"
                    },
                });



                //c.AddSecurityDefinition("Oauth2", new ApiKeyScheme
                c.AddSecurityDefinition("Bearer", new ApiKeyScheme()
                {
                    Description = "JWT Authorization header {token}",
                    Name = "Authorization",
                    In = "header",
                    Type = "apiKey"
                });
                //c.AddSecurityDefinition("Bearer", new ApiKeyScheme
                //{


                //    Description = "Standard Authorization header using the Bearer scheme. Example:\"bearer {token}",
                //    In  = "Headers",
                //    Name = "Authorization",
                //    Type = "apiKey",
                   
                //    //AuthenticationScheme =  "Bearer"

                //});

                c.OperationFilter<SecurityRequirementsOperationFilter>();
            });

            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });

            services.AddScoped<IAuthor, AuthorService>();
            services.AddScoped<IBook, BookService>();
            services.AddScoped<ICategory, CategoryService>();
            services.AddScoped<IGenre, GenreService>();
            services.AddScoped<IPublisher, PublisherService>();
            services.AddScoped<IUser, UserService>();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseSwagger();
            // Enable the Swagger UI middleware and the Swagger generator
            //app.UseSwaggerUI();
            app.UseSwaggerUi3(typeof(Startup).GetTypeInfo().Assembly, settings =>
            {
                settings.GeneratorSettings.DefaultPropertyNameHandling = PropertyNameHandling.CamelCase;

            });
           

            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}
