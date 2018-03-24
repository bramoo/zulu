using AutoMapper;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SpaServices.AngularCli;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Text;
using zulu.Attributes;
using zulu.Data;
using zulu.Services;
using zulu.ViewModels.Event;
using zulu.ViewModels.Report;
using Microsoft.AspNetCore.Mvc.Formatters;
using zulu.ViewModels.Image;
using zulu.ViewModels.ValueResolvers;

namespace zulu
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
      var jwtSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Configuration["JwtSecretKey"]));

      var jwtAppSettingOptions = Configuration.GetSection(nameof(JwtIssuerOptions));
      services.Configure<JwtIssuerOptions>(options =>
      {
        options.Issuer = jwtAppSettingOptions[nameof(JwtIssuerOptions.Issuer)];
        options.Audience = jwtAppSettingOptions[nameof(JwtIssuerOptions.Audience)];
        options.SigningCredentials = new SigningCredentials(jwtSigningKey, SecurityAlgorithms.HmacSha256);
      });
      services.Configure<FacebookOptions>(Configuration.GetSection(nameof(FacebookOptions)));

      services.AddDbContext<AppDbContext>(options =>
          options.UseSqlite("Data Source=zulu.db"));

      services.AddIdentity<AppUser, IdentityRole>()
          .AddEntityFrameworkStores<AppDbContext>();

      services.AddScoped<IJwtService, JwtService>();

      services.AddAuthentication(options =>
      {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

      }).AddJwtBearer(configureOptions => ConfigureJwtBearerOptions(configureOptions, jwtAppSettingOptions, jwtSigningKey));

      services.AddTransient<ContentTypeValueResolver>();

      services.AddAutoMapper(mapperConfig =>
      {
        mapperConfig.AddProfile<CreateEventViewModelProfile>();
        mapperConfig.AddProfile<EditEventViewModelProfile>();
        mapperConfig.AddProfile<EventViewModelProfile>();

        mapperConfig.AddProfile<CreateReportViewModelProfile>();
        mapperConfig.AddProfile<EditReportViewModelProfile>();
        mapperConfig.AddProfile<ReportViewModelProfile>();

        mapperConfig.AddProfile<CreateImageViewModelProfile>();
      });

      services.AddMvc(options =>
      {
        options.Filters.Add(new ValidateModelAttribute());
      }).AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<Startup>());

      // In production, the Angular files will be served from this directory
      services.AddSpaStaticFiles(configuration =>
      {
        configuration.RootPath = "ClientApp/dist";
      });
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
        app.UseExceptionHandler("/Home/Error");
      }

      app.UseAuthentication();
      app.UseStaticFiles();
      app.UseSpaStaticFiles();

      app.UseMvc();

      app.UseSpa(spa =>
      {
        // To learn more about options for serving an Angular SPA from ASP.NET Core,
        // see https://go.microsoft.com/fwlink/?linkid=864501

        spa.Options.SourcePath = "ClientApp";

        if (env.IsDevelopment())
        {
          spa.UseAngularCliServer(npmScript: "start");
        }
      });
    }


    private static void ConfigureJwtBearerOptions(JwtBearerOptions jwtBearerOptions, IConfigurationSection jwtAppSettingOptions, SymmetricSecurityKey jwtSigningKey)
    {
      var tokenValidationParameters = new TokenValidationParameters
      {
        ValidateIssuer = true,
        ValidIssuer = jwtAppSettingOptions[nameof(JwtIssuerOptions.Issuer)],

        ValidateAudience = true,
        ValidAudience = jwtAppSettingOptions[nameof(JwtIssuerOptions.Audience)],

        ValidateIssuerSigningKey = true,
        IssuerSigningKey = jwtSigningKey,

        RequireExpirationTime = false,
        ValidateLifetime = true,
        ClockSkew = TimeSpan.Zero
      };


      jwtBearerOptions.ClaimsIssuer = jwtAppSettingOptions[nameof(JwtIssuerOptions.Issuer)];
      jwtBearerOptions.TokenValidationParameters = tokenValidationParameters;
      jwtBearerOptions.SaveToken = true;
    }
  }
}
