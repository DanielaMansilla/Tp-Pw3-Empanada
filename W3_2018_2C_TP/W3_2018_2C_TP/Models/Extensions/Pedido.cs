using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace W3_2018_2C_TP
{
    [MetadataType(typeof(PedidoMetadata))]
    public partial class Pedido
    {
        public virtual ICollection<int> GustoDeEmpanadaSeleccionados { get; set; }

        public virtual ICollection<int> UsuariosSeleccionados { get; set; }
    }
}