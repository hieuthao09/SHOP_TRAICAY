using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using QL_TraiCay.Models;
using System.IO;
namespace QL_TraiCay.Controllers
{
    public class QuanLyController : Controller
    {
        //
        // GET: /QuanLy/
        QL_TraiCayDataContext dl = new QL_TraiCayDataContext();

        public ActionResult Index()
        {
            return View(dl.TRAICAYs.ToList());
        }
        public ActionResult Loaitc()
        {
            return View(dl.LOAITRAICAYs.ToList());
        }

        
        
               
            
        /*Xử lý Loại Trái Cây */
        [HttpGet]
        public ActionResult Them_LoaiTC()
        {
            
            return View();
        }
        [HttpPost, ActionName("Them_LoaiTC")]
        
        public ActionResult XuLy_ThemLTC(LOAITRAICAY ltc)
        {
            dl.LOAITRAICAYs.InsertOnSubmit(ltc);
            dl.SubmitChanges();
            return RedirectToAction("Loaitc");
        }
        [HttpGet]
        public ActionResult Delete_LoaiTC(string id)
        {
            LOAITRAICAY ltc = dl.LOAITRAICAYs.SingleOrDefault(t => t.MALOAI == id);
           
            if (ltc == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(ltc);
        }
        [HttpPost, ActionName("Delete_LoaiTC")]
        public ActionResult XacNhanXoaLTC(string id)
        {
            LOAITRAICAY ltc = dl.LOAITRAICAYs.SingleOrDefault(t => t.MALOAI == id);
            
            if (ltc == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            dl.LOAITRAICAYs.DeleteOnSubmit(ltc);
            dl.SubmitChanges();
            return RedirectToAction("Loaitc");
        }
        
        [HttpGet]
        public ActionResult Edit_LTC(string id)
        {
            LOAITRAICAY ltc = dl.LOAITRAICAYs.SingleOrDefault(t => t.MALOAI == id);
            
            if (ltc == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(ltc);
        }
        [HttpPost, ActionName("Edit_LTC")]
        
        public ActionResult Edit_LTC(LOAITRAICAY ltc)
        {
            UpdateModel(ltc);
            dl.SubmitChanges();
            return RedirectToAction("Loaitc");
        }
       
       
        public ActionResult ChiTiet_LTC(string id)
        {
            LOAITRAICAY ltc = dl.LOAITRAICAYs.SingleOrDefault(t => t.MALOAI == id);
            ViewBag.MaLTC = ltc.MALOAI;
            if (ltc == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(ltc);
        }
         [HttpPost, ActionName("ChiTiet_LTC")]
        public ActionResult Details_ltc(string id)
        {
            return RedirectToAction("Index");
           
        }



        [HttpGet]
        /*Xử lý Trái cây*/
        public ActionResult Them_TC()
        {
            ViewBag.MaLoai = dl.LOAITRAICAYs.ToList();
            return View();
        }
        [HttpPost, ActionName("Them_TC")]
        public ActionResult XLThem_TC(TRAICAY a, HttpPostedFileBase[] fupload)
        {
            try
            {
                if (fupload == null)
                {
                    ViewBag.ThongBao = "Vui lòng chọn ảnh bìa";
                    ViewBag.MaLoai = dl.LOAITRAICAYs.ToList();
                    return View();
                }
                else
                {

                    foreach (var item in fupload)
                    {
                        var fileName = Path.GetFileName(item.FileName);
                        var path = Path.Combine(Server.MapPath("/Images"), fileName);
                        if (System.IO.File.Exists(path))
                            ViewBag.Thongbao = "Hình ảnh đã tồn tại";
                        else
                        {
                            a.LIST_HINHANH += fileName + ",";
                            item.SaveAs(path);
                        }
                    }

                    a.DUONGDAN = fupload[0].FileName;
                        dl.TRAICAYs.InsertOnSubmit(a);
                        dl.SubmitChanges();
                        return RedirectToAction("Index");
                }
            }
            catch(Exception ex)
            {
                ViewBag.Loi = ex.Message;
                return View("Them_TC");
            }
        }
        [HttpGet]
        public ActionResult Delete(string id)
        {
            TRAICAY tc = dl.TRAICAYs.SingleOrDefault(t => t.MATC == id);
            ViewBag.MaTC = tc.MATC;
            if(tc == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(tc);
        }
        [HttpPost, ActionName("Delete")]
        public ActionResult XacNhanXoa(string id)
        {
            TRAICAY tc = dl.TRAICAYs.SingleOrDefault(t => t.MATC == id);
            ViewBag.MaTC = tc.MATC;
            if (tc == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            dl.TRAICAYs.DeleteOnSubmit(tc);
            dl.SubmitChanges();
            return RedirectToAction("Index");
        }
        
        [HttpGet]
        public ActionResult Edit(string id)
        {
            TRAICAY tc = dl.TRAICAYs.SingleOrDefault(t => t.MATC == id);
            ViewBag.MaLoai = new SelectList(dl.LOAITRAICAYs.ToList().OrderBy(t => t.TENLOAI), "MALOAI", "TENLOAI");
            if(tc == null)
            {
                Response.StatusCode = 404;
                return null;
            }

            return View(tc);
        }
        [HttpPost]
        public ActionResult XuLySua( HttpPostedFileBase[] fLoad, TRAICAY tc, FormCollection ft)
        {
            try
            {
                tc.MALOAI = ft["MALOAI"];
                tc.MATC = ft["MATC"];
                tc.HSD = DateTime.Parse(ft["HSD"]);
                tc.NSX = DateTime.Parse(ft["NSX"]);
                tc.TENTC = ft["TENTC"];
                tc.GIAMOI = decimal.Parse(ft["GIAMOI"]);
                tc.GIAMGIA = decimal.Parse(ft["GIAMGIA"]);
                tc.LUOTXEM = int.Parse(ft["LUOTXEM"]);
                tc.TRANGTHAI = bool.Parse(ft["TRANGTHAI"].ToString());
                if (fLoad == null)
                {
                    ViewBag.ThongBao = "Vui lòng chọn ảnh bìa";
                    return View();
                }
                else
                {

                    foreach (var item in fLoad)
                    {
                        var fileName = Path.GetFileName(item.FileName);
                        var path = Path.Combine(Server.MapPath("/Images"), fileName);
                        if (System.IO.File.Exists(path))
                            ViewBag.Thongbao = "Hình ảnh đã tồn tại";
                        else
                        {
                            tc.LIST_HINHANH += fileName + ",";
                            item.SaveAs(path);
                        }
                    }

                    tc.DUONGDAN = fLoad[0].FileName;


                }
                UpdateModel(tc,tc.MATC);
                dl.SubmitChanges();
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
          
        }
        public ActionResult chitiet_TC(string id)
        {
            TRAICAY tc = dl.TRAICAYs.SingleOrDefault(t => t.MATC == id);
            if (tc == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return RedirectToAction("ChiTietSP", "Home", new { id = id });
            
        }
        [HttpPost, ActionName("chitiet_TC")]
        public ActionResult details_tc()
        {
            return RedirectToAction("Index");
        }
    }
}
