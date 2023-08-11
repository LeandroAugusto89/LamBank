using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Bank.Model
{
    [Table("T2Transacao")]
    public class Transacao
    {
        public Transacao()
        {
            TipoOperacao = null;
            Lancamento = null;
        }
        [Key]
        public int Id { get; set; }
        public int IdTipoOperacao { get; set; }
        public DateTime Data { get; set; }
        [JsonIgnore]
        public TipoOperacao TipoOperacao { get; set; }
        [JsonIgnore]
        public List<Lancamento> Lancamento { get; set; }
    }
}

