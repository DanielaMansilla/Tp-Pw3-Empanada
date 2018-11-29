using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using W3_2018_2C_TP.Models;
using W3_2018_2C_TP.Models.Dto;

namespace W3_2018_2C_TP.Servicios
{
    public class InvitacionPedidoServicio
    {
        public Entities Contexto = new Entities();

        public List<int> Crear(Pedido pedido, List<UsuarioDTO> invitados, int idUsuarioResponsable)
        {
            var idUsuarios = GetInvitados(invitados, idUsuarioResponsable);
            foreach (var id in idUsuarios)
            {
                Contexto.InvitacionPedido.Add(new InvitacionPedido
                {
                    IdPedido = pedido.IdPedido,
                    IdUsuario = id,
                    Token = Guid.NewGuid(),
                    Completado = false
                });

            }

            Contexto.SaveChanges();
            return idUsuarios;
        }

        public InvitacionPedido Crear(Pedido pedido, int idUsuario)
        {
            InvitacionPedido nuevaInvitacion = new InvitacionPedido();
            Pedido p = Contexto.Pedido.Find(pedido.IdPedido);
            nuevaInvitacion.IdPedido = p.IdPedido;
            nuevaInvitacion.IdUsuario = idUsuario;
            nuevaInvitacion.Completado = false;
            nuevaInvitacion.Token = Guid.NewGuid();
            Contexto.InvitacionPedido.Add(nuevaInvitacion);
            Contexto.SaveChanges();
            return nuevaInvitacion;
        }

        private List<int> GetInvitados(List<UsuarioDTO> invitados, int? idUsuarioResponsable)
        {
            List<int> idUsuarios = new List<int>();
            foreach (var invitado in invitados)
            {
                var usuario = Contexto.Usuario.FirstOrDefault(u =>
                string.Equals(u.Email, invitado.Email));
                if (usuario != null)
                    idUsuarios.Add(usuario.IdUsuario);
            }
            if (idUsuarioResponsable != null)
                idUsuarios.Add((int)idUsuarioResponsable);
            return idUsuarios;
        }

        public List<UsuarioDTO> GetByIdPedido(Pedido pedido, int usuarioSesion)
        {
            return Contexto.InvitacionPedido.Where(ip => ip.IdPedido == pedido.IdPedido && ip.IdUsuario != usuarioSesion)
                .Select(ip => new UsuarioDTO { Id = ip.Usuario.IdUsuario, Email = ip.Usuario.Email, CompletoPedido = ip.Completado }).ToList();
        }

        public InvitacionPedido GetInvitacionPedidoPorPedido(int id, int idUsuario)
        {
            return Contexto.InvitacionPedido.FirstOrDefault(ip => ip.IdPedido == id && ip.IdUsuario == idUsuario);
        }

        public void Modificar(int idPedido, List<UsuarioDTO> invitados, int accion)
        {
            var invitacionPedidoModel = Contexto.InvitacionPedido.Where(ip => ip.IdPedido == idPedido).ToList();
            var nuevosInvitados = new List<int>();
        }
        public bool ValidarGustos(ConfirmarGusto datos)
        {
            try
            {
                var estadoPedido = Contexto.InvitacionPedido.Where(i => i.Token == datos.Token).FirstOrDefault();

                if (estadoPedido.Pedido.IdEstadoPedido == 2)
                {
                    return false;
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        public void ConfirmarGustos(ConfirmarGusto datos)
        {
            int idpedido = Contexto.InvitacionPedido
                            .Where(p => p.Token == datos.Token)
                            .Select(p => p.IdPedido).First();
            foreach (InvitacionPedidoGustoEmpanadaUsuario item in datos.GustosEmpanadasCantidad)
            {
                item.IdPedido = idpedido;
                item.IdUsuario = datos.IdUsuario;
                Contexto.InvitacionPedidoGustoEmpanadaUsuario.Add(item);
                Contexto.SaveChanges();
            }
        }

        public List<Usuario> obtenerGustosConfirmados(int idPedido)
        {
            //List<InvitacionPedido> invitacionesDelUsuario = Contexto.InvitacionPedido.Include("Pedido")
            //  .Where(o => o.IdPedido == idPedido).ToList();
            List<Usuario> usuarios = Contexto.Usuario.ToList();
            //int i = 0;
            //foreach (InvitacionPedido item in invitacionesDelUsuario)
            //{
            //    if (usuarios[i].IdUsuario != item.IdUsuario)
            //    {
            //        usuarios.RemoveAt(i);
            //        i++;
            //    }
            //}
            return usuarios;
        }
    }

}