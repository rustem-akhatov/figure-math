using System;
using EnsureThat;
using FigureMath.Data;
using JetBrains.Annotations;

namespace FigureMath.Apps.WebApi.Domain
{
    /// <summary>
    /// Represents the concrete implementation of the figure - Circle.
    /// </summary>
    [FigureImplementation(ImplementedFigureType)]
    public class CircleInfo : FigureInfo
    {
        private double _radius;

        /// <summary>
        /// Implemented type of the figure.
        /// </summary>
        public const FigureType ImplementedFigureType = FigureType.Circle;

        /// <summary>
        /// Initializes a new instance of the <see cref="CircleInfo"/> class.
        /// </summary>
        public CircleInfo()
            : base(ImplementedFigureType)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="CircleInfo"/> class.
        /// </summary>
        /// <param name="figure">Figure data.</param>
        [UsedImplicitly]
        public CircleInfo(Figure figure)
            : base(ImplementedFigureType, figure)
        {
            Radius = figure.FigureProps[PropNames.Radius];
        }

        /// <summary>
        /// Radius of the circle.
        /// </summary>
        public double Radius
        {
            get => _radius;
            set => _radius = EnsureArg.IsGt(value, 0);
        }

        /// <summary>
        /// Area of the circle.
        /// </summary>
        public override double Area => Math.PI * Math.Pow(Radius, 2);
        
        /// <summary>
        /// Contains property names for Circle figure.
        /// </summary>
        internal static class PropNames
        {
            /// <summary>
            /// Name of the Radius property.
            /// </summary>
            /// <remarks>This value is hard coded because of backward compatibility in case of code-refactoring.</remarks>
            public const string Radius = "radius";
            
            /// <summary>
            /// All property names for Circle figure.
            /// </summary>
            public static readonly string[] All = new[] { Radius };
        }
    }
}