using System;
using FigureMath.Data.Enums;

namespace FigureMath.Apps.WebApi.Domain.Services
{
    /// <summary>
    /// Interface of the provider to get <see cref="Type"/> that implements figure.
    /// </summary>
    public interface IFigureInfoTypeProvider
    {
        /// <summary>
        /// Gets <see cref="Type"/> that implements figure.
        /// </summary>
        /// <param name="figureType">Type of the figure for which <see cref="Type"/> will be returned.</param>
        /// <returns>The <see cref="Type"/>.</returns>
        Type GetTypeFor(FigureType figureType);
    }
}