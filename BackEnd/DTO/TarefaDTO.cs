namespace BackEnd.DTO
{
    public class TarefaDTO
    {
        public int ID { get; set; }
        public string Titulo { get; set; }
        public string? Descricao { get; set; }
        public string Status { get; set; }
        public DateTime? DataEntrega { get; set; }
        public List<UsuarioDTO>? UsuariosAtribuidos { get; set; }
        public List<MtReferenciaDTO>? Referencias { get; set; }
        public List<MtTrabalhoDTO>? Anexos { get; set; }
    }
}
