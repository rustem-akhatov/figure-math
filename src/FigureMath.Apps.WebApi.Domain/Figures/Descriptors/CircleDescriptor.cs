using System.Collections.Generic;
using EnsureThat;
using FigureMath.Data.Enums;
using FluentValidation;
using FluentValidation.Results;
using JetBrains.Annotations;

namespace FigureMath.Apps.WebApi.Domain.Figures.Descriptors
{
    /// <summary>
    /// Describes a Circle.
    /// </summary>
    [UsedImplicitly]
    public class CircleDescriptor : IFigureDescriptor
    {
        /// <summary>
        /// Type of the figure this implementation describes.
        /// </summary>
        public FigureType FigureType => FigureType.Circle;

        /// <summary>
        /// Required properties the client must specify.
        /// </summary>
        public string[] RequiredProps => CircleInfo.PropNames.All;

        /// <summary>
        /// Validates values of the figure properties.
        /// </summary>
        /// <param name="figureProps">Figure properties.</param>
        /// <returns>If no failures then empty or validation failures.</returns>
        public ValidationResult ValidateProps(IDictionary<string, double> figureProps)
        {
            EnsureArg.HasItems(figureProps, nameof(figureProps));

            var data = new CircleData
            {
                Radius = figureProps[CircleInfo.PropNames.Radius]
            };

            var validator = new CircleDataValidator();
            
            return validator.Validate(data);
        }
        
        private class CircleData
        {
            public double Radius { get; init; }
        }

        private class CircleDataValidator : AbstractValidator<CircleData>
        {
            public CircleDataValidator()
            {
                RuleFor(data => data.Radius).GreaterThan(0);
            }
        }
    }
}