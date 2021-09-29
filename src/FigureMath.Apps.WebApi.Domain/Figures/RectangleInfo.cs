using EnsureThat;
using FigureMath.Data;
using JetBrains.Annotations;

namespace FigureMath.Apps.WebApi.Domain
{
    /// <summary>
    /// Represents the concrete implementation of the figure - Rectangle.
    /// </summary>
    [FigureImplementation(ImplementedFigureType)]
    public class RectangleInfo : FigureInfo
    {
        private double _length;
        private double _width;

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
        [UsedImplicitly]
        public RectangleInfo(Figure figure)
            : base(ImplementedFigureType, figure)
        {
            Length = figure.FigureProps[PropNames.Length];
            Width = figure.FigureProps[PropNames.Width];
        }

        /// <summary>
        /// Length of the rectangle.
        /// </summary>
        public double Length
        {
            get => _length;
            set => _length = EnsureArg.IsGt(value, 0);
        }

        /// <summary>
        /// Width of the rectangle.
        /// </summary>
        public double Width
        {
            get => _width;
            set => _width = EnsureArg.IsGt(value, 0);
        }

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