using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Preguntado.Models.ViewModel.RP;

namespace Preguntado.Models.Dominio
{
    public class Respuesta:Entidad
    {
        public virtual Pregunta Pregunta { set; get; }

        public bool Escorrecta { set; get; }
        public Respuesta()
        {

        }

        public void ModificarJs(ViewModelsRespuesta vmodel, ApplicationDbContext db)
        {
            Nombre = vmodel.Nombre;
            Escorrecta = vmodel.EsCorrecta;

        }

        public Respuesta( ViewModelsRespuesta vmodel, ApplicationDbContext db)
        {
            Nombre = vmodel.Nombre;
            Escorrecta = vmodel.EsCorrecta;
        }
    }
}