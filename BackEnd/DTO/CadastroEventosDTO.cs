namespace BackEnd.DTO
{
    public class CadastroEventosDTO
    {
        public string Titulo { get; set; }
        public string? Descricao { get; set; }
        public DateTime DataHora { get; set; }
        public int? ProjetoID { get; set; }
        public List<int>? UsuariosAtribuidos { get; set; }
    }
}
