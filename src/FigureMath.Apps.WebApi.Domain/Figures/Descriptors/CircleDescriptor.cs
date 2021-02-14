using FigureMath.Data.Enums;
using JetBrains.Annotations;

namespace FigureMath.Apps.WebApi.Domain.Figures.Descriptors
{
    /// <summary>
    /// Describes a Circle.
    /// </summary>
    [UsedImplicitly]
    public class CircleDescriptor : IFigureDescriptor
    {
        /// <summary>
        /// Type of the figure this implementation describes.
        /// </summary>
        public FigureType FigureType => FigureType.Circle;

        /// <summary>
        /// Required properties the client must specify.
        /// </summary>
        public string[] RequiredProps => CircleInfo.PropNames.All;
    }
}