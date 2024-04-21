using SwarajCustomer_DAL.EDMX;
using System;
using System.Linq;

namespace SwarajCustomer_DAL
{
    public class UserMasterRepository : IDisposable
    {
        // SwarajTestEntities is your context class
        SwarajTestEntities context = new SwarajTestEntities();
        //This method is used to check and validate the user credentials
        public adm_user ValidateUser(string username, string password)
        {
            return context.adm_user.FirstOrDefault(user => user.username.Equals(username, StringComparison.OrdinalIgnoreCase) && user.password == password);
        }
        public void Dispose()
        {
            context.Dispose();
        }
    }
}