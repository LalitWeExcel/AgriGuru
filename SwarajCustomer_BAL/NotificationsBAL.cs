using SwarajCustomer_BAL.Interface;
using SwarajCustomer_Common.Entities;
using SwarajCustomer_DAL.Implementations;
using System.Collections.Generic;

namespace SwarajCustomer_BAL
{
    public class NotificationsBAL : INotificationsBAL
    {
        private UOW unitOfWork = new UOW();
        public List<NotificationsEntity> GetNotificationsByUser(int userId)
        {
            return unitOfWork.NotificationsRepository.GetNotificationsByUser(userId);
        }

        public NotificationEnitity GetPuchNotification(int mst_userId, string code)
        {
            return unitOfWork.NotificationsRepository.GetPuchNotification(mst_userId, code);
        }

        public void SaveNotifications(string title, string description, int user_id, int _type)
        {
            unitOfWork.NotificationsRepository.SaveNotifications(title, description, user_id, _type);
        }

    }
}
