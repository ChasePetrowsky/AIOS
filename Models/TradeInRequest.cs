namespace AIOS.Models
{
    public class TradeInRequest
    {
        public int Year { get; set; }
        public string Make { get; set; } = "";
        public string Model { get; set; } = "";
        public int Mileage { get; set; }
        public string Condition { get; set; } = "";
        public string Notes { get; set; } = "";
    }
}
