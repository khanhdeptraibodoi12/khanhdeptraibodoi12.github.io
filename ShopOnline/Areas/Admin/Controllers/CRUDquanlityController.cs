using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ShopOnline.Models;
using System.IO;
using PagedList;
using PagedList.Mvc;


namespace ShopOnline.Areas.Admin.Controllers
{
    public class CRUDquanlityController : Controller
    {
        // GET: Admin/CRUDquanlity
        menfashionEntities db = new menfashionEntities();
        public ActionResult Index(int? page, string searching)
        {
            var pageNumber = page ?? 1;
            var pageSize = 10;
            var quanlitys = db.Amounts.Where(model => model.Product.productName.Contains(searching) || searching == null).OrderByDescending(model => model.Product.dateCreate).Include(model => model.Product).Include(model => model.Color).Include(model => model.Size).ToPagedList(pageNumber, pageSize);
            return View(quanlitys);
        }
        //CREATE
        [HttpGet]
        public ActionResult Create()
        {
         
            ViewBag.productId = new SelectList(db.Products, "productId", "productName");
            ViewBag.colorId = new SelectList(db.Colors, "colorId", "nameColor");
            ViewBag.sizeId = new SelectList(db.Sizes, "sizeId", "nameSize");
            return View();
        }
        [HttpPost]
      
        public ActionResult Create(Amount amount, FormCollection collection)
        {
            try
            {
                var check = db.Amounts.Where(model => model.productId == amount.productId && model.sizeId == amount.sizeId && model.colorId == amount.colorId).SingleOrDefault();
                if(check != null)
                {
                    ModelState.AddModelError("", "Product name already exists");
                }
                else
                {
                    var quanlity = int.Parse(collection["quanlity"]);
                    amount.quality = quanlity;
                    db.Amounts.Add(amount);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                ViewBag.colorId = new SelectList(db.Colors, "colorId", "nameColor",amount.colorId);
                ViewBag.productId = new SelectList(db.Products, "productId", "productName",amount.productId);
                ViewBag.sizeId = new SelectList(db.Sizes, "sizeId", "nameSize",amount.sizeId);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["msgCreatefailed"] = "Create failed! " + ex.Message;
                return RedirectToAction("Create");
            }
        }

        //EDITs
        [HttpGet]
        public ActionResult Edit(int? quantityId)
        {
            Amount quanlity = db.Amounts.Find(quantityId);
            
            ViewBag.colorId = new SelectList(db.Colors, "colorId", "nameColor", quanlity.colorId);
            ViewBag.productId = new SelectList(db.Products, "productId", "productName", quanlity.productId);
            ViewBag.sizeId = new SelectList(db.Sizes, "sizeId", "nameSize", quanlity.sizeId);
            return View(quanlity);
        }
        [HttpPost]
        public ActionResult Edit(Amount amount, FormCollection collection)
        {
            
               
                var idquantity = int.Parse(collection["idQ"]);
                var quanlity = int.Parse(collection["quanlity"]); 

            var amount1 = db.Amounts.Where(model => model.quantityId == idquantity).SingleOrDefault();
                amount1.colorId = amount.colorId;
                amount1.sizeId = amount.sizeId;
                amount1.productId = amount.productId;
                amount1.quality = quanlity;
                db.SaveChanges();
                if (db.SaveChanges() > 0)
                {
                    TempData["msgEdit"] = "Successfully edited product " + amount.Product.productName;
                }
                ViewBag.colorId = new SelectList(db.Colors, "colorId", "nameColor", amount.colorId);
                ViewBag.productId = new SelectList(db.Products, "productId", "productName", amount.productId);
                ViewBag.sizeId = new SelectList(db.Sizes, "sizeId", "nameSize", amount.sizeId);
                return RedirectToAction("Index");
            
           
        }
    }
}