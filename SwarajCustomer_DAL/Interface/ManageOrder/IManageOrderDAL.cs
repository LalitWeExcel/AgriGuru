using SwarajCustomer_Common.ViewModel;
using System.Collections.Generic;

namespace SwarajCustomer_DAL.Interface.ManageOrder
{
    public interface IManageOrderDAL
    {
        IList<M_ManageOrder> GetManageOrderList(int page, int pageSize, string fromdate, string todate, string OrderStatus, string search, int State, int District, out int recordsCount);
        M_ManageOrder Details(int ids, string ordernumber);
        string ConfirmProhit(int BookingID, string OrderNumber, int admin_id);
        List<DropDownObject> GetProhit( string BookingType);
        M_Responce Update(M_UpdateProhits model, int adminUserId);
        M_Responce UpdatePackage(M_UpdatePackage model, int adminUserId);
    }
}
