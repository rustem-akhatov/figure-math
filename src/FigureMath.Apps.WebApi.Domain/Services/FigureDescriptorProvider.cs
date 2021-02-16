using System;
using System.Collections.Generic;
using System.Linq;
using EnsureThat;
using FigureMath.Apps.WebApi.Domain.Figures.Descriptors;
using FigureMath.Data.Enums;

namespace FigureMath.Apps.WebApi.Domain.Services
{
    /// <summary>
    /// Can be used to get an instance of <see cref="IFigureDescriptor"/> for the specific <see cref="FigureType"/>.
    /// </summary>
    public class FigureDescriptorProvider : IFigureDescriptorProvider
    {
        private readonly Dictionary<FigureType, IFigureDescriptor> _descriptors;

        /// <summary>
        /// Initializes a new instance of the <see cref="FigureDescriptorProvider"/> class.
        /// </summary>
        /// <param name="descriptors">All available descriptors of the figures.</param>
        public FigureDescriptorProvider(IEnumerable<IFigureDescriptor> descriptors)
        {
            _descriptors = EnsureArg.IsNotNull(descriptors, nameof(descriptors)).ToDictionary(descriptor => descriptor.FigureType);
        }

        /// <summary>
        /// Gets an instance of <see cref="IFigureDescriptor"/> for the specific <see cref="FigureType"/>.
        /// </summary>
        /// <param name="figureType">Type of the figure.</param>
        /// <returns>An instance of <see cref="IFigureDescriptor"/>.</returns>
        public IFigureDescriptor GetDescriptorFor(FigureType figureType)
        {
            IFigureDescriptor descriptor = _descriptors.GetValueOrDefault(figureType);

            if (descriptor == null)
                throw new InvalidOperationException($"No figure descriptor found for {figureType}. You need to add it.");

            return descriptor;
        }
    }
}