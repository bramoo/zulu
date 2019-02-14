using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using zulu.Data;
using Karambolo.Extensions.Logging;

namespace zulu
{
  public class Program
  {
    public static void Main(string[] args)
    {
      var webhost = CreateWebHostBuilder(args).Build();

      using (var scope = webhost.Services.CreateScope())
      {
        var services = scope.ServiceProvider;

        try
        {
          // Requires using RazorPagesMovie.Models;
          SeedData.Initialize(services);
        }
        catch (Exception ex)
        {
          var logger = services.GetRequiredService<ILogger<Program>>();
          logger.LogError(ex, "An error occurred seeding the DB.");
        }
      }

      webhost.Run();
    }

    public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
      WebHost.CreateDefaultBuilder(args)
        .ConfigureLogging((context, builder) =>
        {
          builder.AddConfiguration(context.Configuration.GetSection("Logging"));
          builder.AddFile(options => options.RootPath = "/data");
        })
        .UseStartup<Startup>();
  }
}
