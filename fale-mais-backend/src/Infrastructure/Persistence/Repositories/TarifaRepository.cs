using System;
using Microsoft.Data.SqlClient;
using System.Threading.Tasks;
using FaleMais.Domain.Entities;
using FaleMais.Domain.Repositories;
using Microsoft.Extensions.Configuration;

namespace FaleMais.Infrastructure.Persistence.Repositories
{

    public class TarifaRepository : ITarifaRepository
    {
        private readonly string _connectionString;

        public TarifaRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task<Tarifa?> GetTarifaAsync(string origem, string destino)
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();

            const string query = "SELECT TOP 1 Id, Origem, Destino, Valor FROM Tarifas WHERE Origem = @Origem AND Destino = @Destino";

            using var command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@Origem", origem);
            command.Parameters.AddWithValue("@Destino", destino);

            using var reader = await command.ExecuteReaderAsync();

            if (await reader.ReadAsync())
            {
                return new Tarifa
                {
                    Id = reader.GetInt32(0),
                    Origem = reader.GetString(1),
                    Destino = reader.GetString(2),
                    Valor = reader.GetDecimal(3)
                };
            }

            return null;
        }
    }
}