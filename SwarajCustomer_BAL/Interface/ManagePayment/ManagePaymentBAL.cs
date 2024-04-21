using System.Collections.Generic;
using SwarajCustomer_Common.ViewModel;
using SwarajCustomer_DAL.Implementations;

namespace SwarajCustomer_BAL.Interface.ManagePayment
{
    public class ManagePaymentBAL : IManagePaymentBAL
    {
        private UOW unitOfWork = new UOW();

        public IList<M_ManagePayment> GetManagePaymentList(int page, int pageSize, string fromdate, string todate, string status, string mode, string search, out int recordsCount, out int totelSuccessPayment, out int totelFailedPayment, out decimal totelRevenue, int state_id, int district_id)
        {
            return unitOfWork.ManagePaymentRepository.GetManagePaymentList(page, pageSize, fromdate, todate, status, mode, search, out recordsCount,out totelSuccessPayment, out totelFailedPayment, out totelRevenue, state_id, district_id);
        }

        public M_Responce Save(M_SaveManagePayment model)
        {
            return unitOfWork.ManagePaymentRepository.Save(model);
        }

        public M_Responce CheckoutMyCartPaymentSave(M_SaveManagePayment model,  string Username, string Email)
        {
            return unitOfWork.ManagePaymentRepository.CheckoutMyCartPaymentSave(model, Username, Email);
        }

        public M_Responce SavePremiumMembership(M_SavePremiummemberShip model)
        {
            return unitOfWork.ManagePaymentRepository.SavePremiumMembership(model);
        }

    }
}
