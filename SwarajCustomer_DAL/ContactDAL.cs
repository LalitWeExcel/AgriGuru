using SwarajCustomer_Common.Entities;
using SwarajCustomer_DAL.EDMX;
using SwarajCustomer_DAL.Implementations;
using SwarajCustomer_DAL.Interface;
using System;
using System.Data;

namespace SwarajCustomer_DAL
{
    public class ContactDAL : IContactDAL
    {
        Exception ex = new Exception();

        private readonly SwarajTestEntities _databaseContext;

        public ContactDAL(SwarajTestEntities databaseContext)
        {
            _databaseContext = databaseContext;
        }

        public int ContactUs(ContactEntity objContact)
        {
            int result = 0;

            DbParam[] param = new DbParam[5];
            param[0] = new DbParam("@user_id", objContact.UserID, SqlDbType.Int);
            param[1] = new DbParam("@name", objContact.Name, SqlDbType.VarChar);
            param[2] = new DbParam("@phone", objContact.Phone, SqlDbType.VarChar);
            param[3] = new DbParam("@email", objContact.Email, SqlDbType.VarChar);
            param[4] = new DbParam("@remarks", objContact.Remarks, SqlDbType.VarChar);
            DataSet ds = Db.GetDataSet("usp_save_contact_us_item", param);

            if (ds != null && ds.Tables.Count > 0)
            {
                string img = string.Empty;
                if (ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        result = Db.ToInteger(row["result"]);
                    }
                }
            }

            return result;
        }
    }
}
