using COTDO.Interfaces.Repository;
using COTDO.Models.ViewModels.Login;
using COTDO.Repository.Login;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace COTDO.Controllers
{  
    public class LoginController : Controller
    {
        private readonly ILoginRepository _loginRepository;

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Login(LoginVM vm)
        {
            return Json(new { success = true});
        }

        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Register(RegisterVM vm) 
        {
            if (!ModelState.IsValid)
            {
                return Json(new { success = false });
            }

            return Json(new { success = true });
        }
    }
}