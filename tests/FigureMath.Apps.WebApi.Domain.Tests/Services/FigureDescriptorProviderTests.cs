using System;
using System.Collections.Generic;
using System.Linq;
using AutoFixture;
using FigureMath.Data;
using Moq;
using Xunit;

namespace FigureMath.Apps.WebApi.Domain.Tests
{
    public class FigureDescriptorProviderTests
    {
        private readonly Fixture _fixture;
        
        public FigureDescriptorProviderTests()
        {
            _fixture = new Fixture();
        }
        
        [Fact]
        public void Constructor_ShouldInitializeObject_WhenOneDescriptorForFigureTypePassed()
        {
            // Arrange
            IEnumerable<FigureType> figureTypes = _fixture
                .CreateMany<FigureType>()
                .Distinct();
            
            IEnumerable<IFigureDescriptor> descriptors = CreateDescriptors(figureTypes);

            // Act
            var provider = new FigureDescriptorProvider(descriptors);
            
            // Assert
            Assert.NotNull(provider);
        }
        
        [Fact]
        public void Constructor_ShouldThrowException_WhenMultipleDescriptorsForTheSameFigureTypePassed()
        {
            // Arrange
            List<FigureType> figureTypes = _fixture
                .CreateMany<FigureType>()
                .ToList();
            
            figureTypes.AddRange(figureTypes.Take(figureTypes.Count / 2));
            
            IEnumerable<IFigureDescriptor> descriptors = CreateDescriptors(figureTypes);

            // Act
            Exception exception = Record.Exception(() => new FigureDescriptorProvider(descriptors));
            
            // Assert
            Assert.NotNull(exception);
            Assert.IsType<ArgumentException>(exception);
        }
        
        [Fact]
        public void GetDescriptorFor_ShouldReturnDescriptor_WhenFound()
        {
            // Arrange
            FigureType[] figureTypes = _fixture
                .CreateMany<FigureType>()
                .Distinct()
                .ToArray();
            
            IEnumerable<IFigureDescriptor> descriptors = CreateDescriptors(figureTypes);
            
            var provider = new FigureDescriptorProvider(descriptors);

            FigureType testFigureType = figureTypes.First();

            // Act
            IFigureDescriptor descriptor = provider.GetDescriptorFor(testFigureType);

            // Assert
            Assert.NotNull(descriptor);
            Assert.Equal(testFigureType, descriptor.FigureType);
        }

        [Fact]
        public void GetDescriptorFor_ShouldThrowException_WhenNotFound()
        {
            // Arrange
            var testFigureType = _fixture.Create<FigureType>();
            
            List<FigureType> figureTypes = _fixture
                .CreateMany<FigureType>()
                .Distinct()
                .ToList();

            figureTypes.Remove(testFigureType);
            
            IEnumerable<IFigureDescriptor> descriptors = CreateDescriptors(figureTypes);
            
            var provider = new FigureDescriptorProvider(descriptors);

            // Act
            Exception exception = Record.Exception(() => provider.GetDescriptorFor(testFigureType));

            // Assert
            Assert.NotNull(exception);
            Assert.IsType<InvalidOperationException>(exception);
        }

        private static IEnumerable<IFigureDescriptor> CreateDescriptors(IEnumerable<FigureType> figureTypes)
        {
            foreach (FigureType figureType in figureTypes)
            {
                var mock = new Mock<IFigureDescriptor>();

                mock
                    .SetupGet(descriptor => descriptor.FigureType)
                    .Returns(figureType);

                yield return mock.Object;
            }
        }
    }
}