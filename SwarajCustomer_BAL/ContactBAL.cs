using SwarajCustomer_BAL.Interface;
using SwarajCustomer_Common.Entities;
using SwarajCustomer_DAL;
using SwarajCustomer_DAL.Implementations;

namespace SwarajCustomer_BAL
{
    public class ContactBAL : IContactBAL
    {
        private UOW unitOfWork = new UOW();

        public int ContactUs(ContactEntity objContact)
        {
            return unitOfWork.ContactDALRepository.ContactUs(objContact);
        }
    }
}
