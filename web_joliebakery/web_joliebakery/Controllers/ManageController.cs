using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Web;
using System.Web.Mvc;
using web_joliebakery.Models;

namespace web_joliebakery.Controllers
{
    public class ManageController : Controller
    {
        // GET: Manage
        Jolie_bakeryEntities1 db = new Jolie_bakeryEntities1();
        public ActionResult Admin()
        {
            List<TBL_SANPHAM> lsanpham = db.TBL_SANPHAM.ToList();
            return View(lsanpham);
        }
        public ActionResult Custom_Order()
        {
            List<Order> ord = db.Orders.ToList();
            return View(ord);
        }


        public ActionResult Create()
        {
            TBL_SANPHAM _sp = new TBL_SANPHAM();
            return View(_sp);
        }
        [HttpPost]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult Create(TBL_SANPHAM sanpham)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    db.TBL_SANPHAM.Add(sanpham);
                    db.SaveChanges();
                    return RedirectToAction("Admin", "Manage");
                }
                catch (Exception ex)
                {
                    ViewBag.ErrorMessage = "Error: " + ex.Message;
                    return View(sanpham);
                }
            }
            return View(sanpham);
        }

        public ActionResult Edit(int id)
        {
            TBL_SANPHAM _sp = db.TBL_SANPHAM.Find(id);
            if (_sp == null)
            {
                return HttpNotFound();
            }
            return View(_sp);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Edit(TBL_SANPHAM sanpham,int id)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    TBL_SANPHAM _sp_in_db = db.TBL_SANPHAM.Find(id);
                    _sp_in_db.TENSP = sanpham.TENSP;
                    _sp_in_db.GIATIEN = sanpham.GIATIEN;
                    _sp_in_db.GIACU = 0;
                    _sp_in_db.SO_LUONG_TON_TRONG_KHO = sanpham.SO_LUONG_TON_TRONG_KHO;
                    _sp_in_db.HINH_DAI_DIEN = sanpham.HINH_DAI_DIEN;
                    _sp_in_db.HINH_CHI_TIET_1 = sanpham.HINH_CHI_TIET_1;
                    _sp_in_db.HINH_CHI_TIET_2 = sanpham.HINH_CHI_TIET_2;
                    _sp_in_db.HINH_CHI_TIET_3 = sanpham.HINH_CHI_TIET_3;
                    _sp_in_db.HINH_CHI_TIET_4 = sanpham.HINH_CHI_TIET_4;

                    UpdateModel(_sp_in_db);
                    db.SaveChanges();
                    ViewData["TRANGTHAICAPNHAT"] = "SUCCESSFULLY UPDATED";
                    ViewData["Success"] = true;
                }
                catch (Exception ex)
                {
                    ViewData["TRANGTHAICAPNHAT"] = "Error: " + ex.Message;
                    ViewData["Success"] = false;
                }
            }
            return View(sanpham);
        }

        public ActionResult Delete(int id)
        {
            TBL_SANPHAM _sp = db.TBL_SANPHAM.Find(id);
            if (_sp == null)
            {
                return HttpNotFound();
            }
            ViewData["TRANGTHAICAPNHAT"] = "";
            return View(_sp);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Delete(TBL_SANPHAM sanpham,int id)
        {
            if (ModelState.IsValid)
            {
                try
                {
                   
                    TBL_SANPHAM _sp = db.TBL_SANPHAM.Find(id);
                    if (_sp != null)
                    {
                        db.TBL_SANPHAM.Remove(_sp);
                        db.SaveChanges();
                        ViewData["TRANGTHAICAPNHAT"] = "SUCCESS DELETE";
                        ViewData["Success"] = true;
                        return View(sanpham);
                    }
                }
                catch (Exception ex)
                {
                    ViewData["TRANGTHAICAPNHAT"] = "Error: " + ex.Message;
                    ViewData["Success"] = false;
                }
            }
            return View(sanpham);
        }
        public ActionResult Ordered() 
        {
            List<TBL_HOADON> ordered = db.TBL_HOADON.ToList();
            return View(ordered);
        }

        public ActionResult Edit_Ord(int id) 
        {
            TBL_HOADON hd = db.TBL_HOADON.Find(id);
            if (hd == null)
            {
                return HttpNotFound();
            }
            return View(hd);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Edit_Ord(TBL_HOADON hd, int id)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    TBL_HOADON hd_in_db = db.TBL_HOADON.Find(id);
                    hd.TRANG_THAI=hd_in_db.TRANG_THAI;
                    UpdateModel(hd_in_db);
                    db.SaveChanges();
                    ViewData["TRANGTHAICAPNHAT"] = "SUCCESSFULLY UPDATED";
                    ViewData["Success"] = true;
                }
                catch (Exception ex)
                {
                    ViewData["TRANGTHAICAPNHAT"] = "Error: " + ex.Message;
                    ViewData["Success"] = false;
                }
            }
            return View(hd);
        }

    }
}