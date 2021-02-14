// ReSharper disable UnusedAutoPropertyAccessor.Global

using System.Collections.Generic;
using FigureMath.Apps.WebApi.Controllers;
using System.ComponentModel.DataAnnotations;
using FigureMath.Data.Enums;

namespace FigureMath.Apps.WebApi.Models.Figures
{
    /// <summary>
    /// Model of the input parameters for <see cref="FiguresController.PostFigure"/> action.
    /// </summary>
    public class PostFigureModel
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