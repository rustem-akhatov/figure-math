using System.Collections.Generic;
using FigureMath.Data;
using FluentValidation;
using FluentValidation.Results;

namespace FigureMath.Apps.WebApi.Domain
{
    /// <summary>
    /// Describes a Circle.
    /// </summary>
    public class CircleDescriptor : FigureDescriptorBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CircleDescriptor"/> class.
        /// </summary>
        public CircleDescriptor()
            : base(FigureType.Circle, CircleInfo.PropNames.All)
        { }

        protected override ValidationResult ValidatePropsValues(IDictionary<string, double> figureProps)
        {
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