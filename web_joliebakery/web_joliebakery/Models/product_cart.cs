using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace web_joliebakery.Models
{
    public class product_cart
    {
        public int ID { get; set; }
        public string MASP { get; set; }
        public string TENSP { get; set; }
        public float GIATIEN { get; set; }
        public float SO_LUONG { get; set; }
        public string HINH { get; set; }
        public float THANHTIEN { get; set; }
        public product_cart()
        {
            ID = -1;
            MASP = "";
            TENSP = "";
            GIATIEN = 0;
            SO_LUONG = 0;
            HINH = "";
            THANHTIEN = 0;
        }
        public product_cart(int iD, string mASP, string tENSP, float
       gIATIEN, float sO_LUONG, string hINH)
        {
            ID = iD;
            MASP = mASP;
            TENSP = tENSP;
            GIATIEN = gIATIEN;
            SO_LUONG = sO_LUONG;
            HINH = hINH;
            THANHTIEN = sO_LUONG * gIATIEN;
        }
    }
}
