using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using MyEvernote.BussinesLayer.Managers;
using MyEvernote.BussinesLayer;
using MyEvernote.EntitiesLayer;
using MyEvernote.Web.Models;
using MyEvernote.Web.Filters;

namespace MyEvernote.Web.Controllers
{
    [AuthIsLoggedOut]
    [OwnExc]
    public class UserController : Controller
    {
        private UserManager userManager = new UserManager();

        public ActionResult ShowProfile()
        {
            User user = null;
            if (CurrentCookieTester.CookieIsExist(CookieKeys.signedUserToken))
            {
                user = userManager.GetUserForCookie(CurrentCookieTester.GetCurrentToken(CookieKeys.signedUserToken));
            }
            CurrentCookieTester.SetCookie(CookieKeys.updateableUrl, "User/ShowProfile");
            return View(user);
        }

        public ActionResult EditProfile()
        {
            CurrentCookieTester.SetCookie(CookieKeys.updateableUrl, "User/EditProfile");

            return View();
        }
        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult EditProfile(User user, HttpPostedFileBase profileImage, int currentId)
        {
            User CurrentUser = userManager.GetUserByID(currentId);
            BussinessResult<User> result = new BussinessResult<User>();
            ModelState.Remove("CreatedOn");
            ModelState.Remove("ModifiedOn");
            ModelState.Remove("ModifiedUsername");
            ModelState.Remove("ConfirmCode");
            ModelState.Remove("Token");
            if (ModelState.IsValid && CurrentUser != null)
            {
                if (userManager.Get(x=>x.Id!=CurrentUser.Id && x.Username==user.Username) != null)
                {
                    ModelState.AddModelError("Username", "Bu Istifadeci Adi Movcuddur!");
                    return View(user);
                }


                if (profileImage != null)
                {
                    if ((profileImage.ContentType.Split('/')[1] == "jpeg" ||
                                    profileImage.ContentType.Split('/')[1] == "jpg" ||
                                    profileImage.ContentType.Split('/')[1] == "pgn"))
                    {
                        string ownPath = "/Images/UserImages";
                        string fileName = $"user_profileImage_{CurrentUser.Token}_{DateTime.Now.Day}_{DateTime.Now.Month}_{DateTime.Now.Year}_{DateTime.Now.Minute}_{DateTime.Now.Second}.{profileImage.ContentType.Split('/')[1]}";
                        if (!Directory.Exists(ownPath))
                        {
                            Directory.CreateDirectory(ownPath);
                        }
                        profileImage.SaveAs(Server.MapPath(ownPath + "/" + fileName));
                        CurrentUser.ImageRoad = fileName;
                    }
                    else
                    {
                        ModelState.AddModelError("", "Daxil Etdiyiniz File Formati Duzgun Deyil ! Zehmet Olmasa <.jpg>,<.jpeg> ve ya <.png> Formatli File Secin !");
                        return View(user);
                    }

                }


                CurrentUser.Name = user.Name;
                CurrentUser.Surname = user.Surname;
                CurrentUser.Username = user.Username;


                result = userManager.UpdateUser(CurrentUser);

                if (result.Result != null)
                {
                    ViewBag.AlertResult = result.Errors;
                    return View(result.Result);
                }

                foreach (BussinessError bsError in result.Errors)
                {
                    ModelState.AddModelError("", bsError.Detail);
                }

            }
            else
            {
                return View(user);
            }

            if (profileImage != null)
            {
                if (!(profileImage.ContentType.Split('/')[1] == "jpeg" ||
                                    profileImage.ContentType.Split('/')[1] == "jpg" ||
                                    profileImage.ContentType.Split('/')[1] == "pgn"))
                {
                    ModelState.AddModelError("", "Secdiyiniz Profil Resmi Duzgun Formatda Deyil.");
                }
            }

            return View(user);
        }

