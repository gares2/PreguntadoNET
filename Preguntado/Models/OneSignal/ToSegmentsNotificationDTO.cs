using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Preguntado.Models.OneSignal
{
    public class ToSegmentsNotificationDTO : NotificationDTO
    {
        public string[] included_segments { get; set; }

        public ToSegmentsNotificationDTO(PushNotification push, string appId, string[] segments)
                : base(push, appId) => included_segments = segments;
    }
}