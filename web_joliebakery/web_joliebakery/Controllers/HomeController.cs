using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using web_joliebakery.Models;

namespace web_joliebakery.Controllers
{
    public class HomeController : Controller
    {
        Jolie_bakeryEntities1 db = new Jolie_bakeryEntities1();
        public ActionResult Shop()
        {
            List<TBL_SANPHAM> l_sanpham = db.TBL_SANPHAM.ToList();
            return View(l_sanpham);
        }
        public ActionResult Shop_details(int id)
        {
            TBL_SANPHAM sp = db.TBL_SANPHAM.Find(id);
            return View(sp);
        }
        public ActionResult Pay_Product()
        {
            cart _cart = (cart)Session["CART"];
            ViewData["TONGTIEN"] = _cart.TONGTIEN.ToString("N0");
            ViewData["TENKHACHHANG"] = "";
            ViewData["SODIENTHOAI"] = "";
            ViewData["DIACHIKHACHHANG"] = "";
            ViewData["GHICHU"] = "";
            ViewData["TRANGTHAITHANHTOAN"] = "";
            return View(_cart);
        }
        [HttpPost]
        public ActionResult Pay_Product(FormCollection form)
        {
            string HoTenKH = form["TENKHACHHANG"];
            string SDT = form["SODIENTHOAI"];
            string DIACHI = form["DIACHI"];
            string GHICHU = form["GHICHU"];
            ViewData["TENKHACHHANG"] = HoTenKH;
            ViewData["SODIENTHOAI"] = SDT;
            ViewData["DIACHIKHACHHANG"] = DIACHI;
            ViewData["SODIENTHOAI"] = SDT;
            Session["SODIENTHOAI"] = SDT;
            ViewData["GHICHU"] = GHICHU;

            cart _cart = (cart)Session["CART"];

            if (_cart.SP.Count == 0)
            {
                ViewData["TRANGTHAITHANHTOAN"] = "YOU HAVE NO ITEMS IN YOUR CART";
                return View(_cart);
            }

            using (DbContextTransaction tran = db.Database.BeginTransaction())
            {
                try
                {
                    TBL_HOADON hoadon = new TBL_HOADON();
                    hoadon.NGAYBAN = DateTime.Now;
                    hoadon.TEN_KHACH_HANG = HoTenKH;
                    hoadon.SDT = SDT;
                    hoadon.DIA_CHI_GIAO_HANG = DIACHI;
                    hoadon.GHICHU = GHICHU;
                    hoadon.TONG_THANH_TIEN = _cart.TONGTIEN;    
                    hoadon.MAHD = DateTime.Now.ToString().Replace(" ", "_").Replace(":", "_").Replace("/", "_");
                    hoadon.TRANG_THAI = 1;
                    db.TBL_HOADON.Add(hoadon);
                    db.SaveChanges();
                    string _MAHD = hoadon.MAHD;
                    for (int i = 0; i < _cart.SP.Count; i++)
                    {
                        TBL_HOADON_CHITIET chitiethd = new TBL_HOADON_CHITIET();
                        chitiethd.MAHD = _MAHD;
                        chitiethd.MASP = _cart.SP[i].MASP;
                        chitiethd.TENSP = _cart.SP[i].TENSP;
                        chitiethd.SOLUONG = (int)_cart.SP[i].SO_LUONG;
                        chitiethd.GIATIEN = _cart.SP[i].GIATIEN;
                        chitiethd.THANH_TIEN = _cart.SP[i].THANHTIEN;
                        db.TBL_HOADON_CHITIET.Add(chitiethd);
                        db.SaveChanges();
                    }

                    tran.Commit();
                    _cart = new cart();
                    return RedirectToAction("Payment_success");
                }
                catch (Exception ex)
                {
                    tran.Rollback();
                    ViewData["TRANGTHAITHANHTOAN"] = ": " + ex.Message;
                    return View(_cart);
                }
            }
        }
        public ActionResult Online_Payments()
        {
            return View();
        }

        public ActionResult Payment_success()
        {
            return View();
        }

