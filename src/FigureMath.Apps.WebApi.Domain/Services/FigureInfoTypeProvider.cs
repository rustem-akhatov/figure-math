using System;
using System.Collections.Generic;
using System.Reflection;
using FigureMath.Apps.WebApi.Domain.Annotations;
using FigureMath.Apps.WebApi.Domain.Figures;
using FigureMath.Data.Enums;

namespace FigureMath.Apps.WebApi.Domain.Services
{
    /// <summary>
    /// Implementation of the provider to get <see cref="Type"/> that implements figure.
    /// </summary>
    public class FigureInfoTypeProvider : IFigureInfoTypeProvider
    {
        private readonly Dictionary<FigureType, Type> _figureInfoTypeMapping;
        
        /// <summary>
        /// Initializes a new instance of the class <see cref="FigureInfoTypeProvider"/>.
        /// </summary>
        /// <exception cref="InvalidOperationException">
        /// One of the derived classes from <see cref="FigureInfo"/> is not decorated with <see cref="FigureImplementationAttribute"/>.
        /// </exception>
        public FigureInfoTypeProvider()
        {
            var figureInfoType = typeof(FigureInfo);

            _figureInfoTypeMapping = new Dictionary<FigureType, Type>();

            foreach (Type type in figureInfoType.Assembly.GetTypes())
            {
                // Type must be derived from FigureInfo, be public and concrete.
                if (!type.IsAssignableTo(figureInfoType) || !type.IsPublic || type.IsAbstract)
                    continue;

                var attribute = type.GetCustomAttribute<FigureImplementationAttribute>();

                if (attribute == null)
                    throw new InvalidOperationException($"You need to specify {nameof(FigureImplementationAttribute)} for type {type.Name}.");

                _figureInfoTypeMapping.Add(attribute.FigureType, type);
            }
        }

        /// <summary>
        /// Gets <see cref="Type"/> that implements figure.
        /// </summary>
        /// <param name="figureType">Type of the figure for which <see cref="Type"/> will be returned.</param>
        /// <returns>The <see cref="Type"/>.</returns>
        /// <exception cref="InvalidOperationException">Implementation class was not found.</exception>
        public Type GetTypeFor(FigureType figureType)
        {
            Type figureInfoType = _figureInfoTypeMapping.GetValueOrDefault(figureType);

            if (figureInfoType == null)
            {
                throw new InvalidOperationException($"Figure implementation of {figureType} was not found. " +
                                                    $"You need to add class derived from {nameof(FigureInfo)} that will implement figure.");
            }

            return figureInfoType;
        }
    }
}