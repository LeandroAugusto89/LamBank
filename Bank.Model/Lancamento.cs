
using System.ComponentModel.DataAnnotations.Schema;

namespace Bank.Model
{
    [Table ("T2Lancamento")]
    public class Lancamento
    {
        public Lancamento()
        {
            Conta = null;
            Transacao = null;
        }
        public int Id { get; set; }
        public int IdTransacao { get; set; }
        public int IdConta { get; set; }
        public string Historico { get; set; }
        public string TipoLancamento { get; set; }
        public double Valor { get; set; }
        public double SaldoAntes { get; set; }
        public double SaldoApos { get; set; }
        public DateTime Data { get; set; }
        public Conta? Conta { get; set; }
        public Transacao? Transacao { get; set; }

    }
}
