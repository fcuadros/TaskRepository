﻿namespace CiberGestionApi.Entities
{
    public class DetalleFacturaDto
    {
        public int ProductoId { get; set; }
        public int Cantidad { get; set; }
        public decimal PrecioUnitario { get; set; }
    }
}
