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
using Preguntado.Models.Enums;

namespace Preguntado.Controllers
{
    public class UsuarioController : Controller
    {
        private ApplicationUserManager _userManager;
        private ApplicationSignInManager _signInManager;
        // GET: Usuario
        ApplicationDbContext db = new ApplicationDbContext();
        public ActionResult Index()
        {

            //var listItems = Enum.GetValues(Colores.GetType()).OfType<Enum>().Select(e =>
            //new SelectListItem
            //{
            //    Text = e.DisplayName(),
            //    Value = e.ToString(),
            //    Selected = e.Equals(Model)
            //});
                return View();
        }
        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }
        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
        }
        public ActionResult Create()
        {
            var model = new  ViewModelAbmUsuario();
            return View(model);
        }
        [HttpPost]
        public async System.Threading.Tasks.Task<ActionResult> Edit(ViewModelAbmUsuario model)
        {
            if (ModelState.IsValid)
            {
                var repo = new Repositorio<Usuario>(db);
                var usu = repo.Traer(model.Id);

                List<Guid> ListRolesprevios = usu.ApplicationUser.Roles.Select(x => new Guid(x.RoleId)).ToList();
                List<Guid> rolesdelete = ListRolesprevios.Except(model.RolesSeleccionadas).ToList();

                usu.Modificar(model, db);
                repo.Modificar(usu);

                string rol;

                //List<Guid> ListRolesprevios = usu.ApplicationUser.Roles.Select(x => new Guid(x.RoleId)).ToList();
                //List<Guid> rolesdelete = ListRolesprevios.Except(model.RolesSeleccionadas).ToList();

                foreach (Guid item in ListRolesprevios)
                {
                    rol = model.RolesDisponibles.Where(x => x.Id == item).Select(x => x.Nombre).FirstOrDefault();
                    UserManager.RemoveFromRole(usu.ApplicationUser.Id, rol);
                }

                foreach (Guid item in model.RolesSeleccionadas)
                {
                    rol = model.RolesDisponibles.Where(x => x.Id == item).Select(x => x.Nombre).FirstOrDefault();
                    UserManager.AddToRole(usu.ApplicationUser.Id, rol);
                }

                await SignInManager.SignInAsync(usu.ApplicationUser, isPersistent: false, rememberBrowser: false);
                return RedirectToAction("Index", "Usuario");

               // return RedirectToAction("Index");

            }
            return View(model);
        }
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async System.Threading.Tasks.Task<ActionResult> Create(ViewModelAbmUsuario model)
        {
            //ModelState.AddModelError("FechaNacimiento", "La fecha no es válida");

            if (ModelState.IsValid)
            {
                var usuario = new Usuario(model, db);

                //var user = new ApplicationUser { UserName = model.Email, Email = model.Email };

                //var usuario = new Usuario()
                //{
                //    ApplicationUser = user,
                //    NickName = model.NickName
                //};

                //var db = new ApplicationDbContext();
                new Repositorio<Usuario>(db).Crear(usuario);

                UserManager.AddPassword(usuario.ApplicationUser.Id, model.Password);
                string rol;
                foreach (Guid item in model.RolesSeleccionadas)
                {
                    rol = model.RolesDisponibles.Where(x => x.Id == item).Select(x => x.Nombre).FirstOrDefault();
                    UserManager.AddToRole(usuario.ApplicationUser.Id, rol);
                }
             
                await SignInManager.SignInAsync(usuario.ApplicationUser, isPersistent: false, rememberBrowser: false);
                return RedirectToAction("Index", "Usuario");

                //new Repositorio<Actor>(db).Crear(actor);
                //return RedirectToAction("Index");
            }

            return View(model);
        }
        public ActionResult Edit(Guid id)
        {
            var usuario = new Repositorio<Usuario>(db).Traer(id);
            
            var model = new  ViewModelAbmUsuario(usuario);

            return View(model);
        }
        [HttpPost]
        public ActionResult Delete(Guid Id)
        {
            //if (ModelState.IsValid)
            //{
            var repo = new Repositorio<Usuario>(db);
            var usu = repo.Traer(Id);
            repo.Eliminar(usu);

            return Json(new { Id = Id }, JsonRequestBehavior.AllowGet);

            //}
            //return View(model);
        }

        public ActionResult GetData()
        {
            var nombre = Request.QueryString["nombre"];
            var start = Convert.ToInt32(Request.QueryString["start"]);
            var cantidad = Convert.ToInt32(Request.QueryString["length"]);
            var orderColumn = Request.QueryString["order[0][column]"];
            var orderDir = Request.QueryString["order[0][dir]"];
            var mail = Request.QueryString["mail"];
            
            var query = new Repositorio<Usuario>(db).TraerTodos().OrderBy(p => p.Nombre);
            var cantidadTotal = query.Count();

            if (!string.IsNullOrEmpty(nombre))
                query = query.Where(p => p.Nombre.Contains(nombre)).OrderBy(p => p.Nombre);

            if (!string.IsNullOrEmpty(mail))
                query = query.Where(p => p.ApplicationUser.Email.Contains(mail)).OrderBy(p => p.ApplicationUser.Email);

            var cantidadFiltradas = query.Count();

            if (orderDir == "asc")
            {
                if (orderColumn == "1")
                    query = query.OrderBy(r => r.NickName);
                else
                    query = query.OrderBy(r => r.Nombre);
            }
            else
            {
                if (orderColumn == "1")
                    query = query.OrderByDescending(r => r.NickName);
                else
                    query = query.OrderByDescending(r => r.Nombre);
            }

            //var data = query.Skip(start).Take(cantidad).ToList().Select(p => new {
            //    Id = p.Id,
            //    Nombre = p.Nombre,
            //    peli =  string.Join(",", p.peliculas.Select(d => d.Nombre)),
            //    pelis = p.peliculas.Select(t => new
            //    {
            //        Id = t.Id,
            //        Nombre = t.Nombre
            //    }),
            //FechaNacimiento = p.FechaNac.HasValue ? p.FechaNac.Value.ToString("dd/MM/yyyy") : "-"
            //}).ToList();
           // List<string> lista = query.Select(m => m UserManager.GetRolesAsync(p.ApplicationUser.Id.ToString()).Result.ToList();

            var data = query.Skip(start).Take(cantidad).ToList().Select(p => new
            {
                Id = p.Id,
                Nombre = p.Nombre,
                NickName = p.NickName,
                role =  string.Join(",", UserManager.GetRolesAsync(p.ApplicationUser.Id.ToString()).Result.ToList()),               
                roles = UserManager.GetRolesAsync(p.ApplicationUser.Id).Result.ToList(),
                mail =  p.ApplicationUser.Email

            }).ToList();
            var result = new { data = data, recordsTotal = cantidadTotal, recordsFiltered = cantidadFiltradas };
            return Json(result, JsonRequestBehavior.AllowGet);

        }
    }
}