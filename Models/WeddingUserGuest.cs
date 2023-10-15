namespace WeddingPlanner.Models{
    public class WeddingViewModel
    {
        public int Id { get; set; }
        public string? Married { get; set; }
        public int? GuestCount{get;set;}
        public bool Creator{get;set;}
        public bool Asist{get;set;}
        public DateTime? Date { get; set; }
        
    }

}