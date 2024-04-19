using BackEnd.DTO;
using MySql.Data.MySqlClient;

namespace BackEnd.DAO
{
    public class TarefaDAO
    {
        public List<ProjetoDTO> ListarTarefaPorUsuario(int id)
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

            var tarefas = new List<TarefaDTO>();

            while (dataReader.Read())
            {
                var tarefa = new TarefaDTO();
                tarefa.ID = int.Parse(dataReader["ID"].ToString());
                tarefa.Titulo = dataReader["Nome"].ToString();
                tarefa.Descricao = dataReader["Descricao"].ToString();

                tarefas.Add(tarefa);
            }
            conexao.Close();

            return tarefas;
        }
    }
}
