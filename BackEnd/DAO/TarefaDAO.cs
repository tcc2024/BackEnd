using BackEnd.DTO;
using MySql.Data.MySqlClient;

namespace BackEnd.DAO
{
    public class TarefaDAO
    {
        public List<ListarTarefaDTO> ListarTarefaPorUsuario(int id)
        {
            var conexao = ConnectionFactory.Build();
            conexao.Open();

            var query = $"select u.nome as nomeusu, p.nome as nomepro, t.id, t.nome as nometare, t.descricao from usuario as u " +
                $"inner join usuarios_projeto as up on u.id = up.usuario_id " +
                $"inner join projeto as p on up.projeto_id = p.id " +
                $"inner join tarefa as t on p.id = t.projeto_id " +
                $"inner join usuarios_tarefa as ut on t.id = ut.tarefa_id " +
                $"where u.id = @id";

            var comando = new MySqlCommand(query, conexao);
            comando.Parameters.AddWithValue("@id", id);

            var dataReader = comando.ExecuteReader();

            var tarefas = new List<ListarTarefaDTO>();
            var projeto = new ProjetoDTO();

            while (dataReader.Read())
            {
                var tarefa = new ListarTarefaDTO();
                tarefa.ID = int.Parse(dataReader["ID"].ToString());

                if (tarefas.Any(e => e.ID == tarefa.ID))
                {
                    continue;
                }

                tarefa.Nome = dataReader["nometare"].ToString();
                tarefa.Descricao = dataReader["descricao"].ToString();

                tarefa.Projeto = projeto.Nome;

                tarefas.Add(tarefa);
            }
            conexao.Close();

            return tarefas;
        }
        public ListarTarefaDTO ListarTarefaPorID(int id)
        {
            var conexao = ConnectionFactory.Build();
            conexao.Open();

            var query = $"select * from tarefa " +
                        $"where id = @id";

            var comando = new MySqlCommand(query, conexao);
            comando.Parameters.AddWithValue("@id", id);

            var dataReader = comando.ExecuteReader();
            var projeto = new ProjetoDTO();
            var tarefa = new ListarTarefaDTO();

            while (dataReader.Read())
            {
                tarefa.ID = int.Parse(dataReader["ID"].ToString());
                tarefa.Nome = dataReader["Nome"].ToString();
                tarefa.Descricao = dataReader["Descricao"].ToString();
                tarefa.Status = dataReader["Status"].ToString();

                // Verifica se o campo DataEntrega não é DBNull ou vazio antes de converter
                if (dataReader["DataEntrega"] != DBNull.Value && !string.IsNullOrWhiteSpace(dataReader["DataEntrega"].ToString()))
                {
                    tarefa.DataEntrega = DateTime.Parse(dataReader["DataEntrega"].ToString());
                }
                else
                {
                    // Se DataEntrega for DBNull ou vazio, você pode definir um valor padrão, null, ou tratar de outra forma adequada ao seu caso
                    tarefa.DataEntrega = null; // Por exemplo, se seu tipo for DateTime? (nullable)
                }

                //projeto.Nome = dataReader["NomeProjeto"].ToString();

                tarefa.UsuariosAtribuidos = ListarUsuariosPorTarefa(tarefa.ID);

                tarefa.Projeto = projeto.Nome;
            }
            conexao.Close();

            return tarefa;
        }

        public List<ListarUsuarioTarefaDTO> ListarUsuariosPorTarefa(int id)
        {
            var conexao = ConnectionFactory.Build();
            conexao.Open();

            var query = $"select u.id, u.nome from tarefa as e " +
                        $"inner join usuarios_tarefa as ue on e.id = ue.tarefa_id " +
                        $"inner join usuario as u on ue.usuario_id = u.id " +
                        $"where e.id = @id";

            var comando = new MySqlCommand(query, conexao);
            comando.Parameters.AddWithValue("@id", id);

            var dataReader = comando.ExecuteReader();

            var usuariosAtribuidos = new List<ListarUsuarioTarefaDTO>();

            while (dataReader.Read())
            {
                var usuario = new ListarUsuarioTarefaDTO();
                usuario.ID = int.Parse(dataReader["id"].ToString());
                usuario.Nome = dataReader["nome"].ToString();
                usuariosAtribuidos.Add(usuario);
            }
            conexao.Close();

            return usuariosAtribuidos;
        }

        public void CriarTarefa(CriarTarefaDTO tarefa)
        {
            var conexao = ConnectionFactory.Build();
            conexao.Open();

            var query = @"INSERT INTO Tarefa (Nome, Descricao, Status, DataEntrega, Projeto_ID) VALUES
    				(@nome,@descricao,'0',@dataentrega, @projeto);
                    SELECT LAST_INSERT_ID();";

            var comando = new MySqlCommand(query, conexao);
            comando.Parameters.AddWithValue("@nome", tarefa.Nome);
            comando.Parameters.AddWithValue("@descricao", tarefa.Descricao);
            comando.Parameters.AddWithValue("@dataentrega", tarefa.DataEntrega);
            comando.Parameters.AddWithValue("@projeto", tarefa.Projeto_ID);

            int idT = Convert.ToInt32(comando.ExecuteScalar());

            conexao.Close();

            //AdicionarTarefaNoProjeto(idT, tarefa.Projeto_ID);

            foreach (var membro in tarefa.UsuariosAtribuidos)
            {
                AdicionarUsuarioNaTarefa(membro, idT);
            }
            //foreach (var anexo in tarefa.Anexos)
            //{
            //    AdicionarAnexoNaTarefa(idT, anexo.URL);
            //}
        }

