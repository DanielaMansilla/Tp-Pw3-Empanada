using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using W3_2018_2C_TP.Models.Dto;

namespace W3_2018_2C_TP.Servicios
{
    public class GustoEmpanadasServicio
    {
        Entities Contexto = new Entities();
        public List<GustoEmpanada> ObtenerGustos(List<GustoEmpanadaDTO> pgeGustosDisponibles)
        {
            List<GustoEmpanada> gustosSeleccionados = new List<GustoEmpanada>();
            foreach (var gusto in pgeGustosDisponibles)
            {
                if (gusto.IsSelected)
                    gustosSeleccionados.Add(GetById(gusto.Id));
            }

            return gustosSeleccionados;
        }

        public GustoEmpanada GetById(int id)
        {
            return Contexto.GustoEmpanada.Single(e => e.IdGustoEmpanada == id);
        }

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

        public int CantidadTotalDeEmpanadas(int idPedido)
        {
            if (idPedido != 0)
            {
                var ipgeu = Contexto.InvitacionPedidoGustoEmpanadaUsuario.Where(i => i.IdPedido == idPedido).ToList();
                if (ipgeu.Count != 0)
                    return ipgeu.Sum(i => i.Cantidad);
            }
            return 0;
        }
    }
}