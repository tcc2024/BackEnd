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
        [Route("listarTarefa")]
        public IActionResult Listar()
        {
            var id = int.Parse(HttpContext.User.FindFirst("id")?.Value);

            var dao = new TarefaDAO();
            var listarTarefa = dao.ListarTarefaPorUsuario(id);
            return Ok(listarTarefa);
        }

        [HttpPost]
        [Route("CriarTarefa")]
        public IActionResult CriarTarefa([FromBody] TarefaDTO tarefa)
        {
            var id = int.Parse(HttpContext.User.FindFirst("id")?.Value);

            var dao = new TarefaDAO();
            dao.CriarTarefa(tarefa);
            return Ok();
        }

        [HttpDelete]
        [Route("RemoverUsuarioDaTarefa")]
        public IActionResult RemoverUsuarioDaTarefa(int idU, int idT)
        {
            var dao = new TarefaDAO();

            //Listar Usuarios por ID_Projeto

            dao.RemoverUsuario(idU, idT);

            return Ok();
        }

        [HttpDelete]
        [Route("ExcluirTarefa")]
        public IActionResult ExcluirTarefa(int idT)
        {
            var dao = new TarefaDAO();

            dao.ExcluirTarefa(idT);

            return Ok();
        }
    }
}
