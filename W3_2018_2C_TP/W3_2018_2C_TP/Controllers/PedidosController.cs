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

        //punto 3) se inicia un pedido
        [HttpGet]
        public ActionResult Iniciar()
        {
            PedidoGustosEmpanadasDTO pedidoNuevo = new PedidoGustosEmpanadasDTO();
            var gustos = _servicioGustoEmpanada.GetAll();
            foreach (var gusto in gustos)
            {
                pedidoNuevo.GustosDisponibles.Add(new GustoEmpanadaDTO(gusto.IdGustoEmpanada, gusto.Nombre));
            }
            ViewBag.iniciar = true;
            return View(pedidoNuevo);
        }

        //punto 3) a) se inicia un pedido desde cero
        [HttpPost]
        public ActionResult CrearPedido(PedidoGustosEmpanadasDTO pedidoGustosEmpanadas)
        {
            if (ModelState.IsValid)
            {
                var pedidoNuevo = _servicioPedido.CrearPedidoDesdeCero(pedidoGustosEmpanadas);

                var usuarios = _servicioInvitacionPedido.Crear(pedidoNuevo, pedidoGustosEmpanadas.Invitados, SessionManager.UsuarioSession.IdUsuario);

                //_servicioEmail.ArmarMailInicioPedido(usuarios, pedidoNuevo.IdPedido);
                return RedirectToAction("Iniciado", new { id = pedidoNuevo.IdPedido });

            }
            pedidoGustosEmpanadas.Invitados = _servicioUsuario.GetAllByEmail(pedidoGustosEmpanadas.Invitados);
            ViewBag.iniciar = false;
            return View("Iniciar", pedidoGustosEmpanadas);
        }

        //punto 3) b) copiar pedido
        [HttpPost]
        public ActionResult CopiarPedido(int id)
        {
            PedidoGustosEmpanadasDTO pedido = _servicioPedido.CopiarPedido(id);
            ViewBag.iniciar = true;
            return View("Iniciar", pedido);
        }

        // punto 3) c) pedido iniciado
        [HttpGet]
        public ActionResult Iniciado()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Iniciado(int idPedido)
        {
            return View();
        }

        //punto 4) Listar pedidos
        [HttpGet]
        public ActionResult Lista()
        {
            if (SessionManager.UsuarioSession != null)
            {
                List<Pedido> pedidos = _servicioPedido.ListarPedidosResponsableInvitado(SessionManager.UsuarioSession.IdUsuario);
                return View(pedidos);
            }

            //Falta logica de redirigir a /Home/Lista cuando se loguee despues que lo pateo por aca

            return RedirectToAction("Login", "Home");
        }

        //punto 5) Editar pedido
        [HttpGet]
        public ActionResult Editar(int id)
        {
            Pedido pedidoEditar = _servicioPedido.ObtenerPorId(id);
            if (pedidoEditar.EstadoPedido.Nombre == "Cerrado")
            {
                return RedirectToAction("Detalle", "Pedidos", id);
            }
            else
            {
                //Lleno el ddl con los gustos por pedido
                ViewBag.ListaGusto = _servicioPedido.ObtenerGustosPorPedido(pedidoEditar.IdPedido);
                ViewBag.UsuariosInvitados = _servicioPedido.ObtenerUsuariosInvitados(pedidoEditar.IdPedido);

                return View(pedidoEditar);
            }
        }

        [HttpPost]
        public ActionResult Editar(Pedido pedido)
        {
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

        //punto 6) Eliminar pedido
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

        //punto 7) Elegir gustos
        [HttpGet]
        public ActionResult ElegirGustos()
        {
            var gustos = _servicioGustoEmpanada.GetAll();
            return View(gustos);
        }

        //punto 8) Detalle del pedido
        [HttpGet]
        public ActionResult Detalle(int id)
        {
            if (SessionManager.UsuarioSession != null)
            {
                Pedido p = _servicioPedido.ObtenerPorId(id);

                if (p.EstadoPedido.Nombre == "Cerrado" || p.IdUsuarioResponsable != SessionManager.UsuarioSession.IdUsuario)
                {
                    return View(p);
                }else
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