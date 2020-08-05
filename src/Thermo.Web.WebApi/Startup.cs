using AzCloudApp.MessageProcessor.Core.Thermo.DataStore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Thermo.Web.WebApi.Model;
using System.Linq;
using System;
using Thermo.Web.WebApi.Util;

namespace Thermo.Web.WebApi
{
    public class Startup
    {
        private const string ThermoDatabaseContext = "ThermoDatabase";
        private const string UnAuthorizedTokenValidation = "Unauthorized";
        private const string AppSettingConfigurationName = "AppSettings";
        private const string ExpiryClaimDefinition = "exp";

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var configBuilder = new ConfigurationBuilder()
           .AddJsonFile("local.settings.json", optional: true, reloadOnChange: true)
           .AddEnvironmentVariables()
           .Build();

            services.AddCors();

            services.AddControllers();

            services.AddDbContext<ThermoDataContext>(opt => opt.UseSqlServer(Configuration.GetConnectionString(ThermoDatabaseContext)));

            // configure strongly typed settings objects
            var appSettingsSection = Configuration.GetSection(AppSettingConfigurationName);
            services.Configure<AppSettings>(appSettingsSection);

            // configure jwt authentication
            var appSettings = appSettingsSection.Get<AppSettings>();
            var key = Encoding.ASCII.GetBytes(appSettings.Secret);

            services.AddAuthentication(auth =>
            {
                auth.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                auth.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
         .AddJwtBearer(jwtOptions =>
         {
             jwtOptions.Events = new JwtBearerEvents
             {
                 OnTokenValidated = context =>
                 {
                     var userIdentity = context.Principal.Identity.Name;
                     var exp = ClaimUtil.GetExpiryClaimExpiryDate(context.Principal.Claims.Where(x => x.Type == ExpiryClaimDefinition).FirstOrDefault().Value);
                    
                     var connectionString = Configuration.GetConnectionString(ThermoDatabaseContext);
                     
                     var isUserValid = IsUserAuthorized(userIdentity, connectionString, exp);

                     if (!isUserValid)
                       context.Fail(UnAuthorizedTokenValidation);

                     return Task.CompletedTask;
                 }
             };
             jwtOptions.RequireHttpsMetadata = false;
             jwtOptions.SaveToken = true;
             jwtOptions.TokenValidationParameters = new TokenValidationParameters
             {
                 ValidateIssuerSigningKey = true,
                 IssuerSigningKey = new SymmetricSecurityKey(key),
                 ValidateIssuer = false,
                 ValidateAudience = false
             };
         });
        }

        private static bool IsUserAuthorized(string userIdentity, string connection, DateTime? offset)
        {
            // Check token expiry 
            if (offset == null || offset < DateTime.Now)
                return false; 

            var optionsBuilder = new DbContextOptionsBuilder<ThermoDataContext>();
            optionsBuilder.UseSqlServer(connection);

            // Query the user to see if they are legi users

            var themorDataSource = new ThermoDataContext(optionsBuilder.Options);
            var userInDataStore = themorDataSource.Users.Where(x => x.Username == userIdentity).FirstOrDefault();

            return userInDataStore != null ? true : false;
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

           // app.UseHttpsRedirection();

            app.UseRouting();

            app.UseCors(x => x
               .AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader());

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints => endpoints.MapControllers());

        }
    }
}
