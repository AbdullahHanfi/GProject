using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Web;

namespace GApi.Models
{
    public class ProblemBinding
    {
        [Required]
        public int C_ID { get; set; }
        [Required]
        public IFormFile ProbelmFile { get; set; }
        [Required]
        public decimal Time_Limit { get; set; }
        [Required]
        public int Memory_Limit { get; set; }
    }
}