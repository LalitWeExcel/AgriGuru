using System;
using System.Collections.Generic;
using System.Data;
using SwarajCustomer_Common.Entities;
using SwarajCustomer_DAL.EDMX;
using SwarajCustomer_DAL.Implementations;
using SwarajCustomer_DAL.Interface;

namespace SwarajCustomer_DAL
{
    public class FeedBackDAL : IFeedBackDAL
    {
        Exception ex = new Exception();

        private readonly SwarajTestEntities _databaseContext;

        public FeedBackDAL(SwarajTestEntities databaseContext)
        {
            _databaseContext = databaseContext;
        }

        public List<FeedBackEntity> GetFeedBack(int UserID)
        {
            List<FeedBackEntity> _feedbacks = new List<FeedBackEntity>();

            DbParam[] param = new DbParam[1];
            param[0] = new DbParam("@user_id", UserID, SqlDbType.Int);
         
            DataSet ds = Db.GetDataSet("usp_get_feedback", param);

            if (ds != null && ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        FeedBackEntity f = new FeedBackEntity();
                        f.mst_feedback_Id = Db.ToInteger(row["mst_feedback_Id"]);
                        f.code = Db.ToString(row["code"]);
                        f.name = Db.ToString(row["name"]);
                        f.answer_type = Db.ToString(row["answer_type"]);
                        f.sort_order = Db.ToString(row["sort_order"]);
                        _feedbacks.Add(f);
                    }
                }
            }

            return _feedbacks;
        }

        public string SaveFeedBack(List<Feedback> _objects, int userId)
        {
            DataTable dataTable = new DataTable();
            string result = string.Empty;
            dataTable.Columns.AddRange(new DataColumn[3] {
                new DataColumn("mst_feedback_Id", typeof(int)),
                new DataColumn("response", typeof(string)),
                new DataColumn("user_id", typeof(int))
            });

            foreach (var feedback in _objects)
            {
                dataTable.Rows.Add(feedback.mst_feedback_Id, feedback.response, userId);
            }

            DataSet dataSet = new DataSet();
            DbParam[] param = new DbParam[2];

            param[0] = new DbParam("@userId", userId, SqlDbType.Int);
            param[1] = new DbParam("@tbl_feedback", dataTable, SqlDbType.Structured);
            dataSet = Db.GetDataSet("usp_save_feedback", param);

            if (dataSet != null && dataSet.Tables.Count > 0 && dataSet.Tables[0].Rows.Count > 0)
            {
                result = Convert.ToString(dataSet.Tables[0].Rows[0]["result"]);
            }
            return result;
        }

        public string SaveRating(RatingEntity entity)
        {
           string result = string.Empty;
            DataSet dataSet = new DataSet();
            DbParam[] param = new DbParam[5];

            param[0] = new DbParam("@order_no", entity.order_no, SqlDbType.NVarChar);
            param[1] = new DbParam("@purohit_Id", entity.purohit_Id, SqlDbType.Int);
            param[2] = new DbParam("@user_Id", entity.user_Id, SqlDbType.Int);
            param[3] = new DbParam("@rating", entity.rating, SqlDbType.Int);
            param[4] = new DbParam("@remarks", entity.remarks, SqlDbType.NVarChar);
            dataSet = Db.GetDataSet("usp_save_update_rating", param);

            if (dataSet != null && dataSet.Tables.Count > 0 && dataSet.Tables[0].Rows.Count > 0)
            {
                result = Convert.ToString(dataSet.Tables[0].Rows[0]["result"]);
            }
            return result;
        }
    }
}
