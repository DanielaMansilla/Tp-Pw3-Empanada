using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using W3_2018_2C_TP.Models.Dto;
using W3_2018_2C_TP.Models.Enums;

namespace W3_2018_2C_TP.Servicios
{

    public class PedidoServicio
    {
        public Entities Context = new Entities();
        private readonly GustoEmpanadasServicio _servicioGustoEmpanada = new GustoEmpanadasServicio();
        private readonly InvitacionPedidoServicio _servicioInvitacionPedido = new InvitacionPedidoServicio();

        public Pedido CrearPedidoDesdeCero(Pedido p)
        {
            var pedido = p;
          
            pedido.FechaCreacion = DateTime.Now;
            pedido.IdUsuarioResponsable = SessionManager.UsuarioSession.IdUsuario;
            pedido.IdEstadoPedido = 1;
            List<GustoEmpanada> gustosSeleccionados = new List<GustoEmpanada>();
            foreach (var gusto in p.GustoDeEmpanadaSeleccionados)
            {
                    gustosSeleccionados.Add(Context.GustoEmpanada.FirstOrDefault(ge => ge.IdGustoEmpanada == gusto));
            }
           
            pedido.GustoEmpanada = gustosSeleccionados;
            Context.Pedido.Add(pedido);

            if (p.UsuariosSeleccionados != null)
            {
                foreach (var id in pedido.UsuariosSeleccionados)
                {
                    InvitacionPedido invitacion = new InvitacionPedido();

                    invitacion.IdPedido = pedido.IdPedido;
                    invitacion.Completado = false;
                    invitacion.Token = Guid.NewGuid();
                    invitacion.IdUsuario = id;
                    Context.InvitacionPedido.Add(invitacion);
                    EnviarCorreo(invitacion, "Notificación de invitación");
                }
            }

            InvitacionPedido invitacionResponsable = new InvitacionPedido();

            invitacionResponsable.IdPedido = pedido.IdPedido;
            invitacionResponsable.Completado = false;
            invitacionResponsable.Token = Guid.NewGuid();
            invitacionResponsable.IdUsuario = pedido.IdUsuarioResponsable;
            Context.InvitacionPedido.Add(invitacionResponsable);
            Context.SaveChanges();
            EnviarCorreo(invitacionResponsable,"Notificación de Pedido");
            return pedido;
        }

        public void Inicializar(int id)
        {

        }

        public List<Pedido> Listar()
        {
            return Context.Pedido.ToList();
        }

        /// <summary>
        /// Metodo que ordena por fecha de forma descendente todos los pedidos
        /// </summary>
        /// <returns> List<Pedido> </returns>
        public List<Pedido> ListarDescendente()
        {
            return Context.Pedido.OrderByDescending(p => p.FechaCreacion).ToList();
        }
       
        /// <summary>
        ///  Me devuelve la lista de pedidos en los que soy responsable e invitado, mediante id de usuario.
        /// </summary>
        /// <param name="idUsuario"> idUsuario </param>
        /// <returns> List<Pedido> </returns>
        public List<Pedido> ListarPedidosResponsableInvitado(int idUsuario)
        {


            List<Pedido> pedidosResultado = new List<Pedido>();

            List<InvitacionPedido> imvitacionesDelUsuario = Context.InvitacionPedido.Include("Pedido")
                          .Where(o => o.IdUsuario == idUsuario).ToList();

            foreach (var inv in imvitacionesDelUsuario)
            {
                pedidosResultado.Add(inv.Pedido);
            }

            //List<Pedido> pedidosResponsable = Context.Pedido.Where(p => p.IdUsuarioResponsable == idUsuario).ToList();

            //pedidosResultado.AddRange(pedidosResponsable);

            return pedidosResultado.OrderByDescending(p => p.FechaCreacion).ToList();
        }

        public List<Pedido> obtenerListaPorUsuario(Usuario user)
        {
            var estado = Context.Pedido.Where(c => c.IdUsuarioResponsable == user.IdUsuario).ToList();
            return estado;
        }

        public void Eliminar(int id)
        {

            var invitaciones = Context.InvitacionPedido.Where(i => i.IdPedido == id).ToList();
            Context.InvitacionPedido.RemoveRange(invitaciones);
            Context.SaveChanges();

            var gustosPedido = Context.InvitacionPedidoGustoEmpanadaUsuario.Where(i => i.IdPedido == id).ToList();
            Context.InvitacionPedidoGustoEmpanadaUsuario.RemoveRange(gustosPedido);
            Context.SaveChanges();

            Pedido pedidoEliminar = Context.Pedido.FirstOrDefault(pedido => pedido.IdPedido == id);

            pedidoEliminar.GustoEmpanada.Clear();

            Context.Pedido.Remove(pedidoEliminar);
            Context.SaveChanges();
        }


        public Pedido ObtenerPorId(int id)
        {
            return Context.Pedido.FirstOrDefault(pedido => pedido.IdPedido == id);
        }

