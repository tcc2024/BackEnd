﻿using BackEnd.DTO;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;

namespace BackEnd.DAO
{
    public class EventosDAO
    {
        public List<EventosDTO> ListarEventosPorUsuario(int id)
        {
            var conexao = ConnectionFactory.Build();
            conexao.Open();

            var query = $"select e.id as IDevento, e.Titulo, e.Descricao, e.DataHora, p.nome as NomeProjeto from eventos as e " +
                        $"inner join usuarios_eventos as ue on e.id = ue.eventos_id " +
                        $"inner join usuario as u on ue.usuario_id = u.id " +
                        $"inner join usuarios_projeto as up on u.id = up.usuario_id " +
                        $"inner join projeto as p on e.projeto_id = p.id " +
                        $"where u.id = @id";

            var comando = new MySqlCommand(query, conexao);
            comando.Parameters.AddWithValue("@id", id);
                
            var dataReader = comando.ExecuteReader();

            var eventos = new List<EventosDTO>();
            var projeto = new ProjetoDTO();

            while (dataReader.Read())
            {
                var evento = new EventosDTO();
                evento.ID = int.Parse(dataReader["IDevento"].ToString());
                evento.Titulo = dataReader["Nome"].ToString();
                evento.Descricao = dataReader["Descricao"].ToString();
                evento.DataHora = DateTime.Parse(dataReader["DataHora"].ToString());

                projeto.Nome = dataReader["NomeProjeto"].ToString();

                evento.ProjetoID = projeto.ID;
                eventos.Add(evento);
            }
            conexao.Close();

            return eventos;
        }

        public void CriarEvento(CadastroEventosDTO evento)
        {   
            var conexao = ConnectionFactory.Build();
            conexao.Open();

            var query = @"INSERT INTO Eventos (Nome, Descricao, Projeto_ID) VALUES
    				(@nome,@descricao,@projeto,@dataHora);
                    SELECT LAST_INSERT_ID();";

            var comando = new MySqlCommand(query, conexao);
            comando.Parameters.AddWithValue("@nome", evento.Titulo);
            comando.Parameters.AddWithValue("@email", evento.Descricao);
            comando.Parameters.AddWithValue("@projeto", evento.ProjetoID);
            comando.Parameters.AddWithValue("@dataHora", evento.DataHora);

            int idEvento = Convert.ToInt32(comando.ExecuteScalar());

            conexao.Close();

            if (evento.UsuariosAtribuidos.Count > 0)
            {
                foreach (var membroAtribuido in evento.UsuariosAtribuidos)
                {
                    AdicionarUsuarioAoEvento(idEvento, membroAtribuido.ID);
                }
            }
        }

        public void EditarEvento(EventosDTO evento)
        {
            var conexao = ConnectionFactory.Build();
            conexao.Open();

            var query = @"UPDATE Usuarios SET 
								Titulo = @titulo, 
								Descricao = @descricao, 
								DataHora = @datahora
						  WHERE ID = @id";

            var comando = new MySqlCommand(query, conexao);
            comando.Parameters.AddWithValue("@id", evento.ID);
            comando.Parameters.AddWithValue("@titulo", evento.Titulo);
            comando.Parameters.AddWithValue("@descricao", evento.Descricao);
            comando.Parameters.AddWithValue("@datahora", evento.DataHora);

            comando.ExecuteNonQuery();
            conexao.Close();
        }

        public void AdicionarUsuarioAoEvento(int idE, int idU)
        {
            var conexao = ConnectionFactory.Build();
            conexao.Open();

            var query = @"INSERT INTO Usuarios_Eventos (Eventos_ID, Usuario_ID) VALUES
    				(@idE,@idU)";

            var comando = new MySqlCommand(query, conexao);
            comando.Parameters.AddWithValue("@idE", idE);
            comando.Parameters.AddWithValue("@idU", idU);

            comando.ExecuteNonQuery();
            conexao.Close();
        }

        public void RemoverUsuarioDoEvento(int idU, int idE)
        {
            var conexao = ConnectionFactory.Build();
            conexao.Open();

            var query = @"DELETE FROM Usuarios_Eventos WHERE Usuario_ID = @idU AND Eventos_ID = @idE";

            var comando = new MySqlCommand(query, conexao);
            comando.Parameters.AddWithValue("@idU", idU);
            comando.Parameters.AddWithValue("@idE", idE);

            comando.ExecuteNonQuery();
            conexao.Close();
        }

        public void ExcluirEvento(int idE)
        {
            var conexao = ConnectionFactory.Build();
            conexao.Open();

            var query = @"DELETE FROM Eventos WHERE Eventos_ID = @idE";

            var comando = new MySqlCommand(query, conexao);
            comando.Parameters.AddWithValue("@idE", idE);

            comando.ExecuteNonQuery();
            conexao.Close();
        }
    }
}

