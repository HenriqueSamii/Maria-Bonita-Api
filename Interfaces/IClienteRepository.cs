using System.Collections.Generic;
using System.Threading.Tasks;
using Entities;

namespace Interfaces
{
    public interface IClienteRepository
    {
        Task UpdateAsync(int id, Cliente altCliente);
        Task CreateClientesAsync(Cliente cliente);
        void DeleteClientesByIdAsync(int id);

        Task<IEnumerable<Cliente>> GetAllClientesAsync();

        Task<Cliente> GetClientesByIdAsync(int id);

        Task<bool> SaveAllAsync();
    }
}