using MyEvernote.BussinesLayer.Managers;
using MyEvernote.BussinesLayer;
using MyEvernote.BussinesLayer.Tools;
using MyEvernote.EntitiesLayer;
using MyEvernote.EntitiesLayer.CheckModels;
using MyEvernote.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MyEvernote.Web.Models;
using System.Web.Helpers;
using MyEvernote.Web.Filters;

namespace MyEvernote.Web.Controllers
{
    [OwnExc]
    public class MyEvernoteHomeController : Controller
    {
        NoteManager nm = new NoteManager();
        UserManager um = new UserManager();
        DefaultImageHelper ImageHelper = new DefaultImageHelper();
        DefaultDirectoryHelper DefaultDirectoryHelper = new DefaultDirectoryHelper();
        ViewModelEach ViewModel = new ViewModelEach();

        #region PageMostLikedAndOthers
        // Page Actions -- Start
        public ActionResult Index()
        {
            Test test = new Test();

            ViewModel.ImageHelper = ImageHelper;
            ViewModel.DirectoryHelper = DefaultDirectoryHelper;

            if (nm.List() != null)
            {
                ViewModel.Notes = nm.List(x => x.IsDeleted == false && x.IsDraft == false && x.User.IsBanned == false && x.User.IsConfirmed == true && x.User.IsDeleted == false).OrderByDescending(x => x.CreatedOn).ToList();

                return View(ViewModel);
                //return View(nm.GetNotesByQuery().OrderByDescending(x=>x.ModifiedOn).ToList());
            }

            CurrentCookieTester.SetCookie(CookieKeys.updateableUrl, "MyEvernoteHome/Index");

            return View(ViewModel);
        }

        [AuthIsLoggedOut]
        public ActionResult LikedOwn(int? id)
        {
            ViewModel.DirectoryHelper = DefaultDirectoryHelper;
            ViewModel.ImageHelper = ImageHelper;

            User user = null;
            if (id != null)
                user = um.Get(x => x.Id == id.Value);
            List<Note> noteListLiked = new List<Note>();

            if (user.Likes != null)
            {
                foreach (Liked like in user.Likes)
                {
                    noteListLiked.Add(nm.Get(x => x.Id == like.Note.Id));
                }
            }

            ViewModel.Notes = noteListLiked;

            return View("Index", ViewModel);
        }

        public ActionResult ByCategory(int? id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            //CategoryManager cm = new CategoryManager();

            //Category cat = cm.Get(x => x.Id == id.Value && x.IsDeleted == false);
            //if (cat == null)
            //    return HttpNotFound();

            //return View("Index", cat.Notes.Where(x => x.IsDeleted == false && x.IsDraft==false).OrderByDescending(x => x.ModifiedOn).ToList());

            ViewModel.DirectoryHelper = DefaultDirectoryHelper;
            ViewModel.ImageHelper = ImageHelper;

            List<Note> notes = nm.List()
                .Where(x => x.IsDeleted == false && x.IsDraft == false && x.Category.Id==id.Value && x.User.IsBanned == false && x.User.IsConfirmed == true && x.User.IsDeleted == false)
                .OrderByDescending(x => x.CreatedOn).ToList();

            ViewModel.Notes = notes;

            return View("Index", ViewModel);
        }
        public ActionResult MostLiked()
        {
            ViewModel.DirectoryHelper = DefaultDirectoryHelper;
            ViewModel.ImageHelper = ImageHelper;

            List<Note> nts = nm.List(x => x.IsDeleted == false && x.IsDraft == false && x.User.IsBanned == false && x.User.IsConfirmed == true && x.User.IsDeleted == false);

            ViewModel.Notes = nts.OrderByDescending(x => x.LikeCount).ToList();

            return View("Index", ViewModel);
        }
        public ActionResult LastWritten()
        {
            ViewModel.DirectoryHelper = DefaultDirectoryHelper;
            ViewModel.ImageHelper = ImageHelper;

            ViewModel.Notes = nm.List(x => x.IsDeleted == false && x.IsDraft == false && x.User.IsBanned == false && x.User.IsConfirmed == true && x.User.IsDeleted == false).OrderByDescending(x => x.CreatedOn).ToList();

            return View("Index", ViewModel);
        }
        // Page Actions -- End
        #endregion

