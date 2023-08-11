using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Bank.Model
{
    [Table("T2TipoOperacao")]
    public class TipoOperacao
    {
        public TipoOperacao()
        {
            Transacao = null;
        }

        [Key]
        public int Id { get; set; }
        public string Nome { get; set; }
        public string TipoLancamento { get; set; }
        public double? Tarifa { get; set; }

        [JsonIgnore]
        public List<Transacao> Transacao { get; set; }

    }
}
