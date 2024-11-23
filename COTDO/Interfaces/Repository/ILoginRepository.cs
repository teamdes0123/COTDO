using COTDO.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COTDO.Interfaces.Repository
{
    public interface ILoginRepository
    {
        Task<bool> IsValidUser(string cedula);
        Task<User> GetUserByCedula(string cedula);
    }
}
