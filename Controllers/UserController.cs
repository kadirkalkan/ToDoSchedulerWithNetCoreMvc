using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PitonProject.Models.Entities;
using PitonProject.Models.Managers;

namespace PitonProject.Controllers
{
    public class UserController : Controller
    {
        External external = new External();
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(User loggedUser, string ReturnUrl = null)
        {
            if ((loggedUser = LoginUser(loggedUser)) != null)
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, loggedUser.UserName)
                };

                var userIdentity = new ClaimsIdentity(claims, "login");

                ClaimsPrincipal principal = new ClaimsPrincipal(userIdentity);
                await HttpContext.SignInAsync(principal);

                //Just redirect to our index after logging in. 
                if (!string.IsNullOrEmpty(ReturnUrl))
                    return Redirect(ReturnUrl + "?enum=" + Models.Enums.EnumClass.ProcessType.success + "&processMessage=Başarıyla Giriş Yaptınız.");
                else
                    return RedirectToAction("Index", "Home", new { @enum = Models.Enums.EnumClass.ProcessType.success, processMessage = "Başarıyla Giriş Yaptınız." });
            }
            else
            {
                setProcess(Models.Enums.EnumClass.ProcessType.danger, "Kullanıcı adı veya Şifrenizi Hatalı Girdiniz");
                loggedUser = new User();
            }
            return View(loggedUser);
        }

        public async Task<IActionResult> LogOut()
        {
            await HttpContext.SignOutAsync();

            return RedirectToAction("Index", "Home");
        }

        private User LoginUser(User loggedUser)
        {
            using (var context = new DatabaseContext())
            {
                string md5Pass = external.GenerateSha256(loggedUser.Password);
                User confirmedUser = context.Users.FirstOrDefault(x => x.UserName.Equals(loggedUser.UserName) && x.Password.Equals(md5Pass));
                return confirmedUser;
            }
        }

        private bool isUserNameExist(string userName)
        {
            using (var context = new DatabaseContext())
            {
                return context.Users.FirstOrDefault(x => x.UserName.Equals(userName)) != null;
            }
        }

        public IActionResult Register()
        {
            return View(new User());
        }

        [HttpPost]
        public IActionResult Register(User registeredUser)
        {
            if (isUserNameExist(registeredUser.UserName))
            {
                setProcess(Models.Enums.EnumClass.ProcessType.danger, "Bu kullanıcı adı kullanılıyor.");
                return View(registeredUser);
            }
            else
            {

            }
            if (!string.IsNullOrEmpty(registeredUser.UserName) && !string.IsNullOrEmpty(registeredUser.Password))
            {
                using (var context = new DatabaseContext())
                {
                    User newUser = new User()
                    {
                        UserName = registeredUser.UserName,
                        Password = external.GenerateSha256(registeredUser.Password)
                    };
                    context.Users.Add(newUser);
                    context.SaveChanges();
                    return RedirectToAction("Login", new { @enum = Models.Enums.EnumClass.ProcessType.success, processMessage = "Kayıt Başarılı! Giriş Yapabilirsiniz." });
                }
            }
            else
            {
                setProcess(Models.Enums.EnumClass.ProcessType.danger, "Geçersiz Veri Girişi! Lütfen Tekrar Deneyin.");
                return View(registeredUser);
            }
        }


        public void setProcess(Models.Enums.EnumClass.ProcessType processType, string processMessage = null, bool waitParameter = false)
        {
            try
            {
                if (processType != Models.Enums.EnumClass.ProcessType.initialize)
                {
                    if (processType == Models.Enums.EnumClass.ProcessType.danger && String.IsNullOrEmpty(processMessage))
                    {
                        processMessage = "Bir hata meydana geldi lütfen yetkiliye bildirin.";
                    }
                    ViewBag.processType = processType.ToString();
                    ViewBag.processMessage = processMessage;
                    if (waitParameter)
                    {
                        ViewBag.processWait = 3000;
                    }
                }
            }
            catch (Exception ex)
            {
                external.addLog(Models.Enums.EnumClass.LogType.error, ex.ToString(), "UserController", "setProcess");
            }
        }
    }
}
