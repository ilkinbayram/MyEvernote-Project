using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MyEvernote.BussinesLayer.Managers;
using MyEvernote.EntitiesLayer;
using MyEvernote.Web.Filters;
using MyEvernote.Web.Models;

namespace MyEvernote.Web.Controllers
{
    [AuthIsLoggedOut]
    [AuthCheckAdmin]
    [OwnExc]
    public class CategoryController : Controller
    {
        private CategoryManager _categoryManager = new CategoryManager();

        // GET: Category
        public ActionResult Index()
        {
            ViewBag.Success = TempData["successIndex"];
            List<Category> currentCategories = _categoryManager.List(x => x.IsDeleted == false);

            CurrentCookieTester.SetCookie(CookieKeys.updateableUrl, "Category/Index");

            return View(currentCategories);
        }

        // GET: Category/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Category category = _categoryManager.Get(x => x.Id == id.Value && x.IsDeleted==false);
            if (category == null)
            {
                return HttpNotFound();
            }

            CurrentCookieTester.SetCookie(CookieKeys.updateableUrl, $"Category/Index/{id}");

            return View(category);
        }

        // GET: Category/Create
        public ActionResult Create()
        {
            CurrentCookieTester.SetCookie(CookieKeys.updateableUrl, "Category/Create");

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Category category)
        {
            ModelState.Remove("CreatedOn");
            ModelState.Remove("ModifiedOn");
            ModelState.Remove("ModifiedUsername");

            if (ModelState.IsValid)
            {
                if (_categoryManager.Get(x => x.CategoryName == category.CategoryName && x.IsDeleted==false) != null)
                {
                    ModelState.AddModelError("", "Bu Kategoriya Adi Movcuddur. Baska Bir Ad istifade Edin.");
                }
                else
                {
                    int result = _categoryManager.Insert(category);

                    if (result <= 0)
                    {
                        ViewBag.NotInserted = "Kategoriya Elave Edilmedi. Birazdan Tekrar Cehd Edin.";
                        return View(category);
                    }
                    CacheHelper.RemoveCache(CacheHelperKeys.constCategoryList);
                    TempData["successIndex"] = $"{category.CategoryName} isimli Kategoriya Ugurla Elave Edildi";
                    return RedirectToAction("Index");
                }

            }

            return View(category);
        }

        // GET: Category/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Category category = _categoryManager.Get(x=>x.Id==id.Value);
            if (category == null)
            {
                return HttpNotFound();
            }

            CurrentCookieTester.SetCookie(CookieKeys.updateableUrl, $"Category/Edit/{id}");

            return View(category);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Category category)
        {
            ModelState.Remove("CreatedOn");
            ModelState.Remove("ModifiedOn");
            ModelState.Remove("ModifiedUsername");

            Category currentCat = _categoryManager.Get(x => x.Id == category.Id);

            if (ModelState.IsValid)
            {
                if (_categoryManager.Get(x => x.CategoryName == category.CategoryName && x.Id != category.Id && x.IsDeleted==false) != null)
                {
                    ModelState.AddModelError("", "Bu Kategoriya Adi Movcuddur. Baska Bir Ad istifade Edin.");
                }
                else
                {
                    currentCat.CategoryName = category.CategoryName;
                    currentCat.Description = category.Description;
                    int result = _categoryManager.Update(currentCat);

                    if (result <= 0)
                    {
                        ViewBag.NotUpdated = "Kategoriya Guncellenmedi. Birazdan Tekrar Cehd Edin.";
                        return View(category);
                    }
                    CacheHelper.RemoveCache(CacheHelperKeys.constCategoryList);
                    TempData["successIndex"] = $"{category.CategoryName} isimli Kategoriya Ugurla Guncellendi";
                    return RedirectToAction("Index");
                }

            }
            return View(category);
        }

        // GET: Category/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Category category = _categoryManager.Get(x => x.Id == id.Value && x.IsDeleted==false);
            if (category == null)
            {
                return HttpNotFound();
            }

            CurrentCookieTester.SetCookie(CookieKeys.updateableUrl, $"Category/Delete/{id}");

            return View(category);
        }

        // POST: Category/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Category category = _categoryManager.Get(x => x.Id == id && x.IsDeleted==false);
            int result = _categoryManager.Delete(category);
            if (result > 0)
            {
                CacheHelper.RemoveCache(CacheHelperKeys.constCategoryList);
                TempData["successIndex"] = $"{category.CategoryName} isimli Kategoriyanin Silme Emeliyyati Ugurla Heyata Kecirildi.";
                return RedirectToAction("Index");
            }
            ViewBag.NotDeleted = "Ugursuz! Silme Emeliyyati Heyata Kecirilmedi. Zehmet Olmasa Birazdan Tekrar Cehd Edin.";
            return View();
        }

        //protected override void Dispose(bool disposing)
        //{
        //    if (disposing)
        //    {
        //        //db.Dispose();
        //    }
        //    base.Dispose(disposing);
        //}
    }
}
