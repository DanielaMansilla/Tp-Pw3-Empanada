using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace W3_2018_2C_TP.Models
{
    public class ConfirmarGusto
    {
        public int IdUsuario { get; set; }
        public Guid Token { get; set; }
        public InvitacionPedidoGustoEmpanadaUsuario[] GustosEmpanadasCantidad { get; set; }
    }
}