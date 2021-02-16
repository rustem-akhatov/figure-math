using System.Threading;
using System.Threading.Tasks;
using EnsureThat;
using FigureMath.Apps.WebApi.Domain.Figures;
using FigureMath.Apps.WebApi.Domain.Services;
using FigureMath.Common.Data.Extensions;
using FigureMath.Data;
using FigureMath.Data.Entities;
using MediatR;

namespace FigureMath.Apps.WebApi.Domain.Messaging
{
    /// <summary>
    /// Handler for <see cref="GetFigureInfoRequest"/>.
    /// </summary>
    public class GetFigureInfoHandler : IRequestHandler<GetFigureInfoRequest, FigureInfo>
    {
        private readonly IFigureMathDbContext _dbContext;
        private readonly IFigureInfoFactory _figureInfoFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetFigureInfoHandler"/> class.
        /// </summary>
        /// <param name="dbContext">An instance of  <see cref="IFigureMathDbContext"/>.</param>
        /// <param name="figureInfoFactory">An instance of  <see cref="IFigureInfoFactory"/>.</param>
        public GetFigureInfoHandler(IFigureMathDbContext dbContext, IFigureInfoFactory figureInfoFactory)
        {
            _dbContext = EnsureArg.IsNotNull(dbContext, nameof(dbContext));
            _figureInfoFactory = EnsureArg.IsNotNull(figureInfoFactory, nameof(figureInfoFactory));
        }
        
        /// <summary>
        /// Gets concrete implementation of the figure.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>Concrete implementation of the figure.</returns>
        public async Task<FigureInfo> Handle(GetFigureInfoRequest request, CancellationToken cancellationToken)
        {
            EnsureArg.IsNotNull(request, nameof(request));

            Figure figure = await _dbContext.Figures.FindExistsAsync(request.Id, cancellationToken);

            return _figureInfoFactory.Create(figure);
        }
    }
}