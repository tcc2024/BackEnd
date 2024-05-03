namespace BackEnd.DTO
{
    public class ProjetoDTO
    {
        public int? ID { get; set; }
        public string Nome { get; set; }
        public string? Descricao { get; set; }
        public List<TarefaDTO>? Tarefas { get; set; }
        public List<UsuarioDTO>? Usuarios { get; set; }
    }
}
