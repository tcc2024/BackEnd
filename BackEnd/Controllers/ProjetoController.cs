using BackEnd.DAO;
using BackEnd.DTO;
using BackEnd.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Org.BouncyCastle.Crypto.Digests;

namespace BackEnd.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ProjetoController : ControllerBase
    {
        [HttpGet]
        [Route("listarProjeto")]
        public IActionResult Listar()
        {
            var id = int.Parse(HttpContext.User.FindFirst("id")?.Value);

            var dao = new ProjetoDAO();
            var listarProjeto = dao.ListarProjetosPorUsuario(id);
            return Ok(listarProjeto);
        }

        [HttpGet]
        [Route("listarProjetoPorID")]
        public IActionResult ListarPorID(int id)
        {
            var dao = new ProjetoDAO();
            var listarProjeto = dao.ListarProjetoPorID(id);
            return Ok(listarProjeto);
        }

        [HttpPost]
        [Route("CriarProjeto")]
        public IActionResult CriarProjeto([FromBody] ProjetoDTO projeto)
        {
            var id = int.Parse(HttpContext.User.FindFirst("id")?.Value);

            var dao = new ProjetoDAO();
            dao.CriarProjeto(projeto, id);
            return Ok();
        }

        [HttpPut]
        [Route("EditarProjeto")]
        public IActionResult EditarProjeto(ProjetoDTO evento)
        {
            var dao = new ProjetoDAO();
            dao.EditarProjeto(evento);
            return Ok();
        }

        [HttpDelete]
        [Route("AdicionarUsuarioAoProjeto")]
        public IActionResult AdicionarUsuarioAoProjeto(int idU, int idP)
        {
            var dao = new ProjetoDAO();

            dao.AdicionarUsuarioAoProjeto(idU, idP);

            return Ok();
        }

        [HttpDelete]
        [Route("RemoverUsuarioDoProjeto")]
        public IActionResult RemoverUsuarioDoProjeto(int idU, int idP)
        {
            var dao = new ProjetoDAO();

            //Listar Usuarios por ID_Projeto

            dao.RemoverUsuarioDoProjeto(idU, idP);

            return Ok();
        }

        [HttpDelete]
        [Route("ExcluirProjeto")]
        public IActionResult ExcluirProjeto(int idP)
        {
            var dao = new ProjetoDAO();

            dao.ExcluirProjeto(idP);

            return Ok();
        }
    }
}