namespace BackEnd.DTO
{
    public class EventosDTO
    {
        public int ID { get; set; }
        public string Nome { get; set; }
        public string? Descricao { get; set; }
        public DateTime DataHora { get; set; }
        public int? ProjetoID { get; set; }
        public List<int>? UsuariosAtribuidos { get; set; }
    }
}
