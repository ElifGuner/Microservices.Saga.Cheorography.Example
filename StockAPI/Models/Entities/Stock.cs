namespace StockAPI.Models.Entities
{
    public class Stock
    {
        public int Id { get; set; }
        public Guid ProductId { get; set; }
        public int Count { get; set; }

    }
}
