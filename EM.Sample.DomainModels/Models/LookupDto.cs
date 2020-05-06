using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace EM.Sample.DomainModels.Models
{
    public abstract class LookupDto
    {
        [Key]
        public int Id { get; set; }

        [MaxLength(50)]
        public string Name { get; set; }

        [MaxLength(150), Display(Name = "Display Name")]
        public string DisplayName { get; set; }

        [Display(Name = "Display Order"), Range(0, 10000)]
        public int DisplayOrder { get; set; }
        
        public bool? Active { get; set; }
    }
}
