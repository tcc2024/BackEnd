using BackEnd.DAO;
using BackEnd.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;

namespace BackEnd.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TarefaController : ControllerBase
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
