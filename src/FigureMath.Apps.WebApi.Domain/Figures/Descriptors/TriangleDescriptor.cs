using FigureMath.Data.Enums;
using JetBrains.Annotations;

namespace FigureMath.Apps.WebApi.Domain.Figures.Descriptors
{
    /// <summary>
    /// Describes a Triangle.
    /// </summary>
    [UsedImplicitly]
    public class TriangleDescriptor : IFigureDescriptor
    {
        /// <summary>
        /// Type of the figure this implementation describes.
        /// </summary>
        public FigureType FigureType => FigureType.Triangle;

        /// <summary>
        /// Required properties the client must specify.
        /// </summary>
        public string[] RequiredProps => TriangleInfo.PropNames.All;
    }
}