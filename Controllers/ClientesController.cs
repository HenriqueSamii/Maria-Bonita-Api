using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Data;
using Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ClientesController : ControllerBase
    {
        private readonly DataContext context;
        private readonly IHttpClientFactory clientFactory;

        public ClientesController(DataContext context,IHttpClientFactory clientFactory)
        {
            this.context = context;
            this.clientFactory = clientFactory;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Cliente>>> GetClientes()
        {
            return Ok(await GetAllClientesAsync());
        }

        //cliente/1
        [HttpGet("{id}")]
        public async Task<ActionResult<Cliente>> GetCliente(int id)
        {
            return Ok(await GetClientesByIdAsync(id));
        }
        [HttpGet("delete/{id}")]
        public async Task<ActionResult<bool>> DeleteClienteByIdAsync(int id)
        {
            DeleteClientesByIdAsync(id);
            //return await SaveAllAsync();
            if (await SaveAllAsync())
            {
                return Content("Item de id "+ id + " foi apagado");
            }
            else
            {
                return Content("Item de id "+ id + " não foi encontrado");
            }
        }

        [HttpPost("novo")]
        public async Task<ActionResult<int>> CreateCliente(Cliente cliente)
        {

            await CreateClientesAsync(cliente);
            if (await SaveAllAsync())
            {
                return Content("Novo cliente criado");
            }
            else
            {
                return Content("Não foi possivel criar cliente");
            }
        }
        [HttpPost("edit/{id}")]
        public async Task<ActionResult<bool>> EditClienteByIdAsync(int id,[FromBody]Cliente cliente)
        {
            await UpdateAsync(id,cliente);
            if (await SaveAllAsync())
            {
                return Content("Dados de cliente de id "+ id + " foram alterados");
            }
            else
            {
                return Content("Cliente de id "+ id + " não foi encontrado");
            }
        }

        //////////////////////////////////
        ///ClienteRepository
        private async Task UpdateAsync(int id, Cliente altCliente)
        {
            var oldCliente = context.Clientes.Include(x => x.EnderecoCadastrado).Where(b => b.Id == id).First();
            oldCliente.Nome = altCliente.Nome;
            oldCliente.Email = altCliente.Email;
            oldCliente.DataDeNascimento = altCliente.DataDeNascimento;

            ViaCep viaCep = await GetViaCepJson(altCliente.EnderecoCadastrado.CEP);
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
            ViaCep viaCep = await GetViaCepJson(cliente.EnderecoCadastrado.CEP);
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

        /// Viacep Api Util
        public async Task<ViaCep> GetViaCepJson(string cepOriginal){
            cepOriginal = CleanCep(cepOriginal);
            var request = new HttpRequestMessage(HttpMethod.Get,
            "https://viacep.com.br/ws/"+ cepOriginal +"/json/");
            var client = clientFactory.CreateClient();
            var response = await client.SendAsync(request);
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<ViaCep>();
            }
            return null;
        }

        private string CleanCep(string cepOriginal)
        {
            return cepOriginal.Replace("-", string.Empty);
        }
    }
}