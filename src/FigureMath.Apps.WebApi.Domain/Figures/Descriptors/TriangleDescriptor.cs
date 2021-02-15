using System.Collections.Generic;
using EnsureThat;
using FigureMath.Data.Enums;
using FluentValidation;
using FluentValidation.Results;
using JetBrains.Annotations;

namespace FigureMath.Apps.WebApi.Domain.Figures.Descriptors
{
    /// <summary>
    /// Describes a Triangle.
    /// </summary>
    [UsedImplicitly]
    public class TriangleDescriptor : IFigureDescriptor
    {
        /// <summary>
        /// Type of the figure this implementation describes.
        /// </summary>
        public FigureType FigureType => FigureType.Triangle;

        /// <summary>
        /// Required properties the client must specify.
        /// </summary>
        public string[] RequiredProps => TriangleInfo.PropNames.All;

        /// <summary>
        /// Validates values of the figure properties.
        /// </summary>
        /// <param name="figureProps">Figure properties.</param>
        /// <returns>If no failures then empty or validation failures.</returns>
        public ValidationResult ValidateProps(IDictionary<string, double> figureProps)
        {
            EnsureArg.HasItems(figureProps, nameof(figureProps));

            var data = new TriangleData
            {
                Base = figureProps[TriangleInfo.PropNames.Base],
                Height = figureProps[TriangleInfo.PropNames.Height]
            };

            var validator = new TriangleDataValidator();

            return validator.Validate(data);
        }
        
        private class TriangleData
        {
            public double Base { get; init; }

            public double Height { get; init; }
        }
        
        private class TriangleDataValidator : AbstractValidator<TriangleData>
        {
            public TriangleDataValidator()
            {
                RuleFor(data => data.Base).GreaterThan(0);
                
                RuleFor(data => data.Height).GreaterThan(0);
            }
        }
    }
}