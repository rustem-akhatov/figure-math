using FigureMath.Common.AspNet.Hosting;
using FigureMath.Data;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace FigureMath.Apps.WebApi
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            var server = new DefaultServer(typeof(Startup), args);
                
            server.Run(host =>
            {
                // Applies DB Migrations if Environment is either Development or DockerDesktop, otherwise you need to use others methods.
                
                using IServiceScope scope = host.Services.CreateScope();
                var env = scope.ServiceProvider.GetRequiredService<IHostEnvironment>();

                if (env.IsDevelopment() || env.IsDockerDesktop())
                {
                    var dbContext = (FigureMathDbContext)scope.ServiceProvider.GetRequiredService<IFigureMathDbContext>();

                    dbContext.Database.Migrate();
                }
            });
        }

        // EF Core uses this method at design time to access the DbContext.
        [UsedImplicitly]
        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            var server = new DefaultServer(typeof(Startup), args);

            return server.CreateHostBuilder();
        }
    }
}