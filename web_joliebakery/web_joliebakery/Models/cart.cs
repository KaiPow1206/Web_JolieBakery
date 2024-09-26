using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace web_joliebakery.Models
{
    public class cart
    {
        public List<product_cart> SP;
        public float TONGTIEN;
        public cart()
        {
            SP = new List<product_cart>();
            TONGTIEN = 0; 
        }
        public cart(List<product_cart> sP)
        {
            SP = sP;
            TONGTIEN = sP.Sum(t => t.THANHTIEN);
        }
        public void AddToGioHang(product_cart obj)
        {
            SP.Add(obj);
        }
        public float GET_TONGTIEN()
        {
            return SP.Sum(t => t.THANHTIEN);
        }
        public void UpdateItem(int id, int slnew)
        {
            product_cart spnew = SP.Where(t => t.ID == id).FirstOrDefault();
            if (spnew != null)
            {
                spnew.SO_LUONG = slnew;
                spnew.THANHTIEN = spnew.GIATIEN * slnew;
                int vt = SP.IndexOf(spnew);
                if (vt > -1)
                {
                    SP[vt] = spnew;
                }
            }
        }
    }
}