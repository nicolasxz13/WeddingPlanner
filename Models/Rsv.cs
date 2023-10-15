using System.ComponentModel.DataAnnotations;

namespace WeddingPlanner.Models
{
    public class Rsv
    {
        [Key]
        public int Id { get; set; }
        public bool Asist { get; set; } = false;
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public Wedding? wedding { get; set; }
        public User? User { get; set; }
    }
}
