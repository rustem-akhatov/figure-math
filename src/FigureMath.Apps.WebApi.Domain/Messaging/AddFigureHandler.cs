using System.Collections.Immutable;
using System.Threading;
using System.Threading.Tasks;
using EnsureThat;
using FigureMath.Data;
using FigureMath.Data.Entities;
using JetBrains.Annotations;
using MediatR;

namespace FigureMath.Apps.WebApi.Domain.Messaging
{
    /// <summary>
    /// Handler for <see cref="AddFigureRequest"/>.
    /// </summary>
    [UsedImplicitly]
    public class AddFigureHandler : IRequestHandler<AddFigureRequest, Figure>
    {
        private readonly IFigureMathDbContext _dbContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="AddFigureHandler"/> class.
        /// </summary>
        /// <param name="dbContext">An instance of <see cref="IFigureMathDbContext"/>.</param>
        public AddFigureHandler(IFigureMathDbContext dbContext)
        {
            _dbContext = EnsureArg.IsNotNull(dbContext, nameof(dbContext));
        }

        /// <summary>
        /// Saves figure in persistent store.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>Saved figure.</returns>
        public async Task<Figure> Handle(AddFigureRequest request, CancellationToken cancellationToken)
        {
            EnsureArg.IsNotNull(request, nameof(request));
        
            var figure = new Figure
            {
                FigureType = request.FigureType,
                FigureProps = request.FigureProps.ToImmutableDictionary()
            };
        
            // ReSharper disable once MethodHasAsyncOverloadWithCancellation
            _dbContext.Figures.Add(figure);
        
            await _dbContext.SaveChangesAsync(cancellationToken);

            return figure;
        }
    }
}