namespace BackEnd.DTO
{
    public class ListarEventoDTO
    {
        public int ID { get; set; }
        public string Nome { get; set; }
        public string? Descricao { get; set; }
        public DateTime DataHora { get; set; }
        public int? ProjetoID { get; set; }
        public List<ListarUsuarioEventoDTO>? UsuariosAtribuidos { get; set; }

    }

    public class ListarUsuarioEventoDTO
    {
        public int ID { get; set; }
        public string Nome { get; set; }
    }
}
