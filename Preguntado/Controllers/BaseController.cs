using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Preguntado.Models;
using Preguntado.Models.Dominio;
using System.Net;

namespace Preguntado.Controllers
{
    public class BaseController : Controller
    {
        // GET: Base
        public string Contenido { get =>
                System.Web.HttpContext.Current.Request.HttpMethod == "POST" ?
                System.Web.HttpContext.Current.Request.Form["Contenido"] : System.Web.HttpContext.Current.Request.QueryString["Contenido"]; }
        ApplicationDbContext db = new ApplicationDbContext();

        protected string usuarioId { get; set; }
        private string tokenId { get; set; }
        public string error { get; set; }

        public BaseController()
        {
           var t = this.ViewData.Values;
            var usuario = System.Web.HttpContext.Current.Request.HttpMethod == "POST" ? System.Web.HttpContext.Current.Request.Form["usuarioId"] : System.Web.HttpContext.Current.Request.QueryString["usuarioId"];
            var tokenId = System.Web.HttpContext.Current.Request.HttpMethod == "POST" ? System.Web.HttpContext.Current.Request.Form["tokenId"] : System.Web.HttpContext.Current.Request.QueryString["tokenId"];
            usuarioId = usuario;
                var usu = new Repositorio<Usuario>(db).TraerTodos().Where(x => x.ApplicationUser.Id == usuario).FirstOrDefault();
                if (usu != null)
                {
                    if (tokenId != usu.Token)
                        error = Convert.ToString(HttpStatusCode.Unauthorized) + " Token invalido";
                }
                else
                {
                    error = Convert.ToString(HttpStatusCode.Forbidden) + " Usuario Invalido";
                }
        }
    }
}