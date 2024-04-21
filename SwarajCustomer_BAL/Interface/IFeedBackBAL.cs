using SwarajCustomer_Common.Entities;
using System.Collections.Generic;

namespace SwarajCustomer_BAL.Interface
{
    public interface IFeedBackBAL
    {
        List<FeedBackEntity> GetFeedBack(int UserID);
        string SaveFeedBack(List<Feedback> Feedback, int UserID);
        string SaveRating(RatingEntity entity);
    }
}
