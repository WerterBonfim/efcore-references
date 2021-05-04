namespace DominandoEFCore.Domain
{
    public class Governador
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public int Idade { get; set; }
        public string Partido { get; set; }

        public int EstadoId { get; set; }
        public Estado Estado { get; set; }
    }
}