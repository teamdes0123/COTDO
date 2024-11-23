using COTDO.Interfaces.Repository;
using COTDO.Models;
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
        private readonly LoginRepository _loginRepository = new LoginRepository();
        private readonly Response _response = new Response();

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
        public async Task<ActionResult> Register(RegisterVM vm) 
        {
            if (!ModelState.IsValid)
            {
                _response.IsSuccess = false;
                _response.Message = "Debe completar todos los campos del formulario.";
                return Json(new { _response });
            }

            var user = await _loginRepository.GetUserByCedula(vm.Cedula);

            if (user == null) {
                _response.IsSuccess = false;
                _response.Message = "Usted no es elegible para este proceso de concurso.";
                return Json(new { _response });
            }
            else
            {
                if (user.TiempoEnServicio < 5)
                {
                    _response.IsSuccess = false;
                    _response.Message = "Usted no cumple con el tiempo de servicio requerido para participar en este proceso de concurso.";
                    return Json(new { _response });
                }

                _response.IsSuccess = await _loginRepository.CreateUser(vm, user.CodCargo);

                if (_response.IsSuccess)
                {
                    _response.Message = "Su cuenta ha sido creada exitosamente. Será redirigido a la ventana de inicio de sesión.";
                    return Json(new { _response });
                }
                else
                {
                    _response.Message = "No se pudo crear su cuenta en este momento. Por favor, intente nuevamente más tarde.";
                    return Json(new { _response });
                }
            }
        }
    }
}