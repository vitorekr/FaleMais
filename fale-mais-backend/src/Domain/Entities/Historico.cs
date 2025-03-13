namespace FaleMais.Domain.Entities
{
    public class Historico
    {
        public virtual int Id { get; set; }
        public virtual string Origem { get; set; }
        public virtual string Destino { get; set; }
        public virtual int Duracao { get; set; }
        public virtual int PlanoId { get; set; }
        public virtual decimal CustoComPlano { get; set; }
        public virtual decimal CustoSemPlano { get; set; }
        public virtual DateTime DataHora { get; set; } = DateTime.UtcNow;
    }
}
