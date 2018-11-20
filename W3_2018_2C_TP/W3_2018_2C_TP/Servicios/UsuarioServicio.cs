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

        public void CrearUsuario(Usuario usuario)
        {
            Context.Usuario.Add(usuario);
            Context.SaveChanges();
        }

        public Usuario Login(Usuario usuario)
        {
            var user = Context.Usuario.FirstOrDefault(u => u.Email == usuario.Email && u.Password == usuario.Password);
            //if (user == null)
            //    throw new UsuarioInvalidoException();
            return user;
        }

        public List<Usuario> GetAll(string email)
        {
            return Context.Usuario.Where(u => String.IsNullOrEmpty(email) || u.Email.Contains(email)).ToList();
        }

        public UsuarioDTO GetByEmail(string email, bool completoPedido)
        {
            var usuario = Context.Usuario.FirstOrDefault(u => u.Email == email);
            if (usuario != null)
                return new UsuarioDTO(usuario.IdUsuario, usuario.Email, completoPedido);
            return null;
        }
        public List<UsuarioDTO> GetAllByEmail(List<UsuarioDTO> usuarioVm)
        {
            List<UsuarioDTO> lista = new List<UsuarioDTO>();
            foreach (var usuario in usuarioVm)
            {
                lista.Add(GetByEmail(usuario.Email, usuario.CompletoPedido));
            }

            return lista;
        }

       

    }
}