        public void AdicionarUsuarioNaTarefa(int idU, int idT)
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

        public void AdicionarAnexoNaTarefa(int idT, string urlAnexo)
        {
            var conexao = ConnectionFactory.Build();
            conexao.Open();

            var query = @"INSERT INTO Tarefa_has_Anexo (URL, Tarefa_ID) VALUES
    				(@urlAnexo, @idT)";

            var comando = new MySqlCommand(query, conexao);
            comando.Parameters.AddWithValue("@idT", idT);
            comando.Parameters.AddWithValue("@urlAnexo", urlAnexo);

            comando.ExecuteNonQuery();
            conexao.Close();
        }

        public void RemoverAnexoDaTarefa(int idT, int idM)
        {
            var conexao = ConnectionFactory.Build();
            conexao.Open();

            var query = @"DELTE FROM Tarefa_has_Anexo WHERE Anexo_ID = @idM AND Tarefa_ID = @idT";

            var comando = new MySqlCommand(query, conexao);
            comando.Parameters.AddWithValue("@idT", idT);
            comando.Parameters.AddWithValue("@idM", idM);

            comando.ExecuteNonQuery();
            conexao.Close();
        }

        public void RemoverUsuarioDaTarefa(int idU, int idT)
        {
            var conexao = ConnectionFactory.Build();
            conexao.Open();

            var query = @"DELETE FROM Usuarios_Tarefas WHERE Usuario_ID = @idU AND Tarefas_ID = @idT";

            var comando = new MySqlCommand(query, conexao);
            comando.Parameters.AddWithValue("@idU", idU);
            comando.Parameters.AddWithValue("@idT", idT);

            comando.ExecuteNonQuery();
            conexao.Close();
        }

        public void EditarTarefa(TarefaDTO tarefa)
        {
            var conexao = ConnectionFactory.Build();
            conexao.Open();

            var query = @"UPDATE Tarefa SET 
						Nome = @nome,
						Descricao = @descricao,
                        Dataentrega = @dataentrega
						WHERE ID = @id;
                        DELETE FROM Usuarios_Tarefa WHERE Tarefa_ID = @id";

            var comando = new MySqlCommand(query, conexao);
            comando.Parameters.AddWithValue("@id", tarefa.ID);
            comando.Parameters.AddWithValue("@nome", tarefa.Nome);
            comando.Parameters.AddWithValue("@descricao", tarefa.Descricao);
            comando.Parameters.AddWithValue("@dataentrega", tarefa.DataEntrega);
            comando.ExecuteNonQuery();

            if (tarefa.UsuariosAtribuidos.Count > 0)
            {
                foreach (var membroAtribuido in tarefa.UsuariosAtribuidos)
                {
                    AdicionarUsuarioNaTarefa(tarefa.ID, membroAtribuido);
                }
            }
            conexao.Close();
        }

        public void AtualizarStatusTarefa(int tarefa, int status)
        {
            var conexao = ConnectionFactory.Build();
            conexao.Open();

            var query = @"UPDATE Tarefa SET 
                        Status = @status
						WHERE ID = @id";

            var comando = new MySqlCommand(query, conexao);
            comando.Parameters.AddWithValue("@id", tarefa);
            comando.Parameters.AddWithValue("@status", status);

            comando.ExecuteNonQuery();
            conexao.Close();
        }

        public void ExcluirTarefa(int idT)
        {
            var conexao = ConnectionFactory.Build();
            conexao.Open();

            var query = @"DELETE FROM Tarefa WHERE ID = @idU";

            var comando = new MySqlCommand(query, conexao);
            comando.Parameters.AddWithValue("@idT", idT);

            comando.ExecuteNonQuery();
            conexao.Close();
        }

        public List<TarefaDTO> ListarTarefasDoProjeto(int projetoId)
        {
            var conexao = ConnectionFactory.Build();
            conexao.Open();

            var query = $"select u.nome as nomeusu, p.nome as nomepro, t.id, t.nome as nometare, t.descricao, t.status from usuario as u " +
                $"inner join usuarios_projeto as up on u.id = up.usuario_id " +
                $"inner join projeto as p on up.projeto_id = p.id " +
                $"inner join tarefa as t on p.id = t.projeto_id " +
                $"inner join usuarios_tarefa as ut on t.id = ut.tarefa_id " +
                $"where p.id = @projeto";

            var comando = new MySqlCommand(query, conexao);
            comando.Parameters.AddWithValue("@projeto", projetoId);

            var dataReader = comando.ExecuteReader();

            var tarefas = new List<TarefaDTO>();
            var projeto = new ProjetoDTO();

            while (dataReader.Read())
            {
                var tarefa = new TarefaDTO();
                tarefa.ID = int.Parse(dataReader["ID"].ToString());

                if (tarefas.Any(e => e.ID == tarefa.ID))
                {
                    continue;
                }

                tarefa.Nome = dataReader["nometare"].ToString();
                tarefa.Descricao = dataReader["descricao"].ToString();
                tarefa.Status = dataReader["status"].ToString();

                tarefa.Projeto = projeto.Nome;

                tarefas.Add(tarefa);
            }
            conexao.Close();

            return tarefas;
        }
    }
}
