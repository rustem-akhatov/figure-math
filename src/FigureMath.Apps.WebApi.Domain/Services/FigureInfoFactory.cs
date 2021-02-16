using System;
using System.Reflection;
using EnsureThat;
using FigureMath.Apps.WebApi.Domain.Figures;
using FigureMath.Data.Entities;

namespace FigureMath.Apps.WebApi.Domain.Services
{
    /// <summary>
    /// Implementation of the factory to create a specific instance of the class derived from <see cref="FigureInfo"/>
    /// based on data of the figure inside in <see cref="Figure"/>.
    /// </summary>
    public class FigureInfoFactory : IFigureInfoFactory
    {
        private readonly IFigureInfoTypeProvider _figureInfoTypeProvider;

        /// <summary>
        /// Initializes a new instance of the class <see cref="FigureInfoFactory"/>.
        /// </summary>
        public FigureInfoFactory(IFigureInfoTypeProvider figureInfoTypeProvider)
        {
            _figureInfoTypeProvider = EnsureArg.IsNotNull(figureInfoTypeProvider, nameof(figureInfoTypeProvider));
        }

        /// <summary>
        /// Creates an instance of the class derived from <see cref="FigureInfo"/> based on data of the figure inside in <paramref name="figure"/>. 
        /// </summary>
        /// <param name="figure">Figure data.</param>
        /// <returns>Specific figure.</returns>
        /// <exception cref="InvalidOperationException">Derived class does not have a necessary constructor.</exception>
        public FigureInfo Create(Figure figure)
        {
            EnsureArg.IsNotNull(figure, nameof(figure));

            Type figureInfoType = _figureInfoTypeProvider.GetTypeFor(figure.FigureType);

            ConstructorInfo constructor = figureInfoType.GetConstructor(new[] { typeof(Figure) });
            
            if (constructor == null)
            {
                throw new InvalidOperationException($"You need to add a public constructor for type {figureInfoType.Name} " +
                                                    $"that accepts only one parameter of type {nameof(Figure)}.");
            }

            return (FigureInfo)constructor.Invoke(new object[] { figure });
        }
    }
}