using System.ComponentModel.DataAnnotations;

namespace W3_2018_2C_TP
{
    public class UsuarioMetadata
    {
        [Required(ErrorMessage = "El Email es requerido")]
        [EmailAddress(ErrorMessage = "Debe ingresar un formato de Email valido")]
        public string Email { get; set; }

        [Required(ErrorMessage = "El Password es requerido")]
        public string Password { get; set; }
    }
}