using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace W3_2018_2C_TP.Servicios
{

    public class PedidoServicio
    {
        public Entities Context = new Entities();

        public void Agregar(Pedido p)
        {
            var estado = Context.EstadoPedido.FirstOrDefault(e => e.IdEstadoPedido == 1);
            var user = Context.Usuario.FirstOrDefault(u => u.IdUsuario == 1);
            p.FechaCreacion = DateTime.Now ;
            p.EstadoPedido = estado;
            p.Usuario = user;
            Context.Pedido.Add(p);
            Context.SaveChanges();
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

            List<Pedido> pedidosResponsable = Context.Pedido.Where(p => p.IdUsuarioResponsable == idUsuario).ToList();

            pedidosResultado.AddRange(pedidosResponsable);

            return pedidosResultado.OrderByDescending(p => p.FechaCreacion).ToList();
        }


        public void Eliminar(int id)
        {
            Pedido pedidoEliminar = Context.Pedido.FirstOrDefault(pedido => pedido.IdPedido == id);
            Context.Pedido.Remove(pedidoEliminar);
            Context.SaveChanges();
        }


        public Pedido ObtenerPorId(int id)
        {
            return Context.Pedido.FirstOrDefault(pedido => pedido.IdPedido == id);
        }

        public void Editar(Pedido pedido)
        {
            Pedido pedidoEditar = Context.Pedido.FirstOrDefault(o => o.IdPedido == pedido.IdPedido);

            pedidoEditar.IdUsuarioResponsable = pedido.IdUsuarioResponsable;
            pedidoEditar.NombreNegocio = pedido.NombreNegocio;
            pedidoEditar.Descripcion = pedido.Descripcion;
            pedidoEditar.IdEstadoPedido = pedido.IdEstadoPedido;
            pedidoEditar.PrecioUnidad = pedido.PrecioUnidad;
            pedidoEditar.PrecioDocena = pedido.PrecioDocena;
            pedidoEditar.FechaCreacion = pedido.FechaCreacion;
            pedidoEditar.EstadoPedido = pedido.EstadoPedido;
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

