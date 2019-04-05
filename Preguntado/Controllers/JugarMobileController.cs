using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Preguntado.Models;
using Preguntado.Models.Dominio;
using Preguntado.Models.ViewModel.Jugar;
using Microsoft.AspNet.Identity;
using Preguntado.Models.Enums;

namespace Preguntado.Controllers
{
    public class JugarMobileController : BaseController
    {
        // GET: JugarMobile
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult IndexMobile()
        {

            if (string.IsNullOrEmpty(this.error))
            {
                var categorias = new Repositorio<Categoria>(db).TraerTodos().Where(x => x.CantidadPreguntas == x.Preguntas.Where(p => p.Eliminado == false).Count());
                List<JugarCategoriaViewModel> model = new List<JugarCategoriaViewModel>();

                model = categorias.Select(cate => new JugarCategoriaViewModel
                {
                    Id = cate.Id,
                    Nombre = cate.Nombre,
                    color = cate.Color
                    //FechaNacimiento = persona.FechaNacimiento.HasValue ? persona.FechaNacimiento.Value.ToString("dd/MM/yyyy") : "-"
                }).ToList();

                return Json(new { Result = model, Error = "" }, JsonRequestBehavior.AllowGet);
            }
            else
                return Json(new { Error = this.error }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult saveGame(/*JuegoPRViewModel model*/)
        {
            if (string.IsNullOrEmpty(this.error))
            {
                JuegoPRViewModel model = Newtonsoft.Json.JsonConvert.DeserializeObject<JuegoPRViewModel>(this.Contenido);
                //model.UserId = new Guid(User.Identity.GetUserId());
                var respuestas = new Repositorio<Respuesta>(db).TraerTodos().Where(x => x.Pregunta.Id == model.PregId);
                var respOk = respuestas.Where(p => p.Escorrecta == true).Select(x => x.Id).FirstOrDefault();

                var hist = new JuegoHistory(model, db);
                new Repositorio<JuegoHistory>(db).Crear(hist);
                //var pelicula = new Repositorio<Pelicula>(db).Traer(Id);
                return Json(new { Id = respOk, model = model }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                    return Json(new { error = this.error }, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult Result()
        {
            if (string.IsNullOrEmpty(this.error))
            {
                Guid id = Newtonsoft.Json.JsonConvert.DeserializeObject<Guid>(this.Contenido);
                var user = this.usuarioId;
                Usuario usuario = new Repositorio<Usuario>(db).TraerTodos().FirstOrDefault(x => x.ApplicationUser.Id == user);
                var histories = new Repositorio<JuegoHistory>(db).TraerTodos().Where(x => x.Usuario.Id == usuario.Id && x.Categoria.Id == id);
                //var respuestas = new Repositorio<Respuesta>(db).TraerTodos().Where(x=> x.Pregunta.Categoria.Id==)
                List<JuegoPRViewModel> model = new List<JuegoPRViewModel>();

                model = histories.Select(hist => new JuegoPRViewModel
                {
                    Id = hist.Id,
                    Nombre = hist.Pregunta.Nombre,
                    catId = hist.Categoria.Id,
                    PregId = hist.Pregunta.Id,
                    RespSeleccionada = hist.Respuesta.Id,
                    RespuestaDisponibles = hist.Pregunta.Respuestas.Select(t => new JuegoRespuestaViewModel { esCorrecta = t.Escorrecta, Id = t.Id, Nombre = t.Nombre }).ToList()
                    //FechaNacimiento = persona.FechaNacimiento.HasValue ? persona.FechaNacimiento.Value.ToString("dd/MM/yyyy") : "-"
                }).ToList();

                return Json(new { model = model, Result = "" }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { error = this.error }, JsonRequestBehavior.AllowGet);
            }
        }
        ApplicationDbContext db = new ApplicationDbContext();
        public ActionResult play()
        {
            
            if (string.IsNullOrEmpty(this.error))
            {      
               Guid id =  Newtonsoft.Json.JsonConvert.DeserializeObject<Guid>(this.Contenido);
                JuegoPRViewModel model = new JuegoPRViewModel();
                var user =this.usuarioId;
                var historiepreguntas = new Repositorio<JuegoHistory>(db).TraerTodos().Where(x => x.Categoria.Id == id && x.Usuario.ApplicationUser.Id == user);
                var preguntas = new Repositorio<Pregunta>(db).TraerTodos().Where(x => x.Categoria.Id == id);
                var categoria = new Repositorio<Categoria>(db).Traer(id);
                Usuario usuario = new Repositorio<Usuario>(db).TraerTodos().FirstOrDefault(x => x.ApplicationUser.Id == user);
                Random rnd = new Random();
                List<Guid> historyPreg = historiepreguntas.Select(x => x.Pregunta.Id).ToList();
                List<Guid> preguntasall = preguntas.Select(x => x.Id).ToList();
                List<Guid> pregFaltantes = preguntasall.Except(historyPreg).ToList();

                int indice = rnd.Next(pregFaltantes.Count());
                if (pregFaltantes.Count > 0)
                {
                    Pregunta preg = preguntas.ToList().Where(x => x.Id == pregFaltantes[indice]).FirstOrDefault();
                    model.Nombre = preg.Nombre;
                    model.PregId = preg.Id;
                    model.catId = categoria.Id;
                    model.UserId = usuario.Id;

                    model.RespuestaDisponibles = preg.Respuestas.Select(x => new JuegoRespuestaViewModel { Id = x.Id, esCorrecta = x.Escorrecta, Nombre = x.Nombre }).ToList();
                    return Json(new { model = model, Result = "" }, JsonRequestBehavior.AllowGet);
                }
                else
                    return Json(new { Result = categoria.Id }, JsonRequestBehavior.AllowGet);
                //return RedirectToAction("Result", new { id = categoria.Id });
            }
            else
            {
                return Json(new { error = this.error }, JsonRequestBehavior.AllowGet);
            }
        }

    }
}