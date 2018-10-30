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
            //p.FechaCreacion = DateTime.Now.ToString("dd/MM/yyyy");

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

        public void Modificar(Pedido pedido)
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

