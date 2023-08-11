using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Bank.Model
{
    [Table("T2Conta")]
    public class Conta
    {
        public Conta()
        {
            Agencia = null;
            Cliente = null;
            Lancamento = null;
        }
        [Key]
        public int Id { get; set; }
        public string Codigo { get; set; }
        public int IdCliente { get; set; }
        public string Tipo { get; set; }
        public string Ativo { get; set; }
        public double ChequeEspecial { get; set; }
        public double SaldoAtual { get; set; }
        public DateTime DataAbertura { get; set; }
        public DateTime? DataEncerramento { get; set; }
        public string CodigoAgencia { get; set; }
        [JsonIgnore]
        public Agencia? Agencia { get; set; }
        [JsonIgnore]
        public Cliente? Cliente { get; set; }
        [JsonIgnore]
        public List<Lancamento>? Lancamento { get; set; }



    }
}
