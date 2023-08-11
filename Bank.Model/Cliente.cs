using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Bank.Model
{
    [Table("T2Cliente")]
    public class Cliente
    {
        public Cliente()
        {
            Conta = null;
        }

        [Key]
        public int Id { get; set; }
        public string Tipo { get; set; }
        public string? CPF { get; set; }
        public string? CNPJ { get; set; }
        public string? InscricaoEstadual { get; set; }
        public string? EstadoCivil { get; set; }
        public string Nome { get; set; }
        public string Endereco { get; set; }
        public string Telefone { get; set; }
        [JsonIgnore]
        public List<Conta> Conta { get; set; }

    }
}
