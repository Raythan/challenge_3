using APIService.Model;
using APIService.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APIService.Controllers
{
    [Produces("application/json")]
    [Route("v1/[controller]")]
    [ApiController]
    public class CadastroController : ControllerBase
    {
        private readonly MySqlDbContext _context;
        private string Login
        {
            get
            {
                return this.Request.Headers["login"];
            }
        }
        private string Password
        {
            get
            {
                return this.Request.Headers["password"];
            }
        }

        public CadastroController(MySqlDbContext context)
        {
            _context = context;
        }

        [HttpGet("Login")]
        public ActionResult<bool> ValidarUsuario()
        {
            if (!ValidarHeaders())
                return BadRequest(false);
            else
                return Ok(true);
        }
        
        [HttpGet]
        public IEnumerable<Cadastro> GetCadastro()
        {
            return _context.Cadastro;
        }
        
        /// <summary>
        /// Endpoint que retorna o cadastro pelo documento de identificação da tabela.
        /// </summary>
        /// <remarks>
        /// Exemplo:
        ///<![CDATA[
        ///     GET /v1/Cadastro/123.456.789-10
        ///     Atualiza um cadastro no banco com base no CPF.
        ///]]>
        /// </remarks>
        /// <param name="cpf">Parâmetro da rota que identifica o cadastro.</param>
        /// <returns>Um Json com os dados do objeto.</returns>
        /// <response code="200">Sucesso.</response>
        /// <response code="400">Em caso de erro no modelo.</response>
        /// <response code="404">Em caso de não encontrar o cadastro.</response>
        /// <response code="500">Em caso de uma exceção não tratada.</response>
        [HttpGet("{cpf}")]
        public ActionResult<Cadastro> GetCadastro([FromRoute] string cpf)
        {
            try
            {
                if (!ValidarHeaders())
                    return BadRequest("Autorização inválida.");

                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var cadastro = _context.Cadastro.Where(w => w.CPF.Equals(cpf)).FirstOrDefault();

                if (cadastro == null)
                    return NotFound();

                return Ok(cadastro);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        /// <summary>
        /// Endpoint que atualiza o cadastro de uma pessoa.
        /// </summary>
        /// <remarks>
        /// Exemplo:
        ///<![CDATA[
        ///     PUT /v1/Cadastro
        ///     Atualiza um cadastro no banco com base no CPF.
        ///]]>
        /// </remarks>
        /// <param name="cadastro">Objeto com os dados do cadastro para serem atualizados no banco.</param>
        /// <returns>String contendo informações do resultado.</returns>
        /// <response code="200">Sucesso.</response>
        /// <response code="400">Em caso de erro no modelo.</response>
        /// <response code="404">Em caso de não encontrar o cadastro.</response>
        /// <response code="422">Em caso de erro na conexão com o banco ou dados inválidos.</response>
        [HttpPut]
        public async Task<ActionResult<string>> PutCadastro([FromBody] Cadastro cadastro)
        {
            try
            {
                if (!ValidarHeaders())
                    return BadRequest("Autorização inválida.");

                if (!ModelState.IsValid)
                    return BadRequest(ModelState);
                
                if (CadastroService.ConsisteDadosEntrada(cadastro))
                {
                    try
                    {
                        Cadastro cadastroUpdate = BuscaCadastroUnico(cadastro.CPF);

                        if (cadastroUpdate != null)
                        {
                            cadastro.ID = cadastroUpdate.ID;
                            _context.Entry(cadastroUpdate).CurrentValues.SetValues(cadastro);
                            _context.Entry(cadastroUpdate).State = EntityState.Modified;
                            _context.Entry(cadastroUpdate).Property("ID").IsModified = false;

                            await _context.SaveChangesAsync();
                        }
                        else
                            return NotFound("Documento não localizado.");
                    }
                    catch (Exception ex)
                    {
                        return UnprocessableEntity($"Erro ao salvar dados. {ex.Message}");
                    }
                }
                else
                    return UnprocessableEntity("Dados inválidos.");
            }
            catch (Exception ex)
            {
                return UnprocessableEntity("Dados inválidos.");
            }
            
            return Ok("PutCadastro - Cadastro atualizado");
        }

        /// <summary>
        /// Endpoint que faz o insert de um cadastro de pessoa.
        /// </summary>
        /// <remarks>
        /// Exemplo:
        ///<![CDATA[
        ///     POST /v1/Cadastro
        ///     Insere um novo cadastro no banco.
        ///]]>
        /// </remarks>
        /// <param name="cadastro">Objeto com os dados do cadastro para serem inseridos no banco.</param>
        /// <returns>String contendo informações do resultado.</returns>
        /// <response code="201">Sucesso.</response>
        /// <response code="400">Em caso de erro no modelo ou existência de chave cadastrada.</response>
        /// <response code="422">Em caso de erro na conexão com o banco ou dados inválidos.</response>
        [HttpPost]
        public async Task<ActionResult<string>> PostCadastro([FromBody] Cadastro cadastro)
        {
            try
            {
                if (!ValidarHeaders())
                    return BadRequest("Autorização inválida.");

                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                if (CadastroService.ConsisteDadosEntrada(cadastro))
                {
                    try
                    {
                        if (!CadastroExiste(cadastro.CPF))
                        {
                            _context.Cadastro.Add(cadastro);
                            await _context.SaveChangesAsync();
                        }
                        else
                            return UnprocessableEntity("Documento já cadastrado.");
                    }
                    catch (Exception ex)
                    {
                        return UnprocessableEntity($"Erro ao salvar dados. {ex.Message}");
                    }
                }
                else
                    return UnprocessableEntity("Dados inválidos.");
            }
            catch (Exception ex)
            {
                return UnprocessableEntity("Dados inválidos.");
            }

            return CreatedAtAction("PostCadastro", "Cadastro efetuado.");
        }

        /// <summary>
        /// Endpoint que faz remoção de um cadastro de pessoa com base no documento de identificação.
        /// </summary>
        /// <remarks>
        /// Exemplo:
        ///<![CDATA[
        ///     DELETE /v1/Cadastro/123.456.789-10
        ///     Remove um cadastro do banco.
        ///]]>
        /// </remarks>
        /// <param name="doc">Número do documento que serve para identificação no banco.</param>
        /// <returns>String contendo informações do resultado.</returns>
        /// <response code="200">Sucesso.</response>
        /// <response code="400">Em caso de erro no modelo.</response>
        /// <response code="404">Em caso de erro não haver o registro.</response>
        /// <response code="422">Em caso de erro na conexão com o banco ou dados inválidos.</response>
        [HttpDelete("{doc}")]
        public async Task<ActionResult<string>> DeleteCadastro([FromRoute] string doc)
        {
            try
            {
                if (!ValidarHeaders())
                    return BadRequest("Autorização inválida.");

                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var cadastro = _context.Cadastro.Where(w => w.CPF.Equals(doc)).FirstOrDefault();
                if (cadastro == null)
                    return NotFound();

                _context.Cadastro.Remove(cadastro);
                await _context.SaveChangesAsync();

                return Ok("Cadastro removido.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Exceção sem tratamento: {ex.Message}");
            }
        }

        private bool CadastroExiste(string cpf)
        {
            return _context.Cadastro.Any(e => e.CPF.Equals(cpf));
        }

        private Cadastro BuscaCadastroUnico(string cpf)
        {
            return _context.Cadastro.Where(w => w.CPF.Equals(cpf)).FirstOrDefault();
        }

        private bool ValidarHeaders()
        {
            return _context.Header.Any(w => w.LOGIN.Equals(this.Login) && w.PASSWORD.Equals(this.Password));
        }
    }
}