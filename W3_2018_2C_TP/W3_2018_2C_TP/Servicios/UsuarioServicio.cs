using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace W3_2018_2C_TP.Servicios
{
    public class UsuarioServicio
    {
        public Entities Context = new Entities();

<<<<<<< HEAD
        public Usuario UsuarioLogueado(int idUsuario)
        {
            Usuario usuario = Context.Usuario.FirstOrDefault(u => u.IdUsuario == idUsuario);

            return usuario;
=======
        public Usuario IniciarSesion(Usuario u)
        {
            var estado = Context.Usuario.Where(c => c.Email == u.Email)
                 .Where(c => c.Password == u.Password).First();
            return estado;
>>>>>>> 1d19eb745deb66ade10678bbde3b5f04887bcc17
        }
    }
}