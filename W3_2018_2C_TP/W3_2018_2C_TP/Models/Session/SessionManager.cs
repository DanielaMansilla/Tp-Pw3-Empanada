using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace W3_2018_2C_TP
{
    public static class SessionManager
    {
        public static Usuario UsuarioLogin
        {
            get
            {
                return HttpContext.Current.Session["UsuarioLogueado"] as Usuario;       
            }

            set
            {
                HttpContext.Current.Session["UsuarioLogueado"] = value;
            }
        }
    }
}