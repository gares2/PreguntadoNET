using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Preguntado.Models.ViewModel.categoria;
namespace Preguntado.Models.Dominio
{
    public class Categoria:Entidad
    {
       public  int? CantidadPreguntas { get; set; }
        public string Color { get; set; }
        public Categoria()
        {

        }
        public Categoria(ViewModelABMCategoria vmodel, ApplicationDbContext db)
        {
            Nombre = vmodel.Nombre;
            CantidadPreguntas = vmodel.cantidadPreguntas;
            Color = vmodel.Color;
        }
        public void Modificar(ViewModelABMCategoria vmodel, ApplicationDbContext db)
        {
            Nombre = vmodel.Nombre;
            Color = vmodel.Color;
            CantidadPreguntas = vmodel.cantidadPreguntas;
        }
    }
}