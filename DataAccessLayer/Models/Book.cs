using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace DataAccessLayer.Models
{
    public class Book
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        [ForeignKey(nameof(Author))]
        public int AuthorID { get; set; }
        [Required]
        [ForeignKey(nameof(Country))]
        public int IssueCountryID { get; set; }
        [Required(ErrorMessage = "Price is required")]
        [DisplayFormat(DataFormatString ="{0:n2}")]
        [RegularExpression(@"^\d+(\.\d{1,2})?$", ErrorMessage = "enter decimal value of format $9.99")]
        public decimal Price { get; set; }
        [DisplayName("Issue Date")]
        [DataType("Date")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? IssueDate { get; set; }
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }
        [DisplayName("Pages count")]
        public int PageCount { get; set; }
        [Display(Name ="Cover Image")]
        public string ImagePath { get; set; }
        public virtual Author Author { get; set; }
        public virtual Country Country { get; set; }
    }
}
