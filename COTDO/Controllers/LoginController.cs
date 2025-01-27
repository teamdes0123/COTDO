using COTDO.Interfaces.Repository;
using COTDO.Models;
using COTDO.Models.ViewModels.Login;
using COTDO.Repository.Login;
using COTDO.Services.Login;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace COTDO.Controllers
{  
    public class LoginController : Controller
    {
        private readonly LoginService _loginService;
        private readonly Response _response;

        public LoginController()
        {
            _loginService = new LoginService();
            _response = new Response();
        }

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Login(LoginVM vm)
        {
            var response = await _loginService.LogIn(vm);

            if (!response.IsSuccess)
            {
                return Json(new { _response = response });
            }

            var user = response.Data as User;

            var username = $"{user.Nombres} {user.Apellido1}".Trim();

            try
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, username ?? ""),
                    new Claim(ClaimTypes.Email, vm.Username ?? ""),
                    new Claim("Cedula", user.Cedula ?? ""),
                    new Claim("TiempoEnServicio", user.TiempoEnServicio?.ToString() ?? "0")
                };


                var identity = new ClaimsIdentity(claims, "Forms");

                var authTicket = new FormsAuthenticationTicket(
                   1,                       // Versión del ticket
                   username,                // Nombre del usuario
                   DateTime.Now,            // Fecha de emisión
                   DateTime.Now.AddMinutes(30), // Expiración
                   false,        // Persistente o no
                   string.Join("|", claims.Select(c => $"{c.Type}:{c.Value}")) // Datos adicionales
                );

                string encryptedTicket = FormsAuthentication.Encrypt(authTicket);

                var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket)
                {
                    HttpOnly = true,
                    Secure = FormsAuthentication.RequireSSL
                };

                Response.Cookies.Add(cookie);
                _response.Data = null;
                return Json(new { _response = response });
            }
            catch (Exception ex)
            {
                throw ex;
            }       
        }

        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Login");
        }

        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Register(RegisterVM vm) 
        {
            if (!ModelState.IsValid)
            {
                _response.IsSuccess = false;
                _response.Message = "Debe completar todos los campos del formulario.";
                return Json(new { _response });
            }

            var response = await _loginService.CreateAccount(vm);

            return Json(new { _response = response });
        }

        public ActionResult ForgotPassword()
        {
            return View();
        }
    }
}