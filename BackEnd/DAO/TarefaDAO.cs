using BackEnd.DTO;
using MySql.Data.MySqlClient;

namespace BackEnd.DAO
{
    public class TarefaDAO
    {
        public List<TarefaDTO> ListarTarefaPorUsuario(int id)
        {
            var conexao = ConnectionFactory.Build();
            conexao.Open();

            var query = $"select u.nome as nomeusu, p.nome as nomepro, t.id, t.nome as nometare, t.descricao from usuario as u " +
                $"inner join usuarios_projeto as up on u.id = up.usuario_id " +
                $"inner join projeto as p on up.projeto_id = p.id " +
                $"inner join projeto_tarefa as pt on p.id = pt.projeto_id " +
                $"inner join tarefa as t on pt.tarefa_id = t.id " +
                $"inner join usuarios_tarefa as ut on t.id = ut.tarefa_id " +
                $"where u.id = @id";

            var comando = new MySqlCommand(query, conexao);
            comando.Parameters.AddWithValue("@id", id);

            var dataReader = comando.ExecuteReader();

            var tarefas = new List<TarefaDTO>();
            var projeto = new ProjetoDTO();

            while (dataReader.Read())
            {
                var tarefa = new TarefaDTO();
                tarefa.ID = int.Parse(dataReader["ID"].ToString());
                tarefa.Titulo = dataReader["nometare"].ToString();
                tarefa.Descricao = dataReader["descricao"].ToString();

                projeto.Nome = dataReader["nomepro"].ToString();

                tarefa.Projeto = projeto.Nome;

                tarefas.Add(tarefa);
            }
            conexao.Close();

            return tarefas;
        }

        public void CriarTarefa(TarefaDTO tarefa)
        {
            var conexao = ConnectionFactory.Build();
            conexao.Open();

            var query = @"INSERT INTO Tarefa (Nome, Descricao, Status, DataEntrega) VALUES
    				(@nome,@descricao,'Pendente',@dataentrega);
                    SELECT LAST_INSERT_ID();";

            var comando = new MySqlCommand(query, conexao);
            comando.Parameters.AddWithValue("@nome", tarefa.Titulo);
            comando.Parameters.AddWithValue("@descricao", tarefa.Descricao);
            comando.Parameters.AddWithValue("@dataentrega", tarefa.DataEntrega);

            int idT = Convert.ToInt32(comando.ExecuteScalar());

            conexao.Close();

            foreach (var membro in tarefa.UsuariosAtribuidos)
            {
                AdicionarUsuarioNaTarefa(idT, membro.ID);
            }
        }

        public void AdicionarUsuarioNaTarefa(int idT, int idU)
        {
            var conexao = ConnectionFactory.Build();
            conexao.Open();

            var query = @"INSERT INTO Usuarios_Tarefa (Usuario_ID, Tarefa_ID) VALUES
    				(@idU, @idT)";

            var comando = new MySqlCommand(query, conexao);
            comando.Parameters.AddWithValue("@idT", idT);
            comando.Parameters.AddWithValue("@idU", idU);

            comando.ExecuteNonQuery();
            conexao.Close();
        }
    }
}
