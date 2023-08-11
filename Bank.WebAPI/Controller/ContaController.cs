using Bank.Business;
using Bank.Business.Exception;
using Bank.Model;
using Microsoft.AspNetCore.Mvc;

namespace Bank.WebAPI.Controller
{
    [ApiController]
    [Route("api/conta")]
    public class ContaController : ControllerBase
    {
        private readonly ContaBusiness _bsnConta;

        public ContaController()
        {
            _bsnConta = new ContaBusiness();
        }

        /// <summary>
        /// Retorna todas as contas existentes no BD
        /// </summary>
        /// <response code="200">Contas retornadas com sucesso</response>
        /// <response code="500">Erro interno do servidor</response>
        [HttpGet]
        public IActionResult BuscarContas()
        {
            try
            {
                var contas = _bsnConta.BuscarContas();
                return Ok(contas);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.InnerException?.Message);
            }
        }

        /// <summary>
        /// Retorna uma conta existente no BD
        /// </summary>
        /// <response code="200">Conta retornada com sucesso</response>
        /// <response code="404">Conta não encontrada</response>
        /// <response code="500">Erro interno do servidor</response>
        [HttpGet("{conta}/{agencia}")]
        public IActionResult BuscarConta([FromRoute] string conta, [FromRoute] string agencia)
        {
            try
            {
                var contaBD = _bsnConta.BuscarConta(conta, agencia);
                return Ok(contaBD);
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
        /// Cadastra uma conta no BD
        /// </summary>
        /// <param name="conta">Conta a ser cadastrada</param>
        /// <response code="201">Conta cadastrada com sucesso</response>
        /// <response code="409">Conta já existente.</response>
        /// <response code="422">Dados informados estão incorretos</response>
        /// <response code="500">Erro interno do servidor</response>
        [HttpPost]
        public IActionResult CriarConta([FromBody] Conta conta)
        {
            try
            {
                _bsnConta.CriarConta(conta);
                return Created("", null);
            }
            catch (ExistingObjectException ex)
            {
                return StatusCode(409, ex.InnerException?.Message);
            }
            catch (InvalidObjectException ex)
            {
                return StatusCode(422, ex.InnerException?.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.InnerException?.Message);
            }
        }

        /// <summary>
        /// Altera uma conta no BD
        /// </summary>
        /// <param name="conta">Conta a ser alterada</param>
        /// <response code="201">Conta alterada com sucesso</response>
        /// <response code="404">Conta não encontrada</response>
        /// <response code="422">Dados informados estão incorretos</response>
        /// <response code="500">Erro interno do servidor</response>
        [HttpPut]
        public IActionResult AlterarConta([FromBody] Conta conta)
        {
            try
            {
                _bsnConta.AlterarConta(conta);
                return Ok();
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (InvalidObjectException ex)
            {
                return StatusCode(422, ex.InnerException?.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.InnerException?.Message);
            }
        }

        /// <summary>
        /// Remove uma conta no BD
        /// </summary>
        /// <param name="id">Id da conta a ser removida</param>
        /// <response code="201">Conta removida com sucesso</response>
        /// <response code="404">Conta não encontrada</response>
        /// <response code="500">Erro interno do servidor</response>
        [HttpDelete("{id}")]
        public IActionResult EncerrarConta([FromRoute] int id)
        {
            try
            {
                _bsnConta.EncerrarConta(id);
                return Ok();
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