        #region LoginPart
        // Login Part -- Start
        [AuthIsSignedIn]
        public ActionResult Login()
        {
            CurrentCookieTester.SetCookie(CookieKeys.updateableUrl, "MyEvernoteHome/Login");
            return View();
        }
        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Login(UserLogin usl)
        {
            
            if (ModelState.IsValid)
            {
                BussinessResult<User> result = um.LoginUserCheck(usl);
                if (result.Errors.Count > 0)
                {
                    foreach (BussinessError bse in result.Errors)
                    {
                        ModelState.AddModelError(bse.Subject, bse.Detail);
                    }
                }
                else
                {
                    CurrentCookieTester.SetCookie(CookieKeys.signedUserToken, result.Result.Token.ToString());
                    return RedirectToAction("Index");
                }
            }
            // User'i Session'da tut
            // Yonlendir
            return View(usl);
        }
        // Login Part -- End
        #endregion

        #region CreateAccountPart
        // Create Account -- Start
        [AuthIsSignedIn]
        public ActionResult CreateAccount()
        {
            CurrentCookieTester.SetCookie(CookieKeys.updateableUrl, "MyEvernoteHome/CreateAccount");
            return View();
        }
        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult CreateAccount(UserRegister usr, string genderRadio)
        {
            switch (genderRadio)
            {
                case "male":
                    usr.Gender = true;
                    break;
                case "female":
                    usr.Gender = false;
                    break;
                default:
                    break;
            }
            if (string.IsNullOrEmpty(genderRadio) == true)
            {
                ModelState.AddModelError("", "Cins Boş Kecilə Bilməz");
                return View(usr);
            }


            // Istifadeci username mail yoxla
            // Kayit
            // verification
            if (ModelState.IsValid)
            {
                BussinessResult<User> result = um.RegisterUserCheck(usr);

                if (result.Errors.Count > 0)
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(error.Subject, error.Detail);
                    }
                }
                else
                {
                    ViewBag.userName = result.Result.Username;
                    OkViewModel model = new OkViewModel()
                    {
                        RedirectingInfo = "Login Sehifesine Yonlendirilirsiniz...",
                        RedirectUrl = "/MyEvernoteHome/Login",
                        Todo = "Zehmet Olmasa Mailinize Daxil Olub Hesabinizi Aktivlesdirin..."
                    };
                    model.Details.Add("Yeni Hesab Yaratma Emeliyyatiniz Ugurla Heyata Kecirildi.");
                    return View("Ok", model);
                }
            }

