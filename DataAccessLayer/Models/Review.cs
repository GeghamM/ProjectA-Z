using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Models
{
    public class Review
    {
        [Key]
        public int ReviewId { get; set; }
        [Required]
        [Display(Name = "User")]
        public string UserName { get; set; }
        [Required]
        [Display(Name = "Review")]
        public string Text { get; set; }
        [DisplayName("Review Date")]
        [DataType("Date")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? ReviewDate { get; set; }
        public int? BookId { get; set; }
        public int? AuthorId { get; set; }
    }
}
