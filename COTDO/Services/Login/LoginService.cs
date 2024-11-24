using COTDO.Models;
using COTDO.Models.ViewModels.Login;
using COTDO.Repository.Login;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Security;

namespace COTDO.Services.Login
{
    public class LoginService
    {
        private readonly LoginRepository _loginRepository;
        private readonly Response _response;

        public LoginService()
        {
            _loginRepository = new LoginRepository();
            _response = new Response();
        }

        public async Task<Response> CreateAccount(RegisterVM vm)
        {
            bool IsExists = await _loginRepository.IsExistsAccount(vm.Correo);

            if (IsExists)
            {
                _response.IsSuccess = false;
                _response.Message = "El correo ingresado ya está asociado a una cuenta existente. Por favor, utilice un correo diferente para registrarse.";
                return _response;
            }
            else
            {
                var user = await _loginRepository.GetUserByCedula(vm.Cedula);

                if (user == null)
                {
                    _response.IsSuccess = false;
                    _response.Message = "Usted no es elegible para este proceso de concurso.";
                    return _response;
                }
                else
                {
                    if (user.TiempoEnServicio < 5)
                    {
                        _response.IsSuccess = false;
                        _response.Message = "Usted no cumple con el tiempo de servicio requerido para participar en este proceso de concurso.";
                        return _response;
                    }

                    _response.IsSuccess = await _loginRepository.CreateUser(vm, user.CodCargo);

                    if (_response.IsSuccess)
                    {
                        _response.Message = "Su cuenta ha sido creada exitosamente. Será redirigido a la ventana de inicio de sesión.";
                        return _response;
                    }
                    else
                    {
                        _response.Message = "No se pudo crear su cuenta en este momento. Por favor, intente nuevamente más tarde.";
                        return _response;
                    }
                }
            }          
        }

        public async Task<Response> LogIn(LoginVM vm)
        {
            if (!await _loginRepository.IsExistsAccount(vm.Username))
            {
                _response.IsSuccess = false;
                _response.Message = "No se encontró ninguna cuenta registrada con este correo.";
                return _response;
            }
            else
            {
                var user = await _loginRepository.GetCandidate(vm);

                if (user == null)
                {
                    _response.IsSuccess = false;
                    _response.Message = "Usuario o contraseña incorrectos.";
                    return _response;
                }
                else
                {
                    _response.IsSuccess = true;
                    _response.Message = "";
                    _response.Data = user;
                    return _response;
                }
            }         
        }
    }
}