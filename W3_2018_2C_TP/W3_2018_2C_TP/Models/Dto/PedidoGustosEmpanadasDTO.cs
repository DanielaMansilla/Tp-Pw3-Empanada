using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace W3_2018_2C_TP.Models.Dto
{
    public class PedidoGustosEmpanadasDTO
    {
        public Pedido Pedido { get; set; }
        public List<GustoEmpanadaDTO> GustosDisponibles { get; set; }
        public List<UsuarioDTO> Invitados { get; set; }
        public int Acciones { get; set; }

        public PedidoGustosEmpanadasDTO()
        {
            GustosDisponibles = new List<GustoEmpanadaDTO>();
            Invitados = new List<UsuarioDTO>();

        }

        public PedidoGustosEmpanadasDTO(Pedido pedido, List<GustoEmpanada> gustosPedido,
            List<GustoEmpanada> gustosModel, List<UsuarioDTO> invitados)
        {
            Pedido = pedido;
            GustosDisponibles = new List<GustoEmpanadaDTO>();
            Invitados = invitados;
            foreach (var gusto in gustosModel)
            {
                if (gustosPedido.Any(g => g.IdGustoEmpanada == gusto.IdGustoEmpanada))
                    GustosDisponibles.Add(new GustoEmpanadaDTO
                    { Id = gusto.IdGustoEmpanada, Nombre = gusto.Nombre, IsSelected = true });
                else
                    GustosDisponibles.Add(new GustoEmpanadaDTO
                    { Id = gusto.IdGustoEmpanada, Nombre = gusto.Nombre, IsSelected = false });

            }

        }

    }
}