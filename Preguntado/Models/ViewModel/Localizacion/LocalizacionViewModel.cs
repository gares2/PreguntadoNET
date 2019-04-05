using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Preguntado.Models.General;
namespace Preguntado.Models.ViewModel.Localizacion
{
    public class LocalizacionViewModel:EntidadViewModel
    {
        public string IdUsuario { get; set; }
        public List<LocalizacionDetails> Localizaciones { get; set; }


    }
}