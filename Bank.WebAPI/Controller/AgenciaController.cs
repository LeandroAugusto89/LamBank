using Bank.Business;
using Bank.Business.Exception;
using Bank.Model;
using Microsoft.AspNetCore.Mvc;

namespace Bank.WebAPI.Controller
{
    [ApiController]
    [Route("agencia")]
    public class AgenciaController : ControllerBase
    {
        private readonly AgenciaBusiness _bsnAgencia;

        public AgenciaController()
        {
            _bsnAgencia = new AgenciaBusiness();
        }

        /// <summary>
        /// Retorna todas as Agências existentes no BD
        /// </summary>
        /// <response code="200">Agências retornadas com sucesso</response>
        /// <response code="500">Erro interno do servidor</response>
        [HttpGet]
        public IActionResult BuscarAgencias()
        {
            try
            {
                var agencias = _bsnAgencia.BuscarAgencias();
                return Ok(agencias);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.InnerException?.Message);
            }
        }

        /// <summary>
        /// Retorna uma agência existentes no BD
        /// </summary>
        /// <param name="codigo">Código da agência a ser buscada</param>
        /// <response code="200">Agência retornada com sucesso</response>
        /// <response code="404">Agência não encontrada</response>
        /// <response code="500">Erro interno do servidor</response>
        [HttpGet("{codigo}")]
        public IActionResult BuscarAgencia([FromRoute] string codigo)
        {
            try
            {
                var agencia = _bsnAgencia.BuscarAgencia(codigo);
                return Ok(agencia);
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
        /// Cadastra uma Agência no BD
        /// </summary>
        /// <param name="agencia">Agência a ser cadastrada</param>
        /// <response code="201">Agência cadastrada com sucesso</response>
        /// <response code="409">Agência já existente.</response>
        /// <response code="422">Dados informados estão incorretos</response>
        /// <response code="500">Erro interno do servidor</response>
        [HttpPost]
        public IActionResult CriarAgencia([FromBody] Agencia agencia)
        {
            try
            {
                _bsnAgencia.CriarAgencia(agencia);
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
        /// Altera uma Agência no BD
        /// </summary>
        /// <param name="agencia">Agência a ser alterada</param>
        /// <response code="201">Agência alterada com sucesso</response>
        /// <response code="404">Agência não encontrada</response>
        /// <response code="422">Dados informados estão incorretos</response>
        /// <response code="500">Erro interno do servidor</response>
        [HttpPut]
        public IActionResult AlterarAgencia([FromBody] Agencia agencia)
        {
            try
            {
                _bsnAgencia.AlterarAgencia(agencia);
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
        /// Remove uma Agência no BD
        /// </summary>
        /// <param name="codigo">Código da agência a ser removida</param>
        /// <response code="201">Agência removida com sucesso</response>
        /// <response code="404">Agência não encontrada</response>
        /// <response code="500">Erro interno do servidor</response>
        [HttpDelete("{codigo}")]
        public IActionResult ExcluirAgencia([FromRoute] string codigo)
        {
            try
            {
                _bsnAgencia.ExcluirAgencia(codigo);
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

