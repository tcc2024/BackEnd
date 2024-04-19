using BackEnd.DAO;
using BackEnd.DTO;
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

        [HttpPost]
        [Route("CriarProjeto")]
        public IActionResult CriarProjeto([FromBody] ProjetoDTO projeto)
        {
            var id = int.Parse(HttpContext.User.FindFirst("id")?.Value);

            var dao = new ProjetoDAO();
            dao.CriarProjeto(projeto, id);
            return Ok();
        }
    }
}