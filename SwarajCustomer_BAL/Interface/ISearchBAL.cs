using SwarajCustomer_Common.Entities;
using System.Collections.Generic;

namespace SwarajCustomer_BAL.Interface
{
    public interface ISearchBAL
    {
        List<SearchRes> GetSearchResult(double latitude, double longitude, string text);
    }
}
