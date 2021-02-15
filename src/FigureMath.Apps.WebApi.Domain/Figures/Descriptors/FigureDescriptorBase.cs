using System.Collections.Generic;
using System.Linq;
using EnsureThat;
using FigureMath.Data.Enums;
using FluentValidation.Results;

namespace FigureMath.Apps.WebApi.Domain.Figures.Descriptors
{
    /// <summary>
    /// Contains base logic to validate figure props.
    /// </summary>
    public abstract class FigureDescriptorBase : IFigureDescriptor
    {
        private readonly string[] _requiredProps;

        /// <summary>
        /// Initializes basic properties.
        /// </summary>
        /// <param name="figureType">Type of the figure this implementation describes.</param>
        /// <param name="requiredProps">Figure props that must be specified.</param>
        protected FigureDescriptorBase(FigureType figureType, string[] requiredProps)
        {
            FigureType = figureType;
            
            _requiredProps = EnsureArg.IsNotNull(requiredProps, nameof(requiredProps));
        }
        
        /// <summary>
        /// Type of the figure this implementation describes.
        /// </summary>
        public FigureType FigureType { get; }

        /// <summary>
        /// Validates values of the figure properties.
        /// </summary>
        /// <param name="figureProps">Figure properties.</param>
        /// <returns>If no failures then empty or validation failures.</returns>
        public ValidationResult ValidateProps(IDictionary<string, double> figureProps)
        {
            EnsureArg.IsNotNull(figureProps, nameof(figureProps));

            ValidationResult presenceValidationResult = ValidatePropsPresent(figureProps);
            
            // ReSharper disable once ConvertIfStatementToReturnStatement
            if (!presenceValidationResult.IsValid)
                return presenceValidationResult;
            
            return ValidatePropsValues(figureProps);
        }

        protected abstract ValidationResult ValidatePropsValues(IDictionary<string, double> figureProps);

        private ValidationResult ValidatePropsPresent(IDictionary<string, double> figureProps)
        {
            var validationResult = new ValidationResult();
            
            foreach (string presentedProp in figureProps.Keys.Except(_requiredProps))
            {
                validationResult.Errors.Add(new ValidationFailure(presentedProp, $"'{presentedProp}' is redundant. You must remove it to add this figure."));
            }
            
            foreach (string requiredProp in _requiredProps.Except(figureProps.Keys))
            {
                validationResult.Errors.Add(new ValidationFailure(requiredProp, $"'{requiredProp}' is not specified. You must specify it to add this figure."));
            }

            return validationResult;
        }
    }
}