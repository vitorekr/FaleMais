using FaleMais.Domain.Entities;

namespace FaleMais.Domain.Repositories
{
    public interface ITarifaRepository
    {
        Task<Tarifa?> GetTarifaAsync(string origem, string destino);
    }
}
