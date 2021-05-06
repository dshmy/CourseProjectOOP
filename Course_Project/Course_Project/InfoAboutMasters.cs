using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Course_Project
{
    public class InfoAboutMasters
    {
        public int MasterId { get; set; }
        public string MasterLogin { get; set; }
        public string MasterName { get; set; }
        public string MasterSurname { get; set; }
        public int NumberOfCompletedOrders { get; set; }
        public int Income { get; set; }

        public InfoAboutMasters(int masterId, string masterLogin, string masterName, string masterSurname, int numberOfCompletedOrders, int income)
        {
            MasterId = masterId;
            MasterLogin = masterLogin;
            MasterName = masterName;
            MasterSurname = masterSurname;
            NumberOfCompletedOrders = numberOfCompletedOrders;
            Income = income;
        }
    }
}
