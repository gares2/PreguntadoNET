using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Preguntado.Models.OneSignal
{
    public abstract class NotificationDTO
    {
        public string app_id { get; set; }
        public object contents { get; set; }
        public object headings { get; set; }
        public string big_picture { get; set; }
        public string large_icon { get; set; }
        public string url { get; set; }

        public NotificationDTO(PushNotification push, string appId)
        {
            app_id = appId;
            contents = new { en = push.Mensaje, es = push.Mensaje };
            headings = new { en = push.Titulo, es = push.Titulo };
            big_picture = push.ImagenPrincipal;
            large_icon = push.ImagenPequeña;
        }
    }
}