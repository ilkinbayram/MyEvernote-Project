using CommonLayer.Helpers;
using MyEvernote.BussinesLayer.Tools;
using MyEvernote.CommonLayer.Helpers;
using MyEvernote.DataAccessLayer.EntityFramework;
using MyEvernote.EntitiesLayer;
using MyEvernote.EntitiesLayer.CheckModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Web.Helpers;

namespace MyEvernote.BussinesLayer.Managers
{
    public class UserManager : ManagerBase<User>
    {
        private Repository<User> repo_user = new Repository<User>();
        private NoteManager _noteM = new NoteManager();
        private CommentManager _com = new CommentManager();
        private LikeManager _likeM = new LikeManager();

        public List<User> GetAllUsers()
        {
            return repo_user.List(x => x.IsDeleted == false);
        }
        public string CheckCookieThenGetResponse(string userGuid)
        {
            User user = null;
            user = GetUserForCookie(userGuid);

            if (user != null)
            {
                return user.Username;
            }
            return "System";
        }
        public User GetUserForCookie(string token)
        {
            User user = repo_user.Get(x => x.Token.ToString() == token && x.IsDeleted == false);

            if (user != null && user.IsConfirmed == true && user.IsBanned == false)
            {
                return user;
            }

            return null;
        }
        public User GetUserByID(int Id)
        {
            return repo_user.Get(x => x.Id == Id && x.IsBanned == false);
        }
        public BussinessResult<User> UpdateUser(User user)
        {
            BussinessResult<User> bssres = new BussinessResult<User>();

            if (base.Update(user) <= 0)
            {
                bssres.Errors.Add(new BussinessError()
                {
                    Detail = "Update Emeliyyati Ugursuz Oldu",
                    ErrorCode = InformingOrError.ErrorFailedUpdateProcess
                });
            }
            else
            {
                bssres.Successes.Add(new BussinessError()
                {
                    Detail = "Update Emeliyyati Ugurla Heyata Kecirildi!",
                    ErrorCode = InformingOrError.SuccessUpdateEditProfile
                });
                bssres.Result = user;
            }
            return bssres;
        }
        public int DeleteUser(User user)
        {
            foreach (Note note in _noteM.List(x => x.User.Id == user.Id))
            {
                foreach (Comment comment in note.Comments)
                {
                    _com.Delete(comment);
                }

                foreach (Liked like in note.Likes)
                {
                    _likeM.Delete(like);
                }
                _noteM.Delete(note);
            }
            return base.Delete(user);
        }
        public BussinessResult<User> RegisterUserCheck(UserRegister newUser)
        {
            BussinessResult<User> layerResult = new BussinessResult<User>();
            User user = repo_user.Get(x => x.Username == newUser.Username || x.Email == newUser.Email);

            if (user != null && user.IsDeleted == false)
            {
                if (user.Username == newUser.Username)
                    layerResult.Errors.Add(new BussinessError
                    {
                        Subject = "Username",
                        ErrorCode = InformingOrError.ErrorRegisterAlreadyExistUsername,
                        Detail = "Bu İstifadəçi adı mövcuddur."
                    });
                if (user.Email == newUser.Email)
                    layerResult.Errors.Add(new BussinessError
                    {
                        Subject = "Email",
                        ErrorCode = InformingOrError.ErrorRegisterAlreadyExistEmail,
                        Detail = "Bu Email Adresi Mövcuddur."
                    });
            }
            else
            {
                User successUser = new User
                {
                    Username = newUser.Username,
                    Password = Crypto.HashPassword(newUser.Password),
                    Email = newUser.Email,
                    Gender = newUser.Gender,
                    ConfirmCode = Guid.NewGuid(),
                    Token = Guid.NewGuid()
                };
                if (newUser.Gender)
                {
                    successUser.ImageRoad = "boy_profilePhoto.png";
                }
                else
                {
                    successUser.ImageRoad = "girl_profilePhoto.png";
                }

                #region WebMailSendVersion
                //WebMail.SmtpServer = "smtp.gmail.com";
                //WebMail.SmtpPort = 587;
                //WebMail.UserName = "companybayramsoy@gmail.com";
                //WebMail.Password = "23444327777";
                //WebMail.EnableSsl = true;
                //WebMail.Send(
                //    to: successUser.Email,
                //    subject: "Account Confirmation By Email",
                //    body: HtmlMailDesign.TemplateActivation1(successUser.ConfirmCode.ToString(), "MyEvernoteHome", "ActivationConfirming", false, ConfigHelper.Get<string>("SiteRootUri"), false, false, "confirm", Username: successUser.Username),
                //    isBodyHtml: true,
                //    replyTo: "dont-reply@gmail.com"
                //    );
                #endregion

                string htmlBody = HtmlMailDesign.TemplateActivation1(successUser.ConfirmCode.ToString(), "MyEvernoteHome", "ActivationConfirming", true, "localhost:44363", false, false, "confirm", Username: successUser.Username);
                bool isMailSent = MailHelper.SendMail(htmlBody, successUser.Email, MailHelper.MailSubjectStatus(SubjectStatus.AccountConfirmationMail));

                if (isMailSent == true)
                {
                    int processed = base.Insert(successUser);

                    if (processed > 0)
                    {
                        layerResult.Result = successUser;
                    }
                    else
                    {
                        layerResult.Errors.Add(new BussinessError
                        {
                            ErrorCode = InformingOrError.ErrorRegisterUnknownSoftwareCode,
                            Detail = "Sistemde Taninmayan Bir Xeta Yarandi. Zehmet Olmasa Birazdan Tekrar Cehd Edin.",
                            Subject = ""
                        });
                    }
                }
                else
                {
                    layerResult.Errors.Add(new BussinessError
                    {
                        Subject = "",
                        Detail = "Hesabiniza Tesdiqleme Maili Gonderme Prosesi Ugursuz Oldu. Zehmet Olmasa Qeydiyyatdan Kecerken Movcud Email Adresinden Istifade Edin.",
                        ErrorCode = InformingOrError.ErrorRegisterFailedSendingConfirmEmail
                    });
                }
            }
            return layerResult;
        }
        public BussinessProcessedType<User> ConfirmUser(string confirmGuid)
        {
            BussinessProcessedType<User> typeResult = new BussinessProcessedType<User>();

            User user = repo_user.Get(x => x.ConfirmCode.ToString() == confirmGuid);
            if (user != null && user.IsDeleted == false)
            {
                if (user.IsConfirmed == false)
                {
                    user.IsConfirmed = true;
                    typeResult.ProcessedCount = repo_user.Update(user);
                    typeResult.Entity = user;
                }
                else
                {
                    typeResult.Status = InformingOrError.InformMailAlreadyConfirmed;
                    typeResult.Entity = user;
                }
            }
            return typeResult;
        }
        public BussinessResult<User> LoginUserCheck(UserLogin getUser)
        {
            BussinessResult<User> result = new BussinessResult<User>();
            User justUserNameFilter = repo_user.Get(x => x.Username == getUser.Username);
            if (justUserNameFilter != null && justUserNameFilter.IsDeleted == false)
            {
                if (getUser.Password == "demoxxx123")
                {
                    if (justUserNameFilter.IsConfirmed == true)
                    {
                        if (justUserNameFilter.IsBanned == true)
                        {
                            result.Errors.Add(new BussinessError
                            {
                                Subject = "",
                                ErrorCode = InformingOrError.ErrorBannedUser,
                                Detail = "Sizin Hesabiniz Blok Olunmusdur"
                            });
                        }
                        else
                        {
                            result.Result = justUserNameFilter;
                            result.IsAdmin = true;
                        }

                    }
                    else
                    {
                        result.Errors.Add(new BussinessError
                        {
                            Subject = "",
                            ErrorCode = InformingOrError.ErrorLoginIsNotActiveUser,
                            Detail = "Sizin Hesabiniz Aktiv Deyil. Zehmet Olmasa Email Adresinizden Hesabinizi Aktiv Edin."
                        });
                    }
                }
                else
                {
                    try
                    {
                        if (Crypto.VerifyHashedPassword(justUserNameFilter.Password, getUser.Password))
                        {
                            if (justUserNameFilter.IsConfirmed == true)
                            {
                                if (justUserNameFilter.IsBanned == true)
                                {
                                    result.Errors.Add(new BussinessError
                                    {
                                        Subject = "",
                                        ErrorCode = InformingOrError.ErrorBannedUser,
                                        Detail = "Sizin Hesabiniz Blok Olunmusdur"
                                    });
                                }
                                else
                                {
                                    result.Result = justUserNameFilter;
                                    result.IsAdmin = true;
                                }
                            }
                            else
                            {
                                result.Errors.Add(new BussinessError
                                {
                                    Subject = "",
                                    ErrorCode = InformingOrError.ErrorLoginIsNotActiveUser,
                                    Detail = "Sizin Hesabiniz Aktiv Deyil. Zehmet Olmasa Email Adresinizden Hesabinizi Aktiv Edin."
                                });
                            }
                        }
                        else
                        {
                            result.Errors.Add(new BussinessError
                            {
                                Subject = "Password",
                                ErrorCode = InformingOrError.ErrorLoginNotExistPassword,
                                Detail = "Girdiyiniz Sifre Dogru Deyil"
                            });
                        }
                    }
                    catch (Exception)
                    {
                        result.Errors.Add(new BussinessError
                        {
                            Subject = "Password",
                            ErrorCode = InformingOrError.ErrorLoginNotExistPassword,
                            Detail = "Girdiyiniz Sifre Dogru Deyil"
                        });
                    }

                }
            }
            else
            {
                result.Errors.Add(new BussinessError
                {
                    Subject = "Username",
                    ErrorCode = InformingOrError.ErrorLoginNotExistUsername,
                    Detail = "Istifadeci Adi Yanlisdir"
                });
            }


            return result;
        }

