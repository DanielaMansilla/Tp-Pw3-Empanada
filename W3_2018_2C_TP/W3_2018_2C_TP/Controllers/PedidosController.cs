using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
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

            List<Pedido> pedidos = pedidoServicio.Listar();
            return View(pedidos);
        }
        [HttpGet]
        public ActionResult Editar(int id)
        {
            Pedido pedidoModificar = pedidoServicio.ObtenerPorId(id);
            return View(pedidoModificar);
        }

        [HttpPost]
        public ActionResult Modificar(Pedido pedido)
        {
            pedidoServicio.Modificar(pedido);
            return RedirectToAction("Lista", "Pedidos");

        }
        public ActionResult Eliminar(int id)
        {
            pedidoServicio.Eliminar(id);
            return RedirectToAction("Lista", "Pedidos");
        }

        public ActionResult Elegir()
        {
            return View();
        }

        public ActionResult Detalle()
        {
            return View();
        }
    }
  
}