using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Bank.Model
{
    [Table("T2Agencia")]
    public class Agencia
    {
        public Agencia()
        {
            Conta = null;
        }

        [Key]
        public string Codigo { get; set; }
        public string Nome { get; set; }
        public string Cidade { get; set; }
        public string Estado { get; set; }
        [JsonIgnore]
        public List<Conta> Conta { get; set; }
    }
}
