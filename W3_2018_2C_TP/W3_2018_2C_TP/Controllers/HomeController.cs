using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using W3_2018_2C_TP.Servicios;

namespace W3_2018_2C_TP.Controllers
{
    public class HomeController : Controller
    {
        UsuarioServicio servicio = new UsuarioServicio();
        PedidoServicio pedidoServicio = new PedidoServicio();
        public ActionResult Index(Usuario user)
        {
            if (SessionManager.UsuarioSession == null)
            {
                return RedirectToAction("Login");
            }
            List<Pedido> pedidos = pedidoServicio.obtenerListaPorUsuario(user);
            return View(pedidos);
        }

        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(Usuario u, string url)
        {
            if (ModelState.IsValid)
            {
                Usuario user = servicio.IniciarSesion(u);

                //Usuario logueado actualmente lo guardo en session
                SessionManager.UsuarioSession = user;

                if (!string.IsNullOrEmpty(url))
                {
                    //si no es nulo redirige
                    return Redirect(url);
                }
                return RedirectToAction("Lista", "Pedidos", user.IdUsuario);
            }
            else
            {
                return View(u);
            }
        }

        [HttpPost]
        public ActionResult IniciarSesion(Usuario u,string url)
        {
            Usuario nuevo = new Usuario();
            if (ModelState.IsValid)
            {
                Usuario user = servicio.IniciarSesion(u);

                //Usuario logueado actualmente lo guardo en session
                SessionManager.UsuarioSession = user;

                if (!string.IsNullOrEmpty(url))
                {
                    //si no es nulo redirige
                    return Redirect(url);
                }
                return RedirectToAction("Lista", "Pedidos", user.IdUsuario);
            }
            else
            {
                return RedirectToAction("Index", nuevo);
            }
        }

        public ActionResult Logout()
        {
            SessionManager.UsuarioSession = null;
            return RedirectToAction("Login");
        }
    }
}