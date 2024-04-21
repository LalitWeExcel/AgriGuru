using SwarajCustomer_Common;
using SwarajCustomer_Common.Entities;
using SwarajCustomer_DAL.EDMX;
using System;
using System.Linq;

namespace SwarajCustomer_DAL.Common
{
    public class ExceptionLogging
    {
        public static UserLoginEntity getUserAuthInfo(string username, string role)
        {
            try
            {
                UserLoginEntity objUser = new UserLoginEntity();
                using (SwarajTestEntities _databaseContext = new SwarajTestEntities())
                {
                 adm_user result = null;
                 result = _databaseContext.adm_user.Where(u => u.mob_number == username && u.is_active.ToUpper() == "Y").OrderBy(x => x.c_time).FirstOrDefault();
             
                    if (role == Roles.CUST.ToString() || role == Roles.PRHT.ToString() || role == Roles.AST.ToString()
                        || role == Roles.PPRHT.ToString() || role == Roles.PAST.ToString())
                      
                    {
                        #region 'CUST,PRHT,AST',PPRHT,PAST
                        if (result != null)
                        {
                            var objOutput = _databaseContext.adm_user.Where(x => x.adm_user_id == result.adm_user_id && x.is_active == "Y").FirstOrDefault();
                            if (objOutput != null)
                            {
                                objUser.UserID = objOutput.adm_user_id;
                            }
                        }
                        #endregion
                    }
                }
                return objUser;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
