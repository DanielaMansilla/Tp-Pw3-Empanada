using System.ComponentModel.DataAnnotations;
using W3_2018_2C_TP.Models.Enums;

namespace W3_2018_2C_TP
{
    [MetadataType(typeof(PedidoMetadata))]
    public partial class Pedido
    {
        public ReEnviarInvitacion ReEnviarInvitacion { get; set; }
    }
}