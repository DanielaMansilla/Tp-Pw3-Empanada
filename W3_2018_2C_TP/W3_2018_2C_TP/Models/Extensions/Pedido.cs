using System;
using System.ComponentModel.DataAnnotations;
using W3_2018_2C_TP.Servicios;

namespace W3_2018_2C_TP
{
    [MetadataType(typeof(PedidoMetadata))]
    public partial class Pedido
    {
       
        private static readonly Entities Context = new Entities();
        private readonly GustoEmpanadaServicio _servicioGustoEmpanada = new GustoEmpanadaServicio();
        public decimal PrecioTotal => CalcularPrecioTotal();
        public decimal PrecioCalculadoPorUnidad => CalcularPrecioPorUnidad();

        public decimal CalcularPrecioTotal()
        {

            int cantidadTotal = _servicioGustoEmpanada.CantidadTotalDeEmpanadas(IdPedido);
            //decimal precio = PrecioUnidad;
            int cantidadDocenas = cantidadTotal / 12;
            int empanadasSobrantes = cantidadTotal % 12;

            try
            {
                decimal precioTotal = (cantidadDocenas * PrecioDocena) + (empanadasSobrantes * PrecioUnidad);
                return Math.Round(precioTotal);
            }
            catch (Exception)
            {
                return 0;
            }
        }

        private decimal CalcularPrecioPorUnidad()
        {
            int cantidadTotal = _servicioGustoEmpanada.CantidadTotalDeEmpanadas(IdPedido);
            try
            {
                decimal precioPorUnidad = PrecioTotal / cantidadTotal;
                return Math.Round(precioPorUnidad, 2);
            }
            catch (Exception)
            {
                return 0;
            }
        }
    }
}