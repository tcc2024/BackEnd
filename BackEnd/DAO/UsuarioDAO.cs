using MySql.Data.MySqlClient;
using BackEnd.DTO;

namespace BackEnd.DAO
{
    public class UsuarioDAO
    {
        public List<UsuarioDTO> ListarUsuarios()
        {
            var conexao = ConnectionFactory.Build();
            conexao.Open();

            var query = "SELECT * FROM Usuario";

            var comando = new MySqlCommand(query, conexao);
            var dataReader = comando.ExecuteReader();

            var usuarios = new List<UsuarioDTO>();

            while (dataReader.Read())
            {
                var usuario = new UsuarioDTO();
                usuario.ID = int.Parse(dataReader["ID"].ToString());
                usuario.Nome = dataReader["Nome"].ToString();
                usuario.Email = dataReader["Email"].ToString();
                usuario.Senha = dataReader["Senha"].ToString();

                usuarios.Add(usuario);
            }
            conexao.Close();

            return usuarios;
        }

        public int Cadastrar(UsuarioDTO usuario)
        {
            var conexao = ConnectionFactory.Build();
            conexao.Open();

            var query = @"INSERT INTO Usuario (Nome, Email, Senha) VALUES
    				(@nome,@email,@senha)
                    SELECT LAST_INSERT_ID();";

            var comando = new MySqlCommand(query, conexao);
            comando.Parameters.AddWithValue("@nome", usuario.Nome);
            comando.Parameters.AddWithValue("@email", usuario.Email);
            comando.Parameters.AddWithValue("@senha", usuario.Senha);

            var idBD = comando.ExecuteScalar();
            conexao.Close();

            var id = int.Parse(idBD.ToString());
            return id;
        }

        internal bool VerificarUsuario(UsuarioDTO usuario)
        {
            var conexao = ConnectionFactory.Build();
            conexao.Open();

            var query = "SELECT * FROM Usuario WHERE email = @email";

            var comando = new MySqlCommand(query, conexao);
            comando.Parameters.AddWithValue("@email", usuario.Email);

            var dataReader = comando.ExecuteReader();

            var usuarios = new List<UsuarioDTO>();

            while (dataReader.Read())
            {
                usuario = new UsuarioDTO();
                usuario.ID = int.Parse(dataReader["ID"].ToString());
                usuario.Nome = dataReader["Nome"].ToString();
                usuario.Email = dataReader["Email"].ToString();
                usuario.Senha = dataReader["Senha"].ToString();

                usuarios.Add(usuario);
            }
            conexao.Close();

            return usuarios.Count > 0;
        }

        public UsuarioDTO Login(string Email, string Senha)
        {
            var conexao = ConnectionFactory.Build();
            conexao.Open();

            var usuario = new UsuarioDTO();

            var query = "SELECT * FROM Usuario WHERE email = @email and senha = @senha";

            var id = BuscarIDPorEmail(Email);

            var comando = new MySqlCommand(query, conexao);
            comando.Parameters.AddWithValue("@email", Email);
            comando.Parameters.AddWithValue("@senha", Senha);

            var dataReader = comando.ExecuteReader();

            usuario = new UsuarioDTO();

            usuario.ID = id;

            while (dataReader.Read())
            {
                usuario.ID = int.Parse(dataReader["ID"].ToString());
                usuario.Nome = dataReader["Nome"].ToString();
                usuario.Email = dataReader["Email"].ToString();
                usuario.Senha = dataReader["Senha"].ToString();
            }
            conexao.Close();

            return usuario;
        }



        internal UsuarioDTO BuscarUsuarioPorID(int id)
        {
            var conexao = ConnectionFactory.Build();
            conexao.Open();

            var query = "SELECT * FROM Usuario WHERE id = @id";

            var comando = new MySqlCommand(query, conexao);
            comando.Parameters.AddWithValue("@id", id);

            var dataReader = comando.ExecuteReader();

            var usuario = new UsuarioDTO();

            while (dataReader.Read())
            {
                usuario.ID = int.Parse(dataReader["ID"].ToString());
                usuario.Nome = dataReader["Nome"].ToString();
                usuario.Email = dataReader["Email"].ToString();
                usuario.Senha = dataReader["Senha"].ToString();
            }
            conexao.Close();

            return usuario;
        }



        internal int BuscarIDPorEmail(string email)
        {
            var conexao = ConnectionFactory.Build();
            conexao.Open();

            var query = "SELECT id FROM Usuario WHERE email = @email";

            var comando = new MySqlCommand(query, conexao);
            comando.Parameters.AddWithValue("@email", email);

            var idBD = comando.ExecuteScalar();

            var id = int.Parse(idBD.ToString());

            return id;
        }
    }
}
