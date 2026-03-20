namespace pedido.Models
{
    public class Pedido
    {
        public int Id { get; set; }
        public string Numero { get; set; } = string.Empty;
        public DateTime Fecha { get; set; }
        public int IdCliente { get; set; }
    }
}
