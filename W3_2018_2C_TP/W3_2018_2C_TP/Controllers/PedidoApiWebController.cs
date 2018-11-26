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
            //bool estado = servicio.ValidarGustos(datos);
            //if (estado)
            //{
            //    //return Json(new { success = false, Resultado = "ERROR", Mensaje = "ESTO ES UN ERROR" });
            //    /*
            //     success: function (Resultado) {
            //            if (Resultado.success) {
            //                alert(Resultado);
            //            } else {
            //                // OTRA COSA
            //            }                          
            //        },
            //     */
            //    //return null;
            //    return Json(new { Resultado = "OK", Mensaje = "Gustos elegidos satisfactoriamente" });
            //}
            //else
            //{
            //    //GUARDAR invitacion
            //    return Json(new { Resultado = "OK", Mensaje = "Gustos elegidos satisfactoriamente" });
            //}
            return Json(new { Resultado = "OK", Mensaje = "Gustos elegidos satisfactoriamente" });
        }
    }
}
