using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Mvc.Cors.Internal;
using RestFullAPI.Options;
using Swashbuckle.AspNetCore.Swagger;
using RestFullAPI.Models;
using Microsoft.AspNetCore.Identity;
using RestFullAPI.Areas.Identity.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace RestFullAPI
{
    public class Startup
    {
        readonly string MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        //This method gets called by the runtime.Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddEntityFrameworkSqlServer()
                       .AddDbContext<RestFullAPIContext>((serviceProvider, options) =>
           options.UseSqlServer("Server=A003-00058\\SQLSERVER;Database=RestFullAPI;User ID=sa;password=1234@zubair")
                  .UseInternalServiceProvider(serviceProvider));

            //services.AddDefaultIdentity<RestFullAPIUser>()
            //.AddEntityFrameworkStores<RestFullAPIContext>();


            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;

            })
                .AddJwtBearer(options =>
                {
                    options.SaveToken = true;
                    options.RequireHttpsMetadata = false;
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidAudience = "localhost",
                        ValidIssuer = "localhost",
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("this is my custom Secret key for authnetication"))
                    };
                });



            //services.AddTransient<IUserContext, SeedUserContext>();
            services.AddMvc();

            // Adding Swagger
            services.AddSwaggerGen(option => { option.SwaggerDoc("v1", new Info { Title = "RestFullAPI Api", Version = "v1.0.0" }); });

            services.AddCors(options =>
            {
                options.AddPolicy(MyAllowSpecificOrigins,
                builder =>
                {
                    builder.AllowAnyOrigin();
                });
            });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env
                                , UserManager<RestFullAPIUser> userManager)
        {
            app.UseStaticFiles();
            app.UseAuthentication();
            SeedDataInitializer.SeedData(userManager);
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            var swaggerOptions = new RestFullAPI.Options.SwaggerOptions();
            Configuration.GetSection(nameof(RestFullAPI.Options.SwaggerOptions)).Bind(swaggerOptions);



            //m Swagger Configurations
            app.UseSwagger(option => { option.RouteTemplate = swaggerOptions.JsonRoute; });
            app.UseSwaggerUI(option => { option.SwaggerEndpoint(swaggerOptions.UIEndpoint, swaggerOptions.Description); });

            app.UseHttpsRedirection();
            app.UseMvc();
            app.UseCors(MyAllowSpecificOrigins);

        }
    }
}
