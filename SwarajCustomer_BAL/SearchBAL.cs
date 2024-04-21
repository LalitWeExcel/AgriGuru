using SwarajCustomer_BAL.Interface;
using SwarajCustomer_Common.Entities;
using SwarajCustomer_DAL;
using SwarajCustomer_DAL.Implementations;
using System.Collections.Generic;

namespace SwarajCustomer_BAL
{
    public class SearchBAL : ISearchBAL
    {
        private UOW unitOfWork = new UOW();

        public List<SearchRes> GetSearchResult(double latitude, double longitude, string text)
        {
            return unitOfWork.SearchDALRepository.GetSearchResult(latitude, longitude, text);
        }
    }
}
