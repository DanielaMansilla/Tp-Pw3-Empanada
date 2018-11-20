using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace W3_2018_2C_TP.Models.Dto
{
    public class PedidoDTO
    {
        public int idPedido { get; set; }
        public int IdUsuarioResponsable { get; set; }
        public int Rol { get; set; }
        public string RolS { get; set; }
        public string EstadoS { get; set; }
        public string NombreNegocio { get; set; }
        public int Estado { get; set; }
        public System.DateTime FechaCreacion { get; set; }
    }
}