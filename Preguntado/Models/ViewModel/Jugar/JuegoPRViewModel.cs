using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Preguntado.Models.General;
namespace Preguntado.Models.ViewModel.Jugar
{
    public class JuegoPRViewModel: EntidadViewModel
    {
        public Guid catId { get; set; }
        public Guid PregId { get; set; }
        public List<JuegoRespuestaViewModel> RespuestaDisponibles {get; set;}
        public Guid RespSeleccionada { get; set; }
        public Guid UserId { get; set; }

    }
}