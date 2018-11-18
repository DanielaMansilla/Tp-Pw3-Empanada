using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
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

        public InvitacionPedidoGustoEmpanadaUsuario ElegirGustos()
        {
            return null;
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
            //se agrega tambien como invitado al usuario que realizo inicio el pedido
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
            var invitadosModel = GetInvitados(invitados, Sesion.IdUsuario);
            foreach (var invitacionPedido in invitacionPedidoModel)
            {
                if (!invitadosModel.Contains(invitacionPedido.IdUsuario))
                    Contexto.InvitacionPedido.Remove(invitacionPedido);
            }
            var nuevosInvitados = new List<int>();
            foreach (var invitado in invitadosModel)
            {
                if (!invitacionPedidoModel.Select(ip => ip.IdUsuario).Contains(invitado))
                {
                    var nuevaInvitacionPedido = new InvitacionPedido
                    {
                        IdUsuario = invitado,
                        IdPedido = idPedido,
                        Token = Guid.NewGuid(),
                        Completado = false
                    };
                    Contexto.InvitacionPedido.Add(nuevaInvitacionPedido);
                    nuevosInvitados.Add(nuevaInvitacionPedido.IdUsuario);
                }
            }
        //    ServicioEmail servicioMail = new ServicioEmail();
        //    switch (accion)
        //    {
        //        case (int)EmailAcciones.ANadie:
        //            break;
        //        case (int)EmailAcciones.EnviarSoloALosNuevos:
        //            servicioMail.ArmarMailInicioPedido(nuevosInvitados, idPedido);
        //            break;
        //        case (int)EmailAcciones.ReEnviarInvitacionATodos:
        //            var todosLosInivitados = Contexto.InvitacionPedido.Where(ip => ip.IdPedido == idPedido)
        //                .Select(i => i.IdUsuario)
        //                .ToList();
        //            servicioMail.ArmarMailInicioPedido(todosLosInivitados, idPedido);
        //            break;
        //        case (int)EmailAcciones.ReEnviarSoloALosQueNoEligieronGustos:
        //            var invitadosSinGustos = Contexto.InvitacionPedido.Where(ip => ip.IdPedido == idPedido
        //                                                                    && ip.Completado == false)
        //                .Select(i => i.IdUsuario)
        //                .ToList();
        //            servicioMail.ArmarMailInicioPedido(invitadosSinGustos, idPedido);
        //            break;
        //    }
        //    Contexto.SaveChanges();
        //}

        public bool ConfirmarGustos(PedidoRequestDTO pedido)
        {
            try
            {
                var listaDeGustosPorUsuario = Contexto.InvitacionPedidoGustoEmpanadaUsuario.Where(ip => ip.IdPedido == pedido.IdPedido && ip.IdUsuario == pedido.IdUsuario).ToList();

                foreach (InvitacionPedidoGustoEmpanadaUsuario inv in listaDeGustosPorUsuario)
                {
                    Contexto.InvitacionPedidoGustoEmpanadaUsuario.Remove(inv);
                }

                foreach (GustoEmpanadasCantidadDTO g in pedido.GustoEmpanadasCantidad)
                {

                    if (g.Cantidad > 0)
                    {
                        Contexto.InvitacionPedidoGustoEmpanadaUsuario.Add(new InvitacionPedidoGustoEmpanadaUsuario
                        {
                            Cantidad = g.Cantidad,
                            IdGustoEmpanada = g.IdGustoEmpanada,
                            IdPedido = pedido.IdPedido,
                            IdUsuario = pedido.IdUsuario,
                        });
                    }

                }

                Contexto.SaveChanges();

                return true;
            }
            catch
            {
                return false;
            }
        }
    }

}