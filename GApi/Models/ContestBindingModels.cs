using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace GApi.Services
{
    public class ContestBindingModel : ContestBinding
    {
        [Key]
        public int? ID{ get; set; }
    }
    public class ContestBinding
    {
        
        [Required]
        [MaxLength(100)]
        public string Name { get; set; }
        [Required]
        [Display(Name = "Start at")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd-MM-yyyy HH:MM}")]
        public DateTime Start_at { get; set; }
        [Required]
        [Display(Name = "End in")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd-MM-yyyy HH:MM}")]
        public DateTime End_in { get; set; }
    }
}