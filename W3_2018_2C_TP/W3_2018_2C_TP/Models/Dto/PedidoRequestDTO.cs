using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace W3_2018_2C_TP.Models.Dto
{
    public class PedidoRequestDTO
    {
        public List<GustoEmpanadasCantidadDTO> GustoEmpanadasCantidad { get; set; }
        public int IdUsuario { get; set; }
        public int IdPedido { get; set; }
        public string Token { get; set; }
    }
}