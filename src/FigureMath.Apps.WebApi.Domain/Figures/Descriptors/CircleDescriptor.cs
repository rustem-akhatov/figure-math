using System.Collections.Generic;
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