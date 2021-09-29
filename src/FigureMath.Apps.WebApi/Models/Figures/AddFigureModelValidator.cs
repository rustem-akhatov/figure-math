using System.Collections.Generic;
using EnsureThat;
using FigureMath.Apps.WebApi.Domain;
using FluentValidation;
using FluentValidation.Results;
using FluentValidation.Validators;

namespace FigureMath.Apps.WebApi
{
    /// <summary>
    /// Validator for the <see cref="AddFigureModel"/>.
    /// </summary>
    public class AddFigureModelValidator : AbstractValidator<AddFigureModel>
    {
        private readonly IFigureDescriptorProvider _figureDescriptorProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="AddFigureModelValidator"/> class.
        /// </summary>
        /// <param name="figureDescriptorProvider">An instance of <see cref="IFigureDescriptorProvider"/>.</param>
        public AddFigureModelValidator(IFigureDescriptorProvider figureDescriptorProvider)
        {
            _figureDescriptorProvider = EnsureArg.IsNotNull(figureDescriptorProvider, nameof(figureDescriptorProvider));
            
            RuleFor(model => model.FigureProps).Custom(ValidateFigureProps);
        }

        private void ValidateFigureProps(IDictionary<string, double> figureProps, CustomContext context)
        {
            // We need to find out FigureType so we get model from the ParentContext.
            // DO NOT use RuleFor(model => model).Custom(ValidateFigureProps); in constructor because in this case context.PropertyName will be null.
            // So the client don't understand to which property an error message refers.
            
            var model = (AddFigureModel)context.ParentContext.InstanceToValidate;
            
            IFigureDescriptor figureDescriptor = _figureDescriptorProvider.GetDescriptorFor(model.FigureType);

            ValidationResult validationResult = figureDescriptor.ValidateProps(figureProps);

            foreach (ValidationFailure failure in validationResult.Errors)
            {
                string propertyName = failure.PropertyName == null ? context.PropertyName : $"{context.PropertyName}.{failure.PropertyName}";

                context.AddFailure(new ValidationFailure(propertyName, failure.ErrorMessage, failure.AttemptedValue));
            }
        }
    }
}