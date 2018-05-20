using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace DataAccessLayer.Models
{
    public class Author
    {
        [Key]
        public int ID { get; set; }
        [Required]
        [Display(Name="Author")]
        public String FullName { get; set; }
        [Display(Name ="Birth Day")]
        [DataType("Date")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? BirthDate { get; set; }
        public string Description { set; get; }
        [Display(Name ="Image")]
        public string ImagePath { get; set; }
    }
}
