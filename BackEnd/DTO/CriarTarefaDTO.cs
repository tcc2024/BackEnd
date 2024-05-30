namespace BackEnd.DTO
{
    public class CriarTarefaDTO
    {
        public int Id { get; set; }
        public string Nome { get; set;}
        public string Descricao { get; set;}
        public string ?Status {  get; set;}
        public DateTime DataEntrega { get; set; }
        public int Projeto_ID { get; set; }
        public List<int>? UsuariosAtribuidos { get; set; }
        public List<AnexoDTO>? Anexos { get; set; }

    }
}
