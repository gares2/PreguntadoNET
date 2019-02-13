using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Preguntado.Models.General;
using Preguntado.Models.ViewModel.RP;
namespace Preguntado.Models.Dominio
{
    public class Pregunta : Entidad
    {
        public virtual Categoria Categoria  {set; get; }
        public virtual ICollection<Respuesta> Respuestas {set; get; }

        public Pregunta()
        {
                
        }
        public Pregunta(ViewModelRP viewmodel, ApplicationDbContext db)
        {
            Nombre = viewmodel.Nombre;
          
            if (viewmodel.CategoriaSeleccionada != Guid.Empty)
              Categoria   = new Repositorio<Categoria>(db).Traer(viewmodel.CategoriaSeleccionada);

            if (Respuestas == null)
                Respuestas = new List<Respuesta>();

            foreach (var resId in viewmodel.RespuestasSeleccionadas)
            {
                var respuesta = new Respuesta(resId, db);
                new Repositorio<Respuesta>(db).Crear(respuesta);
                Respuestas.Add(new Repositorio<Respuesta>(db).Traer(respuesta.Id));
            }
            //if (viewmodel.FechaNacimiento.HasValue)
            //    FechaNacimiento = viewmodel.FechaNacimiento.Value;
        }

        public void ModificarJs(ViewModelRP viewmodel, ApplicationDbContext db)
        {
            Nombre = viewmodel.Nombre;
            if (viewmodel.CategoriaSeleccionada != Guid.Empty)
                Categoria = new Repositorio<Categoria>(db).Traer(viewmodel.CategoriaSeleccionada);
            else
                Categoria = null;
       

            if (Respuestas == null)
                Respuestas = new List<Respuesta>();

            var respuestaPrevios = Respuestas.Select(a => a.Id).ToList();
            var respuestaEliminados = viewmodel.RespuestasSeleccionadas.Where(x => x.Eliminado == true && x.Id != Guid.Empty).Select(a => a.Id).ToList();
            var respuestaNoEliminados = viewmodel.RespuestasSeleccionadas.Where(x => x.Eliminado == false).ToList();


            foreach (var actorEliminado in respuestaEliminados)
            {
                var item = Respuestas.First(a => a.Id == actorEliminado);
                respuestaPrevios.Remove(item.Id);
                Respuestas.Remove(item);


            }
            bool previo = false;
            foreach (var actNoeli in respuestaNoEliminados)
            {
                foreach (var resId in respuestaPrevios)
                {
                    if (resId.ToString() == actNoeli.Id.ToString())
                    {
                        //var repo = new Repositorio<Actor>(db);
                        //var actoraux = repo.Traer(actId);
                        var item = Respuestas.First(a => a.Id == resId);
                        item.ModificarJs(actNoeli, db);
                        //repo.Modificar(actoraux);

                        //Actores.Remove(item);
                        //new Repositorio<Actor>(db).Crear(actor);
                        //Actores.Add(new Repositorio<Actor>(db).Traer(actoraux.Id));
                        previo = true;
                        break;
                    }
                }
                if (previo == false)
                {
                    var respuesta = new Respuesta(actNoeli, db);
                    new Repositorio<Respuesta>(db).Crear(respuesta);
                    Respuestas.Add(new Repositorio<Respuesta>(db).Traer(respuesta.Id));
                }
                previo = false;

            }
            //{
            //    Actores.Add(new Repositorio<Actor>(db).Traer(actId));
            //}
            //if (viewmodel.FechaNacimiento.HasValue)
            //    FechaNacimiento = viewmodel.FechaNacimiento.Value;
            //else
            //    FechaNacimiento = null;
        }

    }
}