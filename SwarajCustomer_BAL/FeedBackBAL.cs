using System.Collections.Generic;
using SwarajCustomer_BAL.Interface;
using SwarajCustomer_Common.Entities;
using SwarajCustomer_DAL;
using SwarajCustomer_DAL.Implementations;

namespace SwarajCustomer_BAL
{
    public class FeedBackBAL : IFeedBackBAL
    {
        private UOW unitOfWork = new UOW();

        public List<FeedBackEntity> GetFeedBack(int UserID)
        {
            return unitOfWork.FeedBackDALRepository.GetFeedBack(UserID);
        }

       public string SaveFeedBack(List<Feedback> feedbacks,int userId)
        {
            return unitOfWork.FeedBackDALRepository.SaveFeedBack(feedbacks, userId);
        }

        public string SaveRating(RatingEntity entity)
        {
            return unitOfWork.FeedBackDALRepository.SaveRating(entity);
        }
    }
}
