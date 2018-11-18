using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace W3_2018_2C_TP.Models.Dto
{
    public class GustosPedidoUsuarioDTO: GustoEmpanadaDTO
    {
        public List<InvitacionPedidoGustoEmpanadaUsuario> GustosElegidosUsuario { get; set; }
        public InvitacionPedido InvitacionPedido { get; set; }
    }
}