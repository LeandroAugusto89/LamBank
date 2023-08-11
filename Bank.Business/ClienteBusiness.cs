using Bank.Business.Exception;
using Bank.Business.Utils;
using Bank.Model;

namespace Bank.Business
{
    public class ClienteBusiness : BusinessBase
    {
        /// <summary>
        /// Valida os dígitos verificáveis do CPF e CNPJ
        /// </summary>
        /// <param name="documento">Documento do cliente a ser validado</param>
        /// <param name="dados1">Array com os dados que calcula o resto da operação do primeiro dígito</param>
        /// <param name="documento">Array com os dados que calcula o resto da operação do segundo dígito</param>
        /// <exception cref="InvalidObjectException">Campos obrigatórios e com padrão definido</exception>
        private bool ValidaDocumentos(string documento, int[] dados1, int[] dados2)
        {
            int[] documentoArray = documento.ToString().ToCharArray().Select(c => int.Parse(c.ToString())).ToArray();

            int somaMultiplicaPrimeiro = 0;
            int somaMultiplicaSegundo = 0;

            for (int i = 0; i < documento.Length - 2; i++)
            {
                int numero = dados1[i];
                int digito = documentoArray[i];
                int multiplicacaoPrimeiro = numero * digito;
                somaMultiplicaPrimeiro += multiplicacaoPrimeiro;
            }

            int resto = somaMultiplicaPrimeiro % 11;
            int primeiroDigito = resto < 2 ? 0 : 11 - resto;
            documentoArray[documentoArray.Length - 2] = primeiroDigito;

            for (int i = 0; i < documento.Length - 1; i++)
            {
                int numero2 = dados2[i];
                int digito2 = documentoArray[i];
                int multiplicacaoSegundo = numero2 * digito2;
                somaMultiplicaSegundo += multiplicacaoSegundo;
            }

            int resto2 = somaMultiplicaSegundo % 11;
            int segundoDigito = resto2 < 2 ? 0 : 11 - resto2;

            int[] documentoConferir = documento.ToString().ToCharArray().Select(c => int.Parse(c.ToString())).ToArray();

            return primeiroDigito == documentoConferir[documentoConferir.Length - 2] && segundoDigito == documentoConferir[documentoConferir.Length - 1];
        }

