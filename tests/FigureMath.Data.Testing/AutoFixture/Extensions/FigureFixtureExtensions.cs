using System.Collections.Generic;
using System.Collections.Immutable;
using AutoFixture;
using AutoFixture.Dsl;
using EnsureThat;
using FigureMath.Data.Entities;
using FigureMath.Data.Enums;

namespace FigureMath.Data.Testing.AutoFixture.Extensions
{
    /// <summary>
    /// Extension methods for <see cref="IFixture"/> to build <see cref="Figure"/>.
    /// </summary>
    public static class FigureFixtureExtensions
    {
        public static Figure CreateFigure(this IFixture fixture)
        {
            EnsureArg.IsNotNull(fixture, nameof(fixture));

            return fixture.BuildFigure().Create();
        }
        
        public static Figure CreateFigure(this IFixture fixture, FigureType figureType)
        {
            EnsureArg.IsNotNull(fixture, nameof(fixture));

            return fixture
                .BuildFigure()
                .With(figure => figure.FigureType, figureType)
                .Create();
        }
        
        public static IPostprocessComposer<Figure> BuildFigure(this IFixture fixture)
        {
            EnsureArg.IsNotNull(fixture, nameof(fixture));

            return fixture
                .Build<Figure>()
                .With(obj => obj.FigureProps, fixture.Create<Dictionary<string, double>>().ToImmutableDictionary());
        }
    }
}