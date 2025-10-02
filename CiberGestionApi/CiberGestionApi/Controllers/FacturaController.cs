using CiberGestionApi.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using Microsoft.Data.SqlClient;

namespace CiberGestionApi.Controllers
{
    public class FacturaController : ControllerBase
    {
        private readonly IConfiguration _config;

        public FacturaController(IConfiguration config)
        {
            _config = config;
        }

        [HttpGet("listar-solo-serie-cliente")]
        public async Task<IActionResult> ListarSoloSerieYCliente()
        {
            var result = new List<FacturaSimpleDto>();
            var connectionString = _config.GetConnectionString("Evaluacion");

            using var connection = new SqlConnection(connectionString);
            using var command = new SqlCommand("sp_ListarFacturasSimple", connection);
            command.CommandType = CommandType.StoredProcedure;

            await connection.OpenAsync();
            using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                result.Add(new FacturaSimpleDto
                {
                    Serie = reader.GetString(0),
                    ClienteNombre = reader.GetString(1)
                });
            }

            return Ok(result);
        }

        [HttpPost("registrar")]
        public async Task<IActionResult> RegistrarFactura([FromBody] FacturaRegistroDto factura)
        {
            if (factura.Detalles == null || !factura.Detalles.Any())
                return BadRequest("Debe registrar al menos un producto.");

            if (factura.Detalles.Any(d => d.Cantidad <= 0))
                return BadRequest("Todos los productos deben tener una cantidad mayor a cero.");

            var connectionString = _config.GetConnectionString("Evaluacion");

            using var connection = new SqlConnection(connectionString);
            await connection.OpenAsync();

            using var transaction = connection.BeginTransaction();

            try
            {
                // Ejecutar SP para insertar factura
                var cmdFactura = new SqlCommand("sp_InsertarFactura", connection, transaction)
                {
                    CommandType = CommandType.StoredProcedure
                };

                cmdFactura.Parameters.AddWithValue("@Serie", factura.Serie);
                cmdFactura.Parameters.AddWithValue("@Codigo", factura.Codigo);
                cmdFactura.Parameters.AddWithValue("@VendedorId", factura.VendedorId);
                cmdFactura.Parameters.AddWithValue("@ClienteId", factura.ClienteId);
                cmdFactura.Parameters.AddWithValue("@Fecha", factura.Fecha);
                cmdFactura.Parameters.AddWithValue("@Moneda", factura.Moneda);

                var facturaIdParam = new SqlParameter("@FacturaId", SqlDbType.Int)
                {
                    Direction = ParameterDirection.Output
                };
                cmdFactura.Parameters.Add(facturaIdParam);

                await cmdFactura.ExecuteNonQueryAsync();
                int facturaId = (int)facturaIdParam.Value;

                // Ejecutar SP para cada detalle
                foreach (var detalle in factura.Detalles)
                {
                    var cmdDetalle = new SqlCommand("sp_InsertarDetalleFactura", connection, transaction)
                    {
                        CommandType = CommandType.StoredProcedure
                    };

                    cmdDetalle.Parameters.AddWithValue("@FacturaId", facturaId);
                    cmdDetalle.Parameters.AddWithValue("@ProductoId", detalle.ProductoId);
                    cmdDetalle.Parameters.AddWithValue("@Cantidad", detalle.Cantidad);
                    cmdDetalle.Parameters.AddWithValue("@PrecioUnitario", detalle.PrecioUnitario);

                    await cmdDetalle.ExecuteNonQueryAsync();
                }

                transaction.Commit();
                return Ok(new { mensaje = "Factura registrada correctamente", facturaId });
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                return StatusCode(500, $"Error al registrar la factura: {ex.Message}");
            }
        }

    }
}
