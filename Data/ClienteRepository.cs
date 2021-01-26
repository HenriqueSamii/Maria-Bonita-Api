using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Entities;
using Interfaces;
using Microsoft.EntityFrameworkCore;
using Utils;

namespace Data
{
    public class ClienteRepository : IClienteRepository
    {
        private readonly DataContext context;
        private ViaCepApi viaCepApi;
        
        public ClienteRepository(DataContext context,IHttpClientFactory clientFactory)
        {
            this.context = context;
            this.viaCepApi = new ViaCepApi(clientFactory);
        }

        public async Task UpdateAsync(int id, Cliente altCliente)
        {
            var oldCliente = context.Clientes.Include(x => x.EnderecoCadastrado).Where(b => b.Id == id).First();
            oldCliente.Nome = altCliente.Nome;
            oldCliente.Email = altCliente.Email;
            oldCliente.DataDeNascimento = altCliente.DataDeNascimento;

            ViaCep viaCep = await viaCepApi.GetViaCepJson(altCliente.EnderecoCadastrado.CEP);
            if (!viaCep.Equals(null))
            {
                oldCliente.EnderecoCadastrado.CEP = viaCep.Cep;
                oldCliente.EnderecoCadastrado.Cidade = viaCep.Localidade;
                oldCliente.EnderecoCadastrado.Estado = viaCep.Uf;
                oldCliente.EnderecoCadastrado.RuaAvenida = viaCep.Logradouro;
                oldCliente.EnderecoCadastrado.Bairro = viaCep.Bairro;
            }
            else
            {
                oldCliente.EnderecoCadastrado.CEP = "";
                oldCliente.EnderecoCadastrado.Cidade = altCliente.EnderecoCadastrado.Cidade;
                System.Console.WriteLine(oldCliente.EnderecoCadastrado.Cidade);
                oldCliente.EnderecoCadastrado.Estado = altCliente.EnderecoCadastrado.Estado;
                oldCliente.EnderecoCadastrado.RuaAvenida = altCliente.EnderecoCadastrado.RuaAvenida;
                oldCliente.EnderecoCadastrado.Bairro = altCliente.EnderecoCadastrado.Bairro;
            }
            oldCliente.EnderecoCadastrado.PontoDeReferencia = altCliente.EnderecoCadastrado.PontoDeReferencia;
            oldCliente.EnderecoCadastrado.Numero = altCliente.EnderecoCadastrado.Numero;
            oldCliente.EnderecoCadastrado.Complemento = altCliente.EnderecoCadastrado.Complemento;
            context.Entry(oldCliente).State = EntityState.Modified;
        }
        public async Task CreateClientesAsync(Cliente cliente)
        {
            ViaCep viaCep = await viaCepApi.GetViaCepJson(cliente.EnderecoCadastrado.CEP);
            System.Console.WriteLine(viaCep);
            if (viaCep != null)
            {
                cliente.EnderecoCadastrado.CEP = viaCep.Cep;
                cliente.EnderecoCadastrado.Cidade = viaCep.Localidade;
                cliente.EnderecoCadastrado.Estado = viaCep.Uf;
                cliente.EnderecoCadastrado.RuaAvenida = viaCep.Logradouro;
                cliente.EnderecoCadastrado.Bairro = viaCep.Bairro;
            }
            else
            {
                cliente.EnderecoCadastrado.CEP = "";
            }
            context.Clientes.Add(cliente);
        }
        public void DeleteClientesByIdAsync(int id)
        {
            Cliente cliente = (Cliente)context.Clientes.Where(b => b.Id == id).First();
            context.Clientes.Remove(cliente);
        }

        public async Task<IEnumerable<Cliente>> GetAllClientesAsync()
        {
            return await context.Clientes.Include(x => x.EnderecoCadastrado).ToListAsync();
        }

        public async Task<Cliente> GetClientesByIdAsync(int id)
        {
            return await context.Clientes.Include(x => x.EnderecoCadastrado).FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<bool> SaveAllAsync()
        {
            return await context.SaveChangesAsync() > 0;
        }
    }
}