using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Preguntado.Models.Dominio;
using Preguntado.Models.General;

namespace Preguntado.Models.ViewModel.RP
{
    public class FiltroRPViewModel : EntidadViewModel 
    {
        
        public string Pregunta { get; set; }
        public List<SelectBoxItemViewModel> CategoriaDisponibles { get; set; }
        public Guid CategoriaSeleccionado { get; set; }

        public FiltroRPViewModel()
        {
            CategoriaDisponibles = new List<SelectBoxItemViewModel>();
            LlenarListas();
        }

        public FiltroRPViewModel(Categoria model)
        {

        }
        public void LlenarListas()
        {
            var db = new ApplicationDbContext();

            CategoriaDisponibles = new Repositorio<Categoria>(db).TraerTodos().Select(s => new SelectBoxItemViewModel()
            {
                Id = s.Id,
                Nombre = s.Nombre
            }).ToList();
        }
    }
}