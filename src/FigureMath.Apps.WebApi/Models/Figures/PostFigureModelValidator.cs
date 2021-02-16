using System.Collections.Generic;
using EnsureThat;
using FigureMath.Apps.WebApi.Domain.Figures.Descriptors;
using FigureMath.Apps.WebApi.Domain.Services;
using FluentValidation;
using FluentValidation.Results;
using FluentValidation.Validators;
using JetBrains.Annotations;

namespace FigureMath.Apps.WebApi.Models.Figures
{
    /// <summary>
    /// Validator for the <see cref="PostFigureModel"/>.
    /// </summary>
    [UsedImplicitly]
    public class PostFigureModelValidator : AbstractValidator<PostFigureModel>
    {
        private readonly IFigureDescriptorProvider _figureDescriptorProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="PostFigureModelValidator"/> class.
        /// </summary>
        /// <param name="figureDescriptorProvider">An instance of <see cref="IFigureDescriptorProvider"/>.</param>
        public PostFigureModelValidator(IFigureDescriptorProvider figureDescriptorProvider)
        {
            _figureDescriptorProvider = EnsureArg.IsNotNull(figureDescriptorProvider, nameof(figureDescriptorProvider));
            
            RuleFor(model => model.FigureProps).Custom(ValidateFigureProps);
        }

        private void ValidateFigureProps(IDictionary<string, double> figureProps, CustomContext context)
        {
            // We need to find out FigureType so we get model from the ParentContext.
            // DO NOT use RuleFor(model => model).Custom(ValidateFigureProps); in constructor because in this case context.PropertyName will be null.
            // So the client don't understand to which property an error message refers.
            
            var model = (PostFigureModel)context.ParentContext.InstanceToValidate;
            
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