        public int ObtenerInvitacionesConfirmadas(int id)
        {
            return Context.InvitacionPedido.Where(c => c.IdPedido == id)
                .Where(c => c.Completado == true).Count();
        }

        /// <summary>
        /// Edita el pedido que anteriormente fue pasado por id
        /// </summary>
        /// <param name="pedido"></param>
        public void Editar(Pedido pedido)
        {
            Pedido p = Context.Pedido.Find(pedido.IdPedido);
            p.NombreNegocio = pedido.NombreNegocio;
            p.Descripcion = pedido.Descripcion;
            p.PrecioUnidad = pedido.PrecioUnidad;
            p.PrecioDocena = pedido.PrecioDocena;
            p.FechaModificacion = DateTime.Now;
            foreach (var idGusto in pedido.GustoDeEmpanadaSeleccionados)
            {
                GustoEmpanada gustoEmpanadaDisponible = Context.GustoEmpanada.Find(idGusto);
                p.GustoEmpanada.Add(gustoEmpanadaDisponible);
            }
            Context.SaveChanges();
        }

        /// <summary>
        /// Obtengo los usuarios invitados de un pedido mediante el id de pedido
        /// </summary>
        /// <param name="idPedido"></param>
        /// <returns> List<Usuario> </returns>
        public List<Usuario> ObtenerUsuariosInvitados(int idPedido)
        {
            //int idUsuarioResponsable = Context.Pedido.FirstOrDefault(p => p.IdPedido == idPedido).IdUsuarioResponsable;

            //List<Usuario> listUsuariosInvitados = Context.Pedido.Include("InvitacionPedido").Where(p => p.IdPedido == idPedido).Select(u => u.Usuario).Where(u => u.IdUsuario != idUsuarioResponsable).ToList();

            List<Usuario> listUsuariosInvitados = Context.InvitacionPedido.Where(ip => ip.IdPedido == idPedido).Select(ip => ip.Usuario).ToList();

            return listUsuariosInvitados;
        }

        public List<GustoEmpanada> ObtenerGustos()
        {
            return Context.GustoEmpanada.ToList();
        }

        public List<GustoEmpanada> ObtenerGustosPorPedido(int id)
        {
            return Context.Pedido.FirstOrDefault(p => p.IdPedido == id).GustoEmpanada.ToList();
        }

        public void EnviarInvitaciones(Pedido pedido, string ReEnviarInvitacion, List<Usuario> mails)
        {
            switch (ReEnviarInvitacion)
            {
                case "1":
                    foreach (var item in Context.InvitacionPedido.Where(m => m.IdPedido == pedido.IdPedido))
                    {
                         EnviarCorreo(item,"Re envio de Notificación de pedido");
                    }
                    break;
                case "2":
                    List<int> lista = new List<int>();

                    foreach (var item in pedido.UsuariosSeleccionados)
                    {
                        //var valor = from u in Context.InvitacionPedido
                        //                                where u.IdPedido == pedido.IdPedido
                        //                                where u.IdUsuario == item
                        //                                select u;

                        List<InvitacionPedido> invitaciones = Context.InvitacionPedido
                                                                .Where(x => x.IdPedido == pedido.IdPedido)
                                                                .Where(x => x.IdUsuario != SessionManager.UsuarioSession.IdUsuario)
                                                                .ToList();
                        foreach (InvitacionPedido inv in invitaciones)
                        {
                            if (inv.IdUsuario != item)
                            {
                                lista.Add(item);
                            }
                        }
                        //if (valor. == null)
                        //{
                        //    lista.Add(mails[item - 1].IdUsuario);
                        //}
                       
                    }
                    if (lista.Count() > 0)
                    {
                        foreach (var item in lista)
                        {
                            EnviarCorreo(_servicioInvitacionPedido.Crear(pedido, item),"Notificación de invitación de pedido");
                        }
                    }
                    break;
                case "3":
                    foreach (var item in Context.InvitacionPedido.Where(m => m.IdPedido == pedido.IdPedido).Where(m => m.Completado == false))
                    {
                        EnviarCorreo(item, "Re envio de solicitud de elección de gusto");
                    }
                    break;
            }
        }

