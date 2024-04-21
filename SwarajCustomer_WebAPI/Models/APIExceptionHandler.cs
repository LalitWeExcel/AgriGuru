using SwarajCustomer_BAL;
using System;
using static SwarajCustomer_Common.Utility.MessageModel;

namespace SwarajCustomer_WebAPI.Models
{
    public class APIExceptionHandler
    {
        public static UserModal.DataModal ExceptionLogging(Exception ex, string Module)
        {
            CommonDBMethods.LoggingException(ex, Module);
            var result = new UserModal.DataModal()
            {
                Data = null,
                Response = CommonActions.Error.ToString(),
                Message = ex.Message,
                Status = false,

            };

            return result;
        }

        public static UserModal.DataModal ExceptionLoggingDigiSense(Exception ex, string Module, string customMsg)
        {
            CommonDBMethods.LoggingException(ex, Module);
            var result = new UserModal.DataModal()
            {
                Data = null,
                Response = CommonActions.Error.ToString(),
                Message = customMsg,
                Status = false,

            };

            return result;
        }
    }
}