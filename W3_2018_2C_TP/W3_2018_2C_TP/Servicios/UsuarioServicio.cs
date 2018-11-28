using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using W3_2018_2C_TP.Models.Dto;

namespace W3_2018_2C_TP.Servicios
{
    public class UsuarioServicio
    {
        public Entities Context = new Entities();

        public Usuario IniciarSesion(Usuario u)
        {
            var estado = Context.Usuario.Where(c => c.Email == u.Email)
                 .Where(c => c.Password == u.Password).First();
            return estado;
        }

        public List<Usuario> obtenerMailsUsuarios(string Email)
        {
            return Context.Usuario
                .Where(e => e.Email != Email)
                .ToList();
        }
        public List<Usuario> obtenerMailsUsuarios()
        {
            return Context.Usuario.ToList();
        }
    }
}