            return View(usr);
        }
        // Create Account -- End
        #endregion

        #region Logout
        public ActionResult Logout()
        {
            CurrentCookieTester.ExpireCookie(CookieKeys.signedUserToken, 6);
            CurrentCookieTester.SetCookie(CookieKeys.updateableUrl, "MyEvernoteHome/Index");
            return RedirectToAction("Index", "MyEvernoteHome");
        }
        #endregion

        #region Confirmation
        [AuthIsSignedIn]
        public ActionResult ActivationConfirming(string confirm)
        {
            BussinessProcessedType<User> bpResult = um.ConfirmUser(confirm);
            if (bpResult.ProcessedCount > 0)
            {
                CurrentCookieTester.SetCookie(CookieKeys.signedUserToken, bpResult.Entity.Token.ToString());
                return RedirectToAction("Index");
            }

            if (bpResult.Status == InformingOrError.InformMailAlreadyConfirmed)
            {
                ViewBag.userName = bpResult.Entity.Username;

                WarningViewModel model = new WarningViewModel()
                {
                    RedirectingInfo = "Esas Sehifeye Yonlendirilirsiniz...",
                    RedirectSeconds = 8,
                    RedirectUrl = "/MyEvernoteHome/Index",
                    Todo = "Hesabiniz Tesdiqlenmisdir. Artiq Istediyiniz Zaman Login Sehifesinden Hesabiniza Daxil Ola bilersiniz."
                };
                model.Details.Add("Hesabiniz Daha Once Tesdiqlenmisdir. Buna Gore Artiq Aktivlesdirme Linki Passivdir.");

                return View("Warning", model);
            }

            ViewBag.userName = null;
            ErrorViewModel modelUnknown = new ErrorViewModel()
            {
                RedirectingInfo = "Esas Sehifeye Yonlendirilirsiniz...",
                RedirectSeconds = 8,
                RedirectUrl = "/MyEvernoteHome/Index",
                Todo = "Zehmet Olmasa Biraz Gozledikden Sonra Tekrar Cehd Edin"
            };
            modelUnknown.Details.Add(new BussinessError
            {
                ErrorCode = InformingOrError.ErrorRegisterFailedSendingConfirmEmail,
                Detail = "Hesabinizda Tesdiqlenme Emeliyyati Aparilarken Xeta Yarandi."
            });
            return View("Error", modelUnknown);
        }
        [AuthIsSignedIn]
        public ActionResult ForgotPassword()
        {
            return View();
        }
        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult ForgotPassword(string turnedValue)
        {
            if (string.IsNullOrEmpty(turnedValue))
                return View();
            User foundUser = um.Get(x => x.Email == turnedValue || x.Username == turnedValue);
            if (foundUser == null)
            {
                ModelState.AddModelError("", "Daxil Etdiyiniz Parametrlere Uygun Istifadeci Tapilmadi");
                return View();
            }

            BussinessResult<User> result = um.SendMailForConfirm(foundUser, "ChangePassword", "MyEvernoteHome");

            if (result.Result != null)
            {
                OkViewModel ok = new OkViewModel
                {
                    Details = new List<string>
                {
                    "Dogrudur",
                    "Qeyd Etdiyiniz Parametrlere Uygun Istifadeci Tapildi."
                },
                    RedirectingInfo = "Siz Esas Sehifeye Yonlendirilirsiniz..",
                    RedirectUrl = "/MyEvernoteHome/Index",
                    IsRedirecting = true,
                    RedirectSeconds = 15,
                    Todo = "Sizin E-Mail Adresinize Dogrulama Linki Gonderildi. Email Hesabiniza Daxil Olub Sifrenizi Yenileye Bilersiniz."
                };
                return View("Ok", ok);
            }
            else
            {
                foreach (BussinessError error in result.Errors)
                {
                    ModelState.AddModelError("", error.Detail);
                }
                return View();
            }
        }
        [AuthIsSignedIn]
        public ActionResult ChangePassword(string turnedGuid)
        {
            if (string.IsNullOrEmpty(turnedGuid))
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            User user = um.Get(x => x.ConfirmCode.ToString() == turnedGuid);
            if (user == null)
            {
                WarningViewModel warning = new WarningViewModel
                {
                    Details = new List<string>
                    {
                        "Link Movcud Deyil Ve Ya Siz Evvelce Bu Link Ile Sifrenizi Deyisdirmisiniz.",
                        "Tekrar Deyismek Ucun Sifreni Unutdum Bolmesine Daxil Olub Sifrenizi Yenileye Bilersiniz."
                    },
                    RedirectSeconds = 15,
                    RedirectingInfo = "Linke Tiklamadiginiz Muddetce Login Sehifesine Yonlendirileceksiniz",
                    RedirectUrl = "/MyEvernoteHome/Login",
                    Title = "Diqqet !",
                    IsRedirecting = true,
                    Todo = "Zehmet Olmasa Line Tiklayin Ve Yeniden Sifrenizi Deyisdirmek Ucun Email Alin"
                };
                return View("Warning", warning);
            }
            ViewBag.TurnedUser = user;
            return View();
        }
        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult ChangePassword(UserRegister user, int userID)
        {
            User currentUser = um.Get(x => x.Id == userID);
            BussinessResult<User> result = new BussinessResult<User>();
            ModelState.Remove("Email");
            ModelState.Remove("Gender");
            ModelState.Remove("Username");

            if (ModelState.IsValid)
            {
                currentUser.Password = Crypto.HashPassword(user.Password);
                currentUser.ConfirmCode = Guid.NewGuid();
                result = um.Update(currentUser);
                if (result.Errors.Count == 0)
                {

                    OkViewModel ok = new OkViewModel
                    {
                        Details = new List<string>
                        {
                            "Sifreniz Deyistirildi"
                        },
                        Title = "Tebrikler!",
                        RedirectUrl = "/MyEvernoteHome/Login",
                        RedirectSeconds = 6,
                        IsRedirecting = true,
                        RedirectingInfo = "Siz Login Sehifesine Yonlendirilirsiniz",
                        Todo = "Artiq Hesabiniza Yeni Sifrenizle Giris Ede Bilersiniz"
                    };
                    return View("Ok", ok);
                }

                foreach (BussinessError error in result.Errors)
                {
                    ModelState.AddModelError("", error.Detail);
                }
            }
            ViewBag.TurnedUser = currentUser;
            return View(user);
        }
        #endregion

        #region OpenToPeople

        public ActionResult About()
        {
            CurrentCookieTester.SetCookie(CookieKeys.updateableUrl, "MyEvernoteHome/About");
            return View();
        }

        public ActionResult Contact()
        {
            CurrentCookieTester.SetCookie(CookieKeys.updateableUrl, "MyEvernoteHome/Contact");
            return View();
        }

        public ActionResult AccessDenied()
        {
            if (CurrentCookieTester.GetCurrentUser(CookieKeys.signedUserToken) == null)
                return new RedirectResult("/MyEvernoteHome/Index");

            ErrorViewModel errorModel = new ErrorViewModel()
            {
                IsRedirecting = true,
                RedirectingInfo = "Esas Sehifeye Yonlendirilirsiniz...",
                RedirectSeconds = 8,
                RedirectUrl = "/MyEvernoteHome/Index",
                Title = "Hormetli" + CurrentCookieTester.GetCurrentUsername(CookieKeys.signedUserToken),
                Details = new List<BussinessError>
                {
                    new BussinessError
                    {
                        AlertColor = "danger",
                        Detail = "Icazeniz Olmayan Sorgu Isteyi Gondermisiniz.",
                        ErrorCode = InformingOrError.ErrorAccessDenied
                    }
                },
                Todo = "Sadece Icazeniz Olan Bolmelere Daxil Olmaginiz Xais Olunur"
            };
            CurrentCookieTester.SetCookie(CookieKeys.updateableUrl, "MyEvernoteHome/Index");
            return View("Error",errorModel);
        }

        public ActionResult ErrorTurned()
        {
            if (TempData["Last-Error"] == null)
            {
                return View("Index");
            }
            Exception exception = TempData["Last-Error"] as Exception;

            ErrorViewModel errorModel = new ErrorViewModel
            {
                AlertColor = "danger",
                IsRedirecting = true,
                RedirectingInfo = "Yonlendiyiniz Sehife: Ana Sehife ",
                RedirectSeconds = 15,
                RedirectUrl = "/MyEvernoteHome/Index",
                Title = "DIQQET !",
                Todo = "Uzlesdiyiniz Problemin Hellinde Sixinti Yasayirsinizsa, Zehmet Olmasa Sistem Administratoru Ile Elaqe Saxlayin.",
                Details = new List<BussinessError>
                {
                    new BussinessError
                    {
                        AlertColor = "danger",
                        ErrorCode = InformingOrError.ExceptionTurnedError,
                        Detail = exception.Message
                    }
                }
            };
            return View("Error",errorModel);
        }

        #endregion

        [HttpPost]
        public ActionResult QuicklyRegistrate(string email)
        {
            BussinessJsonResult jsonResult;

            if (GeneralUtilities.EmailIsValid(email))
            {
                jsonResult = um.QuicklyRegister(email);
            }
            else
            {
                jsonResult = new BussinessJsonResult
                {
                    Status = 0,
                    Message = "Your Email Is Not Valid Email! Please Write Correct Email!"
                };
            }

            return Json(new { status = jsonResult.Status, message = jsonResult.Message}, JsonRequestBehavior.AllowGet);
        }
    }
}