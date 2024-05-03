using BackEnd.DAO;
using BackEnd.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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

        [HttpPost]
        [Route("CriarEvento")]
        public IActionResult CriarProjeto([FromBody] EventosDTO evento)
        {
            var dao = new EventosDAO();
            dao.CriarEvento(evento);
            return Ok();
        }
        [HttpDelete]
        public IActionResult RemoverUsuarioDoEvento(int id)
        {
            var dao = new EventosDAO();

            dao.RemoverUsuario(id);

            return Ok();
        }
    }
}
