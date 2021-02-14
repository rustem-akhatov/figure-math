using AutoFixture;
using AutoFixture.Kernel;
using EnsureThat;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace FigureMath.Data.Testing
{
    /// <summary>
    /// Extension methods for <see cref="Fixture"/>.
    /// </summary>
    public static class FigureMathDbContextFixtureExtensions
    {
        /// <summary>
        /// Creates <see cref="FigureMathDbContext"/> using in-memory database.
        /// </summary>
        /// <param name="fixture">An instance of <see cref="Fixture"/>.</param>
        /// <returns>Instance of <see cref="FigureMathDbContext"/>.</returns>
        public static FigureMathDbContext CreateFigureMathDbContext(this Fixture fixture)
        {
            EnsureArg.IsNotNull(fixture, nameof(fixture));
            
            return new FigureMathDbContext(CreateFigureMathDbContextOptions(fixture));
        }
        
        private static DbContextOptions<FigureMathDbContext> CreateFigureMathDbContextOptions(ISpecimenBuilder fixture)
        {
            return new DbContextOptionsBuilder<FigureMathDbContext>()
                .UseInMemoryDatabase(fixture.Create<string>())
                .ConfigureWarnings(options => options.Ignore(InMemoryEventId.TransactionIgnoredWarning))
                .Options;
        }
    }
}