        public void EnviarCorreo(InvitacionPedido invitacion, string MensajeNotificacion)
        {
            var fromAddress = new MailAddress("diego.gustavo.sejas2013@gmail.com", "Diego");
            Usuario usuario = Context.Usuario.Where(u => u.IdUsuario == invitacion.IdUsuario).First();
            var toAddress = new MailAddress(usuario.Email, usuario.Email);
            string fromPassword = "diegoozzy";
            string subject = MensajeNotificacion;
            string body = "<h1>Risotto Empanadas</h1><br> Invitacion: Http://" + HttpContext.Current.Request.Url.Authority + "/pedidos/elegir/" + invitacion.Token;

            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromAddress.Address, fromPassword)
                
            };
            using (var message = new MailMessage(fromAddress, toAddress)
            {
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            })
            {
                smtp.Send(message);
            }
        }

        public void cerrarPedido(Pedido pedido)
        {
            Pedido p = Context.Pedido.Find(pedido.IdPedido);
            p.FechaModificacion = DateTime.Now;
            p.IdEstadoPedido = 2;
            Context.SaveChanges();

            foreach (var idUsuario in pedido.UsuariosSeleccionados)
            {
                var invitacion = Context.InvitacionPedido.Where(m => m.IdPedido == p.IdPedido)
                                                    .Where(m => m.IdUsuario == idUsuario)
                                                    .First();
                EnviarMailCerrado(invitacion, pedido);
            }
        }

        public void EnviarMailCerrado(InvitacionPedido invitacionPedido, Pedido pedido)
        {
            Usuario usuario = Context.Usuario.Find(invitacionPedido.IdUsuario);

            int cantidadTotal = invitacionPedido.Pedido.InvitacionPedidoGustoEmpanadaUsuario.Sum(m => m.Cantidad);
            int docenasTotales = cantidadTotal / 12;
            int resto = cantidadTotal - (docenasTotales * 12);
            int TotalPorDocenas = docenasTotales * invitacionPedido.Pedido.PrecioDocena;
            int precioResto = resto * invitacionPedido.Pedido.PrecioUnidad;
            int Total = TotalPorDocenas + precioResto;

            List<String> usuarioPrecioPorAbonar = new List<String>();

            foreach (var item in pedido.UsuariosSeleccionados)
            {
                Usuario user = Context.Usuario.Find(item);
                int cantidadTotalesPorUsuario = user.InvitacionPedidoGustoEmpanadaUsuario
                                                            .Where(m => m.IdUsuario == item)
                                                            .Where(m => m.IdPedido == invitacionPedido.IdPedido)
                                                            .Sum(m => m.Cantidad);
                int docenasTotalesPorUsuario = cantidadTotalesPorUsuario / 12;
                int restoPorUsuario = cantidadTotalesPorUsuario - (docenasTotalesPorUsuario * 12);
                int RestoPorUsuario = restoPorUsuario * invitacionPedido.Pedido.PrecioUnidad;
                int TotalPorDocenasDeUsuario = docenasTotalesPorUsuario * invitacionPedido.Pedido.PrecioDocena;
                int TotalPorUsuario = TotalPorDocenasDeUsuario + RestoPorUsuario;
                usuarioPrecioPorAbonar.Add("Invitado: " + user.Email + " Precio a abonar: $" + Convert.ToString(RestoPorUsuario));
            }

            List<string> detalle = new List<string>();

            var newlist = invitacionPedido.Pedido.InvitacionPedidoGustoEmpanadaUsuario.GroupBy(d => d.IdGustoEmpanada)
            .Select(
                g => new
                {
                    Key = g.Key,
                    Value = g.Sum(s => s.Cantidad),
                    Category = g.First().GustoEmpanada,
                    Name = g.First().GustoEmpanada.Nombre
                });
            foreach (var item in newlist.ToList())
            {
                detalle.Add(item.Name + ": " + item.Value);
            }

            var fromAddress = new MailAddress("diego.gustavo.sejas2013@gmail.com", "From Name");
            var toAddress = new MailAddress("diego.gustavo.sejas2013@gmail.com", "To Name");
            string fromPassword = "diegoozzy";
            string subject = "Subject";
            string body = "";

            if (invitacionPedido.IdUsuario == invitacionPedido.Pedido.IdUsuarioResponsable)
            {
                body = "<h1>Rissotto</h1>Precio Total:</b> $" + Total + "<br><b>Invitados:</b><br> " +
                    String.Join(",<br>", usuarioPrecioPorAbonar.ToArray()) + "<br><b>Detalle:</b><br>" + String.Join(",<br>", detalle.ToArray()) +
                    "<br><b>Total de empanadas: </b>" + cantidadTotal;
            }
            else
            {
                List<string> datosInvitados = new List<string>();
                foreach (var item in invitacionPedido.Pedido.InvitacionPedidoGustoEmpanadaUsuario.Where(m => m.IdUsuario == invitacionPedido.IdUsuario))
                {
                    GustoEmpanada empanadas = Context.GustoEmpanada.Find(item.IdGustoEmpanada);
                    datosInvitados.Add("Gusto: " + empanadas.Nombre + ", Cantidad: " + item.Cantidad);
                }
                body = "<h1>Rissotto</h1>Total de empanadas del pedido: " + cantidadTotal + "<br>" +
                    String.Join(",<br>", datosInvitados.ToArray()) + "<br>Precio Total: $" + Total + "</b>";
            }

            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromAddress.Address, fromPassword)
            };
            using (var message = new MailMessage(fromAddress, toAddress)
            {
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            })
            {
                smtp.Send(message);
            }
        }
    }
}

