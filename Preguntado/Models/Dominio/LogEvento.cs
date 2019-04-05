using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Preguntado.Models.Enums;

namespace Preguntado.Models.Dominio
{
    public class LogEvento : Entidad
    {
        public AccionesLogEnum Accion { get; set; }
        public EntidadLogEnum Entidad { get; set; }
        public Guid EntidadId { get; set; }
        public string usuarioId { get; set; }
    }
}