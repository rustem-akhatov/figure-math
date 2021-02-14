using FigureMath.Data.Enums;
using JetBrains.Annotations;

namespace FigureMath.Apps.WebApi.Domain.Figures.Descriptors
{
    /// <summary>
    /// Describes a Rectangle.
    /// </summary>
    [UsedImplicitly]
    public class RectangleDescriptor : IFigureDescriptor
    {
        /// <summary>
        /// Type of the figure this implementation describes.
        /// </summary>
        public FigureType FigureType => FigureType.Rectangle;

        /// <summary>
        /// Required properties the client must specify.
        /// </summary>
        public string[] RequiredProps => RectangleInfo.PropNames.All;
    }
}