using System.Collections.Generic;
using FigureMath.Data.Enums;
using FluentValidation.Results;

namespace FigureMath.Apps.WebApi.Domain.Figures.Descriptors
{
    /// <summary>
    /// Describes a concrete figure. It's type and required properties (at this moment to calculate area of the figure).
    /// </summary>
    public interface IFigureDescriptor
    {
        /// <summary>
        /// Type of the figure this implementation describes.
        /// </summary>
        FigureType FigureType { get; }

        /// <summary>
        /// Validates values of the figure properties.
        /// </summary>
        /// <param name="figureProps">Figure properties.</param>
        /// <returns>If no failures then empty or validation failures.</returns>
        ValidationResult ValidateProps(IDictionary<string, double> figureProps);
    }
}