using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using QL_TraiCay.Models;

namespace QL_TraiCay.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/
        QL_TraiCayDataContext dl = new QL_TraiCayDataContext();
        public ActionResult Index()
        {
            ViewBag.VN = dl.LOAITRAICAYs.Where(t => t.MALOAI.Contains("VN")).ToList();
            ViewBag.NK = dl.LOAITRAICAYs.Where(t => t.MALOAI.Contains("NK")).ToList();
            ViewBag.TL = dl.LOAITRAICAYs.Where(t => t.MALOAI.Contains("TL")).ToList();
            return View();
        }
        public ActionResult DanhMuc_SP_2()
        {
            ViewBag.VN = dl.LOAITRAICAYs.Where(t => t.MALOAI.Contains("VN")).ToList();
            ViewBag.NK = dl.LOAITRAICAYs.Where(t => t.MALOAI.Contains("NK")).ToList();
            ViewBag.TL = dl.LOAITRAICAYs.Where(t => t.MALOAI.Contains("TL")).ToList();

            return PartialView();
        }
        public ActionResult carousel_Anh()
        {
            return PartialView(dl.TRAICAYs.Take(2).ToList());
        }
        public ActionResult DS_SP_TheoLoai(string id)
        {
            return View(dl.TRAICAYs.Where(t => t.MALOAI == id).ToList());
        }
        public ActionResult TimKiem(string timkiem)
        {
            return View(dl.TRAICAYs.Where(t => (t.TENTC.Contains(timkiem) || t.GIAMGIA.ToString().Contains(timkiem) || t.LUOTXEM.ToString().Contains(timkiem))).ToList());
        }
        public ActionResult ChiTietSP(string id)
        {
            string mncc = dl.NHACC_TRAICAYs.First(t => t.MATC == id).MANCC;
            ViewBag.Nhacc = dl.NHACUNGCAPs.First(t => t.MANCC == mncc).MANCC;
            string mnsx = dl.NHASX_TRAICAYs.First(t => t.MATC == id).MANSX;
            ViewBag.Nhasx = dl.NHASANXUATs.First(t => t.MANSX == mnsx).MANSX;

            TRAICAY tc = dl.TRAICAYs.First(t => t.MATC == id);
            TAIKHOAN taikhoan = Session["tk"] as TAIKHOAN;
            ViewBag.Link = 1;
            if(tk == null)
            {
                return View(tc);
            }
            is_user = dl._USERs.FirstOrDefault(u => u.MATK == tk.MATK) != null;

            if (!is_user)
            {
                ViewBag.Link = 0;
            }
            return View(tc);
            
        }
        public ActionResult ChiTietNCC(string id)
        {
            return PartialView(dl.NHACUNGCAPs.FirstOrDefault(t => t.MANCC == id));
        }
        public ActionResult ChiTietNSX(string id)
        {
            return PartialView(dl.NHASANXUATs.First(t => t.MANSX == id));
        }
        static TAIKHOAN tk = null;
        static bool is_user = true;

        
        public ActionResult DangKi()
        {
            return PartialView();
        }

        public ActionResult SauKhiDangKi(FormCollection collection)
        {
            // Tạo ngẫu nhiên các mã tài khoản, mã giỏ hàng, mã user.
            string MATK = string.Empty;
            string MAGH = string.Empty;
            string MAUSER = string.Empty;

            while (true)
            {
                MATK = "TK" + RandomString.RandomNumber(1000, 9999).ToString();
                if (dl.TAIKHOANs.FirstOrDefault(t => t.MATK == MATK) == null)
                    break;
            }

            while (true)
            {
                MAGH = "GH" + RandomString.RandomNumber(1000, 9999).ToString();
                if (dl.GIOHANGs.FirstOrDefault(g => g.MAGH == MAGH) == null)
                    break;
            }

            while (true)
            {
                MAUSER = "USER" + RandomString.RandomNumber(1000, 9999).ToString();
                if (dl._USERs.FirstOrDefault(u => u.MAUSER == MAUSER) == null)
                    break;
            }

            // Thêm một tài khoản
            TAIKHOAN taikhoan = new TAIKHOAN();
            taikhoan.MATK = MATK;
            taikhoan.HOTEN = collection["hoten"];
            taikhoan.USERNAME = collection["username"];
            taikhoan.MATKHAU = collection["password"];

            dl.TAIKHOANs.InsertOnSubmit(taikhoan);

            // Thêm một giỏ hàng của user mới
            GIOHANG giohang = new GIOHANG();
            giohang.MAGH = MAGH;
            giohang.THANHTIEN = 0;

            dl.GIOHANGs.InsertOnSubmit(giohang);

            // Thêm user
            _USER user = new _USER();
            user.MAUSER = MAUSER;
            user.MATK = taikhoan.MATK;
            user.MAGH = giohang.MAGH;
            user.HOTEN = collection["hoten"];
            user.SDT = collection["sdt"];
            user.AVARTA = collection["avatar"];
            user.DIACHI = collection["diachi"];
            user.EMAIL = collection["email"];

            dl._USERs.InsertOnSubmit(user);
            dl.SubmitChanges();

            return View("ThongBao", (object)"Đăng ký thành công !");
        }
      
        public ActionResult DangNhap(int id)
        {   
            TAIKHOAN taikhoan = Session["tk"] as TAIKHOAN;
            if (id == 0 && taikhoan != null)
            {
                ViewBag.tb = 1;
                return PartialView(id);
            }
            ViewBag.tb = 0;
            return PartialView(id);
        }

        public ActionResult DangXuat()
        {
            tk = null;
            Session["tk"] = tk;
            return RedirectToAction("Index");
        }

        [HttpPost]
        public void laytaikhoan(TAIKHOAN tk)
        {
            Session["tk"] = tk;
        }
        public ActionResult SauKhiDangNhap(FormCollection collection)
        {
            tk = dl.TAIKHOANs.FirstOrDefault(taikhoan =>
                taikhoan.USERNAME == collection["username"] && taikhoan.MATKHAU == collection["password"]);
            laytaikhoan(tk);
            if (tk == null)
            {
               return PartialView("DangNhap", 1);
            }
               

            is_user = dl._USERs.FirstOrDefault(u => u.MATK == tk.MATK) != null;

            if (is_user)
                return RedirectToAction("Index");
            else
                return RedirectToAction("Index", "QuanLy");
        }
        public ActionResult ThongBao(string id)
        {
            return View(id);
        }

        public ActionResult HienThiTenNguoiDung()
        {
            try
            {
                TAIKHOAN taikhoan = Session["tk"] as TAIKHOAN;
                if(taikhoan!=null)
                {
                    _USER tim_user = dl._USERs.FirstOrDefault(u => u.MATK == taikhoan.MATK);
                    ViewBag.is_sign_in = tim_user.HOTEN;
                    return PartialView((object)tim_user.HOTEN);
                 
                }
                else
                {
                    ViewBag.is_sign_in = "0";
                    return PartialView((object)"Chưa đăng nhập");
                }
               
            }
            catch
            {
                return PartialView((object)"Chưa đăng nhập");;
            }
          
        }
    }
}
