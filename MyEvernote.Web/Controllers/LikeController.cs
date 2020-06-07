using MyEvernote.BussinesLayer.Managers;
using MyEvernote.EntitiesLayer;
using MyEvernote.Web.Filters;
using MyEvernote.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace MyEvernote.Web.Controllers
{
    [OwnExc]
    public class LikeController : Controller
    {
        private NoteManager _noteManager = new NoteManager();
        private LikeManager _likeManager = new LikeManager();

        [HttpPost]
        public ActionResult GetLikedIds(int[] ids)
        {
            List<int> likedIds = new List<int>();
            User currentUser = CurrentCookieTester.GetCurrentUser(CookieKeys.signedUserToken);

            if (currentUser != null)
            {
                likedIds = _likeManager.List(x => x.User.Id == currentUser.Id)
                .Where(x => ids.Contains(x.Note.Id)).GroupBy(x => x.Note.Id).Select(x => x.First())
                .Select(x => x.Note.Id).ToList();
            }

            return Json(new { result=likedIds});
        }

        [HttpPost]
        public ActionResult SetLikeProcess(int? id, bool isInsert)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            Note note = _noteManager.Get(x => x.Id == id.Value && x.IsDeleted == false);
            User user = CurrentCookieTester.GetCurrentUser(CookieKeys.signedUserToken);

            if (note == null && user == null)
                return new RedirectResult("/MyEvernoteHome/Index");

            if (isInsert == true)
            {
                Liked liked = new Liked();
                liked.Note = note;
                liked.User = user;

                if (_likeManager.Get(x => x.Note.Id == note.Id && x.User.Id == user.Id) == null)
                {
                    if (_likeManager.Insert(liked) > 0)
                    {
                        note.LikeCount++;
                        _noteManager.Update(note);
                        return Json(new { result = 1, likeCount = note.LikeCount, likeStatus = isInsert }, JsonRequestBehavior.AllowGet);
                    }
                    return Json(new { result = 0, message = "Like Prosesi Ugursuz Oldu. Sistemde Xeta Yarandi Zehmet Olmasa Birazdan Tekrar Cehd Edin", likeCount = note.LikeCount, likeStatus = !isInsert }, JsonRequestBehavior.AllowGet);
                }

                return new RedirectResult("/MyEvernoteHome/Index");
            }
            else
            {
                Liked deletingLike = _likeManager.Get(x => x.Note.Id == note.Id && x.User.Id == user.Id);

                if (deletingLike != null)
                {
                    if (_likeManager.PermanentlyDelete(deletingLike) > 0)
                    {
                        note.LikeCount--;
                        _noteManager.Update(note);
                        return Json(new { result = 1, likeCount = note.LikeCount, likeStatus = isInsert }, JsonRequestBehavior.AllowGet);
                    }

                    return Json(new { result = 0, message = "Post Beyenilme Prosesi zamani Xeta Formalasdi. Postunuz Beyenilmedi. Zehmet Olmasa Birazdan Tekrar Cehd Edin", likeCount = note.LikeCount, likeStatus = !isInsert });
                }

                //return Json(new { result = -1, message = "Post Daha Once Dislike Olunmusdur. Sisteminizde Xeta Yaranmisdir Zehmet Olmasa Sehifeni Yenileyin Ve Tekrar Cehd Edin"});
                return new RedirectResult("/MyEvernoteHome/Index");
            }
        }
    }
}