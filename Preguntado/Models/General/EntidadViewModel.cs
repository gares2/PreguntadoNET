using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Preguntado.Models.General
{
    public class EntidadViewModel
    {
        public Guid Id { get; set; }
        [Required(ErrorMessage = "Debe Completar el Nombre")]
        public string Nombre { get; set; }

    }
}