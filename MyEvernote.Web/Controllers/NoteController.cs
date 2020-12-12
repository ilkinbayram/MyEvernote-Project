using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MyEvernote.BussinesLayer;
using MyEvernote.BussinesLayer.Managers;
using MyEvernote.BussinesLayer.Tools;
using MyEvernote.EntitiesLayer;
using MyEvernote.Web.Filters;
using MyEvernote.Web.Models;

namespace MyEvernote.Web.Controllers
{
    [AuthIsLoggedOut]
    [OwnExc]
    public class NoteController : Controller
    {
        private UserManager userManager = new UserManager();
        private NoteManager _noteManager = new NoteManager();
        private CategoryManager _categoryManager = new CategoryManager();
        private LikeManager _likeManager = new LikeManager();
        private CommentManager _commentManager = new CommentManager();
        private DefaultDirectoryHelper directoryHelper = new DefaultDirectoryHelper();

        // GET: Note
        public ActionResult Index()
        {
            User currentUser = CurrentCookieTester.GetCurrentUser(CookieKeys.signedUserToken);
            if (TempData["NoteUpdate"] != null)
                ViewBag.ResultMethod = TempData["NoteUpdate"];
            if (TempData["NoteDelete"] != null)
                ViewBag.ResultMethod = TempData["NoteDelete"];
            if (TempData["NoteInsert"] != null)
                ViewBag.ResultMethod = TempData["NoteInsert"];
            if (currentUser != null)
            {
                if (currentUser.IsAdmin == false)
                    return View(_noteManager.List(x => x.IsDeleted == false && x.User.Id == currentUser.Id).OrderBy(x=>x.NoteTitle).ToList());
            }
            CurrentCookieTester.SetCookie(CookieKeys.updateableUrl, "Note/Index");
            return View(_noteManager.List(x => x.IsDeleted == false).OrderBy(x => x.NoteTitle).ToList());
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Note note = _noteManager.Get(x => x.Id == id.Value);
            if (note == null)
            {
                return HttpNotFound();
            }
            CurrentCookieTester.SetCookie(CookieKeys.updateableUrl, $"Note/Details/{id}");
            return View(note);
        }

        public ActionResult Create()
        {
            CurrentCookieTester.SetCookie(CookieKeys.updateableUrl, "Note/Create");
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Note note, HttpPostedFileBase notePhoto, int? categoryId)
        {
            ModelState.Remove("CreatedOn");
            ModelState.Remove("ModifiedOn");
            ModelState.Remove("ModifiedUsername");

            Category currentCategory = null;
            if (categoryId != null)
                currentCategory = _categoryManager.Get(x => x.Id == categoryId);
            if (currentCategory == null)
            {
                ModelState.AddModelError("", "Secdiyiniz Kateqoriya Tapilmadi. Zehmet Olmasa Tekrar Cehd Edin");
                return View(note);
            }
            else
            {
                note.Category = currentCategory;
            }

            if (ModelState.IsValid)
            {
                

                if (notePhoto != null)
                {
                    if (notePhoto.ContentType.Split('/')[1] == "jpg" ||
                    notePhoto.ContentType.Split('/')[1] == "jpeg" ||
                    notePhoto.ContentType.Split('/')[1] == "png")
                    {
                        string ownPath = Server.MapPath(directoryHelper.NoteImagesDir);
                        string fileName = $"noteProfilePhoto_{DateTime.Now.Day}_{DateTime.Now.Month}_{DateTime.Now.Year}_{DateTime.Now.Hour}_{DateTime.Now.Minute}_{DateTime.Now.Second}.{notePhoto.ContentType.Split('/')[1]}";
                        if (!Directory.Exists(ownPath))
                            Directory.CreateDirectory(ownPath);
                        notePhoto.SaveAs(ownPath+fileName);
                        note.ImageCap = fileName;
                    }
                    else
                    {
                        ModelState.AddModelError("", "Daxil Etdiyiniz Fayl Formati Duzgun Formatda Deyil.");
                        return View(note);
                    }
                }

                note.User = CurrentCookieTester.GetCurrentUser(CookieKeys.signedUserToken);
                BussinessResult<Note> result = _noteManager.Insert(note);
                if (result.Errors.Count > 0)
                {
                    foreach (BussinessError error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Detail);
                    }
                    return View(result.Result);
                }

                TempData["NoteInsert"] = result.Successes;
                return RedirectToAction("Index");
            }

