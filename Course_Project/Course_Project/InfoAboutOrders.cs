using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Course_Project
{
    public class InfoAboutOrders
    {
        public int OrderId { get; set; }
        public int MasterId { get; set; }
        public string MasterName { get; set; }
        public string MasterSurname { get; set; }
        public string CustomerName { get; set; }
        public int Price { get; set; }
        public string TypeOfService { get; set; }
        public string Producer { get; set; }
        public string Model { get; set; }
        public string Description { get; set; }

        public InfoAboutOrders(int orderId, string customerName, int price, string typeOfService, string producer, string model, string description)
        {
            OrderId = orderId;
            CustomerName = customerName;
            Price = price;
            TypeOfService = typeOfService;
            Producer = producer;
            Model = model;
            Description = description;
        }

        public InfoAboutOrders(int orderId, int masterId, string masterName, string masterSurname, string customerName, int price, string typeOfService, string producer, string model, string description)
        {
            OrderId = orderId;
            MasterId = masterId;
            MasterName = masterName;
            MasterSurname = masterSurname;
            CustomerName = customerName;
            Price = price;
            TypeOfService = typeOfService;
            Producer = producer;
            Model = model;
            Description = description;
        }
    }
}
