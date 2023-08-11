using Bank.Business;
using Bank.Business.Exception;
using Bank.Model;
using Microsoft.AspNetCore.Mvc;

namespace Bank.WebAPI.Controller
{
    [ApiController]
    [Route("api/cliente")]
    public class ClienteController : ControllerBase
    {
        private readonly ClienteBusiness _bsnCliente;

        public ClienteController()
        {
            _bsnCliente = new ClienteBusiness();
        }

        /// <summary>
        /// Retorna todos os clientes existentes no BD
        /// </summary>
        /// <response code="200">Clientes retornados com sucesso</response>
        /// <response code="500">Erro interno do servidor</response>
        [HttpGet]
        public IActionResult BuscarClientes()
        {
            try
            {
                var clientes = _bsnCliente.BuscarClientes();
                return Ok(clientes);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.InnerException?.Message);
            }
        }

        /// <summary>
        /// Retorna um cliente existentes no BD
        /// </summary>
        /// <param name="id">Id do cliente a ser buscado</param>
        /// <response code="200">Cliente retornado com sucesso</response>
        /// <response code="404">Cliente não encontrado</response>
        /// <response code="500">Erro interno do servidor</response>
        [HttpGet("{id}")]
        public IActionResult BuscarCliente([FromRoute] int id)
        {
            try
            {
                var cliente = _bsnCliente.BuscarCliente(id);
                return Ok(cliente);
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
        /// Cadastra um cliente no BD
        /// </summary>
        /// <param name="cliente">Cliente a ser cadastrado</param>
        /// <response code="201">Cliente cadastrado com sucesso</response>
        /// <response code="409">Cliente já existente.</response>
        /// <response code="422">Dados informados estão incorretos</response>
        /// <response code="500">Erro interno do servidor</response>
        [HttpPost]
        public IActionResult CriarCliente([FromBody] Cliente cliente)
        {
            try
            {
                _bsnCliente.CriarCliente(cliente);
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
        /// Altera um Cliente no BD
        /// </summary>
        /// <param name="cliente">Cliente a ser alterado</param>
        /// <response code="201">Cliente alterado com sucesso</response>
        /// <response code="404">Cliente não encontrado</response>
        /// <response code="422">Dados informados estão incorretos</response>
        /// <response code="500">Erro interno do servidor</response>
        [HttpPut]
        public IActionResult AlterarCliente([FromBody] Cliente cliente)
        {
            try
            {
                _bsnCliente.AlterarCliente(cliente);
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
        /// Remove um Cliente no BD
        /// </summary>
        /// <param name="id">Id a ser removido</param>
        /// <response code="201">Cliente removido com sucesso</response>
        /// <response code="404">Cliente não encontrado</response>
        /// <response code="409">Cliente não pode ser excluído por ter conta</response>
        /// <response code="500">Erro interno do servidor</response>
        [HttpDelete("{id}")]
        public IActionResult ExcluirCliente([FromRoute] int id)
        {
            try
            {
                _bsnCliente.ExcluirCliente(id);
                return Ok();
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (ConflictObjectException ex)
            {
                return StatusCode(409, ex.InnerException?.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.InnerException?.Message);
            }
        }
    }
}
