using SwarajCustomer_Common.ViewModel;
using SwarajCustomer_DAL.Implementations;
using System.Collections.Generic;

namespace SwarajCustomer_BAL.Interface.Advertising
{
    public class AdvertisingBAL : IAdvertisingBAL
    {
        private UOW unitOfWork = new UOW();

        public int Delete(int Id, string type)
        {
            return unitOfWork.AdvertisingRepository.Delete(Id, type);
        }
        public List<DropDownObject> GetTimeSlot()
        {
            return unitOfWork.AdvertisingRepository.GetTimeSlot();
        }

        public List<DropDownObject> GetCategorys()
        {
            return unitOfWork.AdvertisingRepository.GetCategorys();
        }
        public List<DropDownObject> GetLanguages()
        {
            return unitOfWork.AdvertisingRepository.GetLanguages();
        }
 
        public  List<MasterDropDown> GetMasterCategorys(int category_id )
        {
            return unitOfWork.AdvertisingRepository.GetMasterCategorys(category_id);
        }
        public IList<M_Advertisement> GetAdvertisementList(int page, int pageSize, string search, string type, int State, int District,  out int recordsCount)
        {
            return unitOfWork.AdvertisingRepository.GetAdvertisementList(page, pageSize, search, type, State, District, out recordsCount);
        }

        public M_ResponceResult SaveUpdate(M_SaveAdvertisement model, int adminUserId)
        {
            return unitOfWork.AdvertisingRepository.SaveUpdate(model, adminUserId);
        }

        public M_SaveAdvertisement Get(int ids)
        {
            return unitOfWork.AdvertisingRepository.Get(ids);
        }

        public M_ResponceResult SaveUpdateIndependentads(M_SaveIndependentads model, int adminUserId)
        {
            return unitOfWork.AdvertisingRepository.SaveUpdateIndependentads(model, adminUserId);
        }

        public int ActivateDeactivate(string Id, string status, int AdminUserId,string type)
        {
            return unitOfWork.AdvertisingRepository.ActivateDeactivate(Id, status, AdminUserId, type);
        }


        #region manage fav videos
        public IList<M_ManageFavVideos> ManageFavVideosList(int page, int pageSize, string search, int languageId, out int recordsCount)
        {
            return unitOfWork.AdvertisingRepository.ManageFavVideosList(page, pageSize, search,  languageId, out recordsCount);
        }

        public M_ResponceResult SaveManageFavVideos(M_ManageFavVideos model, int adminUserId)
        {
            return unitOfWork.AdvertisingRepository.SaveManageFavVideos(model, adminUserId);
        }
        public IList<M_ManageFavVideosRequest> GetFavVideos(int languageId)
        {
            return unitOfWork.AdvertisingRepository.GetFavVideos(languageId);
        }
        #endregion
    }
}