        public ActionResult DeleteProfile()
        {
            CurrentCookieTester.SetCookie(CookieKeys.updateableUrl, "User/DeleteProfile");
            return View();
        }
        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult DeleteProfile(string password, int currentId)
        {
            User user = userManager.GetUserByID(currentId);

            if (user != null)
            {
                if (user.Password == "demoxxx123")
                {
                    if (user.Password == password)
                    {
                        int result = userManager.DeleteUser(user);
                        if (result > 0)
                        {
                            CurrentCookieTester.ExpireCookie(CookieKeys.signedUserToken, 6);
                            return RedirectToAction("Index", "MyEvernoteHome");
                        }
                        ViewBag.Error = "Silme Emeliyyati Heyata Kecirilmedi. Zehmet Olmasa Birazdan Tekrar Cehd Edin...";
                    }
                    else
                    {
                        ViewBag.Error = "Girdiyiniz Sifre Dogru Deyil !";
                    }
                }
                else
                {
                    if (Crypto.VerifyHashedPassword(user.Password, password))
                    {
                        int result = userManager.DeleteUser(user);
                        if (result > 0)
                        {
                            CurrentCookieTester.ExpireCookie(CookieKeys.signedUserToken, 6);
                            return RedirectToAction("Index", "MyEvernoteHome");
                        }
                        ViewBag.Error = "Silme Emeliyyati Heyata Kecirilmedi. Zehmet Olmasa Birazdan Tekrar Cehd Edin...";
                    }
                    else
                    {
                        ViewBag.Error = "Girdiyiniz Sifre Dogru Deyil !";
                    }
                }
            }
            return View();
        }
        [AuthCheckAdmin]
        public ActionResult UserList()
        {
            if (TempData["newUserAdd"] != null)
                ViewBag.ResultError = TempData["newUserAdd"];
            if(TempData["userEdit"] != null)
                ViewBag.ResultError = TempData["userEdit"];
            if(TempData["Delete"]!=null)
                ViewBag.ResultError = TempData["Delete"];
            string currentToken = CurrentCookieTester.GetCurrentToken(CookieKeys.signedUserToken);
            List<User> _userList = userManager.List(x=>x.IsDeleted==false && x.IsAdmin==false && x.Token.ToString()!=currentToken);
            
            CurrentCookieTester.SetCookie(CookieKeys.updateableUrl, "User/UserList");

            return View(_userList);
        }
        [AuthCheckAdmin]
        public ActionResult CreateUser()
        {
            CurrentCookieTester.SetCookie(CookieKeys.updateableUrl, "User/CreateUser");

            return View();
        }
        [HttpPost,ValidateAntiForgeryToken]
        public ActionResult CreateUser(User user)
        {
            ModelState.Remove("CreatedOn");
            ModelState.Remove("ModifiedOn");
            ModelState.Remove("ModifiedUsername");
            ModelState.Remove("ConfirmCode");
            ModelState.Remove("Token");

            if (ModelState.IsValid)
            {
                BussinessResult<User> bsRes = userManager.Insert(user);
                if (bsRes.Errors.Count>0)
                {
                    foreach (var error in bsRes.Errors)
                    {
                        ModelState.AddModelError("", error.Detail);
                    }
                    return View(user);
                }

                TempData["newUserAdd"] = bsRes.Successes;
                return RedirectToAction("UserList");
            }
            return View(user);
        }
        [AuthCheckAdmin]
        public ActionResult UserEdit(int? id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            
            User user = userManager.Get(x => x.Id == id.Value);
            if (user == null) return new HttpStatusCodeResult(HttpStatusCode.NotFound);

            CurrentCookieTester.SetCookie(CookieKeys.updateableUrl, $"User/UserEdit/{id}");

            return View(user);
        }
        [HttpPost,ValidateAntiForgeryToken]
        public ActionResult UserEdit(User user)
        {
            User currentUser = userManager.Get(x => x.Id == user.Id);
            ModelState.Remove("CreatedOn");
            ModelState.Remove("ModifiedOn");
            ModelState.Remove("ModifiedUsername");
            ModelState.Remove("ConfirmCode");
            ModelState.Remove("Token");

            if (ModelState.IsValid)
            {
                currentUser.Gender = user.Gender;
                currentUser.IsAdmin = user.IsAdmin;
                currentUser.IsBanned = user.IsBanned;
                currentUser.IsConfirmed = user.IsConfirmed;
                currentUser.Name = user.Name;
                currentUser.Surname = user.Surname;
                BussinessResult<User> res = userManager.Update(currentUser);
                if (res.Result != null)
                {
                    TempData["userEdit"] = res.Errors;
                    return RedirectToAction("UserList");
                }

                foreach (BussinessError error in res.Errors)
                {
                    ModelState.AddModelError("", error.Detail);
                }
            }
            return View(user);
        }
        [AuthCheckAdmin]
        public ActionResult UserDelete(int? id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            User user = userManager.Get(x => x.Id == id.Value);
            if (user == null) return new HttpStatusCodeResult(HttpStatusCode.NotFound);

            CurrentCookieTester.SetCookie(CookieKeys.updateableUrl, $"User/UserDelete/{id}");

            return View(user);
        }
        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult UserDelete(int id)
        {
            User deleteUser = userManager.Get(x => x.Id == id);
            if (deleteUser != null)
            {
                BussinessResult<User> bsres = userManager.Delete(deleteUser);
                TempData["Delete"] = bsres.Errors;
                return RedirectToAction("UserList");
            }
            else
            {
                TempData["Delete"] = new List<BussinessError>()
                {
                    new BussinessError
                    {
                        AlertColor = "danger",
                        Detail = "Silmek Istediyiniz Hesab Tapilmadi... Zehmet Olmasa Hemin Hesabi Tekrar Yoxlayin"
                    }
                };
                return RedirectToAction("UserList");
            }
        }
        [AuthCheckAdmin]
        public ActionResult UserDetails(int? id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            User user = userManager.Get(x => x.Id == id.Value);
            if (user == null) return new HttpStatusCodeResult(HttpStatusCode.NotFound);

            CurrentCookieTester.SetCookie(CookieKeys.updateableUrl, $"User/UserDetails/{id}");

            return View(user);
        }
        
    }
}
