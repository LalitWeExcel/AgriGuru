using System.Collections.Generic;

namespace SwarajCustomer_Common.ViewModel
{
    public class DashBoardViewModel
    {
        public M_DashBoardUserCount DashBoardUserCount { get; set; } = new M_DashBoardUserCount();
        public List<UpcomingBirthdaysList> UpcomingBirthdays { get; set; } = new List<UpcomingBirthdaysList>();
        public List<MonthWiseRevenue> MonthWiseRevenueList { get; set; } = new List<MonthWiseRevenue>();
    }

    public class M_DashBoardUserCount
    {
        public int UserCount { get; set; } = 0;
        public int ActiveUserCount { get; set; } = 0;
        public int ProhitAstroCount { get; set; } = 0;
        public int ActiveProhitAstroCount { get; set; } = 0;
        public int TotalBooking { get; set; } = 0;
        public int TotalBookingUpcoming { get; set; } = 0;
        public int TotalTodayBooking { get; set; } = 0;
        public decimal TotalRevenue { get; set; } = 0;
        public decimal TotalRemainingRevenue { get; set; } = 0; 
    }

    public class UpcomingBirthdaysList
    {
        public int adm_user_id { get; set; }
        public string ImageUrl { get; set; }
        public string CustomerName { get; set; }
        public string Contact { get; set; }
        public string DOB { get; set; }
        public string Email { get; set; }
        public int BirthdayEmailSent { get; set; }
        public int Diff_In_Day { get; set; }
        public int UpComeingBirthdayEmailSent { get; set; }

    }

    public class MonthWiseRevenue
    {
        public string Month { get; set; }
        public decimal Amount{ get; set; }
    }
}
