using System;

namespace Entities
{
    public class Cliente
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public DateTime DataDeNascimento { get; set; }
        public Endereco EnderecoCadastrado { get; set; }
        public string Email { get; set; }
    }
}
