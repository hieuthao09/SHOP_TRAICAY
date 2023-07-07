using QL_TraiCay.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QL_TraiCay.Controllers
{
    public class GioHangController : Controller
    {
        //
        // GET: /GioHang/

        // Chọn mua 1 trái cây
        public ActionResult ChonMua(string id)
        {
            GioHang gh = (GioHang)Session["gh"];
            if (gh== null)
            {
                gh = new GioHang();
                Cartitem sp = new Cartitem(id);
                gh.Ds.Add(sp);
            }
            else
            {
                int sl = gh.SoMatHang() + 1;
                int kq = gh.Them(id, sl);
              
            }

            Session["gh"] = gh;
            return RedirectToAction("Index","Home");
        }

        // Chọn mua nhiều trái cây
        //public ActionResult ChonMuaNhieu(string id, FormCollection collection)
        //{
        //    Cartitem.GioHang gh = (Cartitem.GioHang)Session["gh"];
        //    if (gh == null)
        //        gh = new Cartitem.GioHang();

        //    Session["gh"] = gh;
        //    int kq = gh.Them(id, int.Parse(collection["sl"]));

        //    return RedirectToAction("Index", "Home");
        //}

        public ActionResult XemGioHang()
        {

            GioHang gh = (GioHang)Session["gh"];
            if(gh==null)
            {

            }
            return View(gh);
        }
        public ActionResult Xoa(string id)
        {

            GioHang gh = (GioHang)Session["gh"];

            int kq = gh.Xoa(id);
            Session["gh"] = gh;

            return RedirectToAction("XemGioHang", "GioHang");
        }

        public ActionResult AddSL(string id)
        {

            GioHang gh = (GioHang)Session["gh"];

            gh.Them(id, 1);

            return RedirectToAction("XemGioHang", "GioHang");
        }
        public ActionResult XoaGio()
        {
            GioHang gh = (GioHang)Session["gh"];
            gh.XoaGioHang();
            Session["gh"] = gh;
            return RedirectToAction("Xemgiohang", "GioHang");
        }
    }
}
