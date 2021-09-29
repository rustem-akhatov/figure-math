using FigureMath.Data;

namespace FigureMath.Apps.WebApi.Domain
{
    /// <summary>
    /// Interface of the factory to create a specific instance of the class derived from <see cref="FigureInfo"/>
    /// based on data of the figure inside in <see cref="Figure"/>. 
    /// </summary>
    public interface IFigureInfoFactory
    {
        /// <summary>
        /// Creates an instance of the class derived from <see cref="FigureInfo"/> based on data of the figure inside in <paramref name="figure"/>. 
        /// </summary>
        /// <param name="figure">Figure data.</param>
        /// <returns>Specific figure.</returns>
        FigureInfo Create(Figure figure);
    }
}