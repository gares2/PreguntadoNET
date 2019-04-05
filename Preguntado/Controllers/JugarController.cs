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

    [Authorize]
    public class JugarController : Controller
    {
        // GET: Jugar
        ApplicationDbContext db = new ApplicationDbContext();
        [Authorize(Roles = RoleConst.Suscriptor)]
        public ActionResult Result(Guid id)
        {

            var user = User.Identity.GetUserId();
            Usuario usuario = new Repositorio<Usuario>(db).TraerTodos().FirstOrDefault(x => x.ApplicationUser.Id == user);
            var histories = new Repositorio<JuegoHistory>(db).TraerTodos().Where(x => x.Usuario.Id == usuario.Id && x.Categoria.Id== id);
            //var respuestas = new Repositorio<Respuesta>(db).TraerTodos().Where(x=> x.Pregunta.Categoria.Id==)
            List<JuegoPRViewModel> model = new List<JuegoPRViewModel>();
            
            model = histories.Select(hist => new JuegoPRViewModel
            {
                Id = hist.Id,
                Nombre = hist.Pregunta.Nombre,
                catId = hist.Categoria.Id,
                PregId = hist.Pregunta.Id,
                RespSeleccionada = hist.Respuesta.Id,
                RespuestaDisponibles = hist.Pregunta.Respuestas.Select(t=> new JuegoRespuestaViewModel { esCorrecta= t.Escorrecta, Id= t.Id, Nombre= t.Nombre }).ToList()
                //FechaNacimiento = persona.FechaNacimiento.HasValue ? persona.FechaNacimiento.Value.ToString("dd/MM/yyyy") : "-"
            }).ToList();

            return View(model);

        }
        [Authorize(Roles = RoleConst.Suscriptor)]
        public ActionResult Index()
        {
            var categorias = new Repositorio<Categoria>(db).TraerTodos().Where(x=>  x.CantidadPreguntas== x.Preguntas.Where(p=> p.Eliminado==false).Count() );

            List <JugarCategoriaViewModel> model = new List< JugarCategoriaViewModel>();
           
            model = categorias.Select(cate => new JugarCategoriaViewModel
            {
                Id = cate.Id,
                Nombre = cate.Nombre,
                color = cate.Color
                //FechaNacimiento = persona.FechaNacimiento.HasValue ? persona.FechaNacimiento.Value.ToString("dd/MM/yyyy") : "-"
            }).ToList();

            return View(model);
        }

        public ActionResult IndexMobile()
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

        [Authorize(Roles = RoleConst.Suscriptor)]
        [HttpPost]
        public ActionResult saveGame(JuegoPRViewModel model)
        {
            //model.UserId = new Guid(User.Identity.GetUserId());
            var respuestas = new Repositorio<Respuesta>(db).TraerTodos().Where(x=> x.Pregunta.Id== model.PregId);
            var respOk = respuestas.Where(p=> p.Escorrecta==true ).Select(x => x.Id).FirstOrDefault();

            var hist = new JuegoHistory(model, db);
            
            new Repositorio<JuegoHistory>(db).Crear(hist);
            //var pelicula = new Repositorio<Pelicula>(db).Traer(Id);

            return Json(new { Id = respOk, model = model }, JsonRequestBehavior.AllowGet);

        }
        [Authorize(Roles = RoleConst.Suscriptor)]
        public ActionResult play(Guid id)
        {

            JuegoPRViewModel model = new JuegoPRViewModel();
            var user = User.Identity.GetUserId();
            var historiepreguntas = new Repositorio<JuegoHistory>(db).TraerTodos().Where(x=> x.Categoria.Id ==id && x.Usuario.ApplicationUser.Id == user);
            var preguntas = new Repositorio<Pregunta>(db).TraerTodos().Where(x => x.Categoria.Id == id);
            var categoria = new Repositorio<Categoria>(db).Traer(id);
            Usuario usuario = new Repositorio<Usuario>(db).TraerTodos().FirstOrDefault(x=> x.ApplicationUser.Id==user);
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
                return View(model);
            }
            else
            return RedirectToAction("Result", new { id = categoria.Id });

        }
    }
}