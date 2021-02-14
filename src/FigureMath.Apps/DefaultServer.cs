using System;
using System.Diagnostics;
using System.IO;
using EnsureThat;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NLog;
using NLog.Web;

namespace FigureMath.Apps
{
    /// <summary>
    /// Represents an ASP.NET host with pre-configured settings (eg. logging, time zone and etc.).
    /// </summary>
    public class DefaultServer
    {
        private static readonly Logger _logger;
    
        private readonly Type _startupType;
        private readonly string[] _args;

        static DefaultServer()
        {
            _logger = ConfigureNLog().GetCurrentClassLogger();
        }
        
        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultServer"/>.
        /// </summary>
        /// <param name="startupType">Specify the startup type to be used by the web host.</param>
        /// <param name="args">The command line args.</param>
        public DefaultServer(Type startupType, string[] args)
        {
            _startupType = EnsureArg.IsNotNull(startupType, nameof(startupType));
            _args = args;
        }
        
        /// <summary>
        /// Runs an application and block the calling thread until host shutdown.
        /// </summary>
        /// <param name="beforeRun">Can be used to do something just after host constructed and before it runs.</param>
        public void Run(Action<IHost> beforeRun = null)
        {
            Trace.CorrelationManager.ActivityId = Guid.NewGuid();
            
            try
            {
                _logger.Info("Initialize application...");
                
                IHost host = CreateHostBuilder().Build();

                beforeRun?.Invoke(host);

                host.Run();
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Stopped program because of exception");
                throw;
            }
            finally
            {
                // Ensure to flush and stop internal timers/threads before application-exit (Avoid segmentation fault on Linux)
                LogManager.Shutdown();
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HostBuilder"/> class with pre-configured settings.
        /// </summary>
        /// <returns>The initialized <see cref="IHostBuilder"/>.</returns>
        public IHostBuilder CreateHostBuilder() =>
            Host.CreateDefaultBuilder(_args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup(_startupType);
                })
                .ConfigureLogging((context, logging) =>
                {
                    logging.ClearProviders();

                    LogManager.ThrowExceptions = context.HostingEnvironment.IsDevelopment();

                    LogManager.ReconfigExistingLoggers();
                })
                .UseNLog();
        
        private static LogFactory ConfigureNLog()
        {
            var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production";
            
            var configFile = $"nlog.{env}.config";
            
            configFile = File.Exists(configFile) ? configFile : "nlog.config";
            
            return NLogBuilder.ConfigureNLog(configFile);
        }
    }
}