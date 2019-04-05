using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Preguntado.Models.General;
namespace Preguntado.Models.ViewModel.Localizacion
{
    public class LocalizacionDetails : EntidadViewModel
    {
        public string latitud { get; set; }         
        public string longitud { get; set; }
        public DateTime? fecha { get; set; }

    }
}