using Bank.Business.Exception;
using Bank.Business.Utils;
using Bank.Model;

namespace Bank.Business
{
    public class AgenciaBusiness : BusinessBase
    {
        /// <summary>
        /// Verifica agência encontrada
        /// </summary>
        /// <param name="codigo">Código da agência a ser encontrada</param>
        /// <exception cref="NotFoundException">Agência não encontrada</exception>
        private Agencia ValidaAgenciaEncontrada(string codigo)
        {
            var agenciaBD = BDContext.Agencia.FirstOrDefault(a => a.Codigo == codigo) ?? 
                throw new NotFoundException("Agência não encontrada!");
            return agenciaBD;
        }

        /// <summary>
        /// Verifica se o código da agência tem apenas 3 dígitos, se são numéricos e a agência e existe no BD
        /// </summary>
        /// <param name="codigo">Código da agência a ser verificado</param>
        /// <exception cref="InvalidObjectException">Código da agência com dados fora do padrão</exception>
        /// <exception cref="ExistingObjectException">Código da agência já existe</exception>
        private void ValidaCodigoAgencia(string codigo)
        {
            var agencia = BDContext.Agencia.FirstOrDefault(a => a.Codigo == codigo);
            if (agencia != null)
                throw new ExistingObjectException("Código da agência já existe!");

            if (!PadraoDados.InstanciaPadraoDados.ValidaTresDigitos(codigo))
                throw new InvalidObjectException("Agência com dados fora do padrão!");
        }

        /// <summary>
        /// Retorna todas as agências no BD com suas propriedades
        /// </summary>
        public List<Agencia> BuscarAgencias()
        {
            return BDContext.Agencia.ToList();
        }

        /// <summary>
        /// Retorna uma a agência no BD com suas propriedades
        /// </summary>
        /// <param name="codigo">Codigo da agência a ser retornada</param>
        public Agencia BuscarAgencia(string codigo)
        {
            var agencia = ValidaAgenciaEncontrada(codigo);
            return agencia;
        }

        /// <summary>
        /// Cria uma a agência no BD com suas propriedades
        /// </summary>
        /// <param name="agencia">Agência a ser adicionada</param>
        public void CriarAgencia(Agencia agencia)
        {
            ValidaCodigoAgencia(agencia.Codigo);

            using (var tran = BDContext.Database.BeginTransaction())
            {
                BDContext.Agencia.Add(agencia);
                BDContext.SaveChanges();
                tran.Commit();
            }
        }

        /// <summary>
        /// Altera uma agência no BD com suas propriedades
        /// </summary>
        /// <param name="agencia">Agência a ser adicionada</param>
        public void AlterarAgencia(Agencia agencia)
        {

            var agenciaBD = ValidaAgenciaEncontrada(agencia.Codigo);

            agenciaBD.Cidade = agencia.Cidade;
            agenciaBD.Estado = agencia.Estado;
            agenciaBD.Nome = agencia.Nome;

            using (var tran = BDContext.Database.BeginTransaction())
            {
                BDContext.Agencia.Update(agenciaBD);
                BDContext.SaveChanges();
                tran.Commit();
            }
        }

        /// <summary>
        /// Remove uma determinada agência
        /// </summary>
        /// <param name="codigo">Código da agência a ser removida</param>
        public void ExcluirAgencia(string codigo)
        {
            var codigoBD = ValidaAgenciaEncontrada(codigo);

            using (var tran = BDContext.Database.BeginTransaction())
            {
                BDContext.Agencia.Remove(codigoBD);
                BDContext.SaveChanges();
                tran.Commit();
            }
        }
    }
}

