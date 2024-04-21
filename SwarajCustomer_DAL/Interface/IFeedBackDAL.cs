using SwarajCustomer_Common.Entities;
using System.Collections.Generic;

namespace SwarajCustomer_DAL.Interface
{
    public interface IFeedBackDAL
    {
        List<FeedBackEntity> GetFeedBack(int UserID);
        string SaveFeedBack(List<Feedback> feedbacks, int userId);
        string SaveRating(RatingEntity entity);
    }
}
