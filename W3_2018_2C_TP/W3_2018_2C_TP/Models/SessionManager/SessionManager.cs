using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace W3_2018_2C_TP
{
    public static class SessionManager
    {
        internal static readonly object UsuarioLogin;

        public static Usuario UsuarioSession
        {
            get
            {
                return HttpContext.Current.Session["UserSession"] as Usuario;
            }
            set
            {
                HttpContext.Current.Session["UserSession"] = value;
            }
        }
    }
}