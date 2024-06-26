﻿using BackEnd.DTO;
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
        public ListarProjetoDTO ListarProjetoPorID(int id)
        {
            var conexao = ConnectionFactory.Build();
            conexao.Open();

            var query = $"select * from projeto " +
                        $"where id = @id";

            var comando = new MySqlCommand(query, conexao);
            comando.Parameters.AddWithValue("@id", id);

            var dataReader = comando.ExecuteReader();
            var projeto = new ListarProjetoDTO();

            while (dataReader.Read())
            {
                projeto.ID = int.Parse(dataReader["ID"].ToString());
                projeto.Nome = dataReader["Nome"].ToString();
                projeto.Descricao = dataReader["Descricao"].ToString();
                projeto.UsuariosAtribuidos = ListarUsuariosPorProjeto(projeto.ID);
            }
            conexao.Close();

            return projeto;
        }

        public List<ListarUsuarioProjetoDTO> ListarUsuariosPorProjeto(int id)
        {
            var conexao = ConnectionFactory.Build();
            conexao.Open();

            var query = $"select u.id, u.nome from projeto as p " +
                        $"inner join usuarios_projeto as up on p.id = up.projeto_id " +
                        $"inner join usuario as u on up.usuario_id = u.id " +
                        $"where p.id = @id";

            var comando = new MySqlCommand(query, conexao);
            comando.Parameters.AddWithValue("@id", id);

            var dataReader = comando.ExecuteReader();

            var usuariosAtribuidos = new List<ListarUsuarioProjetoDTO>();

            while (dataReader.Read())
            {
                var usuario = new ListarUsuarioProjetoDTO();
                usuario.ID = int.Parse(dataReader["id"].ToString());
                usuario.Nome = dataReader["nome"].ToString();
                usuariosAtribuidos.Add(usuario);
            }
            conexao.Close();

            return usuariosAtribuidos;
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
            AdicionarUsuarioAoProjeto(idNovo, idP);
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
                    AdicionarUsuarioAoProjeto(membro.ID, idProjeto);
                }
        }

        public void EditarProjeto(ProjetoDTO projeto)
        {
            var conexao = ConnectionFactory.Build();
            conexao.Open();

            var query = @"UPDATE Usuarios SET 
								Nome = @nome,
								Descricao = @descricao
						  WHERE ID = @id";

            var comando = new MySqlCommand(query, conexao);
            comando.Parameters.AddWithValue("@id", projeto.ID);
            comando.Parameters.AddWithValue("@nome", projeto.Nome);
            comando.Parameters.AddWithValue("@descricao", projeto.Descricao);

            comando.ExecuteNonQuery();
            conexao.Close();
        }

        public void AdicionarUsuarioAoProjeto(int idU, int idP)
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

        public void RemoverUsuarioDoProjeto(int idU, int idP)
        {
            var conexao = ConnectionFactory.Build();
            conexao.Open();

            var query = @"DELETE FROM Usuarios_Projeto WHERE Usuario_ID = @idU AND Eventos_ID = @idP";

            var comando = new MySqlCommand(query, conexao);
            comando.Parameters.AddWithValue("@idU", idU);
            comando.Parameters.AddWithValue("@idP", idP);

            comando.ExecuteNonQuery();
            conexao.Close();
        }

        public void ExcluirProjeto(int idP)
        {
            var conexao = ConnectionFactory.Build();
            conexao.Open();

            var query = @"DELETE FROM Projeto WHERE ID = @idP";

            var comando = new MySqlCommand(query, conexao);
            comando.Parameters.AddWithValue("@idP", idP);

            comando.ExecuteNonQuery();
            conexao.Close();
        }
    }
}
