namespace CiberGestionApi.Entities
{
    public class FacturaRegistroDto
    {
        public string Serie { get; set; } = string.Empty;
        public string Codigo { get; set; } = string.Empty;
        public int VendedorId { get; set; }
        public int ClienteId { get; set; }
        public DateTime Fecha { get; set; }
        public string Moneda { get; set; } = "SOL";
        public List<DetalleFacturaDto> Detalles { get; set; } = new();
    }
}
