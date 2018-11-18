﻿using System;
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
            List<Pedido> pedidos = pedidoServicio.obtenerListaPorUsuario(user);
            return View(pedidos);
        }

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult IniciarSesion(Usuario u)
        {
            if (ModelState.IsValid)
            {
                Usuario user = servicio.IniciarSesion(u);
                return RedirectToAction("Index", user);
            }
            else
            {
                return View("Index", u);
            }
        }
    
        public ActionResult Error()
            {
                return View();
            }
        }
}