using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Preguntado.Models.Dominio;
using Preguntado.Models;
using Microsoft.AspNet.Identity.Owin;
using Preguntado.Models.ViewModel;
using Microsoft.AspNet.Identity;
using Preguntado.Models.ViewModel.categoria;
using Preguntado.Models.Enums;
using Preguntado.Helper;
namespace Preguntado.Controllers
{
    [Authorize]
    public class CategoriaController : Controller
    {
        // GET: Categoria

        ApplicationDbContext db = new ApplicationDbContext();
        public ActionResult Index()
        {
            var Categorias = new Repositorio<Categoria>(db).TraerTodos().ToList();
            var model = new List<ViewModelGrillaCategoria>();

            if (Categorias != null)
            {
                model = Categorias.Select(cate => new ViewModelGrillaCategoria
                {
                    Id = cate.Id,
                    Nombre = cate.Nombre,
                    cantidadPreguntas = cate.CantidadPreguntas,
                    Color = cate.Color
                }).ToList();
            }
            return View(model);
        }
        [Authorize(Roles = RoleConst.Administrador)]
        public ActionResult Create()
        {
            var model = new ViewModelABMCategoria();
            return View(model);
        }
        [Authorize(Roles = RoleConst.Administrador)]
        [HttpPost]
        public ActionResult Create(ViewModelABMCategoria model)
        {
            //ModelState.AddModelError("FechaNacimiento", "La fecha no es válida");

            if (ModelState.IsValid)
            {
                var cate = new Categoria(model, db);
                new Repositorio<Categoria>(db).Crear(cate);

                LoggerEventos.LogearEvento("Crear Categoria: " + cate.Nombre, User.Identity.GetUserId(), cate.Id, AccionesLogEnum.Crear, EntidadLogEnum.Categoria);

                return RedirectToAction("Index");
            }
            return View(model);
        }
        [Authorize(Roles = RoleConst.Administrador)]
        public ActionResult Edit(Guid id)
        {
            var cate = new Repositorio<Categoria>(db).Traer(id);

            var model = new ViewModelABMCategoria(cate);
            
            return View(model);
        }
        [Authorize(Roles = RoleConst.Administrador)]
        [HttpPost]
        public ActionResult Edit(ViewModelABMCategoria model)
        {
            if (ModelState.IsValid)
            {
                var repo = new Repositorio<Categoria>(db);
                var categoria = repo.Traer(model.Id);

                categoria.Modificar(model, db);
                repo.Modificar(categoria);
                return RedirectToAction("Index");

            }
            return View(model);
        }
        [Authorize(Roles = RoleConst.Administrador)]
        [HttpPost]
        public ActionResult Delete(Guid Id)
        {
            //if (ModelState.IsValid)
            //{
            var repo = new Repositorio<Categoria>(db);
            var cate = repo.Traer(Id);
            repo.Eliminar(cate);

            return Json(new { Id = Id }, JsonRequestBehavior.AllowGet);

            //}
            //return View(model);
        }

    }
}