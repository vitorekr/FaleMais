namespace FaleMais.Domain.Entities
{
    public class Tarifa
    {
        public virtual int Id { get; set; }
        public string Origem { get; set; } = string.Empty;
        public string Destino { get; set; } = string.Empty;
        public virtual decimal Valor { get; set; }
    }
}
