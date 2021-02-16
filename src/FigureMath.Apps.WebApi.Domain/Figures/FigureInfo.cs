using System;
using EnsureThat;
using FigureMath.Apps.WebApi.Domain.Annotations;
using FigureMath.Data.Entities;
using FigureMath.Data.Enums;
using JetBrains.Annotations;

namespace FigureMath.Apps.WebApi.Domain.Figures
{
    /// <summary>
    /// Represents a specific figure and can be used to calculate math measures.
    /// </summary>
    public abstract class FigureInfo
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FigureInfo"/> class.
        /// </summary>
        /// <param name="figureType">Type of the figure.</param>
        protected FigureInfo(FigureType figureType)
        {
            FigureType = figureType;
        }

        /// <summary>
        /// Checks and keeps data of the figure.
        /// </summary>
        /// <param name="figureType">Expected type of the figure.</param>
        /// <param name="figure">Figure data.</param>
        /// <exception cref="InvalidOperationException">Derived class is not decorated with <see cref="FigureImplementationAttribute"/>.</exception>
        /// <exception cref="InvalidOperationException">Type of the figure is not matched.</exception>
        protected FigureInfo(FigureType figureType, Figure figure)
            : this(figureType)
        {
            EnsureArg.IsNotNull(figure, nameof(figure));

            if (figure.FigureType != figureType)
                throw new InvalidOperationException($"Expected figure is {figureType}. Actual figure is {figure.FigureType}.");

            Id = figure.Id;
        }

        /// <summary>
        /// Identifier of the figure.
        /// </summary>
        [UsedImplicitly]
        public long Id { get; }

        /// <summary>
        /// Type of the figure.
        /// </summary>
        [UsedImplicitly]
        public FigureType FigureType { get; }

        /// <summary>
        /// Area of the figure.
        /// </summary>
        public abstract double Area { get; }
    }
}