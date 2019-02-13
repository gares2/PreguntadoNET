using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Preguntado.Models.General;
using Preguntado.Models.Dominio;
namespace Preguntado.Models.ViewModel.RP
{
    public class ViewModelsRespuesta :EntidadViewModel
    {
        public string IdJs { get; set; }
        public bool Eliminado { get; set; }
        public bool EsCorrecta { get; set; }

    }
}