            return View(note);
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Note note = _noteManager.Get(x => x.Id == id.Value);
            if (note == null)
            {
                return HttpNotFound();
            }
            CurrentCookieTester.SetCookie(CookieKeys.updateableUrl, $"Note/Edit/{id}");
            return View(note);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Note note, HttpPostedFileBase notePhoto, int? categoryId)
        {
            ModelState.Remove("CreatedOn");
            ModelState.Remove("ModifiedOn");
            ModelState.Remove("ModifiedUsername");
            ModelState.Remove("categoryId");
            Note currentNote = _noteManager.Get(x => x.Id == note.Id);
            Category currentCategory = null;

            if (categoryId != null)
                currentCategory = _categoryManager.Get(x => x.Id == categoryId);
            if (currentCategory != null)
                currentNote.Category = currentCategory;

            if (ModelState.IsValid)
            {
                // TODO : Check and Update
                if (notePhoto != null)
                {
                    if (notePhoto.ContentType.Split('/')[1] == "jpg" ||
                    notePhoto.ContentType.Split('/')[1] == "jpeg" ||
                    notePhoto.ContentType.Split('/')[1] == "png")
                    {
                        string ownPath = Server.MapPath(directoryHelper.NoteImagesDir);
                        string fileName = $"noteProfilePhoto_{DateTime.Now.Day}_{DateTime.Now.Month}_{DateTime.Now.Year}_{DateTime.Now.Hour}_{DateTime.Now.Minute}_{DateTime.Now.Second}.{notePhoto.ContentType.Split('/')[1]}";
                        if (!Directory.Exists(ownPath))
                            Directory.CreateDirectory(ownPath);
                        notePhoto.SaveAs(ownPath+fileName);
                        currentNote.ImageCap = fileName;
                    }
                    else
                    {
                        ModelState.AddModelError("", "Daxil Etdiyiniz Fayl Formati Duzgun Formatda Deyil.");
                        return View(note);
                    }
                }

                currentNote.IsDraft = note.IsDraft;
                currentNote.NoteTitle = note.NoteTitle;
                currentNote.Text = note.Text;

                BussinessResult<Note> result = _noteManager.Update(currentNote);

                if (result.Errors.Count > 0)
                {
                    foreach (BussinessError error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Detail);
                    }
                    return View(note);
                }

                TempData["NoteUpdate"] = result.Successes;
                return RedirectToAction("Index");
            }
            return View(note);
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Note note = _noteManager.Get(x => x.Id == id.Value);
            if (note == null)
            {
                return HttpNotFound();
            }
            CurrentCookieTester.SetCookie(CookieKeys.updateableUrl, $"Note/Delete/{id}");
            return View(note);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Note note = _noteManager.Get(x => x.Id == id);
            BussinessResult<Note> result = null;
            // TODO : Check And Remove

            if (note != null)
            {
                foreach (Liked like in note.Likes)
                {
                    _likeManager.Delete(like);
                }
                foreach (Comment comment in note.Comments)
                {
                    _commentManager.Delete(comment);
                }
                result = _noteManager.Delete(note);
            }
                

            if (result.Errors.Count > 0)
            {
                foreach (BussinessError error in result.Errors)
                {
                    ModelState.AddModelError("", error.Detail);
                }
                return View(note);
            }

            TempData["NoteDelete"] = result.Successes;
            return RedirectToAction("Index");
        }

        [AuthCheckAdmin]
        public ActionResult UserNotes(int? id)
        {
            User turnUser = null;
            List<Note> noteList = null;
            if (id != null)
                turnUser = userManager.Get(x => x.Id == id);
            if (turnUser != null)
                noteList = turnUser.Notes;
            return View("Index", noteList);
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
