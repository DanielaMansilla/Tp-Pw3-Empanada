using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace W3_2018_2C_TP.Models.Dto
{
    public class MailDTO
    {
        public string Email { get; set; }
        public List<GustoEmpanada> GustosEmpanadas { get; set; }
        public List<InvitadosMailDTO> InvitadosMail { get; set; }
        public int CantidadTotal { get; set; }
        public decimal PrecioTotal { get; set; }
        public string Link { get; set; }

        public MailDTO()
        {
            GustosEmpanadas = new List<GustoEmpanada>();
            InvitadosMail = new List<InvitadosMailDTO>();
        }
    }
}