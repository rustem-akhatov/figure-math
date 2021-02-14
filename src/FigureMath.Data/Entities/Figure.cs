// ReSharper disable UnusedAutoPropertyAccessor.Local

using System.Collections.Immutable;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using FigureMath.Data.Enums;

namespace FigureMath.Data.Entities
{
    /// <summary>
    /// Defines a figure. Uses to keep information about figure in persistent store. 
    /// </summary>
    [Table("figure")]
    public class Figure
    {
        /// <summary>
        /// Max length of the <see cref="FigureType"/>.
        /// </summary>
        public const int FigureTypeMaxLength = 50;
        
        /// <summary>
        /// Identifier of the figure.
        /// </summary>
        [Key]
        [Required]
        public long Id { get; private set; }

        /// <summary>
        /// Type of the figure.
        /// </summary>
        [Required]
        public FigureType FigureType { get; init; }

        /// <summary>
        /// Figure properties.
        /// </summary>
        [Required]
        [Column(TypeName = "jsonb")]
        public IImmutableDictionary<string, double> FigureProps { get; init; }
    }
}