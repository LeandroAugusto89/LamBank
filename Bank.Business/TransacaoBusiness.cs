
using Bank.Business.Enums;
using Bank.Business.Exception;
using Bank.Business.Utils;
using Bank.Model;

namespace Bank.Business
{
    public class TransacaoBusiness : BusinessBase
    {
        /// <summary>
        /// Cria uma transação de depósito
        /// </summary
        /// <param name="contaCredito">Conta que receberá o depósito</param>
        public void Depositar(UtilTransacao contaCredito)
        {
            var conta = BDContext.Conta.FirstOrDefault(c => c.Codigo == contaCredito.Credito.Conta && c.CodigoAgencia == contaCredito.Credito.Agencia) ??
                throw new NotFoundException("Conta não encontrada!");

            var dataAgora = DateTime.Now;
            Historico historico = new Historico();
            TipoLancamento tipoLancamento = new TipoLancamento();

            var transacao = new Transacao()
            {
                Data = dataAgora,
                IdTipoOperacao = Convert.ToInt32(Enums.TipoTransacao.Deposito),
            };

            var lancamento = new Lancamento()
            {
                //IdTransacao = transacao.Id,
                IdConta = conta.Id,
                Historico = historico.DEPOSITO,
                TipoLancamento = TipoLancamento.CREDITO,
                Valor = contaCredito.Valor,
                SaldoAntes = conta.SaldoAtual,
                SaldoApos = conta.SaldoAtual + contaCredito.Valor,
                Data = dataAgora,
                Transacao = transacao,
            };

            conta.SaldoAtual = lancamento.SaldoApos;

            using (var tran = BDContext.Database.BeginTransaction())
            {
                BDContext.Transacao.Add(transacao);
                lancamento.IdTransacao = transacao.Id;
                BDContext.Lancamento.Add(lancamento);
                BDContext.SaveChanges();
                tran.Commit();
            }
        }

        /// <summary>
        /// Cria uma transação de saque
        /// </summary
        /// <param name="contaDebito">Conta que efetivará o saque</param>
        public void Sacar(UtilTransacao contaDebito)
        {
            var conta = BDContext.Conta.FirstOrDefault(c => c.Codigo == contaDebito.Debito.Conta && c.CodigoAgencia == contaDebito.Debito.Agencia) ??
                throw new NotFoundException("Conta não encontrada!");


            var dataAgora = DateTime.Now;
            Historico historico = new Historico();
            TipoLancamento tipoLancamento = new TipoLancamento();

            var transacao = new Transacao()
            {
                Data = dataAgora,
                IdTipoOperacao = Convert.ToInt32(Enums.TipoTransacao.Deposito),
            };

            var lancamento = new Lancamento()
            {
                //IdTransacao = transacao.Id,
                IdConta = conta.Id,
                Historico = historico.SAQUE,
                TipoLancamento = TipoLancamento.DEBITO,
                Valor = contaDebito.Valor,
                SaldoAntes = conta.SaldoAtual,
                SaldoApos = conta.SaldoAtual - contaDebito.Valor,
                Data = dataAgora,
                Transacao = transacao,
            };

            conta.SaldoAtual = lancamento.SaldoApos;

            using (var tran = BDContext.Database.BeginTransaction())
            {
                BDContext.Transacao.Add(transacao);
                lancamento.IdTransacao = transacao.Id;
                BDContext.Lancamento.Add(lancamento);
                BDContext.SaveChanges();
                tran.Commit();
            }
        }

        /// <summary>
        /// Cria uma transferência que debitá o valor na contaDebito e credita o valor na contaCredito
        /// </summary
        /// <param name="transferencia">Dados das contas a que a transferência refere-se</param>
        public void Transferir(UtilTransacao contas)
        {
            var contaDebito = BDContext.Conta.FirstOrDefault(c => c.Codigo == contas.Debito.Conta && c.CodigoAgencia == contas.Debito.Agencia) ??
                throw new NotFoundException("Conta não encontrada!");
            var contaCredito = BDContext.Conta.FirstOrDefault(c => c.Codigo == contas.Credito.Conta && c.CodigoAgencia == contas.Credito.Agencia) ??
                throw new NotFoundException("Conta não encontrada!");

            var dataAgora = DateTime.Now;
            Historico historico = new Historico();
            TipoLancamento tipoLancamento = new TipoLancamento();

            var transacao = new Transacao()
            {
                Data = dataAgora,
                IdTipoOperacao = Convert.ToInt32(Enums.TipoTransacao.Deposito),
            };

            var tipoOperacao = BDContext.TipoOperacao.FirstOrDefault(t => t.Id == transacao.IdTipoOperacao);

            var lancamentoDebito = new Lancamento()
            {
                //IdTransacao = transacao.Id,
                IdConta = contaDebito.Id,
                Historico = tipoOperacao.Nome,
                TipoLancamento = TipoLancamento.DEBITO,
                Valor = contas.Valor,
                SaldoAntes = contaDebito.SaldoAtual,
                SaldoApos = contaDebito.SaldoAtual - contas.Valor,
                Data = dataAgora,
                Transacao = transacao,
            };

            contaDebito.SaldoAtual = lancamentoDebito.SaldoApos;

            var lancamentoCredito = new Lancamento()
            {
                //IdTransacao = transacao.Id,
                IdConta = contaCredito.Id,
                Historico = tipoOperacao.Nome,
                TipoLancamento = TipoLancamento.CREDITO,
                Valor = contas.Valor,
                SaldoAntes = contaCredito.SaldoAtual,
                SaldoApos = contaCredito.SaldoAtual + contas.Valor,
                Data = dataAgora,
                Transacao = transacao,
            };

            contaCredito.SaldoAtual = lancamentoCredito.SaldoApos;


            using (var tran = BDContext.Database.BeginTransaction())
            {
                BDContext.Transacao.Add(transacao);
                lancamentoDebito.IdTransacao = transacao.Id;
                lancamentoCredito.IdTransacao = transacao.Id;
                BDContext.Lancamento.Add(lancamentoDebito);
                BDContext.Lancamento.Add(lancamentoCredito);
                BDContext.SaveChanges();
                tran.Commit();
            }

        }
    }
}
