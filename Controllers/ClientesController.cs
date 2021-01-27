using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Data;
using Entities;
using Microsoft.AspNetCore.Mvc;

namespace Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ClientesController : ControllerBase
    {
        private ClienteRepository clienteRepository;
        private readonly IHttpClientFactory clientFactory;

        public ClientesController(DataContext context,IHttpClientFactory clientFactory)
        {
            this.clienteRepository = new ClienteRepository(context,clientFactory);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Cliente>>> GetClientes()
        {
            return Ok(await clienteRepository.GetAllClientesAsync());
        }

        //cliente/1
        [HttpGet("{id}")]
        public async Task<ActionResult<Cliente>> GetCliente(int id)
        {
            return Ok(await clienteRepository.GetClientesByIdAsync(id));
        }
        [HttpDelete("delete/{id}")]
        public async Task<ActionResult<bool>> DeleteClienteByIdAsync(int id)
        {
            clienteRepository.DeleteClientesByIdAsync(id);
            if (await clienteRepository.SaveAllAsync())
            {
                return Content("Item de id "+ id + " foi apagado");
            }
            else
            {
                return Content("Item de id "+ id + " não foi encontrado");
            }
        }

        [HttpPost("new")]
        public async Task<ActionResult<int>> CreateCliente(Cliente cliente)
        {

            await clienteRepository.CreateClientesAsync(cliente);
            if (await clienteRepository.SaveAllAsync())
            {
                return Content("Novo cliente criado");
            }
            else
            {
                return Content("Não foi possivel criar cliente");
            }
        }
        [HttpPut("edit/{id}")]
        public async Task<ActionResult<bool>> EditClienteByIdAsync(int id,[FromBody]Cliente cliente)
        {
            await clienteRepository.UpdateAsync(id,cliente);
            if (await clienteRepository.SaveAllAsync())
            {
                return Content("Dados de cliente de id "+ id + " foram alterados");
            }
            else
            {
                return Content("Cliente de id "+ id + " não foi encontrado");
            }
        }
    }
}