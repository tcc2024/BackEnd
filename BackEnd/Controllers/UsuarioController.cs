using BackEnd.Azure;
using BackEnd.DAO;
using BackEnd.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BackEnd.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UsuarioController : ControllerBase
    {
        [HttpGet]
        [Route("listarUsuarios")]
        public IActionResult ListarUsuario()
        {
            var dao = new UsuarioDAO();
            var usuarios = dao.ListarUsuarios();
            
            return Ok(usuarios);
        }

        [HttpPost]
        [Route("Cadastrar")]
        [AllowAnonymous]
        public IActionResult Cadastrar([FromBody] UsuarioDTO usuario)
        {
            var usuDAO = new UsuarioDAO();
            var projDAO = new ProjetoDAO();

            bool usuarioExiste = usuDAO.VerificarUsuario(usuario);
            if (usuarioExiste)
            {
                var mensagem = "E-mail já existe na base de dados";
                return Conflict(mensagem);
            }

            if (usuario.Base64 is not null)
            {
                var azureBlobStorage = new AzureBlobStorage();
                usuario.ImagemURL = azureBlobStorage.UploadImage(usuario.Base64);
            }

            var idNovo = usuDAO.Cadastrar(usuario);
            projDAO.CriarProjetoMinhasTarefas(idNovo);
            return Ok();
        }

        [HttpPost]
        [Route("Login")]
        [AllowAnonymous]
        public IActionResult Login([FromForm] string email, [FromForm] string senha)
        {
            var dao = new UsuarioDAO();
            var usuarioLogado = dao.Login(email, senha);

            if (usuarioLogado.ID == 0)
            {
                return Unauthorized();
            }
            var token = GenerateJwtToken(usuarioLogado);

            return Ok(new { token });
        }

        [HttpPut]
        [Route("EditarUsuario")]
        public IActionResult EditarUsuario([FromBody]UsuarioDTO usuario)
        {
            var dao = new UsuarioDAO();
            dao.EditarUsuario(usuario);
            return Ok();
        }

        [HttpPut]
        [Route("EditarSenhaUsuario")]
        public IActionResult EditarSenhaUsuario(UsuarioDTO usuario)
        {
            var dao = new UsuarioDAO();
            dao.EditarSenhaUsuario(usuario);
            return Ok();
        }

        [HttpGet]
        [Route("getuserdata")]
        public IActionResult GetUserData()
        {
            var dao = new UsuarioDAO();
            var id = int.Parse(HttpContext.User.FindFirst("id")?.Value);
            var usuario = dao.BuscarUsuarioPorID(id);

            return Ok(usuario);
        }

        private string GenerateJwtToken(UsuarioDTO usuario)
        {
            var secretKey = "PU8a9W4sv2opkqlOwmgsn3w3Innlc4D5";
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
                {
                    new Claim("ID", usuario.ID.ToString()),
                    new Claim("Email", usuario.Email),
                };

            var token = new JwtSecurityToken(
                "BackEnd", //Nome da sua api
                "BackEnd", //Nome da sua api
                claims, //Lista de claims
                expires: DateTime.UtcNow.AddDays(1), //Tempo de expiração do Token, nesse caso o Token expira em um dia
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
