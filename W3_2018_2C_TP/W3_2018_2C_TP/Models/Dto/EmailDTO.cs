using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace W3_2018_2C_TP.Models.Dto
{
    public class EmailDTO
    {
        public int IdPedido { get; set; }
        public int Acciones { get; set; }
        public List<string> NuevosInvitados { get; set; }
    }
}