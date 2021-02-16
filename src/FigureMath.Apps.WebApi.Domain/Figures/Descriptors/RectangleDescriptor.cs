using System.Collections.Generic;
using FigureMath.Data.Enums;
using FluentValidation;
using FluentValidation.Results;

namespace FigureMath.Apps.WebApi.Domain.Figures.Descriptors
{
    /// <summary>
    /// Describes a Rectangle.
    /// </summary>
    public class RectangleDescriptor : FigureDescriptorBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RectangleDescriptor"/> class.
        /// </summary>
        public RectangleDescriptor()
            : base(FigureType.Rectangle, RectangleInfo.PropNames.All)
        { }

        protected override ValidationResult ValidatePropsValues(IDictionary<string, double> figureProps)
        {
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