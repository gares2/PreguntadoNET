using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Preguntado.Models.General;
using Preguntado.Models.Dominio;
namespace Preguntado.Models.ViewModel.RP
{
    public class ViewModelRPGrilla: EntidadViewModel
    {
        public string Categoria { get; set; }
        public string Pregunta { get; set; }
        public string Respuestas { get; set; }
        public ViewModelRPGrilla()
        {

        }

    }
}