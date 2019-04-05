using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Preguntado.Models.Dominio;
using Preguntado.Models.Enums;
using Preguntado.Models;
using System.Threading;

namespace Preguntado.Helper
{
    public class LoggerEventos
    {
        public static void LogearEvento(string nombre, string usuarioId, Guid entidadGuid, AccionesLogEnum accion, EntidadLogEnum tipoEntidad)
        {
            var log = new LogEvento()
            {
                Nombre = nombre,
                usuarioId = usuarioId,
                EntidadId = entidadGuid,
                Accion = accion,
                Entidad = tipoEntidad
            };

            var t = new Thread(() => GuardarEvento(log));
            t.Start();
        }

        public static void GuardarEvento(LogEvento evento)
        {
            try
            {
                ApplicationDbContext db = new ApplicationDbContext();
                new Repositorio<LogEvento>(db).Crear(evento);
            }
            catch (Exception e) { }
        }
    }
}