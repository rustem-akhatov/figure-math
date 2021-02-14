using FigureMath.Data.Entities;
using FigureMath.Data.Enums;
using JetBrains.Annotations;

namespace FigureMath.Apps.WebApi.Domain.Figures
{
    /// <summary>
    /// Represents the concrete implementation of the figure - Rectangle.
    /// </summary>
    [UsedImplicitly]
    [FigureImplementation(ImplementedFigureType)]
    public class RectangleInfo : FigureInfo
    {
        /// <summary>
        /// Implemented type of the figure.
        /// </summary>
        public const FigureType ImplementedFigureType = FigureType.Rectangle;

        /// <summary>
        /// Initializes a new instance of the <see cref="RectangleInfo"/> class.
        /// </summary>
        public RectangleInfo()
            : base(ImplementedFigureType)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="RectangleInfo"/> class.
        /// </summary>
        /// <param name="figure">Figure data.</param>
        public RectangleInfo(Figure figure)
            : base(ImplementedFigureType, figure)
        {
            Length = figure.FigureProps[PropNames.Length];
            Width = figure.FigureProps[PropNames.Width];
        }

        /// <summary>
        /// Length of the rectangle.
        /// </summary>
        [UsedImplicitly]
        public double Length { get; set; }

        /// <summary>
        /// Width of the rectangle.
        /// </summary>
        [UsedImplicitly]
        public double Width { get; set; }

        /// <summary>
        /// Area of the rectangle.
        /// </summary>
        public override double Area => Width * Length;

        /// <summary>
        /// Contains property names for Rectangle figure.
        /// </summary>
        internal static class PropNames
        {
            /// <summary>
            /// Name of the Length property.
            /// </summary>
            /// <remarks>This value is hard coded because of backward compatibility in case of code-refactoring.</remarks>
            public const string Length = "length";

            /// <summary>
            /// Name of the Width property.
            /// </summary>
            /// <remarks>This value is hard coded because of backward compatibility in case of code-refactoring.</remarks>
            public const string Width = "width";
            
            /// <summary>
            /// All property names for Rectangle figure.
            /// </summary>
            public static readonly string[] All = new[] { Length, Width };
        }
    }
}