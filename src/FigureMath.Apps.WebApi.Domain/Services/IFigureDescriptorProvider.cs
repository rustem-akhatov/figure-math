using FigureMath.Apps.WebApi.Domain.Figures.Descriptors;
using FigureMath.Data.Enums;

namespace FigureMath.Apps.WebApi.Domain.Services
{
    /// <summary>
    /// Can be used to get an instance of <see cref="IFigureDescriptor"/> for the specific <see cref="FigureType"/>.
    /// </summary>
    public interface IFigureDescriptorProvider
    {
        /// <summary>
        /// Gets an instance of <see cref="IFigureDescriptor"/> for the specific <see cref="FigureType"/>.
        /// </summary>
        /// <param name="figureType">Type of the figure.</param>
        /// <returns>An instance of <see cref="IFigureDescriptor"/>.</returns>
        IFigureDescriptor GetDescriptorFor(FigureType figureType);
    }
}