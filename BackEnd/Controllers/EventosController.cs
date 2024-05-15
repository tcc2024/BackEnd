
using BackEnd.DAO;
using BackEnd.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;

namespace BackEnd.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventosController : ControllerBase
    {
        [HttpGet]
        [Route("listarEvento")]
        public IActionResult Listar()
        {
            var id = int.Parse(HttpContext.User.FindFirst("id")?.Value);

            var dao = new EventosDAO();
            var listarEvento = dao.ListarEventosPorUsuario(id);
            return Ok(listarEvento);
        }

        [HttpPut]
        [Route("EditarEvento")]
        public IActionResult EditarEvento(EventosDTO evento)
        {
            var dao = new EventosDAO();
            dao.EditarEvento(evento);
            return Ok();
        }

        [HttpPut]
        [Route("EditarEvento")]
        public IActionResult EditarUsuariosNoEvento(EventosDTO evento)
        {

            return Ok();
        }

        [HttpPost]
        [Route("CriarEvento")]
        public IActionResult CriarProjeto([FromBody] EventosDTO evento)
        {
            var dao = new EventosDAO();
            dao.CriarEvento(evento);
            return Ok();
        }

        [HttpDelete]
        [Route("RemoverUsuarioDoEvento")]
        public IActionResult RemoverUsuarioDoEvento(int idU, int idE)
        {
            var dao = new EventosDAO();

            dao.RemoverUsuarioDoEvento(idU, idE);

            return Ok();
        }

        [HttpDelete]
        [Route("ExcluirEvento")]
        public IActionResult ExcluirEvento(int idE)
        {
            var dao = new EventosDAO();

            dao.ExcluirEvento(idE);

            return Ok();
        }
    }
}