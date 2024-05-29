namespace BackEnd.DTO
{
    public class TarefaDTO
    {
        public int ID { get; set; }
        public string Projeto { get; set; }
        public string Nome { get; set; }
        public string? Descricao { get; set; }
        public string Status { get; set; }
        public DateTime? DataEntrega { get; set; }
        public List<UsuarioDTO>? UsuariosAtribuidos { get; set; }
        public List<AnexoDTO>? Anexos { get; set; }
    }
}
