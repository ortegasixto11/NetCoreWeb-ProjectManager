using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using ProjectManager.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


public static class SessionHelper
{
    //Esta variable las inicializo en el Startup
    static internal IHttpContextAccessor _contextAccessor;

    public static void SetUserLoggin(ApplicationUser user)
    {
        _contextAccessor.HttpContext.Session.SetString("UserLoggin", JsonConvert.SerializeObject(user));
        if (user.Role.ToUpper() == "ADMIN")
            _contextAccessor.HttpContext.Session.SetString("UserLogginIsAdmin", JsonConvert.SerializeObject(true));
        else
            _contextAccessor.HttpContext.Session.SetString("UserLogginIsAdmin", JsonConvert.SerializeObject(false));
    }

    public static bool UserLogginIsAdmin()
    {
        if (_contextAccessor.HttpContext.Session.GetString("UserLogginIsAdmin") == null) return false;
        else return JsonConvert.DeserializeObject<bool>(_contextAccessor.HttpContext.Session.GetString("UserLogginIsAdmin"));
    }

    public static ApplicationUser GetUserLoggin()
    {
        if (_contextAccessor.HttpContext.Session.GetString("UserLoggin") == null)
            return null;
        else
            return JsonConvert.DeserializeObject<ApplicationUser>(_contextAccessor.HttpContext.Session.GetString("UserLoggin"));
    }

    public static void ResetUserLoggin()
    {
        _contextAccessor.HttpContext.Session.SetString("UserLoggin", "");
    }

}

