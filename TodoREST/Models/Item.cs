namespace TodoREST.Models
{
    public class Item
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Notes { get; set; }
        public bool IsComplete { get; set; }
    }
}
