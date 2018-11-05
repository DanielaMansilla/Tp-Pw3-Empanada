using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace W3_2018_2C_TP
{
    public class PedidoMetadata
    {
        [Required(ErrorMessage = "Requerido")]
        public string NombreNegocio { get; set; }

        [Required(ErrorMessage = "Requerido")]
        public int PrecioUnidad { get; set; }

        [Required(ErrorMessage = "Requerido")]
        public int PrecioDocena { get; set; }

        //Gustos de empanadas disponibles
        [Required(ErrorMessage = "Requerido")]
        public virtual ICollection<GustoEmpanada> GustoEmpanada { get; set; }

        //Invitados
        [Required(ErrorMessage = "Requerido")]
        public virtual Usuario Usuario { get; set; }
    }
}