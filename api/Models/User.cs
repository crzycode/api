using System.ComponentModel.DataAnnotations;

namespace api.Models
{
    public class User
    {
        [Key]
        public int User_id { get; set; }
        public string Card_holdername { get; set; } = string.Empty;
        public int CardNumber { get; set; }
        public int Expirydate { get; set; }
        public int ExpiryYear { get; set; }
        public int CVC { get; set; }
    }
}
