using SwarajCustomer_Common.Entities;
using System.Collections.Generic;

namespace SwarajCustomer_BAL.Interface
{
    public interface INotificationsBAL
    {
        void SaveNotifications(string title, string description, int user_id, int type);
        List<NotificationsEntity> GetNotificationsByUser(int userId);
        NotificationEnitity GetPuchNotification(int mst_userId,string code);

    }
}
