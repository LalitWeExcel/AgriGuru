﻿using SwarajCustomer_DAL.EDMX;
using System;

namespace SwarajCustomer_DAL.Implementations
{
    public class LogsExceptio
    {
        public static void ExceptionLog(Exception ex, string Module)
        {
            using (var objEntity = new SwarajTestEntities())
            {
                adm_APIExceptionLog obj = new adm_APIExceptionLog();
                if (ex.InnerException != null)
                    obj.Message = Convert.ToString(ex.InnerException);
                else
                    obj.Message = Convert.ToString(ex.Message);
                obj.Module = Module;
                obj.Source = ex.Source;
                obj.Datetime = DateTime.UtcNow;
                objEntity.adm_APIExceptionLog.Add(obj);
                objEntity.SaveChanges();
            }
        }
    }
}
