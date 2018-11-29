using System;
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
                return RedirectToAction("Login", "Home", new { url });  
            }

            List<GustoEmpanada> listaGustos = _servicioGustoEmpanada.GetAll();
            var mails = _servicioUsuario.obtenerMailsUsuarios(SessionManager.UsuarioSession.Email);

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
                return RedirectToAction("Login", "Home", new { url });
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
                return RedirectToAction("Login", "Home", new { url });
            }
           
            List<Pedido> pedidos = _servicioPedido.ListarPedidosResponsableInvitado(SessionManager.UsuarioSession.IdUsuario);
            return View(pedidos);
        }

        [HttpGet]
        public ActionResult Editar(int id)
        {
            if (SessionManager.UsuarioSession == null)
            {
                string url = Url.Content(Request.Url.PathAndQuery);
                return RedirectToAction("Login", "Home", new {url });
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
                        break;
                    }
                }
            }
            List<Usuario> gustosElegidos = _servicioInvitacionPedido.obtenerGustosConfirmados(id);
            Session["mails"] = mails;
            ViewBag.GustosElegidos = gustosElegidos;
            ViewBag.Lista = new MultiSelectList(InitGustos, "IdGustoEmpanada", "Nombre");
            ViewBag.Mails = new MultiSelectList(mails, "IdUsuario", "Email");
            ViewBag.Mailseleccionados = new MultiSelectList(mailsNuevos, "IdUsuario", "Email");

            return View(pedido);
        }

        [HttpPost]
        public ActionResult Editar(Pedido pedido, string EnviarInvitaciones, string btnConfirmar)
        {
            if (SessionManager.UsuarioSession == null)
            {
                string url = Url.Content(Request.Url.PathAndQuery);
                return RedirectToAction("Login", "Home", new { url });
            }
            if (ModelState.IsValid)
            {
                if (btnConfirmar == "Confirmar")
                {
                    _servicioPedido.cerrarPedido(pedido);
                }
                List<Usuario> mails = Session["mails"] as List<Usuario>;
                _servicioPedido.EnviarInvitaciones(pedido, EnviarInvitaciones, mails);
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
                return RedirectToAction("Login", "Home", new { url });
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
                return RedirectToAction("Login", "Home", new { url });
            }
            Session["EliminarMensaje"] = "Pedido " + p.NombreNegocio + " ha sido eliminado exitosamente";
            _servicioPedido.Eliminar(p.IdPedido);            
            return RedirectToAction("Lista", "Pedidos");
        }

        [HttpGet]
        public ActionResult Elegir(int id)
        {
            if (SessionManager.UsuarioSession == null)
            {
                string url = Url.Content(Request.Url.PathAndQuery);
                return RedirectToAction("Login", "Home", new { url });
            }

            Pedido pedido = _servicioPedido.ObtenerPorId(id);
            InvitacionPedido token = _servicioInvitacionPedido.GetInvitacionPedidoPorPedido(id, SessionManager.UsuarioSession.IdUsuario);
            List<GustoEmpanada> InitGustos = _servicioPedido.ObtenerGustosPorPedido(id);
            //List<int> lista = new List<int>();
            //ViewBag.ListaNueva = lista;
            ViewBag.Token = token.Token;
            ViewBag.Lista = new MultiSelectList(InitGustos, "IdGustoEmpanada", "Nombre");

            return View(pedido);
        }

        [HttpGet]
        public ActionResult Detalle(int id)
        {
            if (SessionManager.UsuarioSession == null)
            {
                string url = Url.Content(Request.Url.PathAndQuery);
                return RedirectToAction("Login", "Home", new { url });
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
                        break;
                    }
                }
            }

            ViewBag.Lista = new MultiSelectList(InitGustos, "IdGustoEmpanada", "Nombre");
            ViewBag.Mailseleccionados = new MultiSelectList(mailsNuevos, "IdUsuario", "Email");

            return View(pedido);

        }

        [HttpGet]
        public ActionResult Copiar(int id)
        {
            if (SessionManager.UsuarioSession == null)
            {
                string url = Url.Content(Request.Url.PathAndQuery);
                return RedirectToAction("Login", "Home", new { url });
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
                        break;
                    }
                }
            }

            
            ViewBag.Lista = new MultiSelectList(InitGustos, "IdGustoEmpanada", "Nombre");
            ViewBag.Mails = new MultiSelectList(mails, "IdUsuario", "Email");
            ViewBag.Mailseleccionados = new MultiSelectList(mailsNuevos, "IdUsuario", "Email");

            return View(pedido);

        }
    }
  
}