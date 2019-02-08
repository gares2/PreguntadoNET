using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Preguntado.Models.General;
using Preguntado.Models.Dominio;
namespace Preguntado.Models.ViewModel.categoria
{
    public class ViewModelABMCategoria:EntidadViewModel
    {

        public int? cantidadPreguntas { get; set; }
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