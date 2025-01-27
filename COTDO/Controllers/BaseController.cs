using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;

namespace COTDO.Controllers
{
    public class BaseController : Controller
    {
        protected ClaimsPrincipal CurrentUser => User as ClaimsPrincipal;
    }
}