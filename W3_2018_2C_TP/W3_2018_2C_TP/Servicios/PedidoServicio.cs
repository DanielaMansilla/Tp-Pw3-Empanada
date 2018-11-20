using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using W3_2018_2C_TP.Models.Dto;
using W3_2018_2C_TP.Models.Enums;

namespace W3_2018_2C_TP.Servicios
{

    public class PedidoServicio
    {
        public Entities Context = new Entities();
        private readonly GustoEmpanadasServicio _servicioGustoEmpanada = new GustoEmpanadasServicio();
        private readonly InvitacionPedidoServicio _servicioInvitacionPedido = new InvitacionPedidoServicio();
 
        public Pedido CrearPedidoDesdeCero(PedidoGustosEmpanadasDTO pge)
        {
            var pedido = pge.Pedido;
           
            pedido.FechaCreacion = DateTime.Now;
            pedido.IdUsuarioResponsable =SessionManager.UsuarioSession.IdUsuario;
            pedido.IdEstadoPedido = (int)EstadosDelPedido.Abierto;
            List<GustoEmpanada> gustosSeleccionados = new List<GustoEmpanada>();
            foreach (var gusto in pge.GustosDisponibles)
            {
                if (gusto.IsSelected)
                    gustosSeleccionados.Add(Context.GustoEmpanada.FirstOrDefault(ge => ge.IdGustoEmpanada == gusto.Id));
            }
           
            pedido.GustoEmpanada = gustosSeleccionados;
            Context.Pedido.Add(pedido);
            Context.SaveChanges();
            return pedido;
        }

        public void Inicializar(int id)
        {

        }

        public List<Pedido> Listar()
        {
            return Context.Pedido.ToList();
        }

        /// <summary>
        /// Metodo que ordena por fecha de forma descendente todos los pedidos
        /// </summary>
        /// <returns> List<Pedido> </returns>
        public List<Pedido> ListarDescendente()
        {
            return Context.Pedido.OrderByDescending(p => p.FechaCreacion).ToList();
        }
       
        /// <summary>
        ///  Me devuelve la lista de pedidos en los que soy responsable e invitado, mediante id de usuario.
        /// </summary>
        /// <param name="idUsuario"> idUsuario </param>
        /// <returns> List<Pedido> </returns>
        public List<Pedido> ListarPedidosResponsableInvitado(int idUsuario)
        {
            // forma corta
            //List<Pedido> pedidos = Context.InvitacionPedido.Include("Pedido")
            //                .Where(o => o.IdUsuario == idUsuario)
            //                .Select(i => i.Pedido).ToList();

            List<Pedido> pedidosResultado = new List<Pedido>();

            List<InvitacionPedido> imvitacionesDelUsuario = Context.InvitacionPedido.Include("Pedido")
                          .Where(o => o.IdUsuario == idUsuario).ToList();

            foreach (var inv in imvitacionesDelUsuario)
            {
                pedidosResultado.Add(inv.Pedido);
            }

            //List<Pedido> pedidosResponsable = Context.Pedido.Where(p => p.IdUsuarioResponsable == idUsuario).ToList();

            //pedidosResultado.AddRange(pedidosResponsable);

            return pedidosResultado.OrderByDescending(p => p.FechaCreacion).ToList();
        }

        public List<Pedido> obtenerListaPorUsuario(Usuario user)
        {
            var estado = Context.Pedido.Where(c => c.IdUsuarioResponsable == user.IdUsuario).ToList();
            return estado;
        }

        public void Eliminar(int id)
        {

            var invitaciones = Context.InvitacionPedido.Where(i => i.IdPedido == id).ToList();
            Context.InvitacionPedido.RemoveRange(invitaciones);
            Context.SaveChanges();

            var gustosPedido = Context.InvitacionPedidoGustoEmpanadaUsuario.Where(i => i.IdPedido == id).ToList();
            Context.InvitacionPedidoGustoEmpanadaUsuario.RemoveRange(gustosPedido);
            Context.SaveChanges();

            Pedido pedidoEliminar = Context.Pedido.FirstOrDefault(pedido => pedido.IdPedido == id);

            pedidoEliminar.GustoEmpanada.Clear();

            Context.Pedido.Remove(pedidoEliminar);
            Context.SaveChanges();
        }


        public Pedido ObtenerPorId(int id)
        {
            return Context.Pedido.FirstOrDefault(pedido => pedido.IdPedido == id);
        }

        public int ObtenerInvitacionesConfirmadas(int id)
        {
            return Context.InvitacionPedido.Where(c => c.IdPedido == id)
                .Where(c => c.Completado == true).Count();
        }

        /// <summary>
        /// Edita el pedido que anteriormente fue pasado por id
        /// </summary>
        /// <param name="pedido"></param>
        public void Editar(Pedido pedido)
        {
            Pedido pedidoEditar = Context.Pedido.FirstOrDefault(o => o.IdPedido == pedido.IdPedido);

            //pedidoEditar.IdUsuarioResponsable = pedido.IdUsuarioResponsable;
            pedidoEditar.NombreNegocio = pedido.NombreNegocio;
            pedidoEditar.Descripcion = pedido.Descripcion;
            //pedidoEditar.IdEstadoPedido = pedido.IdEstadoPedido;
            pedidoEditar.PrecioUnidad = pedido.PrecioUnidad;
            pedidoEditar.PrecioDocena = pedido.PrecioDocena;
            pedidoEditar.FechaModificacion = DateTime.Now;
            //pedidoEditar.EstadoPedido = pedido.EstadoPedido;
            pedidoEditar.InvitacionPedido = pedido.InvitacionPedido;
            pedidoEditar.InvitacionPedidoGustoEmpanadaUsuario = pedido.InvitacionPedidoGustoEmpanadaUsuario;
            pedidoEditar.Usuario = pedido.Usuario;
            Context.SaveChanges();
        }

        /// <summary>
        /// Obtengo los usuarios invitados de un pedido mediante el id de pedido
        /// </summary>
        /// <param name="idPedido"></param>
        /// <returns> List<Usuario> </returns>
        public List<Usuario> ObtenerUsuariosInvitados(int idPedido)
        {
            //int idUsuarioResponsable = Context.Pedido.FirstOrDefault(p => p.IdPedido == idPedido).IdUsuarioResponsable;

            //List<Usuario> listUsuariosInvitados = Context.Pedido.Include("InvitacionPedido").Where(p => p.IdPedido == idPedido).Select(u => u.Usuario).Where(u => u.IdUsuario != idUsuarioResponsable).ToList();

            List<Usuario> listUsuariosInvitados = Context.InvitacionPedido.Where(ip => ip.IdPedido == idPedido).Select(ip => ip.Usuario).ToList();

            return listUsuariosInvitados;
        }

        public List<GustoEmpanada> ObtenerGustos()
        {
            return Context.GustoEmpanada.ToList();
        }

        public List<GustoEmpanada> ObtenerGustosPorPedido(int id)
        {
            return Context.Pedido.FirstOrDefault(p => p.IdPedido == id).GustoEmpanada.ToList();
        }
    }
}

