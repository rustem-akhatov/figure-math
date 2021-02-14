using System.Collections.Generic;
using EnsureThat;
using FigureMath.Data.Entities;
using FigureMath.Data.Enums;
using MediatR;

namespace FigureMath.Apps.WebApi.Domain.Messaging
{
    /// <summary>
    /// Allows to save information about the figure in the persistent store.
    /// </summary>
    public class AddFigureRequest : IRequest<Figure>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AddFigureRequest"/> class.
        /// </summary>
        /// <param name="figureType">Type of the figure.</param>
        /// <param name="figureProps">Figure properties.</param>
        public AddFigureRequest(FigureType figureType, IDictionary<string, double> figureProps)
        {
            FigureType = figureType;
            FigureProps = EnsureArg.IsNotNull(figureProps, nameof(figureProps));
        }

        /// <summary>
        /// Type of the figure.
        /// </summary>
        public FigureType FigureType { get; }

        /// <summary>
        /// Figure properties.
        /// </summary>
        public IDictionary<string, double> FigureProps { get; }
    }
}