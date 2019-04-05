using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Preguntado.Models.ViewModel.Localizacion;
namespace Preguntado.Models.Dominio
{
    public class Localizacion :Entidad
    {
        public string latitud { get; set; }
        public string longitud { get; set; }
        public DateTime? fecha { get; set; }
        public virtual Usuario Usuario { get; set; }
        public Localizacion()
        {

        }
        public Localizacion(LocalizacionDetails model, ApplicationDbContext db, string idUsuario)
        {
            this.Usuario = new Repositorio<Usuario>(db).TraerTodos().FirstOrDefault(x => x.ApplicationUser.Id == idUsuario);
            Nombre = "Location";
            latitud = model.latitud;
            longitud = model.longitud;
            fecha = model.fecha;

        }
    }
}