using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Entities
{
    public class Endereco
    {
        [ForeignKey("Cliente")]
        public int ClienteId { get; set; }
        public int Id { get; set; }
        public string Estado { get; set; }
        public string Cidade { get; set; }
        public string Bairro { get; set; }
        public string RuaAvenida { get; set; }
        // Quando digitar o CEP deverá aparecer o endereço.(Neste caso serão os atributos Estado,Cidade,Bairro,RuaAvenida)
        public string CEP { get; set; }        
        public string Numero { get; set; }
        public string Complemento { get; set; }
        public string PontoDeReferencia { get; set; }
        [JsonIgnore]
        public Cliente Cliente { get; set; }
    }
}