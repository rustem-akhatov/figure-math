using System.Threading;
using System.Threading.Tasks;
using EnsureThat;
using Microsoft.EntityFrameworkCore;

namespace FigureMath.Common.Data
{
    /// <summary>
    /// Extension methods for <see cref="DbSet{TEntity}"/>.
    /// </summary>
    public static class DbSetExtensions
    {
        /// <summary>
        /// Finds an entity with the given primary key values using <see cref="DbSet{TEntity}.FindAsync(object[], CancellationToken)"/>.
        /// If no entity is found then throws an exception.
        /// </summary>
        /// <param name="dbSet">The instance of the <see cref="DbSet{TEntity}"/>.</param>
        /// <param name="keyValue">The values of the primary key for the entity to be found.</param>
        /// <param name="cancellationToken">A <see cref="CancellationToken" /> to observe while waiting for the task to complete.</param>
        /// <typeparam name="TEntity">Type of the entity.</typeparam>
        /// <returns>The entity found, or <see cref="EntityNotFoundException"/>.</returns>
        /// <exception cref="EntityNotFoundException">The entity was not found.</exception>
        public static ValueTask<TEntity> FindExistsAsync<TEntity>(this DbSet<TEntity> dbSet, object keyValue, CancellationToken cancellationToken = default)
            where TEntity : class
        {
            return FindExistsAsync(dbSet, new[] { keyValue }, cancellationToken);
        }

        /// <summary>
        /// Finds an entity with the given primary key values using <see cref="DbSet{TEntity}.FindAsync(object[], CancellationToken)"/>.
        /// If no entity is found then throws an exception.
        /// </summary>
        /// <param name="dbSet">The instance of the <see cref="DbSet{TEntity}"/>.</param>
        /// <param name="keyValues">The values of the primary key for the entity to be found.</param>
        /// <param name="cancellationToken">A <see cref="CancellationToken" /> to observe while waiting for the task to complete.</param>
        /// <typeparam name="TEntity">Type of the entity.</typeparam>
        /// <returns>The entity found, or <see cref="EntityNotFoundException"/>.</returns>
        /// <exception cref="EntityNotFoundException">The entity was not found.</exception>
        public static async ValueTask<TEntity> FindExistsAsync<TEntity>(this DbSet<TEntity> dbSet, object[] keyValues, CancellationToken cancellationToken = default)
            where TEntity : class
        {
            EnsureArg.IsNotNull(dbSet, nameof(dbSet));

            TEntity entity = await dbSet.FindAsync(keyValues, cancellationToken);
            
            if (entity == null)
                throw new EntityNotFoundException($"{typeof(TEntity).Name} was not found by keys '{string.Join('|', keyValues)}'", typeof(TEntity));

            return entity;
        }
    }
}