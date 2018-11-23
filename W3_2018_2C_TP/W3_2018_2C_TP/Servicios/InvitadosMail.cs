using W3_2018_2C_TP.Models.Dto;

namespace W3_2018_2C_TP.Servicios
{
    internal class InvitadosMail : InvitadosMailDTO
    {
        public string Email { get; set; }
        public decimal PrecioTotal { get; set; }
    }
}