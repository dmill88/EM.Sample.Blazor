using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using EM.Sample.DomainModels.Enums;

namespace EM.Sample.DomainModels.Models
{
    public class BlogDto
    {
        public BlogDto()
        {
        }

        [Key]
        public int Id { get; set; }

        [Required]
        public Guid GUID { get; set; } = Guid.NewGuid();
        
        [MaxLength(350), Display(Name = "Name"), Required]
        public string Name { get; set; }
        
        public int PrimaryAuthorId { get; set; }

        public AuthorDto PrimaryAuthor { get; set; }

        public int BlogStatusId { get; set; } = (int)BlogStatuses.Draft;

        public BlogStatusDto BlogStatus { get; set; }

        [MaxLength(350), Display(Name = "Display Name"), Required]
        public string DisplayName { get; set; }

        [Display(Name = "Display Order"), Range(0, 10000)]
        public int DisplayOrder { get; set; } = 1;

        public string Description { get; set; }

        public List<string> Tags { get; set; } = new List<string>();
    }
}