        public ActionResult Bills(string searchString)
        {
            ViewData["TRANGTHAITIMKIEM"] = searchString;

            string SDT = Session["SODIENTHOAI"] as string;

            List<TBL_HOADON> ds_hoadon = new List<TBL_HOADON>();

            if (!string.IsNullOrEmpty(SDT))
            {
                ds_hoadon = db.TBL_HOADON.Where(hd => hd.SDT == SDT).ToList();
            }

            return View("Bills", ds_hoadon);
        }



        [HttpPost]
        public ActionResult Bills(FormCollection form)
        {
            string SDT = form["SODIENTHOAI"].ToString();
            if (string.IsNullOrEmpty(SDT))
            {
                ViewData["TRANGTHAITIMKIEM"] = "YOU DID NOT ENTER A PHONE NUMBER";
                List<TBL_HOADON> ds_hoadon = new List<TBL_HOADON>();
                return View(ds_hoadon);
            }
            else
            {
                Session["SODIENTHOAI"] = SDT;
                List<TBL_HOADON> ds_hoadon = db.TBL_HOADON.Where(hd => hd.SDT == SDT).ToList();
                if (ds_hoadon.Count == 0)
                {
                    ViewData["TRANGTHAITIMKIEM"] = "NO INVOICE WERE FOUND FOR THIS PHONE NUMBER: " + " " + SDT;
                }
                return View(ds_hoadon);
            }

        }

        public ActionResult About()
        {

            return View();
        }
        public ActionResult Custom()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Custom(Order order)
        {
            if (ModelState.IsValid)
            {
                db.Orders.Add(order);
                db.SaveChanges();
                ViewBag.Message = "Thank you for submitting your request to Jollie Bakery. We will contact you soon to confirm.";
            }
            return View(order);
        }
        public ActionResult Account() 
        {
            return View();
        }
        public ActionResult Cart()
        {
            cart _cart = (cart)Session["CART"];
            return View(_cart);
        }
        public ActionResult Delete_Cart(int id)
        {
            cart _cart = (cart)Session["CART"];
            product_cart obj = _cart.SP.Where(t => t.ID == id).FirstOrDefault();
            if (obj != null)
            {
                _cart.SP.Remove(obj);
                _cart.TONGTIEN = _cart.GET_TONGTIEN();
                Session["CART"] = _cart;
                Session["UNIT"] = _cart.SP.Count;

            }
            string returnURL = Request.UrlReferrer.Segments[2];
            return RedirectToAction(returnURL);
        }
        public JsonResult AddToCarts(int id)
        {
            try
            {
                cart _cart = (cart)Session["CART"];
                product_cart obj = _cart.SP.Where(t => t.ID == id).FirstOrDefault();
                if (obj != null)
                {
                    _cart.UpdateItem(id, (int)obj.SO_LUONG + 1);
                }
                else
                {
                    int unit = int.Parse(Session["UNIT"].ToString());
                    Session["UNIT"] = unit + 1;
                    obj = new product_cart();   
                    TBL_SANPHAM sp = db.TBL_SANPHAM.Find(id);

                    obj.ID = sp.IDSP;
                    obj.MASP = sp.MASP;
                    obj.TENSP = sp.TENSP;
                    obj.GIATIEN = (float)sp.GIATIEN;
                    obj.SO_LUONG = 1;
                    obj.HINH=sp.HINH_DAI_DIEN;
                    obj.THANHTIEN = obj.GIATIEN * obj.SO_LUONG;
                    _cart.AddToGioHang(obj);
                }
                _cart.TONGTIEN = _cart.SP.Sum(t => t.THANHTIEN);
                Session["CART"] =_cart;
                var reponse = new { Code = 0, CountMATHANG = _cart.SP.Count, Msg = "SUCCESS" };
                return Json(reponse);
            }
            catch (Exception ex)
            {
                var reponse = new { Code = 1, CountMATHANG = 0, Msg = ex.Message };
                return Json(reponse);
            }
        }
    }
}