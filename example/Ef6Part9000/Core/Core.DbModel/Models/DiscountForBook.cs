using System.ComponentModel.DataAnnotations;

namespace Core.DbModel.Models
{
    public class DiscountForBook
    {
        [Key]
        public int Id { get; set; }      

        public int DiscountValue { get; set; }
    }
}
