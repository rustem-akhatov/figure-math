using System.Collections.Generic;
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
    public class TriangleDescriptor : FigureDescriptorBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TriangleDescriptor"/> class.
        /// </summary>
        public TriangleDescriptor()
            : base(FigureType.Triangle, TriangleInfo.PropNames.All)
        { }

        protected override ValidationResult ValidatePropsValues(IDictionary<string, double> figureProps)
        {
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