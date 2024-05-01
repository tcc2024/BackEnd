namespace BackEnd.DTO
{
    public class EventosDTO
    {
        public int ID { get; set; }
        public string Titulo { get; set; }
        public string? Descricao { get; set; }
        public DateTime DataHora { get; set; }
        public string Projeto { get; set; }
        public List<UsuarioDTO>? UsuariosAtribuidos { get; set; }
    }
}
