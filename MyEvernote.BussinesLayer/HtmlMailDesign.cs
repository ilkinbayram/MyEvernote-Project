using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyEvernote.BussinesLayer
{
    public class HtmlMailDesign
    {
        public static string TemplateActivation1(string Guid, string ControllerName, string ActionName, bool securedDomain = false, string domainName = "", bool hasArea = false, bool guidIsID = false, string GuidSendingParameter = "", string areaName = "", string Username = "", bool IsQuicklyRegister = false, string quicklyPassowrd = "123")
        {
            string url;
            if (securedDomain == true)
            {
                if (guidIsID == true)
                {
                    if (hasArea == true)
                    {
                        url = $"https://{domainName}/{areaName}/{ControllerName}/{ActionName}/{Guid}";
                    }
                    else
                    {
                        url = $"https://{domainName}/{ControllerName}/{ActionName}/{Guid}";
                    }
                }
                else
                {
                    if (hasArea == true)
                    {
                        url = $"https://{domainName}/{areaName}/{ControllerName}/{ActionName}?{GuidSendingParameter}={Guid}";
                    }
                    else
                    {
                        url = $"https://{domainName}/{ControllerName}/{ActionName}?{GuidSendingParameter}={Guid}";
                    }
                }
            }
            else
            {
                if (guidIsID == true)
                {
                    if (hasArea == true)
                    {
                        url = $"http://{domainName}/{areaName}/{ControllerName}/{ActionName}/{Guid}";
                    }
                    else
                    {
                        url = $"http://{domainName}/{ControllerName}/{ActionName}/{Guid}";
                    }
                }
                else
                {
                    if (hasArea == true)
                    {
                        url = $"http://{domainName}/{areaName}/{ControllerName}/{ActionName}?{GuidSendingParameter}={Guid}";
                    }
                    else
                    {
                        url = $"http://{domainName}/{ControllerName}/{ActionName}?{GuidSendingParameter}={Guid}";
                    }
                }
            }

            string template = $"<div style = 'background-color: rgb(255, 228, 209); text-align: center !important;'><div style = 'text-align: center !important;'><img style = 'width: 100%; display: inline-block !important; border-radius: 25px;' src = 'https://media.gettyimages.com/photos/security-user-interface-technology-with-fingerprint-picture-id964033964?s=612x612' alt = 'activation'/></div><div style = 'text-align: center;'><h3 style = 'color: rgb(81, 0, 0); font-size: 64px !important;'> Xos Gorduk { Username} </h3><p style = 'text-align: center;color: darkblue; font-size: 48px !important;'> Sehifemize Daxil Oldugunuza Gore Tesekkur Edirik. Hesabinizi Aktiv Isletmeyiniz Ucun Aktivasiya Linki Ile Hesabinizi Tesdiqlemelisiniz." + (IsQuicklyRegister==true?$" Qeyd: Sizin Parolaniz - {quicklyPassowrd}":"") + $"</p></div ></div><div style = 'background-color: rgb(255, 228, 209); text-align: center;'><p><b style = 'font-size: 48px !important; color: #2e2e2e'><i> Sizin Aktivasiya Linkiniz:</i></b><a style = 'text-decoration: none !important; padding: 20px !important; box-shadow: inset 1px 1px 20px 2px rgb(179, 247, 205) !important; font-weight: bold; color: rgb(0, 204, 79) !important;' href = '{url}' target = '_blank'><span style = 'width: 500px; font-size: 48px !important; height: 100px; text-align:center; border-radius: 20px; background-color: rgb(0, 57, 22); padding: 20px !important;'> Aktiv Et</span></a></p></div>";

            return template;
        }

        public static string TemplateChangePassword(string Guid, string ControllerName, string ActionName, bool securedDomain = true, string domainName = "", bool hasArea = false, bool guidIsID = false, string GuidSendingParameter = "", string areaName = "", string Username = "")
        {
            string url;
            if (securedDomain == true)
            {
                if (guidIsID == true)
                {
                    if (hasArea == true)
                    {
                        url = $"https://{domainName}/{areaName}/{ControllerName}/{ActionName}/{Guid}";
                    }
                    else
                    {
                        url = $"https://{domainName}/{ControllerName}/{ActionName}/{Guid}";
                    }
                }
                else
                {
                    if (hasArea == true)
                    {
                        url = $"https://{domainName}/{areaName}/{ControllerName}/{ActionName}?{GuidSendingParameter}={Guid}";
                    }
                    else
                    {
                        url = $"https://{domainName}/{ControllerName}/{ActionName}?{GuidSendingParameter}={Guid}";
                    }
                }
            }
            else
            {
                if (guidIsID == true)
                {
                    if (hasArea == true)
                    {
                        url = $"http://{domainName}/{areaName}/{ControllerName}/{ActionName}/{Guid}";
                    }
                    else
                    {
                        url = $"http://{domainName}/{ControllerName}/{ActionName}/{Guid}";
                    }
                }
                else
                {
                    if (hasArea == true)
                    {
                        url = $"http://{domainName}/{areaName}/{ControllerName}/{ActionName}?{GuidSendingParameter}={Guid}";
                    }
                    else
                    {
                        url = $"http://{domainName}/{ControllerName}/{ActionName}?{GuidSendingParameter}={Guid}";
                    }
                }
            }

            string template = $"<div style = 'background-color: rgb(255, 228, 209); text-align: center !important;'><div style = 'text-align: center !important;'><img style = 'width: 100%; display: inline-block !important; border-radius: 25px;' src = 'https://media.gettyimages.com/photos/security-user-interface-technology-with-fingerprint-picture-id964033964?s=612x612' alt = 'activation'/></div><div style = 'text-align: center;'><h3 style = 'color: rgb(81, 0, 0); font-size: 64px !important;'> Xos Gorduk { Username} </h3><p style = 'text-align: center;color: darkblue; font-size: 48px !important;'> Sifrenizi Yenilemek Ucun Asagidaki Linke Tikladiqdan Yeni Sifrenizi Yonlendirildiyiniz Sehifede Tekrar Qeyd Etmekle Heyata Kecire Bilesiniz.</p></div ></div><div style = 'background-color: rgb(255, 228, 209); text-align: center;'><p><b style = 'font-size: 48px !important; color: #2e2e2e'><i> Şifrə Yeniləmə Linkiniz:</i></b><a style = 'text-decoration: none !important; padding: 20px !important; box-shadow: inset 1px 1px 20px 2px rgb(179, 247, 205) !important; font-weight: bold; color: rgb(0, 204, 79) !important;' href = '{url}' target = '_blank'><span style = 'width: 500px; font-size: 48px !important; height: 100px; text-align:center; border-radius: 20px; background-color: rgb(0, 57, 22); padding: 20px !important;'> Şifrəni Yenilə</span></a></p></div>";

            return template;
        }
    }
}
