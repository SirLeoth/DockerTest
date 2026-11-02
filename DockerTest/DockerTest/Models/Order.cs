namespace DockerTest.Models
{

    public class Order
    {
        public int Id { get; set; }

        public string CustomerEmail { get; set; } = "";

        public string Item { get; set; } = "";

        public decimal Price { get; set; }

        public DateTime CreatedAt { get; set; }

        public string Status { get; set; } = "Pending";
    }

}
