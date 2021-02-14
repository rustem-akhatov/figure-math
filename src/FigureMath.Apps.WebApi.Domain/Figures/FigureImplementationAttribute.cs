// ReSharper disable AutoPropertyCanBeMadeGetOnly.Global

using System;
using FigureMath.Data.Enums;

namespace FigureMath.Apps.WebApi.Domain.Figures
{
    /// <summary>
    /// Must be used to decorate concrete implementation of the figure derived from <see cref="FigureInfo"/> class.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class FigureImplementationAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FigureImplementationAttribute"/> class.
        /// </summary>
        /// <param name="figureType">Type of the figure that class implements.</param>
        public FigureImplementationAttribute(FigureType figureType)
        {
            FigureType = figureType;
        }

        /// <summary>
        /// Type of the figure that class implements.
        /// </summary>
        public FigureType FigureType { get; set; }
    }
}