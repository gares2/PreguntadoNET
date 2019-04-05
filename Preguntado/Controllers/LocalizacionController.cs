using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Preguntado.Models.ViewModel.Localizacion;
using Preguntado.Models.Dominio;
using Microsoft.AspNet.Identity;
using Preguntado.Models.Enums;
using Preguntado.Models;

namespace Preguntado.Controllers
{
    public class LocalizacionController : BaseController
    {
        ApplicationDbContext db = new ApplicationDbContext();

        // GET: Localizacion
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult GetLocalization()
        {

            if (string.IsNullOrEmpty(this.error))
            {
                List<Localizacion> lista = new Repositorio<Localizacion>(db).TraerTodos().Where(x => x.Usuario.ApplicationUser.Id == this.usuarioId ).ToList();
                List<LocalizacionDetails> model = new List<LocalizacionDetails>();

                model = lista.Select(loca => new LocalizacionDetails
                {
                    Id = loca.Id,
                    Nombre = loca.Nombre,
                    fecha = loca.fecha,
                    latitud =loca.latitud,
                    longitud = loca.longitud
                    //FechaNacimiento = persona.FechaNacimiento.HasValue ? persona.FechaNacimiento.Value.ToString("dd/MM/yyyy") : "-"
                }).ToList();

                return Json(new { Result = model, Error = "" }, JsonRequestBehavior.AllowGet);
            }
            else
                return Json(new { Error = this.error }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        [AllowAnonymous]
        public ActionResult save(/*JuegoPRViewModel model*/)
        {
            if (string.IsNullOrEmpty(this.error))
            {
               List <LocalizacionDetails> model = Newtonsoft.Json.JsonConvert.DeserializeObject<List<LocalizacionDetails>>(this.Contenido);

                foreach (var item in model)
                {
                    Localizacion local = new Localizacion(item, db, this.usuarioId);
                    new Repositorio<Localizacion>(db).Crear(local);
                }

                //model.UserId = new Guid(User.Identity.GetUserId());
                //var respuestas = new Repositorio<Localizacion>(db).TraerTodos().Where(x => x.Pregunta.Id == model.PregId);
                //var respOk = respuestas.Where(p => p.Escorrecta == true).Select(x => x.Id).FirstOrDefault();
                //var hist = new JuegoHistory(model, db);
                //new Repositorio<JuegoHistory>(db).Crear(hist);
                //var pelicula = new Repositorio<Pelicula>(db).Traer(Id);
                //return Json(new { Id = respOk, model = model }, JsonRequestBehavior.AllowGet);
                            
                return Json(new {  resp = "ok" }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { error = this.error }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}