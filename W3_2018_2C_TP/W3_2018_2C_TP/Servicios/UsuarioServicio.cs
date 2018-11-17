using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace W3_2018_2C_TP.Servicios
{
    public class UsuarioServicio
    {
        public Entities Context = new Entities();

        public Usuario UsuarioLogueado(int idUsuario)
        {
            Usuario usuario = Context.Usuario.FirstOrDefault(u => u.IdUsuario == idUsuario);

            return usuario;
        }
    }
}