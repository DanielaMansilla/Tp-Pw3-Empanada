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

        //Creo metodo que ordena por fecha de forma descendente los pedidos
        public List<Pedido> ListarDescendente()
        {
            return Context.Pedido.OrderByDescending(p => p.FechaCreacion).ToList();
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

        public void Editar(Pedido pedido)
        {
            Pedido pedidoModificar = Context.Pedido.FirstOrDefault(o => o.IdPedido == pedido.IdPedido);

            pedidoModificar.IdUsuarioResponsable = pedido.IdUsuarioResponsable;
            pedidoModificar.NombreNegocio = pedido.NombreNegocio;
            pedidoModificar.Descripcion = pedido.Descripcion;
            pedidoModificar.IdEstadoPedido = pedido.IdEstadoPedido;
            pedidoModificar.PrecioUnidad = pedido.PrecioUnidad;
            pedidoModificar.PrecioDocena = pedido.PrecioDocena;
            pedidoModificar.FechaCreacion = pedido.FechaCreacion;
            pedidoModificar.EstadoPedido = pedido.EstadoPedido;
            pedidoModificar.InvitacionPedido = pedido.InvitacionPedido;
            pedidoModificar.InvitacionPedidoGustoEmpanadaUsuario = pedido.InvitacionPedidoGustoEmpanadaUsuario;
            pedidoModificar.Usuario = pedido.Usuario;
            Context.SaveChanges();
        }
        public List<GustoEmpanada> ObtenerGustos()
        {
            return Context.GustoEmpanada.ToList();
        }
    }
}

