using System.Collections.Generic;
using EnsureThat;
using FigureMath.Data.Enums;
using FluentValidation;
using FluentValidation.Results;
using JetBrains.Annotations;

namespace FigureMath.Apps.WebApi.Domain.Figures.Descriptors
{
    /// <summary>
    /// Describes a Rectangle.
    /// </summary>
    [UsedImplicitly]
    public class RectangleDescriptor : IFigureDescriptor
    {
        /// <summary>
        /// Type of the figure this implementation describes.
        /// </summary>
        public FigureType FigureType => FigureType.Rectangle;

        /// <summary>
        /// Required properties the client must specify.
        /// </summary>
        public string[] RequiredProps => RectangleInfo.PropNames.All;

        /// <summary>
        /// Validates values of the figure properties.
        /// </summary>
        /// <param name="figureProps">Figure properties.</param>
        /// <returns>If no failures then empty or validation failures.</returns>
        public ValidationResult ValidateProps(IDictionary<string, double> figureProps)
        {
            EnsureArg.HasItems(figureProps, nameof(figureProps));

            var data = new RectangleData
            {
                Width = figureProps[RectangleInfo.PropNames.Width],
                Length = figureProps[RectangleInfo.PropNames.Length]
            };

            var validator = new RectangleDataValidator();

            return validator.Validate(data);
        }
        
        private class RectangleData
        {
            public double Width { get; init; }

            public double Length { get; init; }
        }
        
        private class RectangleDataValidator : AbstractValidator<RectangleData>
        {
            public RectangleDataValidator()
            {
                RuleFor(data => data.Length).GreaterThan(0);
                
                RuleFor(data => data.Width).GreaterThan(0);
            }
        }
    }
}