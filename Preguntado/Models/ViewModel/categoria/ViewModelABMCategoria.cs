using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Preguntado.Models.General;
using Preguntado.Models.Dominio;
using System.ComponentModel.DataAnnotations;
namespace Preguntado.Models.ViewModel.categoria
{
    public class ViewModelABMCategoria:EntidadViewModel
    {
        [Required(ErrorMessage = "Debe Completar la cant. de preguntas")]
        public int? cantidadPreguntas { get; set; }
        [Required(ErrorMessage = "Debe Completar el color")]
        public string Color { get; set; }

        public ViewModelABMCategoria()
        {

        }

        public ViewModelABMCategoria(Categoria cate)
        {
            Nombre = cate.Nombre;
            cantidadPreguntas = cate.CantidadPreguntas;
            Color = cate.Color;
        }
    }
}