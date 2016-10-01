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
        [Display(Name="Author Name")]
        public String FullName { get; set; }
        [DataType(DataType.Date)]
        [Display(Name ="Birth Day")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? BirthDate { get; set; }
        public string Description { set; get; }
        [Display(Name ="Image")]
        public string ImagePath { get; set; }
    }
}
