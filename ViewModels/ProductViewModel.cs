namespace MyProject.ViewModels
{
    public class ProductViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int Quantiy { get; set; }
        public double Price { get; set; }
        public double TotalPriceWithVAT { get; set; }
    }
}
