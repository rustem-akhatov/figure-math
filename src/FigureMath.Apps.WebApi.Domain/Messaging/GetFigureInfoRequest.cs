using MediatR;

namespace FigureMath.Apps.WebApi.Domain
{
    /// <summary>
    /// Allows to get concrete implementation of the figure.
    /// </summary>
    public class GetFigureInfoRequest : IRequest<FigureInfo>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GetFigureInfoRequest"/> class.
        /// </summary>
        /// <param name="id">Identifier of the figure.</param>
        public GetFigureInfoRequest(long id)
        {
            Id = id;
        }
        
        /// <summary>
        /// Identifier of the figure.
        /// </summary>
        public long Id { get; }
    }
}