        /// <summary>
        /// Valida as informações do cliente PF e PJ
        /// </summary>
        /// <param name="cliente">Cliente a validado</param>
        /// <exception cref="InvalidObjectException">Campos obrigatórios e com padrão definido</exception>
        private void ValidaDadosCliente(Cliente cliente)
        {
            TipoCliente tipoCliente = new TipoCliente();
            if (cliente.Tipo == tipoCliente.PESSOA_FISICA)
            {
                int[] dados1 = { 10, 9, 8, 7, 6, 5, 4, 3, 2 };
                int[] dados2 = { 11, 10, 9, 8, 7, 6, 5, 4, 3, 2 };
                if (!ValidaDocumentos(cliente.CPF, dados1, dados2))
                    throw new InvalidObjectException("CPF inválido!");

                if (!PadraoDados.InstanciaPadraoDados.ValidaOnzeDigitos(cliente.Telefone))
                    throw new InvalidObjectException("Telefone com dados fora do padrão!");

                if (!PadraoDados.InstanciaPadraoDados.ValidaOnzeDigitos(cliente.CPF))
                    throw new InvalidObjectException("CPF com dados fora do padrão!");
            }
            else if (cliente.Tipo == tipoCliente.PESSOA_JURIDICA)
            {
                int[] dados1 = { 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
                int[] dados2 = { 6, 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
                if (!ValidaDocumentos(cliente.CNPJ, dados1, dados2))
                    throw new InvalidObjectException("CNPJ inválido!");

                if (!PadraoDados.InstanciaPadraoDados.ValidaOnzeDigitos(cliente.Telefone))
                    throw new InvalidObjectException("Telefone com dados fora do padrão!");

                if (!PadraoDados.InstanciaPadraoDados.ValidaQuatorzeDigitos(cliente.CNPJ))
                    throw new InvalidObjectException("CNPJ com dados fora do padrão!");

                if (!PadraoDados.InstanciaPadraoDados.ValidaNoveDigitos(cliente.InscricaoEstadual))
                    throw new InvalidObjectException("Inscrição Estadual com dados fora do padrão!");

            }
            else
            {
                throw new InvalidObjectException("Cliente deve ser PF ou PJ!");
            }
            

            if (string.IsNullOrEmpty(cliente.Nome) || string.IsNullOrEmpty(cliente.Endereco) || string.IsNullOrEmpty(cliente.Telefone))
                throw new InvalidObjectException("Os campos Nome, Endereço e Telefone são obrigatórios");
        }

        /// <summary>
        /// Verifica se o CPF ou CNPJ já existe no BD
        /// </summary>
        /// <param name="id">Id do cliente a ser encontrado</param>
        /// <exception cref="InvalidObjectException">CPF ou CNPJ já existe no Banco de Dados</exception>
        private void ValidaCpfCnpjExiste(Cliente cliente)
        {
            var clienteBD = BDContext.Cliente.FirstOrDefault(c => c.CPF == cliente.CPF || c.CNPJ == cliente.CNPJ);
            if (clienteBD == null)
                throw new InvalidObjectException("CPF ou CNPJ já existe no Banco de Dados!");
        }

        /// <summary>
        /// Verifica cliente encontrado
        /// </summary>
        /// <param name="id">Id do cliente a ser encontrado</param>
        /// <exception cref="NotFoundException">Cliente não encontrado</exception>
        private Cliente ValidaClienteEncontrado(int id)
        {
            var cliente = BDContext.Cliente.FirstOrDefault(c => c.Id == id);
            if (cliente == null)
                throw new NotFoundException("Cliente não encontrado!");
            return cliente;
        }

        /// <summary>
        /// Verifica se o cliente possui conta
        /// </summary>
        /// <param name="id">Id do cliente a ser encontrado</param>
        /// <exception cref="ConflictObjectException">Cliente possui conta!</exception>
        private void ValidaContaExiste(int id)
        {
            var contaBD = BDContext.Conta.FirstOrDefault(c => c.IdCliente == id);
            if (contaBD != null)
                throw new ConflictObjectException("Cliente possui conta!");
        }

        /// <summary>
        /// Retorna todos os clientes no BD com suas propriedades
        /// </summary>
        public List<Cliente> BuscarClientes()
        {
            return BDContext.Cliente.ToList();
        }

        /// <summary>
        /// Retorna um cliente no BD com suas propriedades
        /// </summary>
        /// <param name="id">Id do cliente a ser retornada</param>
        public Cliente BuscarCliente(int id)
        {
            var cliente = ValidaClienteEncontrado(id);

            return cliente;
        }

        /// <summary>
        /// Cria um cliente no BD com suas propriedades
        /// </summary
        /// <param name="cliente">Cliente a ser adicionado</param>
        public void CriarCliente(Cliente cliente)
        {
            ValidaCpfCnpjExiste(cliente);
            ValidaDadosCliente(cliente);

            using (var tran = BDContext.Database.BeginTransaction())
            {
                BDContext.Cliente.Add(cliente);
                BDContext.SaveChanges();
                tran.Commit();
            }
        }

        /// <summary>
        /// Altera um cliente no BD com suas propriedades
        /// </summary>
        /// <param name="cliente">Cliente a ser alterada</param>
        public void AlterarCliente(Cliente cliente)
        {
            var clienteBD = ValidaClienteEncontrado(cliente.Id);
            ValidaDadosCliente(cliente);
            clienteBD.Telefone = cliente.Telefone;
            clienteBD.EstadoCivil = cliente.EstadoCivil;
            clienteBD.Endereco = cliente.Endereco;
            clienteBD.Nome = cliente.Nome;

            using (var tran = BDContext.Database.BeginTransaction())
            {
                BDContext.Cliente.Update(clienteBD);
                BDContext.SaveChanges();
                tran.Commit();
            }
        }

        /// <summary>
        /// Remove um determinado cliente
        /// </summary>
        /// <param name="id">Id do cliente a ser removido</param>
        public void ExcluirCliente(int id)
        {
            var clienteBD = ValidaClienteEncontrado(id);
            ValidaContaExiste(id);

            using (var tran = BDContext.Database.BeginTransaction())
            {
                BDContext.Cliente.Remove(clienteBD);
                BDContext.SaveChanges();
                tran.Commit();
            }
        }
    }
}

