using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using W3_2018_2C_TP.Models.Enums;
using W3_2018_2C_TP.Servicios;

namespace W3_2018_2C_TP.Controllers
{
    public class PedidosController : Controller
    {

        PedidoServicio pedidoServicio = new PedidoServicio();
        UsuarioServicio usuarioServicio = new UsuarioServicio();

        [HttpGet]
        public ActionResult Iniciar()
        {
            //iniciar pedido
            //a)Desde cero
            ViewBag.ListaGusto = pedidoServicio.ObtenerGustos();
            return View();
        }

        [HttpPost]
        public ActionResult Iniciar(Pedido p)
        {
            pedidoServicio.Agregar(p);

            return RedirectToAction("Lista", "Pedidos");
        }

        //[HttpPost]
        //public ActionResult Iniciar(int idOtroPedido)
        //{
        //    pedidoServicio.Inicializar(idOtroPedido);//no funca
        //    return RedirectToAction("Lista", "Pedidos");
        //}
        [HttpGet]
        public ActionResult Iniciado()
        {
            //iniciar pedido
            //c)pedido iniciado
            return View();
        }

        [HttpPost]
        public ActionResult Iniciado(int idPedido)
        {
            //iniciar pedido
            //c)pedido iniciado
            return View();
        }

        [HttpGet]
        public ActionResult Lista()
        {
<<<<<<< HEAD
            List<Pedido> pedidos = pedidoServicio.Listar();

            return View(pedidos);
        }

        [HttpPost]
        public ActionResult Lista(int idUsuario)
        {
            List<Pedido> pedidos = pedidoServicio.ListarPedidosResponsableInvitado(idUsuario);

            ViewBag.UsuarioLogueado = usuarioServicio.UsuarioLogueado(idUsuario).IdUsuario;

=======
            if (Session["EliminarMensaje"] != null)
            {
                ViewBag.Mensaje = Session["EliminarMensaje"].ToString();
            }
            List<Pedido> pedidos = pedidoServicio.ListarDescendente();
>>>>>>> 1d19eb745deb66ade10678bbde3b5f04887bcc17
            return View(pedidos);
        }

        [HttpGet]
        public ActionResult Editar(int idPedido)
        {
            Pedido pedidoEditar = pedidoServicio.ObtenerPorId(idPedido);

            if (pedidoEditar.EstadoPedido.Nombre == "Cerrado")
            {
                return RedirectToAction("Detalle", "Pedidos", pedidoEditar.IdPedido);
            }
            else
            {
                //Lleno el ddl con los gustos por pedido
                ViewBag.ListaGusto = pedidoServicio.ObtenerGustosPorPedido(pedidoEditar.IdPedido);
                ViewBag.UsuariosInvitados = pedidoServicio.ObtenerUsuariosInvitados(pedidoEditar.IdPedido);
                
                return View(pedidoEditar);
            }
        }

        [HttpPost]
        public ActionResult Editar(Pedido pedido)
        {
            if (ModelState.IsValid)
            {
                pedidoServicio.Editar(pedido);
                return RedirectToAction("Lista", "Pedidos");
            }
            else
            {
                return View(pedido.IdPedido);
            }
        }

        [HttpGet]
        public ActionResult Eliminar(int id)
        {
            ViewBag.Cantidad = pedidoServicio.ObtenerInvitacionesConfirmadas(id);
            return View(pedidoServicio.ObtenerPorId(id));
        }

        [HttpPost]
        public ActionResult Eliminar(Pedido p)
        {
            Session["EliminarMensaje"] = "Pedido " + p.NombreNegocio + " ha sido eliminado exitosamente";
            pedidoServicio.Eliminar(p.IdPedido);            
            return RedirectToAction("Lista", "Pedidos");
        }

        public ActionResult Elegir()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Detalle(int idPedido)
        {
            Pedido p = pedidoServicio.ObtenerPorId(idPedido);

            ViewBag.UsuarioLogueado = usuarioServicio.UsuarioLogueado(p.IdUsuarioResponsable).IdUsuario;

            if (p.EstadoPedido.Nombre == "Cerrado" || p.Usuario.IdUsuario == usuarioServicio.UsuarioLogueado(p.IdUsuarioResponsable).IdUsuario)
            {
                return View(p);
            }

            else
            {
                //Lo reenvio a la lista de pedidos
                return RedirectToAction("Lista");
            }
        }
    }
  
}