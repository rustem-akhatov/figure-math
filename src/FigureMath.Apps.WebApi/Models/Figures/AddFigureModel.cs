// ReSharper disable UnusedAutoPropertyAccessor.Global

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using FigureMath.Data;

namespace FigureMath.Apps.WebApi
{
    /// <summary>
    /// Model of the input parameters for <see cref="FiguresController.AddFigure"/> action.
    /// </summary>
    public class AddFigureModel
    {
        /// <summary>
        /// Type of the figure.
        /// </summary>
        public FigureType FigureType { get; set; }

        /// <summary>
        /// Figure properties.
        /// </summary>
        [Required]
        public IDictionary<string, double> FigureProps { get; set; }
    }
}