using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Preguntado.Models.General;
namespace Preguntado.Models.ViewModel.categoria
{
    public class ViewModelGrillaCategoria : EntidadViewModel
    {
        public int? cantidadPreguntas { get; set; }
        public string Color { get; set; }

        public  ViewModelGrillaCategoria()
        {
            
        }
    }
}