using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ShopOnline.Models;


namespace ShopOnline.Areas.Admin.Controllers
{
    public class CRUDsizesController : Controller
    {
        menfashionEntities db = new menfashionEntities();
        // GET: Admin/CRUDsizes
        public ActionResult Index()
        {
            if (Session["infoAdmin"] == null)
            {
                return RedirectToAction("Login", "LoginMember");
            }
            else
            {
                return View();
            }
            
        }
        [HttpGet]
        public JsonResult DsSizes()
        {
            try
            {
                var dsSizes = (from i in db.Sizes
                                  select new
                                  {
                                      sizesId = i.sizeId,
                                      sizesName = i.nameSize
                                  }).ToList();

                return Json(new { code = 200, dsSizes = dsSizes, msg = "Successfully get list Category!" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { code = 500, msg = "Get list Category false: " + ex.Message, JsonRequestBehavior.AllowGet });
            }
        }
        [HttpPost]
        public JsonResult AddSizes(string sizesName)
        {
            try
            {
                var sizes = new Size();
                sizes.nameSize = sizesName;

                db.Sizes.Add(sizes);
                db.SaveChanges();

                return Json(new { code = 200, msg = "Successfully added a new product category!!" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { code = 500, msg = "Error: " + ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public JsonResult Detail(int sizesId)
        {
            try
            {
                var detail = (from i in db.Sizes
                              select new
                              {
                                  sizesId = i.sizeId,
                                  sizesName = i.nameSize
                              }).SingleOrDefault(model => model.sizesId == sizesId);
                //var detail = db.ProductCategories.Where(model => model.categoryId == categoryId).SingleOrDefault();
                return Json(new { code = 200, detail = detail, msg = "Successfully get detail!" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { code = 500, msg = "Failed get detail!" + ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public JsonResult Update(int sizesId, string sizesName)
        {
            try
            {
                var sizes = (from i in db.Sizes
                                select i).SingleOrDefault(model => model.sizeId == sizesId);
                sizes.nameSize = (string)sizesName;
                db.SaveChanges();
                return Json(new { code = 200, msg = "Successfully update!" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { code = 500, msg = "Failed get update!" + ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public JsonResult Delete(int sizesId)
        {
            try
            {
                Size sizes = (from i in db.Sizes
                                            select i).SingleOrDefault(model => model.sizeId == sizesId);
                db.Sizes.Remove(sizes);
                db.SaveChanges();
                return Json(new { code = 200, msg = "Successfully delete!" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { code = 500, msg = "Failed delete! " + ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}