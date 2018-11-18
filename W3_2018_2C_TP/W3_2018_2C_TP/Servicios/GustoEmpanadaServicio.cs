using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services.Description;

namespace W3_2018_2C_TP.Servicios
{
    public class GustoEmpanadaServicio 
    {

        Entities Contexto = new Entities();

        public List<GustoEmpanada> GetAll()
        {
            return Contexto.GustoEmpanada.ToList();
        }

        public List<GustoEmpanada> GetGustosEnPedido(int idPedido)
        {
            
            var pedido = Contexto.Pedido.Include("GustoEmpanada").FirstOrDefault(p => p.IdPedido == idPedido);
            return pedido.GustoEmpanada.ToList();
        }

        public List<InvitacionPedidoGustoEmpanadaUsuario> GetGustosDeUsuario(int idPedido, int idUsuario)
        {
            return Contexto.InvitacionPedidoGustoEmpanadaUsuario.Where(ip => ip.IdPedido == idPedido && ip.IdUsuario == idUsuario).ToList();
        }
    }
}