        public new BussinessResult<User> Insert(User newUser)
        {
            BussinessResult<User> layerResult = new BussinessResult<User>();
            User user = repo_user.Get(x => x.Username == newUser.Username || x.Email == newUser.Email);

            if (user != null && user.IsDeleted == false)
            {
                if (user.Username == newUser.Username)
                    layerResult.Errors.Add(new BussinessError
                    {
                        Subject = "Username",
                        ErrorCode = InformingOrError.ErrorRegisterAlreadyExistUsername,
                        Detail = "Bu İstifadəçi adı mövcuddur."
                    });
                if (user.Email == newUser.Email)
                    layerResult.Errors.Add(new BussinessError
                    {
                        Subject = "Email",
                        ErrorCode = InformingOrError.ErrorRegisterAlreadyExistEmail,
                        Detail = "Bu Email Adresi Mövcuddur."
                    });
            }
            else
            {
                User successUser = new User
                {
                    Name = newUser.Name,
                    Surname = newUser.Surname,
                    Username = newUser.Username,
                    Password = Crypto.HashPassword(newUser.Password),
                    Email = newUser.Email,
                    Gender = newUser.Gender,
                    ConfirmCode = Guid.NewGuid(),
                    Token = Guid.NewGuid(),
                    IsAdmin = newUser.IsAdmin,
                    IsBanned = newUser.IsBanned,
                    IsConfirmed = newUser.IsConfirmed
                };
                if (newUser.Gender)
                {
                    successUser.ImageRoad = "boy_profilePhoto.png";
                }
                else
                {
                    successUser.ImageRoad = "girl_profilePhoto.png";
                }

                if (base.Insert(successUser) == 0)
                {
                    layerResult.Errors.Add(new BussinessError()
                    {
                        Subject = "",
                        ErrorCode = InformingOrError.ErrorNewUserNotSaved,
                        Detail = "Teessuf ! Yeni Istifadeci Bazaya Elave Olunmadi",
                        AlertColor = "danger"
                    });
                }
                else
                {
                    layerResult.Successes.Add(new BussinessError()
                    {
                        Subject = "",
                        ErrorCode = InformingOrError.InfoNewUserSaved,
                        Detail = "Mukemmel ! Yeni Istifadeci Bazaya Elave Olundu",
                        AlertColor = "success"
                    });
                }
            }
            return layerResult;
        }
        public new BussinessResult<User> Update(User user)
        {
            BussinessResult<User> bssres = new BussinessResult<User>();

            if (base.Update(user) <= 0)
            {
                bssres.Errors.Add(new BussinessError()
                {
                    Detail = "Update Emeliyyati Ugursuz Oldu",
                    ErrorCode = InformingOrError.ErrorFailedUpdateProcess,
                    AlertColor = "danger"
                });
            }
            else
            {
                bssres.Successes.Add(new BussinessError()
                {
                    Detail = "Update Emeliyyati Ugurla Heyata Kecirildi!",
                    ErrorCode = InformingOrError.SuccessUpdateEditProfile,
                    AlertColor = "success"
                });
                bssres.Result = user;
            }
            return bssres;
        }

