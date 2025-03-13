namespace FaleMais.Domain.Entities
{
    public class Plano
    {
        public virtual int Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public virtual int MinutosGratis { get; set; }
    }
}
