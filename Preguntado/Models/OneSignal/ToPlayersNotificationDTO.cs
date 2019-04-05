using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Preguntado.Models.OneSignal
{
    public class ToPlayersNotificationDTO : NotificationDTO
    {
        public string[] include_player_ids { get; set; }

        public ToPlayersNotificationDTO(PushNotification push, string appId, string[] playerIds)
            : base(push, appId) => include_player_ids = playerIds;

        public ToPlayersNotificationDTO(PushNotification push, string appId)
            : base(push, appId) => include_player_ids = new string[] { push.OneSignalId };
    }
}