
using Bank.Business.Utils;
using Bank.Data;
using Bank.Model;
using Microsoft.EntityFrameworkCore;

namespace Bank.Business
{
    public class LancamentoBusiness : BusinessBase
    {
        /// <summary>
        /// Traz o extrato de um cliente por um período específico
        /// </summary>
        /// <param name="codConta">Conta do cliente a ser verificada</param>
        /// <param name="codAgencia">Agência do cliente a ser verificada</param>
        /// <param name="dataInicial">Data inicial do período</param>
        /// <param name="dataFinal">Data final do período</param>
        public List<ExtratoUtilitario> BuscarExtrato(string codConta, string codAgencia, DateTime dataInicial, DateTime dataFinal)
        {         
            var contaBD = BDContext.Conta.FirstOrDefault(c => c.CodigoAgencia == codAgencia && c.Codigo == codConta);

            var lancamentos = BDContext.Lancamento.Include(t => t.Transacao).ThenInclude(t => t.TipoOperacao).OrderBy(t => t.Data).Where(t => t.Data.Date >= dataInicial.Date && t.Data.Date <= dataFinal.Date
                && (t.Conta.Id == contaBD.Id)).ToList();

            var lancamentoBD = BDContext.Lancamento.OrderByDescending(l => l.Data).FirstOrDefault(l => l.IdConta == contaBD.Id && l.Data <= dataInicial.Date);

            var saldoInicial = lancamentoBD != null ? lancamentoBD.SaldoApos : 0;
            var saldoAcumulado = saldoInicial;

            var extratos = new List<ExtratoUtilitario>();

            // Primeira linha do extrato
            extratos.Add(new ExtratoUtilitario
            {
                Data = dataInicial,
                Historico = "Saldo anterior",
                Transacao = null,
                Valor = null,
                Saldo = saldoInicial,
            });

            foreach (var lancamento in lancamentos)
            {
                // Traz a data do lançamento
                var dataLancamento = lancamento.Data.Date;

                // Verifica se a próxima transação é diferente da transação atual e pertence ao mesmo dia da transação atual e que não exista dentro da lista extratos um lançamento com mesmo ID
                var proximoLancamento = lancamentos.FirstOrDefault(l => l != lancamento && l.Data.Date == dataLancamento && !extratos.Any(e => e.Transacao == l.IdTransacao));

                // Campos a serem setados
                var extratoCampos = new ExtratoUtilitario
                {
                    Data = lancamento.Data,
                    Historico = lancamento.Historico,
                    Transacao = lancamento.IdTransacao,
                    Valor = lancamento.TipoLancamento == TipoLancamento.DEBITO ? lancamento.Valor * -1 : lancamento.Valor,
                    Saldo = null,
                };

                // Se a próxima transação não pertencer ao mesmo dia, atualiza nesta transacao o saldo informa o acumulado
                if (proximoLancamento == null)
                    extratoCampos.Saldo = saldoAcumulado + extratoCampos.Valor;

                // Adiciona os campos a serem setados na lista extratos
                extratos.Add(extratoCampos);

                // Soma ao saldoAcumulado o valor da transação
                saldoAcumulado += extratoCampos.Valor ?? 0;
            }

            // Ultima linha do extrato
            extratos.Add(new ExtratoUtilitario
            {
                Data = dataFinal,
                Historico = "Saldo Final",
                Transacao = null,
                Valor = null,
                Saldo = saldoAcumulado,
            });

            return extratos;

        }
    }
}
