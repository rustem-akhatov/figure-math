using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace FigureMath.Data
{
    /// <summary>
    /// Represents a session with the database and can be used to query and save instances of the entities.
    /// </summary>
    public interface IFigureMathDbContext
    {
        /// <summary>
        /// Can be used to query and save <see cref="Figure"/> entities.
        /// </summary>
        DbSet<Figure> Figures { get; }
        
        /// <summary>
        /// Saves all changes made in this context to the database.
        /// </summary>
        /// <param name="cancellationToken"> A <see cref="CancellationToken" /> to observe while waiting for the task to complete. </param>
        /// <returns>A task that represents the asynchronous save operation.
        /// The task result contains the number of state entries written to the database.</returns>
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}