using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Preguntado.Models.ViewModel;
namespace Preguntado.Models.Dominio
{
    public class Usuario : Entidad
    {
        public Usuario()
        {
            Nombre = "Usuario";
        }
        public Usuario(ViewModelAbmUsuario vmodel, ApplicationDbContext db)
        {
            var user = new ApplicationUser { UserName = vmodel.Email, Email = vmodel.Email };
            NickName = vmodel.NickName;
            Nombre = vmodel.Nombre;

            ApplicationUser = user;
        }
        public virtual ApplicationUser ApplicationUser { get; set; }
        public string NickName { get; set; }
        public void Modificar(ViewModelAbmUsuario model, ApplicationDbContext db)
        {
            Nombre = model.Nombre;
            NickName = model.NickName;

        }
    }
}