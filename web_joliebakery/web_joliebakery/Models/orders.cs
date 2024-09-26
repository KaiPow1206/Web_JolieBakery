using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace web_joliebakery.Models
{
    public class Orders
    {
        public int Id { get; set; }
        public string Loai { get; set; }
        public string Name { get; set; }

        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string CakeName { get; set; }
        public DateTime Birthday { get; set; }
        public string Message { get; set; }
        public string CakeType { get; set; }
        public string CakeTaste { get; set; }
        public string AnotherTaste { get; set; }
    }

}