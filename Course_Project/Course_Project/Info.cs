using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Course_Project
{
    public class Info
    {
        public int Order_id { get; set; }
        public string Name { get; set; }
        public int Price { get; set; }
        public string Service { get; set; }
        public string Producer { get; set; }
        public string Model { get; set; }
        public string Description { get; set; }
        public byte[] Photo { get; set; }

        public Info(int order, string name, int price, string service)
        {
            Order_id = order;
            Name = name;
            Price = price;
            Service = service;
        }

        public Info(int order, string name, int price, string service, string prod, string model, string desc, byte[] ph)
        {
            Order_id = order;
            Name = name;
            Price = price;
            Service = service;
            Producer = prod;
            Model = model;
            Description = desc;
            Photo = ph;
        }

    }
}
