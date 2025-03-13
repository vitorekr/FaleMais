using System;
using Microsoft.Data.SqlClient;
using System.Threading.Tasks;
using FaleMais.Domain.Entities;
using FaleMais.Domain.Repositories;
using Microsoft.Extensions.Configuration;

namespace FaleMais.Infrastructure.Persistence.Repositories
{

    public class PlanoRepository : IPlanoRepository
    {
        private readonly string _connectionString;

        public PlanoRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task<Plano?> GetPlanoAsync(string nome)
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();

            const string query = "SELECT TOP 1 Id, Nome, MinutosGratis FROM Planos WHERE Nome = @Nome";

            using var command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@Nome", nome);

            using var reader = await command.ExecuteReaderAsync();

            if (await reader.ReadAsync())
            {
                return new Plano
                {
                    Id = reader.GetInt32(0),
                    Nome = reader.GetString(1),
                    MinutosGratis = reader.GetInt32(2)
                };
            }

            return null;
        }
    }
}
