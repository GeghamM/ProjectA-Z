using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace DataAccessLayer.Models
{
    public class Country
    {
        [Key]
        public int ID { get; set; }
        [Required]
        [Display(Name="Country")]
        public string Name { get; set; }
        [Required]
        public int Code { get; set; }
    }
}
