using SwarajCustomer_Common.ViewModel;
using System.Collections.Generic;

namespace SwarajCustomer_BAL.Interface.ManagePayment
{
    public interface IManagePaymentBAL
    {
        M_Responce CheckoutMyCartPaymentSave(M_SaveManagePayment model,  string Username, string Email);
        M_Responce Save(M_SaveManagePayment model);
        IList<M_ManagePayment> GetManagePaymentList(int page, int pageSize, string fromdate, string todate, string status, string mode,
            string search, out int recordsCount, out int totelSuccessPayment, out int totelFailedPayment, out decimal totelRevenue,
            int state_id, int district_id);
    
        M_Responce SavePremiumMembership(M_SavePremiummemberShip model);

    }
}

