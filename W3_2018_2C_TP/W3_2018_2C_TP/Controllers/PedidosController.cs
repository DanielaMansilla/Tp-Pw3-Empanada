﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using W3_2018_2C_TP.Servicios;
using W3_2018_2C_TP.Models.Dto;

namespace W3_2018_2C_TP.Controllers
{
    public class PedidosController : Controller
    {
       
        //Se instancia los servicios
        private readonly PedidoServicio _servicioPedido = new PedidoServicio();
        private readonly GustoEmpanadasServicio _servicioGustoEmpanada = new GustoEmpanadasServicio();
        private readonly InvitacionPedidoServicio _servicioInvitacionPedido = new InvitacionPedidoServicio();
        private readonly UsuarioServicio _servicioUsuario = new UsuarioServicio();
        private readonly EmailServicio _servicioEmail = new EmailServicio();

        [HttpGet]
        public ActionResult Iniciar()
        {
            if (SessionManager.UsuarioSession == null)
            {
                string url = Url.Content(Request.Url.PathAndQuery);
                return RedirectToAction("Login", "Home", new { redirigir = url });
            }
            List<GustoEmpanada> listaGustos = _servicioGustoEmpanada.GetAll();
            var mails = _servicioUsuario.obtenerMailsUsuarios();

            ViewBag.Lista = new MultiSelectList(listaGustos, "IdGustoEmpanada", "Nombre");
            ViewBag.Mails = new MultiSelectList(mails, "IdUsuario", "Email");

            return View();
        }
        [HttpPost]
        public ActionResult CrearPedido(Pedido pedido)
        {
            if (SessionManager.UsuarioSession == null)
            {
                string url = Url.Content(Request.Url.PathAndQuery);
                return RedirectToAction("Login", "Home", new { redirigir = url });
            }
            if (pedido.GustoDeEmpanadaSeleccionados == null)
            {
                TempData["ErrorMessage"] = "No se han seleccionado gustos";
                return RedirectToAction("Iniciar");
            }

            if (ModelState.IsValid)
            {
                var pedidoNuevo = _servicioPedido.CrearPedidoDesdeCero(pedido);
                TempData["IdPedido"] = pedidoNuevo.IdPedido;
                return RedirectToAction("Iniciado");
            }

            List<GustoEmpanada> listaGustos = _servicioGustoEmpanada.GetAll();
            var mails = _servicioUsuario.obtenerMailsUsuarios();
            ViewBag.Lista = new MultiSelectList(listaGustos, "IdGustoEmpanada", "Nombre");
            ViewBag.Mails = new MultiSelectList(mails, "IdUsuario", "Email"); 

            return View("Iniciar", pedido);
        }

        public ActionResult Iniciado()
        {
            Pedido p = new Pedido();
            p.IdPedido = Convert.ToInt32(TempData["IdPedido"]);
            return View(p);
        }

        [HttpGet]
        public ActionResult Lista()
        {
            if (SessionManager.UsuarioSession == null)
            {
                string url = Url.Content(Request.Url.PathAndQuery);
                return RedirectToAction("Login", "Home", new { redirigir = url });
            }
            if (Session["EliminarMensaje"] != null)
            {
                List<Pedido> pedidos = _servicioPedido.ListarPedidosResponsableInvitado(SessionManager.UsuarioSession.IdUsuario);
                return View(pedidos);
            }

            //Falta logica de redirigir a /Home/Lista cuando se loguee despues que lo pateo por aca

            return RedirectToAction("Login", "Home");
        }

