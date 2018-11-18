using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace W3_2018_2C_TP
{
    public class PedidoMetadata
    {
        [Required(ErrorMessage = "Debe ingresar el nombre del negocio")]
        public string NombreNegocio { get; set; }

        [Required(ErrorMessage = "Debe ingresar precio por unidad")]
        public int PrecioUnidad { get; set; }

        [Required(ErrorMessage = "Debe ingresar precio por docena")]
        public int PrecioDocena { get; set; }

        //Gustos de empanadas disponibles
        [Required(ErrorMessage = "Debe ingresar los gustos")]
        public virtual ICollection<GustoEmpanada> GustoEmpanada { get; set; }

        //Invitados
        [Required(ErrorMessage = "Debe seleccionar los invitados")]
        public virtual Usuario Usuario { get; set; }
    }
}