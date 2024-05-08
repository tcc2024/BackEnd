﻿using BackEnd.DTO;
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
            foreach (var referencia in tarefa.Referencias)
            {
                AdicionarReferenciaNaTarefa(idT, referencia.URL);
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

        public void AdicionarReferenciaNaTarefa(int idT, string urlReferencia)
        {
            var conexao = ConnectionFactory.Build();
            conexao.Open();

            var query = @"INSERT INTO tarefa_has_mtreferencia (URL, Tarefa_ID) VALUES
    				(@urlReferencia, @idT)";

            var comando = new MySqlCommand(query, conexao);
            comando.Parameters.AddWithValue("@idT", idT);
            comando.Parameters.AddWithValue("@urlReferencia", urlReferencia);

            comando.ExecuteNonQuery();
            conexao.Close();
        }

        public void AdicionarAnexoNaTarefa(int idT, string urlAnexo)
        {
            var conexao = ConnectionFactory.Build();
            conexao.Open();

            var query = @"INSERT INTO tarefa_has_mttrabalho (URL, Tarefa_ID) VALUES
    				(@urlAnexo, @idT)";

            var comando = new MySqlCommand(query, conexao);
            comando.Parameters.AddWithValue("@idT", idT);
            comando.Parameters.AddWithValue("@urlAnexo", urlAnexo);

            comando.ExecuteNonQuery();
            conexao.Close();
        }

        public void RemoverReferenciaDaTarefa(int idT, int idM)
        {
            var conexao = ConnectionFactory.Build();
            conexao.Open();

            var query = @"DELTE FROM MtReferencia_has_Tarefa WHERE MtReferencia_ID = @idM AND Tarefa_ID = @idT";

            var comando = new MySqlCommand(query, conexao);
            comando.Parameters.AddWithValue("@idT", idT);
            comando.Parameters.AddWithValue("@idM", idM);

            comando.ExecuteNonQuery();
            conexao.Close();
        }

        public void RemoverAnexoDaTarefa(int idT, int idM)
        {
            var conexao = ConnectionFactory.Build();
            conexao.Open();

            var query = @"DELTE FROM Tarefa_has_MtTrabalho WHERE MtTrabalho_ID = @idM AND Tarefa_ID = @idT";

            var comando = new MySqlCommand(query, conexao);
            comando.Parameters.AddWithValue("@idT", idT);
            comando.Parameters.AddWithValue("@idM", idM);

            comando.ExecuteNonQuery();
            conexao.Close();
        }

        public void RemoverUsuario(int idU, int idT)
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
    }
}
    