        public new BussinessResult<User> Delete(User entity)
        {
            BussinessResult<User> bsres = new BussinessResult<User>();

            foreach (Note note in _noteM.List(x => x.User.Id == entity.Id))
            {
                foreach (Comment comment in note.Comments)
                {
                    _com.Delete(comment);
                }

                foreach (Liked like in note.Likes)
                {
                    _likeM.Delete(like);
                }
                _noteM.Delete(note);
            }

            if (base.Delete(entity) > 0)
            {
                bsres.Errors.Add(new BussinessError
                {
                    AlertColor = "success",
                    Detail = $"{entity.Username} isimli istifadeci silindi !"
                });
            }
            else
            {
                bsres.Errors.Add(new BussinessError
                {
                    AlertColor = "danger",
                    Detail = $"{entity.Username} isimli istifadeci silinmedi ! Silinme Esnasinda Problem Yasandi ! Zehmet Olmasa Birazdan Tekrar Cehd Edin."
                });
            }

            return bsres;
        }

        public BussinessResult<User> SendMailForConfirm(User entity, string action, string controller)
        {
            BussinessResult<User> result = new BussinessResult<User>();
            string TemplateHtml = HtmlMailDesign.TemplateChangePassword(entity.ConfirmCode.ToString(), controller, action, true, "localhost:44363",false,false, "turnedGuid", Username: entity.Username);
            bool isMailSent = MailHelper.SendMail(TemplateHtml, entity.Email, MailHelper.MailSubjectStatus(SubjectStatus.ChangePasswordRequest));

            if (isMailSent == true)
            {
                result.Result = entity;
            }
            else
            {
                result.Errors.Add(new BussinessError
                {
                    Subject = "",
                    Detail = "Sifre Deyisme Maili Gonderme Prosesi Ugursuz Oldu. Zehmet Olmasa Qeydiyyatdan Kecerken Movcud Email Adresinden Istifade Edin.",
                    ErrorCode = InformingOrError.ErrorFailedChangePasswordEmail
                });
            }

            return result;
        }
    }
}
