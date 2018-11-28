using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using W3_2018_2C_TP.Models;
using W3_2018_2C_TP.Servicios;

namespace W3_2018_2C_TP.Controllers
{
    public class PedidoApiWebController : ApiController
    {
        InvitacionPedidoServicio servicio = new InvitacionPedidoServicio();
        [HttpPost]
        public IHttpActionResult ConfirmarGustos([FromBody]ConfirmarGusto datos)
        {
            try
            {
                bool estado = servicio.ValidarGustos(datos);
                if (estado)
                {
                    servicio.ConfirmarGustos(datos);
                    return Json(new { Resultado = "OK", Mensaje = "Gustos elegidos satisfactoriamente" });
                }
                else
                {
                    return Json(new { success = false, Resultado = "ERROR", Mensaje = "Error al confirmar los gustos" });
                }
            }
            catch (Exception err)
            {
                return Json(new { success = false, Resultado = "ERROR", Mensaje = "No se pudo efectuar la operación porque " + err.Message});
            }

        }
    }
}
