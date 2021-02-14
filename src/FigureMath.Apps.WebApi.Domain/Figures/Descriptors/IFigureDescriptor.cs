using FigureMath.Data.Enums;

namespace FigureMath.Apps.WebApi.Domain.Figures.Descriptors
{
    /// <summary>
    /// Describes a concrete figure. It's type and required properties (at this moment to calculate area of the figure).
    /// </summary>
    public interface IFigureDescriptor
    {
        /// <summary>
        /// Type of the figure this implementation describes.
        /// </summary>
        FigureType FigureType { get; }
        
        /// <summary>
        /// Required properties the client must specify.
        /// </summary>
        // ReSharper disable once ReturnTypeCanBeEnumerable.Global
        string[] RequiredProps { get; }
    }
}