using MyEvernote.BussinesLayer.Managers;
using MyEvernote.EntitiesLayer;
using MyEvernote.Web.Filters;
using MyEvernote.Web.Models;
using MyEvernote.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace MyEvernote.Web.Controllers
{
    [OwnExc]
    public class CommentController : Controller
    {
        private NoteManager _noteManager = new NoteManager();
        private CommentManager _commentManager = new CommentManager();
        
        public ActionResult ShowNoteComments(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            Note note = _noteManager.GetListWithQueryable().Include("Comments").Include("User").Where(x => x.Id == id).FirstOrDefault();
            if (note == null)
                return HttpNotFound();

            ViewModelEach viewModel = new ViewModelEach
            {
                Note = note,
                Comments = note.Comments.Where(x=>x.IsDeleted==false).OrderBy(x => x.ModifiedOn).ToList()
            };

            return PartialView("_PartialPageComments",viewModel);
        }
        [HttpPost]
        public ActionResult Create(Comment comment, int? noteID)
        {
            User user = CurrentCookieTester.GetCurrentUser(CookieKeys.signedUserToken);
            Note note = _noteManager.Get(x => x.Id == noteID);
            if (note == null || user == null)
                return HttpNotFound();

            ModelState.Remove("CreatedOn");
            ModelState.Remove("ModifiedOn");
            ModelState.Remove("ModifiedUsername");

            if (ModelState.IsValid)
            {
                comment.Note = note;
                comment.User = user;

                if (_commentManager.Insert(comment)>0)
                {
                    return Json(new { result = 1, message="Komment Elave Olundu" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { result = 0, message="Komment Elave Olunmadi" }, JsonRequestBehavior.AllowGet);
                }
            }

            return Json(new { result=-1, message="Qeyd Etdiyiniz Komment setri minimum 1, maksimum 3000 simvol hecminde olmalidir."});
        }

        [HttpPost]
        public ActionResult Edit(string Text, int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            Comment comment = _commentManager.Get(x => x.Id == id.Value && x.IsDeleted==false);
            if(comment==null)
                return new RedirectResult("/MyEvernoteHome/Index");

            if (!string.IsNullOrEmpty(Text))
            {
                comment.Text = Text;
                if (_commentManager.Update(comment) > 0)
                {
                    return Json(new { result = 1, message = "Commentiniz Guncellendi" },JsonRequestBehavior.AllowGet);
                }

                return Json(new { result = 0, message = "Commentiniz Guncellenmedi. Guncelleme Esnasinda Xeta Yarandi." }, JsonRequestBehavior.AllowGet);
            }

            return Json(new { result=-1, message="Comment Setrini Bosluq Seklinde Gondere Bilmezsiniz. Eger Commentinizi Silmek Isteyirsinizse Sil Duymesinden Istifade Edin."}, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            Comment comment = _commentManager.Get(x => x.Id == id && x.IsDeleted==false);
            if (comment == null)
                return new RedirectResult("/MyEvernoteHome/Index");

            if (_commentManager.Delete(comment) > 0)
            {
                return Json(new { result = 1, message = "Commentiniz Silindi" }, JsonRequestBehavior.AllowGet);
            }

            return Json(new { result = 0, message = "Commentiniz Silinerken Xeta Yarandi. Zehmet Olmasa Birazdan Tekrar Cehd Edin" }, JsonRequestBehavior.AllowGet);
        }
    }
}