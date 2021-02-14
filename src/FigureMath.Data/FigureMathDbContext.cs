// ReSharper disable UnusedAutoPropertyAccessor.Global

using System.Collections.Immutable;
using System.Text.Json;
using FigureMath.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace FigureMath.Data
{
    /// <summary>
    /// Represents a session with the database and can be used to query and save instances of the entities.
    /// </summary>
    public class FigureMathDbContext : DbContext, IFigureMathDbContext
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FigureMathDbContext" /> class using the specified options.
        /// </summary>
        /// <param name="options">The options for this context.</param>
        public FigureMathDbContext(DbContextOptions<FigureMathDbContext> options)
            : base(options)
        { }

        /// <summary>
        /// Can be used to query and save <see cref="Figure"/> entities.
        /// </summary>
        public DbSet<Figure> Figures { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSnakeCaseNamingConvention();
            
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // We will store properties of the figure as json in PostgreSQL.
            modelBuilder
                .Entity<Figure>()
                .Property(entity => entity.FigureProps)
                .HasConversion(
                    propValue => JsonSerializer.Serialize(propValue, null),
                    dbValue => JsonSerializer.Deserialize<ImmutableDictionary<string, double>>(dbValue, null));

            // Type of the figure as a string for easier supporting new types.
            modelBuilder
                .Entity<Figure>()
                .Property(entity => entity.FigureType)
                .HasMaxLength(Figure.FigureTypeMaxLength)
                .HasConversion<string>();

            base.OnModelCreating(modelBuilder);
        }
    }
}