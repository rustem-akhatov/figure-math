using EnsureThat;
using FigureMath.Apps.WebApi.Domain.Annotations;
using FigureMath.Data.Entities;
using FigureMath.Data.Enums;
using JetBrains.Annotations;

namespace FigureMath.Apps.WebApi.Domain.Figures
{
    /// <summary>
    /// Represents the concrete implementation of the figure - Triangle.
    /// </summary>
    [UsedImplicitly]
    [FigureImplementation(ImplementedFigureType)]
    public class TriangleInfo : FigureInfo
    {
        private double _base;
        private double _height;

        /// <summary>
        /// Implemented type of the figure.
        /// </summary>
        public const FigureType ImplementedFigureType = FigureType.Triangle;

        /// <summary>
        /// Initializes a new instance of the <see cref="TriangleInfo"/> class.
        /// </summary>
        public TriangleInfo()
            : base(ImplementedFigureType)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="TriangleInfo"/> class.
        /// </summary>
        /// <param name="figure">Figure data.</param>
        public TriangleInfo(Figure figure)
            : base(ImplementedFigureType, figure)
        {
            Base = figure.FigureProps[PropNames.Base];
            Height = figure.FigureProps[PropNames.Height];
        }

        /// <summary>
        /// Base of the triangle.
        /// </summary>
        [UsedImplicitly]
        public double Base
        {
            get => _base;
            set => _base = EnsureArg.IsGt(value, 0);
        }

        /// <summary>
        /// Height of the triangle.
        /// </summary>
        [UsedImplicitly]
        public double Height
        {
            get => _height;
            set => _height = EnsureArg.IsGt(value, 0);
        }

        /// <summary>
        /// Area of the triangle.
        /// </summary>
        public override double Area => Base * Height / 2;

        /// <summary>
        /// Contains property names for Triangle figure.
        /// </summary>
        internal static class PropNames
        {
            /// <summary>
            /// Name of the Base Property.
            /// </summary>
            /// <remarks>This value is hard coded because of backward compatibility in case of code-refactoring.</remarks>
            public const string Base = "base";

            /// <summary>
            /// Name of the Height property.
            /// </summary>
            /// <remarks>This value is hard coded because of backward compatibility in case of code-refactoring.</remarks>
            public const string Height = "height";
            
            /// <summary>
            /// All property names for Triangle figure.
            /// </summary>
            public static readonly string[] All = new[] { Base, Height};
        }
    }
}