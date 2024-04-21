using SwarajCustomer_BAL;
using SwarajCustomer_BAL.Interface;
using SwarajCustomer_Common.Entities;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace SwarajCustomer_WebAPI.Areas.OTP.Controllers
{
    public class OTPController : Controller
    {
        private IUserBAL objUser;

        public ActionResult GetOtpList()
        {
            try
            {
                objUser = new UserBAL();

                if (ModelState.IsValid)
                {
                    var Result = objUser.GetOTPList();
                    ViewBag.TotalRecords = Result == null ? 0 : Result.Count;
                    ViewBag.Result = Result;
                    TempData["GetOTP"] = Result;
                    TempData.Keep();

                    return View();
                }
                else
                {
                    var Result = (List<OtpList>)TempData["GetOTP"];
                    TempData.Keep();
                    ViewBag.Result = Result;
                    return View();
                }
            }
            catch (Exception ex)
            {
                //APIExceptionHandler.ExceptionLogging(ex, "User/GetOtpList");
            }
            return View();
        }
    }
}