using BackEnd.DTO;
using MySql.Data.MySqlClient;

namespace BackEnd.DAO
{
    public class ProjetoDAO
    {
        public List<ProjetoDTO> ListarProjetosPorUsuario(int id)
        {
            var usuario = new UsuarioDTO();

            var conexao = ConnectionFactory.Build();
            conexao.Open();

            var query = $"select p.id, p.nome, p.descricao from projeto as p " +
                        $"inner join usuarios_projeto as up on p.id = up.projeto_id " +
                        $"inner join usuario as u on up.usuario_id = u.id " +
                        $"where u.id= @id";

            var email = usuario.Email;

            var comando = new MySqlCommand(query, conexao);
            comando.Parameters.AddWithValue("@id", id);

            var dataReader = comando.ExecuteReader();

            var projetos = new List<ProjetoDTO>();

            while (dataReader.Read())
            {
                var projeto = new ProjetoDTO();
                projeto.ID = int.Parse(dataReader["ID"].ToString());
                projeto.Nome = dataReader["Nome"].ToString();
                projeto.Descricao = dataReader["Descricao"].ToString();

                projetos.Add(projeto);
            }
            conexao.Close();

            return projetos;
        }
    }
}
