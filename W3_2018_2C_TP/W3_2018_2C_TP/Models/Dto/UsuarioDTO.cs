using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace W3_2018_2C_TP.Models.Dto
{
    public class UsuarioDTO
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public bool CompletoPedido { get; set; }

        public UsuarioDTO() { }

        public UsuarioDTO(int id, string email)
        {
            Id = id;
            Email = email;
        }

        public UsuarioDTO(int id, string email, bool completoPedido)
        {
            Id = id;
            Email = email;
            CompletoPedido = completoPedido;
        }
    }
}