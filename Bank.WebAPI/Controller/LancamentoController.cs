using Bank.Business;
using Bank.Business.Exception;
using Microsoft.AspNetCore.Mvc;

namespace Bank.WebAPI.Controller
{
    [ApiController]
    [Route("api/lancamento")]
    public class LancamentoController : ControllerBase
    {
        private readonly LancamentoBusiness _bsnLancamento;

        public LancamentoController()
        {
            _bsnLancamento = new LancamentoBusiness();
        }


        [HttpGet("extrato/{codigoConta}/{codigoAgencia}")]
        public IActionResult BuscarExtrato([FromRoute] string codigoConta, [FromRoute] string codigoAgencia, [FromQuery] DateTime dataInicio, [FromQuery] DateTime dataFim)
        {
            try
            {
                var extrato = _bsnLancamento.BuscarExtrato(codigoConta, codigoAgencia, dataInicio, dataFim);
                return Ok(extrato);
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
