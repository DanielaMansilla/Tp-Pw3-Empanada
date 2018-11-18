using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using W3_2018_2C_TP.Models.Enums;
using W3_2018_2C_TP.Servicios;
using W3_2018_2C_TP.Models.Dto;

namespace W3_2018_2C_TP.Controllers
{
    public class PedidosController : Controller
    {
       
        private readonly PedidoServicio _servicioPedido = new PedidoServicio();
        private readonly GustoEmpanadaServicio _servicioGustoEmpanada = new GustoEmpanadaServicio();
        private readonly InvitacionPedidoServicio _servicioInvitacionPedido = new InvitacionPedidoServicio();
        private readonly UsuarioServicio _servicioUsuario = new UsuarioServicio();
        //private readonly EmailServicio _servicioEmail = new EmailServicio();

        [HttpGet]
        public ActionResult Iniciar()
        {
            PedidoGustosEmpanadasDTO pgeVm = new PedidoGustosEmpanadasDTO();
            var gustos = _servicioGustoEmpanada.GetAll();
            foreach (var gusto in gustos)
            {
                pgeVm.GustosDisponibles.Add(new GustoEmpanadaDTO(gusto.IdGustoEmpanada, gusto.Nombre));

            }
            ViewBag.iniciar = true;
            return View(pgeVm);
        }
        [HttpPost]
        public ActionResult CrearPedido(PedidoGustosEmpanadasDTO pedidoGustosEmpanadas)
        {
            if (ModelState.IsValid)
            {
                var pedidoNuevo = _servicioPedido.CrearPedidoDesdeCero(pedidoGustosEmpanadas);
                //var usuarios = _servicioInvitacionPedido.Crear(pedidoNuevo, pedidoGustosEmpanadas.Invitados, Sesion.IdUsuario);
                //_servicioEmail.ArmarMailInicioPedido(usuarios, pedidoNuevo.IdPedido);
                return RedirectToAction("Iniciado", new { id = pedidoNuevo.IdPedido });

            }
            //pedidoGustosEmpanadas.Invitados = _servicioUsuario.GetAllByEmail(pedidoGustosEmpanadas.Invitados);
            ViewBag.iniciar = false;
            return View("Iniciar", pedidoGustosEmpanadas);
        }

       
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
            List<Pedido> pedidos = _servicioPedido.ListarDescendente();
            return View(pedidos);
        }

        [HttpGet]
        public ActionResult Editar(int id)
        {
            Pedido pedidoModificar = _servicioPedido.ObtenerPorId(id);
            return View(pedidoModificar);
        }

        [HttpPost]
        public ActionResult Editar(Pedido pedido)
        {
            _servicioPedido.Editar(pedido);
            return RedirectToAction("Lista", "Pedidos");

        }

        [HttpGet]
        public ActionResult Eliminar(int id)
        {
            ViewBag.Cantidad = _servicioPedido.ObtenerInvitacionesConfirmadas(id);
            return View(_servicioPedido.ObtenerPorId(id));
        }

        [HttpPost]
        public ActionResult Eliminar(Pedido p)
        {
            Session["EliminarMensaje"] = "Pedido " + p.NombreNegocio + " ha sido eliminado exitosamente";
            _servicioPedido.Eliminar(p.IdPedido);            
            return RedirectToAction("Lista", "Pedidos");
        }

        public ActionResult Elegir()
        {
            var gustos = _servicioGustoEmpanada.GetAll();
            return View(gustos);
        }
        [HttpGet]
        public ActionResult Detalle(int id)
        {
            Pedido p = _servicioPedido.ObtenerPorId(id);

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