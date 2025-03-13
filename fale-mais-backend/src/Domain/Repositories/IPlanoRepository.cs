using System.Threading.Tasks;
using FaleMais.Domain.Entities;

namespace FaleMais.Domain.Repositories
{
    public interface IPlanoRepository
    {
        Task<Plano?> GetPlanoAsync(string nome);
    }
}
