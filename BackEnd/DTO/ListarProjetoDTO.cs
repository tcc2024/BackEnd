namespace BackEnd.DTO
{
    public class ListarProjetoDTO
    {
        public int ID { get; set; }
        public string Nome { get; set; }
        public string? Descricao { get; set; }
        public List<ListarUsuarioProjetoDTO>? UsuariosAtribuidos { get; set; }
    }

    public class ListarUsuarioProjetoDTO
    {
        public int ID { get; set; }
        public string Nome { get; set; }
    }
}
