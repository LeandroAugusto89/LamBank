using Bank.Business.Enums;
using Bank.Business.Exception;
using Bank.Business.Utils;
using Bank.Model;
using Microsoft.EntityFrameworkCore;

namespace Bank.Business
{
    public class ContaBusiness : BusinessBase
    {
        /// <summary>
        /// Verifica se a conta existe no BD
        /// </summary>
        /// <param name="id">Id da conta a ser encontrada</param>
        /// <exception cref="NotFoundException">Conta não encontrada</exception>
        private Conta ValidaContaExiste(string conta, string agencia)
        {
            var contaBD = BDContext.Conta.Include(c => c.Cliente).FirstOrDefault(c => c.CodigoAgencia == agencia && c.Codigo == conta) ?? 
                throw new NotFoundException("Conta não encontrada!");
            return contaBD;
        }

        /// <summary>
        /// Verifica se a conta está ativa
        /// </summary>
        /// <param name="id">Conta a ser verificada</param>
        /// <exception cref="InvalidObjectException">A conta deve estar inativa para ser encerrada</exception>
        private Conta ValidaContaAtiva(int id)
        {
            var conta = BDContext.Conta.FirstOrDefault(c => c.Id == id);
            if (conta.Ativo != "S")
                throw new InvalidObjectException("A conta deve estar ativa 'S' para ser encerrada!");
            return conta;
        }

        /// <summary>
        /// Verifica se a cliente existe no BD
        /// </summary>
        /// <param name="id">Id da conta a ser encontrada</param>
        /// <exception cref="NotFoundException">Conta não encontrada</exception>
        private Cliente ValidaClienteExiste(Conta conta)
        {
            var cliente = BDContext.Cliente.FirstOrDefault(c => c.Id == conta.IdCliente) ?? throw new NotFoundException("Cliente não encontrado!");
            return cliente;
        }

        /// <summary>
        /// Verifica se o cliente e agência existem, se já existe esse número de conta na mesma agência, o tipo da conta,
        /// padrão de dados da conta, quantidade mínima e máxima de constas de um mesmo cliente pelo seu tipo
        /// </summary>
        /// <param name="conta">Conta do cliente a ser verificada</param>
        /// 
        private void ValidaClienteAgenciaConta(Conta conta)
        {
            var cliente = ValidaClienteExiste(conta);
            var agencia = BDContext.Agencia.FirstOrDefault(a => a.Codigo == conta.CodigoAgencia) ?? throw new NotFoundException("Agência não encontrada!");

            if (BDContext.Conta.FirstOrDefault(c => c.Codigo == conta.Codigo && c.CodigoAgencia == conta.CodigoAgencia) != null)
                throw new NotFoundException("Já existe conta com esse número nessa mesma agência!");

            TipoConta tipoConta = new TipoConta();
            if (conta.Tipo != tipoConta.CONTA_CORRENTE && conta.Tipo != tipoConta.CONTA_POUPANCA)
                throw new InvalidObjectException("O tipo deve ser C (conta corrente) ou P (conta poupança)");

            if (!PadraoDados.InstanciaPadraoDados.ValidaCincoDigitos(conta.Codigo))
                throw new InvalidObjectException("Conta com dados fora do padrão!");

            TipoCliente tipoCliente = new TipoCliente();
            var numeroContas = BDContext.Conta.Count(c => c.IdCliente == conta.IdCliente && c.CodigoAgencia == conta.CodigoAgencia);
            var parametroPF = BDContext.Parametro.FirstOrDefault(p => p.Id == Convert.ToInt32(TipoParametro.MaxContaPF));
            if (cliente.Tipo == tipoCliente.PESSOA_FISICA && numeroContas >= Convert.ToInt32(TipoParametro.MaxContaPF))
                    throw new NotFoundException("O cliente PF já possui 3 contas na mesma agência");
            var parametroPJ = BDContext.Parametro.FirstOrDefault(p => p.Id == Convert.ToInt32(TipoParametro.MaxContaPJ));
            if (cliente.Tipo == tipoCliente.PESSOA_JURIDICA && numeroContas >= Convert.ToInt32(TipoParametro.MaxContaPJ))
                    throw new NotFoundException("O cliente PJ já possui 5 contas na mesma agência");

        }

        /// <summary>
        /// Retorna todos as contas no BD com suas propriedades
        /// </summary>
        public List<Conta> BuscarContas()
        {
            return BDContext.Conta.ToList();
        }

        /// <summary>
        /// Retorna uma conta no BD com suas propriedades
        /// </summary>
        /// <param name="id">Id da conta a ser retornada</param>
        public Conta BuscarConta(string conta, string agencia)
        {
            // Include com cliente
            var contaBD = ValidaContaExiste(conta, agencia);
            return contaBD;
        }

        /// <summary>
        /// Cria uma conta no BD com suas propriedades
        /// </summary
        /// <param name="conta">Conta a ser adicionada</param>
        public void CriarConta(Conta conta)
        {
            ValidaClienteAgenciaConta(conta);
            conta.DataAbertura = DateTime.Now;
            conta.Ativo = "S";

            using (var tran = BDContext.Database.BeginTransaction())
            {
                BDContext.Conta.Add(conta);
                BDContext.SaveChanges();
                tran.Commit();
            }
        }

        /// <summary>
        /// Altera uma conta no BD com suas propriedades
        /// </summary>
        /// <param name="conta">Conta a ser alterada</param>
        public void AlterarConta(Conta conta)
        {
            ValidaClienteExiste(conta);
            var contaBD = BDContext.Conta.FirstOrDefault(c => c.Id == conta.Id);
            contaBD.ChequeEspecial = conta.ChequeEspecial;

            using (var tran = BDContext.Database.BeginTransaction())
            {
                BDContext.Conta.Update(contaBD);
                BDContext.SaveChanges();
                tran.Commit();
            }
        }

        /// <summary>
        /// Encerra uma determinada conta
        /// </summary>
        /// <param name="id">Id da conta a ser encerrada</param>
        public void EncerrarConta(int id)
        {
            var contaBD = ValidaContaAtiva(id);
            contaBD.Ativo = "N";
            contaBD.DataEncerramento = DateTime.Now;

            using (var tran = BDContext.Database.BeginTransaction())
            {
                BDContext.Conta.Update(contaBD);
                BDContext.SaveChanges();
                tran.Commit();
            }
        }
    }
}
