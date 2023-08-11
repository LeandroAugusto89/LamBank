using Bank.Business.Exception;
using Bank.Business;
using Microsoft.AspNetCore.Mvc;
using Bank.Business.Utils;

namespace Bank.WebAPI.Controller
{
    [ApiController]
    [Route("api/transacao")]
    public class TransacaoController : ControllerBase
    {
        private readonly TransacaoBusiness _bsnTransacao;

        public TransacaoController()
        {
            _bsnTransacao = new TransacaoBusiness();
        }

        /// <summary>
        /// Registra um depósito na conta no BD
        /// </summary>
        /// <param name="deposito">Conta que receberá o depósito</param>
        /// <response code="201">Depósito cadastrado com sucesso</response>
        /// <response code="404">Conta não encontrada</response>
        /// <response code="500">Erro interno do servidor</response>
        [HttpPost("depositar")]
        public IActionResult Depositar([FromBody] UtilTransacao contaCredito)
        {
            try
            {
                _bsnTransacao.Depositar(contaCredito);
                return Created("", null);
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.InnerException?.Message);
            }
        }

        /// <summary>
        /// Registra um saque na conta no BD
        /// </summary>
        /// <param name="saque">Conta que receberá o saque</param>
        /// <response code="201">Saque cadastrado com sucesso</response>
        /// <response code="404">Conta não encontrada</response>
        /// <response code="500">Erro interno do servidor</response>
        [HttpPost("sacar")]
        public IActionResult Sacar([FromBody] UtilTransacao contaDebito)
        {
            try
            {
                _bsnTransacao.Sacar(contaDebito);
                return Created("", null);
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.InnerException?.Message);
            }
        }

        /// <summary>
        /// Registra uma transferência de débito numa conta e outra de crédito em outra conta
        /// </summary>
        /// <param name="conta">Contas que indicam as trasnferências</param>
        /// <response code="201">Transferência cadastrada com sucesso</response>
        /// <response code="404">Contas não encontradas</response>
        /// <response code="500">Erro interno do servidor</response>
        [HttpPost("transferir")]
        public IActionResult Transferir([FromBody] UtilTransacao contas)
        {
            try
            {
                _bsnTransacao.Transferir(contas);
                return Created("", null);
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.InnerException?.Message);
            }
        }

    }
}