        [HttpGet]
        public ActionResult Editar(int id)
        {
            if (SessionManager.UsuarioSession == null)
            {
                string url = Url.Content(Request.Url.PathAndQuery);
                return RedirectToAction("Login", "Home", new { redirigir = url });
            }
            Pedido pedido = _servicioPedido.ObtenerPorId(id);

            List<GustoEmpanada> InitGustos = _servicioPedido.ObtenerGustos();
            
            foreach (GustoEmpanada item in pedido.GustoEmpanada)
            {
                InitGustos.Remove(item);
            }

            List<Usuario> mails = _servicioUsuario.obtenerMailsUsuarios();
            List<Usuario> mailsNuevos = new List<Usuario>();

            for (int i = 0; i < mails.Count; i++)
            {
                foreach (InvitacionPedido item in pedido.InvitacionPedido)
                {
                    if (mails[i].IdUsuario == item.IdUsuario && item.IdUsuario != SessionManager.UsuarioSession.IdUsuario)
                    {
                        mailsNuevos.Add(mails[i]);
                        mails.Remove(mails[i]);
                    }
                }
            }

            if (pedido.EstadoPedido.Nombre == "Cerrado")
            {
                return RedirectToAction("Detalle", "Pedidos", pedido.IdPedido);
            }
            else
            {

                ViewBag.Lista = new MultiSelectList(InitGustos, "IdGustoEmpanada", "Nombre");
                ViewBag.Mails = new MultiSelectList(mails, "IdUsuario", "Email");
                ViewBag.Mailseleccionados = new MultiSelectList(mailsNuevos, "IdUsuario", "Email");

                return View(pedido);
            }
          
          
        }

        [HttpPost]
        public ActionResult Editar(Pedido pedido)
        {
            if (SessionManager.UsuarioSession == null)
            {
                string url = Url.Content(Request.Url.PathAndQuery);
                return RedirectToAction("Login", "Home", new { redirigir = url });
            }
            if (ModelState.IsValid)
            {
                //Falta logica de reenvio de email dependediendo la opcion elegida en el drop down list

                _servicioPedido.Editar(pedido);
                return RedirectToAction("Lista", "Pedidos");
            }
            else
            {
                return Redirect("/Pedidos/Editar/" + pedido.IdPedido);
            }

        }

        [HttpGet]
        public ActionResult Eliminar(int id)
        {
            if (SessionManager.UsuarioSession == null)
            {
                string url = Url.Content(Request.Url.PathAndQuery);
                return RedirectToAction("Login", "Home", new { redirigir = url });
            }
            ViewBag.Cantidad = _servicioPedido.ObtenerInvitacionesConfirmadas(id);
            return View(_servicioPedido.ObtenerPorId(id));
        }

        [HttpPost]
        public ActionResult Eliminar(Pedido p)
        {
            if (SessionManager.UsuarioSession == null)
            {
                string url = Url.Content(Request.Url.PathAndQuery);
                return RedirectToAction("Login", "Home", new { redirigir = url });
            }
            Session["EliminarMensaje"] = "Pedido " + p.NombreNegocio + " ha sido eliminado exitosamente";
            _servicioPedido.Eliminar(p.IdPedido);            
            return RedirectToAction("Lista", "Pedidos");
        }

        public ActionResult Elegir()
        {
            if (SessionManager.UsuarioSession == null)
            {
                string url = Url.Content(Request.Url.PathAndQuery);
                return RedirectToAction("Login", "Home", new { redirigir = url });
            }
            var gustos = _servicioGustoEmpanada.GetAll();
            return View(gustos);
        }

        [HttpGet]
        public ActionResult Detalle(int id)
        {
            if (SessionManager.UsuarioSession == null)
            {
                string url = Url.Content(Request.Url.PathAndQuery);
                return RedirectToAction("Login", "Home", new { redirigir = url });
            }
            Pedido p = _servicioPedido.ObtenerPorId(id);

                if (p.EstadoPedido.Nombre == "Cerrado" || p.IdUsuarioResponsable != SessionManager.UsuarioSession.IdUsuario)
                {
                    return View(p);
                }

                else
                {
                    //Lo reenvio a la lista de pedidos
                    return RedirectToAction("Lista");
                }
            }
            else
            {
                //Falta logica de redirigir a /Pedidos/Detalle/id cuando se loguee despues que lo pateo al Login

                return RedirectToAction("Login", "Home");
            }
            
        }
    }
  
}