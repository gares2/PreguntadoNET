using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Preguntado.Models.ViewModel.Jugar;
using Microsoft.AspNet.Identity;
namespace Preguntado.Models.Dominio
{
    public class JuegoHistory :Entidad
    {
        public virtual Usuario Usuario { get; set; }
        public virtual Categoria Categoria { set; get; }
        public virtual Respuesta Respuesta { set; get; }
        public virtual Pregunta Pregunta { set; get; }

        public JuegoHistory()
        {
                
        }

        public JuegoHistory(JuegoPRViewModel model ,ApplicationDbContext db)
        {
            Nombre = "History";
           Categoria =  new Repositorio<Categoria>(db).Traer(model.catId);
           Respuesta = new Repositorio<Respuesta>(db).Traer(model.RespSeleccionada);
           Pregunta = new Repositorio<Pregunta>(db).Traer(model.PregId);
           Usuario = new Repositorio<Usuario>(db).Traer(model.UserId);
            
        }
    }
}