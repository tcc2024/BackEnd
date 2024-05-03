using BackEnd.DTO;
using MySql.Data.MySqlClient;

namespace BackEnd.DAO
{
    public class ProjetoDAO
    {
        public List<ProjetoDTO> ListarProjetosPorUsuario(int id)
        {
            var conexao = ConnectionFactory.Build();
            conexao.Open();

            var query = $"select p.id, p.nome, p.descricao from projeto as p " +
                        $"inner join usuarios_projeto as up on p.id = up.projeto_id " +
                        $"inner join usuario as u on up.usuario_id = u.id " +
                        $"where u.id= @id";

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

        public void CriarProjetoMinhasTarefas(int idNovo)
        {
            var conexao = ConnectionFactory.Build();
            conexao.Open();

            var query = @"INSERT INTO Projeto (Nome, Descricao) VALUES
    				('Minhas Tarefas','Crie, gerencie e organize suas tarefas pessoais aqui!');
                    SELECT LAST_INSERT_ID();";

            var comando = new MySqlCommand(query, conexao);

            int idP = Convert.ToInt32(comando.ExecuteScalar());

            conexao.Close();
            AdicionarUsuarioAoProjeto(idP, idNovo);
        }


        public void CriarProjeto(ProjetoDTO projeto, int idCriador)
        {
            var conexao = ConnectionFactory.Build();
            conexao.Open();

            if (projeto.Usuarios is null)
            {
                projeto.Usuarios = new List<UsuarioDTO>() { new UsuarioDTO() { ID = idCriador } };
            }

            if (projeto.Usuarios is not null && !projeto.Usuarios.Any(membro => membro.ID == idCriador))
                {
                    projeto.Usuarios.Add(new UsuarioDTO() { ID = idCriador });
                }

                var query = @"INSERT INTO Projeto (Nome, Descricao) VALUES
    				(@nome,@descricao);
                    SELECT LAST_INSERT_ID();";

                var comando = new MySqlCommand(query, conexao);
                comando.Parameters.AddWithValue("@nome", projeto.Nome);
                comando.Parameters.AddWithValue("@descricao", projeto.Descricao);

                int idProjeto = Convert.ToInt32(comando.ExecuteScalar());

                conexao.Close();

                foreach (var membro in projeto.Usuarios)
                {
                    AdicionarUsuarioAoProjeto(idProjeto, membro.ID);
                }
            }

            public void AdicionarUsuarioAoProjeto(int idP, int idU)
            {
                var conexao = ConnectionFactory.Build();
                conexao.Open();

                var query = @"INSERT INTO Usuarios_Projeto (Projeto_ID, Usuario_ID) VALUES
    				(@idP,@idU)";

                var comando = new MySqlCommand(query, conexao);
                comando.Parameters.AddWithValue("@idP", idP);
                comando.Parameters.AddWithValue("@idU", idU);

                comando.ExecuteNonQuery();
                conexao.Close();
            }
        }
    }
