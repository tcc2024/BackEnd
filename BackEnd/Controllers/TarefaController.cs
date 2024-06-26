﻿using BackEnd.DAO;
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

        [HttpGet]
        [Route("listarTarefaPorID")]
        public IActionResult ListarPorID(int id)
        {
            var dao = new TarefaDAO();
            var listarTarefa = dao.ListarTarefaPorID(id);
            return Ok(listarTarefa);
        }

        [HttpPost]
        [Route("CriarTarefa")]
        public IActionResult CriarTarefa([FromBody] CriarTarefaDTO tarefa)
        {
            var id = int.Parse(HttpContext.User.FindFirst("id")?.Value);

            var dao = new TarefaDAO();
            dao.CriarTarefa(tarefa);
            return Ok();
        }

        [HttpPut]
        [Route("EditarTarefa")]
        public IActionResult EditarEvento(TarefaDTO tarefa)
        {
            var dao = new TarefaDAO();
            dao.EditarTarefa(tarefa);
            return Ok();
        }

        [HttpPut]
        [Route("AtualizarStatusTarefa/{tarefa}/{status}")]
        public IActionResult AtualizarStatusTarefa(int tarefa, int status)
        {
            var dao = new TarefaDAO();
            dao.AtualizarStatusTarefa(tarefa, status);
            return Ok();
        }


        [HttpPost]
        [Route("AdicionarUsuarioNaTarefa")]
        public IActionResult AdicionarUsuarioNaTarefa(int idU, int idT)
        {
            var dao = new TarefaDAO();

            dao.AdicionarUsuarioNaTarefa(idU, idT);

            return Ok();
        }

        [HttpDelete]
        [Route("RemoverUsuarioDaTarefa")]
        public IActionResult RemoverUsuarioDaTarefa(int idU, int idT)
        {
            var dao = new TarefaDAO();

            dao.RemoverUsuarioDaTarefa(idU, idT);

            return Ok();
        }

        [HttpDelete]
        [Route("RemoverAnexoDaTarefa")]
        public IActionResult RemoverAnexoDaTarefa(int idT, int idM)
        {
            var dao = new TarefaDAO();

            dao.RemoverAnexoDaTarefa(idT, idM);

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

        [HttpGet]
        [Route("projeto/{projeto}/tarefas")]
        public IActionResult ListarTarefasDoProjeto([FromRoute] int projeto)
        {
            var dao = new TarefaDAO();
            var tarefas = dao.ListarTarefasDoProjeto(projeto);
            return Ok(tarefas);
        }
    }
}
