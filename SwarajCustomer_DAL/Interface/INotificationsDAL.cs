using SwarajCustomer_Common.Entities;
using System.Collections.Generic;

namespace SwarajCustomer_DAL.Interface
{
    public  interface INotificationsDAL
    {
        List<NotificationsEntity> GetNotificationsByUser(int userId);
        NotificationEnitity GetPuchNotification(int mst_userId, string code);
        void SaveNotifications(string title, string description, int user_id, int type);
    }
}
