using SwarajCustomer_Common.Entities;
using System.Collections.Generic;

namespace SwarajCustomer_DAL.Interface
{
    public interface ISearchDAL
    {
        List<SearchRes> GetSearchResult(double latitude, double longitude, string text);
    }
}
