namespace DockerTest.Models
{

    public class CreateOrderRequest
    {
        public string CustomerEmail { get; set; } = "";

        public string Item { get; set; } = "";

        public decimal Price { get; set; }
    }

}
