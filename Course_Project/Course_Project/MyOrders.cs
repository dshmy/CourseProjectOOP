using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Course_Project
{
    public class MyOrders
    {
        public string MasterSurname { get; set; }
        public string TypeOfService { get; set; }
        public int Price { get; set; }
        public byte[] Photo { get; set; }

        public MyOrders(string masterSurname, string typeOfService, int price, byte[] photo)
        {
            MasterSurname = masterSurname;
            TypeOfService = typeOfService;
            Price = price;
            Photo = photo;
        }
    }
}
