namespace CursoEFCore.Domain
{
    //[Table("TB_Cliente")]
    public class Cliente
    {
        //[Key]
        public int Id { get; set; }

        //[Required]
        public string Nome { get; set; }

        //[Column("Fone")]
        public string Telefone { get; set; }
        public string CEP { get; set; }
        public string Estado { get; set; }
        public string Cidade { get; set; }

        // For create an example of rollback
        //public string Email { get; set; }

        public override string ToString()
        {
            return $"Id: {Id} Nome: {Nome}";
        }
    }
}