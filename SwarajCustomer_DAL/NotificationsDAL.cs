using SwarajCustomer_Common.Entities;
using SwarajCustomer_DAL.EDMX;
using SwarajCustomer_DAL.Implementations;
using SwarajCustomer_DAL.Interface;
using System;
using System.Collections.Generic;
using System.Data;

namespace SwarajCustomer_DAL
{
    public class NotificationsDAL : INotificationsDAL
    {
        private SwarajTestEntities context;

        public NotificationsDAL(SwarajTestEntities context)
        {
            this.context = context;
        }
        public void SaveNotifications(string title, string description, int user_id, int type)
        {
            DbParam[] param = new DbParam[4];
            param[0] = new DbParam("@title", title, SqlDbType.NVarChar);
            param[1] = new DbParam("@description", description, SqlDbType.NVarChar);
            param[2] = new DbParam("@user_id", user_id, SqlDbType.Int);
            param[3] = new DbParam("@notifications_type", Convert.ToString(type), SqlDbType.NVarChar);
            Db.Insert("usp_save_trn_notifications", param);
        }
        public List<NotificationsEntity> GetNotificationsByUser(int userId)
        {
            List<NotificationsEntity> _notifications = new List<NotificationsEntity>();

            DbParam[] param = new DbParam[1];
            param[0] = new DbParam("@user_id", userId, SqlDbType.Int);

            DataSet ds = Db.GetDataSet("usp_get_trn_notifications", param);

            if (ds != null && ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        NotificationsEntity n = new NotificationsEntity();
                        n.adm_user_id = Db.ToInteger(row["adm_user_id"]);
                        n.username = Db.ToString(row["username"]);
                        n.mob_number = Db.ToString(row["mob_number"]);
                        n.email = Db.ToString(row["email"]);
                        n.trn_notifications_Id = Db.ToInteger(row["trn_notifications_Id"]);
                        n.title = Db.ToString(row["title"]);
                        n.description = Db.ToString(row["description"]);
                        n.notifications_type = Db.ToString(row["notifications_type"]);
                        n.date = Db.ToDateTime(row["date"]);
                        _notifications.Add(n);
                    }
                }
            }

            return _notifications;
        }

        public NotificationEnitity GetPuchNotification(int adm_user_id, string code)
        {
            var _notifications = new NotificationEnitity();

            DbParam[] param = new DbParam[2];
            param[0] = new DbParam("@code", code.Trim(), SqlDbType.NVarChar);
            param[1] = new DbParam("@adm_user_id", adm_user_id, SqlDbType.Int);

            DataSet ds = Db.GetDataSet("mst_DeviceToken_for_pushNotification", param);

            if (ds != null && ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        _notifications.device_key = Db.ToString(row["device_key"]);
                        _notifications.device_type = Db.ToInteger(row["device_type"]);
                        _notifications.contents = Db.ToString(row["contents"]);
                        _notifications.contentsId = Db.ToInteger(row["contentsId"]);
                        _notifications.contentsType = Db.ToString(row["contentsType"]);
                        _notifications.user_id = Db.ToInteger(row["user_id"]);
                        _notifications.CustomerMobile = Db.ToString(row["CustomerMobile"]);
                    }
                }
            }

            return _notifications;
        }
    }
}
