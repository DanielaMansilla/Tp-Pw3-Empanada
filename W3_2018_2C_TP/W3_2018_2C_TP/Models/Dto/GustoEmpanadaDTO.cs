using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace W3_2018_2C_TP.Models.Dto
{
    public class GustoEmpanadaDTO
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public bool IsSelected { get; set; }

        public GustoEmpanadaDTO() { }
        public GustoEmpanadaDTO(int id, string nombre)
        {
            Id = id;
            Nombre = nombre;
            IsSelected = false;
        }
    }
}