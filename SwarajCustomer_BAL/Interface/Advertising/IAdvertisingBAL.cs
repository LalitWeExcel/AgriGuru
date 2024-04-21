﻿using SwarajCustomer_Common.ViewModel;
using System.Collections.Generic;

namespace SwarajCustomer_BAL.Interface.Advertising
{
    public interface IAdvertisingBAL
    {
        int Delete(int Id, string type);
        List<DropDownObject> GetTimeSlot();
        List<DropDownObject> GetCategorys();
        List<DropDownObject> GetLanguages();
        
        List<MasterDropDown> GetMasterCategorys(int category_id);

        IList<M_Advertisement> GetAdvertisementList(int page, int pageSize, string search, string type, int State, int District , out int recordsCount);
        M_SaveAdvertisement Get(int ids);
        M_ResponceResult SaveUpdate(M_SaveAdvertisement model, int adminUserId);
        M_ResponceResult SaveUpdateIndependentads(M_SaveIndependentads model, int adminUserId);
        int ActivateDeactivate(string Id, string status, int AdminUserId, string type);

         #region  ManageFavVideosList
          M_ResponceResult SaveManageFavVideos(M_ManageFavVideos model, int adminUserId);      
          IList<M_ManageFavVideos> ManageFavVideosList(int page, int pageSize, string search, int languageId, out int recordsCount);
         IList<M_ManageFavVideosRequest> GetFavVideos(int languageId);
        

        #endregion


    }
}
