using API.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using Settings;

namespace API
{
    public class Program
    {
        private static ILogger log;
        private static SettingsProvider<Settings> settingsProvider;

        public static void Main(string[] args)
        {
            settingsProvider = new SettingsProvider<Settings>();
            log = new LoggerConfiguration()
                .Enrich.FromLogContext()
                .WriteTo.Console()
                .CreateLogger();
            var app = ConfigureServices(args).Build();
            if (app.Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }
#if RELEASE
            app.UseHttpsRedirection();
#endif
            app.MapControllers();
            app.Run();
        }

        private static WebApplicationBuilder ConfigureServices(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddControllers();
            builder.Services.AddSingleton(settingsProvider);
            builder.Services.AddSingleton(log);

            builder.Services.AddSingleton<IFirebaseService, FirebaseService>();

            builder.Services.AddHttpClient();
            builder.Services.AddMemoryCache();

            return builder;
        }
    }
}