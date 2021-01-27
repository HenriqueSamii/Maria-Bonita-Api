using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Entities;

namespace Utils
{
    public class ViaCepApi
    {
        private readonly IHttpClientFactory clientFactory;
        public ViaCepApi(IHttpClientFactory clientFactory)
        {
            this.clientFactory = clientFactory;
        }
        //Method to get data needed from the Via Cap API
        public async Task<ViaCep> GetViaCepJson(string cepOriginal){
            cepOriginal = CleanCep(cepOriginal);
            var request = new HttpRequestMessage(HttpMethod.Get,
            "https://viacep.com.br/ws/"+ cepOriginal +"/json/");
            var client = clientFactory.CreateClient();
            var response = await client.SendAsync(request);
            if (response.IsSuccessStatusCode)//if cep found send ViaCep with info else send null
            {
                return await response.Content.ReadFromJsonAsync<ViaCep>();
            }
            return null;
        }

        //Method to transform cpf string to one that can be accepted by the Via Cap API
        private string CleanCep(string cepOriginal)
        {
            return cepOriginal.Replace("-", string.Empty);
        }
    }
}