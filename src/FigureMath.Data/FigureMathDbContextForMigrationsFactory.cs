using System;
using System.Linq;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace FigureMath.Data
{
    // EF Core uses this class at design time to access the DbContext.
    [UsedImplicitly]
    public class FigureMathDbContextForMigrationsFactory : IDesignTimeDbContextFactory<FigureMathDbContext>
    {
        /// <summary>
        /// Creates a new instance of a derived context.
        /// </summary>
        /// <param name="args"> Arguments provided by the design-time service. </param>
        /// <returns> An instance of <see cref="FigureMathDbContext"/>.</returns>
        public FigureMathDbContext CreateDbContext(string[] args)
        {
            // Connection string can be configured via Environment Variable EF_CONNECTION_STRING.
            // For the current shell (EF_CONNECTION_STRING="") or for all processes (export EF_CONNECTION_STRING="").

            string connectionString = Environment.GetEnvironmentVariable("EF_CONNECTION_STRING");
            connectionString ??= "Host=localhost;Username=postgres;Password=password;Database=figure_math_db";
            
            Console.WriteLine($"Connection string: {string.Join(";", connectionString.Split(";").Where(p => !p.StartsWith("Password=")))}");

            var dbOptions = new DbContextOptionsBuilder<FigureMathDbContext>()
                .UseNpgsql(connectionString)
                .Options;
            
            return new FigureMathDbContext(dbOptions);
        }
    }
}