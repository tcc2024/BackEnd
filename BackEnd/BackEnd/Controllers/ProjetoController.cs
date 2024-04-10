using BackEnd.DAO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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
    }
}
