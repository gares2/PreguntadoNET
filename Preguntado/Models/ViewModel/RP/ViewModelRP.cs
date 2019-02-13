using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Preguntado.Models.Dominio;
using Preguntado.Models.General;

namespace Preguntado.Models.ViewModel.RP
{
    public class ViewModelRP : EntidadViewModel
    {

        public ViewModelRP()
        {
            LlenarListas();
        }

        public ViewModelRP(Pregunta model)
        {
            Id = model.Id;
            Nombre = model.Nombre;
            CategoriaSeleccionada = model.Categoria != null ? model.Categoria.Id : Guid.Empty;
            
            //FechaNacimiento = model.FechaNacimiento.HasValue ? model.FechaNacimiento : DateTime.Now;
            // ActoresSeleccionadas = model.Actores.Any() ? model.Actores.Select(n => n.Id).ToList() : new List<Guid>();
            RespuestasSeleccionadas = model.Respuestas.Any() ? model.Respuestas.Select(re => new ViewModelsRespuesta
            {
                Id = re.Id,
                Nombre = re.Nombre,

                IdJs = re.Id.ToString(),
                Eliminado = false,
                EsCorrecta =re.Escorrecta,
                //FechaNacimiento = persona.FechaNacimiento.HasValue ? persona.FechaNacimiento.Value.ToString("dd/MM/yyyy") : "-"
            }).ToList() : new List<ViewModelsRespuesta>();

            LlenarListas();
        }
    
       public List<SelectBoxItemViewModel> CategoriasDisponibles { get; set; }
       public Guid CategoriaSeleccionada { get; set; }
       public List<ViewModelsRespuesta> RespuestasSeleccionadas { get; set; }

        public void LlenarListas()
        {
            var db = new ApplicationDbContext();

            CategoriasDisponibles = new Repositorio<Categoria>(db).TraerTodos().Select(s => new SelectBoxItemViewModel()
            {
                Id = s.Id,
                Nombre = s.Nombre
            }).ToList();
        }

    }
}