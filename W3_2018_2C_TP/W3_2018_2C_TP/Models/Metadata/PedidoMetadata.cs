using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace W3_2018_2C_TP
{
    public class PedidoMetadata
    {
        [Required(ErrorMessage = "El Nombre del Negocio es requerido")]
        //[Range(1, 200, ErrorMessage = "El Nombre del Negocio debe tener entre 1 y 200 caracteres")]
        public string NombreNegocio { get; set; }

        [Required(ErrorMessage = "El Precio por Unidad es obligatorio")]
        [Range(0, int.MaxValue, ErrorMessage = "Ingrese solo valores numericos")]
        public int PrecioUnidad { get; set; }

        [Required(ErrorMessage = "El Precio por Docena es obligatorio")]
        [Range(0, int.MaxValue, ErrorMessage = "Ingrese solo valores numericos")]
        public int PrecioDocena { get; set; }

        //Gustos de empanadas disponibles
        //[Required(ErrorMessage = "Debe seleccionar al menos un Gusto de Empanada, dato requerido")]
        public virtual ICollection<GustoEmpanada> GustoEmpanada { get; set; }

        public virtual ICollection<int> GustoDeEmpanadaSeleccionados { get; set; }

        public virtual ICollection<int> UsuariosSeleccionados { get; set; }

        //Invitados
        //[Required(ErrorMessage = "Debe haber al menos un Invitado, dato requerido")]
        public virtual Usuario Usuario { get; set; }
    }
}