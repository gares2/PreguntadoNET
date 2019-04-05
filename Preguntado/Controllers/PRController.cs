using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Preguntado.Models.ViewModel.RP;
using Preguntado.Models.Dominio;
using Preguntado.Models;
using Preguntado.Models.Enums;
namespace Preguntado.Controllers
{
    [Authorize]
    public class PRController : Controller
    {

        // GET: PR
        ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult Index()
        {
            FiltroRPViewModel filtro = new FiltroRPViewModel();

            return View(filtro);
        }

        public ActionResult GetData()
        {
            var nombre = Request.QueryString["nombre"];
            var start = Convert.ToInt32(Request.QueryString["start"]);
            var cantidad = Convert.ToInt32(Request.QueryString["length"]);
            var orderColumn = Request.QueryString["order[0][column]"];
            var orderDir = Request.QueryString["order[0][dir]"];
            var Categoria = Request.QueryString["CategoriaSeleccionado"];
            var fechadesde = Request.QueryString["fechadesde"];
            var fechahasta = Request.QueryString["fechahasta"];

            var query = new Repositorio<Pregunta>(db).TraerTodos().OrderBy(p => p.Nombre);
            var cantidadTotal = query.Count();

            if (!string.IsNullOrEmpty(nombre))
                query = query.Where(p => p.Nombre.Contains(nombre)).OrderBy(p => p.Nombre);
            {
            }

            if (!string.IsNullOrEmpty(Categoria))
            { 
                Guid cate = new Guid(Categoria);

                if (cate != Guid.Empty)
                {
                    Guid x;
                    Guid.TryParse(Categoria, out x);
                    query = query.Where(p => p.Categoria.Id == x).OrderBy(p => p.Nombre);
                }
            }

            var cantidadFiltradas = query.Count();

            if (orderDir == "asc")
            {
                if (orderColumn == "1")
                    query = query.OrderBy(r => r.Categoria.Nombre);
                else
                    query = query.OrderBy(r => r.Nombre);
            }
            else
            {
                if (orderColumn == "1")
                    query = query.OrderByDescending(r => r.Categoria.Nombre);
                else
                    query = query.OrderByDescending(r => r.Nombre);
            }
           
            var data = query.Skip(start).Take(cantidad).ToList().Select(p => new
            {
                Id = p.Id,
                Pregunta = p.Nombre,
                peli = string.Join(",", p.Respuestas.Select(d => d.Nombre)),
                Categoria =  p.Categoria.Nombre, //string.Join(",", p.peliculas.Select(d => d.Genero.Nombre)),
                correctas = p.Respuestas.Where(x=> x.Escorrecta==true).Select(x=> x.Nombre).FirstOrDefault(),
                respuestas = p.Respuestas.Select(t => new
                {
                    Id = t.Id,
                    Nombre = t.Nombre
                    //genero = t.Genero.Nombre
                }).ToList(),
                 
                //FechaNacimiento = p.FechaNac.HasValue ? p.FechaNac.Value.ToString("dd/MM/yyyy") : "-"
            }).ToList();
            var result = new { data = data, recordsTotal = cantidadTotal, recordsFiltered = cantidadFiltradas };
            return Json(result, JsonRequestBehavior.AllowGet);

        }
        [Authorize(Roles = RoleConst.Administrador)]
        public ActionResult Create()
        {
            var model = new ViewModelRP();

            return View(model);
        }
        [Authorize(Roles = RoleConst.Administrador)]
        public ActionResult Edit(Guid Id)
        {
            var pregunta = new Repositorio<Pregunta>(db).Traer(Id);
            var model = new ViewModelRP(pregunta);
            //return Json(new { Id = Id}, JsonRequestBehavior.AllowGet);
            return View(model);
        }
        [Authorize(Roles = RoleConst.Administrador)]
        public ActionResult EditJSJason(Guid Id)
        {
            var pregunta = new Repositorio<Pregunta>(db).Traer(Id);
            var model = new ViewModelRP(pregunta);
            return Json(new { Id = Id, model = model }, JsonRequestBehavior.AllowGet);

        }
        [Authorize(Roles = RoleConst.Administrador)]
        [HttpPost]
        public ActionResult EditJSJason(ViewModelRP model)
        {
            var errores = new List<string>();

            if (string.IsNullOrEmpty(model.Nombre))
                errores.Add("Debe completar el Nombre");
            //  if (!model.FechaNacimiento.HasValue)
            //    errores.Add("Debe completar ll Fecha de Nacimiento");
            if (model.CategoriaSeleccionada == Guid.Empty)
                errores.Add("Debe completar La categoria");
            if (model.RespuestasSeleccionadas.Where(x=> x.Eliminado!=true).ToList().Count != 4)
                errores.Add("La Pregunta debe tener Cuatro Respuestas");
            var correctas = model.RespuestasSeleccionadas.Where(x => x.EsCorrecta).ToList();
            if (correctas.Count != 1)
                errores.Add("Debe Existir una Respuesta correcta");

            var groupBy = model.RespuestasSeleccionadas.GroupBy(r => r.Nombre).Select(a => new {
                Cuenta = a.Count(),
            }).ToList();


            foreach (var item in groupBy)
            {
                if (item.Cuenta > 1)
                    errores.Add("No puede haber dos respuestas con el mismo Nombre");
            }


            if (!errores.Any())
            {
                //var pelicula = new Pelicula(model, db);
                //new Repositorio<Pelicula>(db).Crear(pelicula);
            
                var repo = new Repositorio<Pregunta>(db);
                var pregunta = repo.Traer(model.Id);
                pregunta.ModificarJs(model, db);
                repo.Modificar(pregunta);

            }
            if (errores.Any())
                return Json(new { Result = "", Error = errores }, JsonRequestBehavior.AllowGet);

            return Json(new { Result = "OK", Error = "" }, JsonRequestBehavior.AllowGet);

        }
        [Authorize(Roles = RoleConst.Administrador)]
        [HttpPost]
        public ActionResult Delete(Guid Id)
        {
            //if (ModelState.IsValid)
            //{
            var repo = new Repositorio<Pregunta>(db);
            var pregunta = repo.Traer(Id);
            repo.Eliminar(pregunta);

            return Json(new { Id = Id }, JsonRequestBehavior.AllowGet);
            //}
            //return View(model);
        }
        [Authorize(Roles = RoleConst.Administrador)]
        [HttpPost]
        public ActionResult CreateJS(ViewModelRP model)
        {
            var errores = new List<string>();

            if (string.IsNullOrEmpty(model.Nombre))
                errores.Add("Debe completar el Nombre");
            //  if (!model.FechaNacimiento.HasValue)
            //    errores.Add("Debe completar ll Fecha de Nacimiento");
            if (model.CategoriaSeleccionada == Guid.Empty)
                errores.Add("Debe completar La categoria");
             if (model.RespuestasSeleccionadas.Count != 4)
                errores.Add("La Pregunta debe tener Cuatro Respuestas");
            var correctas = model.RespuestasSeleccionadas.Where(x => x.EsCorrecta).ToList();
            if (correctas.Count != 1)
                errores.Add("Debe Existir una Respuesta correcta");

            var groupBy = model.RespuestasSeleccionadas.GroupBy(r =>  r.Nombre).Select(a => new {
                Cuenta = a.Count(),
            }).ToList();

            foreach (var item in groupBy)
            {
                 if(item.Cuenta > 1)
                    errores.Add("No puede haber dos respuestas con el mismo Nombre");
            }

            if (!errores.Any())
            {
                var pregunta = new  Pregunta (model, db);
                new Repositorio<Pregunta>(db).Crear(pregunta);
            }

            if (errores.Any())
                return Json(new { Result = "", Error = errores }, JsonRequestBehavior.AllowGet);

            return Json(new { Result = "OK", Error = "" }, JsonRequestBehavior.AllowGet);
        }

    }
}