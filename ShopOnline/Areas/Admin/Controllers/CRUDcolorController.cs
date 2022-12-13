using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ShopOnline.Models;

namespace ShopOnline.Areas.Admin.Controllers
{
    public class CRUDcolorController : Controller
    {
        menfashionEntities db = new menfashionEntities();
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
        public JsonResult DsColor()
        {
            try
            {
                var dsColor = (from i in db.Colors
                                  select new
                                  {
                                      colorId = i.colorId,
                                      colorName = i.nameColor
                                  }).ToList();

                return Json(new { code = 200, dsColor = dsColor, msg = "Successfully get list Category!" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { code = 500, msg = "Get list Category false: " + ex.Message, JsonRequestBehavior.AllowGet });
            }
        }
        [HttpPost]
        public JsonResult AddColor(string colorName)
        {
            try
            {
                var color = new Color();
                color.nameColor = colorName;

                db.Colors.Add(color);
                db.SaveChanges();

                return Json(new { code = 200, msg = "Successfully added a new product category!!" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { code = 500, msg = "Error: " + ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public JsonResult Detail(int colorId)
        {
            try
            {
                var detail = (from i in db.Colors
                              select new
                              {
                                  colorId = i.colorId,
                                  colorName = i.nameColor
                              }).SingleOrDefault(model => model.colorId == colorId);
                //var detail = db.ProductCategories.Where(model => model.categoryId == categoryId).SingleOrDefault();
                return Json(new { code = 200, detail = detail, msg = "Successfully get detail!" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { code = 500, msg = "Failed get detail!" + ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public JsonResult Update(int colorId, string colorName)
        {
            try
            {
                var color = (from i in db.Colors
                                select i).SingleOrDefault(model => model.colorId == colorId);
                color.nameColor = (string)colorName;
                db.SaveChanges();
                return Json(new { code = 200, msg = "Successfully update!" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { code = 500, msg = "Faily update" + ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public JsonResult Delete(int colorId)
        {
            try
            {
                Color color = (from i in db.Colors
                                            select i).SingleOrDefault(model => model.colorId == colorId);
                db.Colors.Remove(color);
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