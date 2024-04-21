using SwarajCustomer_Common.ViewModel;
using SwarajCustomer_DAL.Implementations;
using System.Collections.Generic;

namespace SwarajCustomer_BAL.Interface.ManageOrder
{
    public class ManageOrderBAL : IManageOrderBAL
    {
        private UOW unitOfWork = new UOW();

        public string ConfirmProhit(int BookingID, string OrderNumber,int admin_id)
        {
            return unitOfWork.ManageOrdeRepository.ConfirmProhit(BookingID, OrderNumber, admin_id);
        }

        public M_ManageOrder Details(int ids, string ordernumber)
        {
            return unitOfWork.ManageOrdeRepository.Details(ids, ordernumber);
        }

        public List<DropDownObject> GetProhit(string BookingType)
        {
            return unitOfWork.ManageOrdeRepository.GetProhit( BookingType);
        }

        public IList<M_ManageOrder> GetManageOrderList(int page, int pageSize, string fromdate, string todate, string OrderStatus, string search, int State, int District, out int recordsCount)
        {
            return unitOfWork.ManageOrdeRepository.GetManageOrderList(page, pageSize, fromdate, todate, OrderStatus, search, State, District, out recordsCount);
        }

        public M_Responce Update(M_UpdateProhits model, int adminUserId)
        {
            return unitOfWork.ManageOrdeRepository.Update(model, adminUserId);
        }
        public M_Responce UpdatePackage(M_UpdatePackage model, int adminUserId)
        {
            return unitOfWork.ManageOrdeRepository.UpdatePackage(model, adminUserId);
        }
   
    }
}
