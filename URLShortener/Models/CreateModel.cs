using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace URLShortener.Models
{
    public class CreateModel
    {
        [Required]
        [Display(Name="URL to shorten")]
        public string URL { get; set; }
        public string URLShortened { get; set; }
    }
}
