namespace Entities
{
    public class ViaCep
    {
        public string Cep { get; set; }
        public string Logradouro { get; set; }
        public string Bairro { get; set; }
        public string Localidade { get; set; }
        public string Uf { get; set; }
     
        // Via Cep Api complete Json format
        // {
        //     "cep": "01001-000",
        //     "logradouro": "Praça da Sé",
        //     "complemento": "lado ímpar",
        //     "bairro": "Sé",
        //     "localidade": "São Paulo",
        //     "uf": "SP",
        //     "ibge": "3550308",
        //     "gia": "1004",
        //     "ddd": "11",
        //     "siafi": "7107"
        // }
    }
}