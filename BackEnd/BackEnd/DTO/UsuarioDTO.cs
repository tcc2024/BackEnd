namespace BackEnd.DTO
{
    public class UsuarioDTO
    {
        public int ID { get; set; }
        public string Nome { get; set; }
        public string Email { get; set; }
        public string Senha { get; set; }
        public List<ProjetoDTO> Projetos { get; set; }
        public List<TarefaDTO>? Tarefas { get; set; }
    }
}
