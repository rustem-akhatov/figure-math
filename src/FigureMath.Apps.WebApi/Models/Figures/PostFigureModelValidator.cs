using System.Collections.Generic;
using System.Linq;
using EnsureThat;
using FigureMath.Apps.WebApi.Domain.Figures.Descriptors;
using FluentValidation;
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
            
            // All the props we need to calculate area (at this moment) for the figure.
            HashSet<string> requiredProps = figureDescriptor.RequiredProps.ToHashSet();

            if (figureProps.Count == 0)
            {
                context.AddFailure($"You need to specify these properties {ConvertPropsToLine(requiredProps)}.");
                return;
            }
                    
            if (figureProps.Count > requiredProps.Count)
            {
                context.AddFailure($"Too many properties specified. You need to specify only these properties {ConvertPropsToLine(requiredProps)}.");
                return;
            }

            foreach (string key in figureProps.Keys)
            {
                requiredProps.Remove(key);
            }

            if (requiredProps.Count > 0)
            {
                context.AddFailure($"These properties were not specified {ConvertPropsToLine(requiredProps)}. You must specify them.");
            }
        }

        private static string ConvertPropsToLine(IEnumerable<string> props)
        {
            return $"[{string.Join(", ", props)}]";
        }
    }
}