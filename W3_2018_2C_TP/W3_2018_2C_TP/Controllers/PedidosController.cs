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
            if (Session["EliminarMensaje"] != null)
            {
                ViewBag.Mensaje = Session["EliminarMensaje"].ToString();
            }
            List<Pedido> pedidos = pedidoServicio.ListarDescendente();
            return View(pedidos);
        }

        [HttpGet]
        public ActionResult Editar(int id)
        {
            Pedido pedidoModificar = pedidoServicio.ObtenerPorId(id);
            return View(pedidoModificar);
        }

        [HttpPost]
        public ActionResult Editar(Pedido pedido)
        {
            pedidoServicio.Editar(pedido);
            return RedirectToAction("Lista", "Pedidos");

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
        public ActionResult Detalle(int id)
        {
            Pedido p = pedidoServicio.ObtenerPorId(id);

            if (p.EstadoPedido.Nombre == "Cerrado" || p.Usuario.Rol == Rol.Invitado)
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