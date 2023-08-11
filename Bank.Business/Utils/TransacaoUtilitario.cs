
namespace Bank.Business.Utils
{
    
    public class ContaTransacao
    {
        public string Agencia { get; set; }
        public string Conta { get; set; }
        public string TipoConta { get; set; }
    }

    public class UtilTransacao
    {
        public ContaTransacao? Credito { get; set; }
        public ContaTransacao? Debito { get; set; }
        public double Valor { get; set; }
    }

}
