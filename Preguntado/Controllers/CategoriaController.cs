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

namespace Preguntado.Controllers
{
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

        public ActionResult Create()
        {
            var model = new ViewModelABMCategoria();
            return View(model);
        }

        [HttpPost]
        public ActionResult Create(ViewModelABMCategoria model)
        {
            //ModelState.AddModelError("FechaNacimiento", "La fecha no es válida");

            if (ModelState.IsValid)
            {
                var cate = new Categoria(model, db);
                new Repositorio<Categoria>(db).Crear(cate);
                return RedirectToAction("Index");
            }
            return View(model);
        }

        public ActionResult Edit(Guid id)
        {
            var cate = new Repositorio<Categoria>(db).Traer(id);

            var model = new ViewModelABMCategoria(cate);

            return View(model);
        }
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


    }
}