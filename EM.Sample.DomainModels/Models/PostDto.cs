using EM.Sample.DomainModels.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EM.Sample.DomainModels.Models
{
    public class PostDto
    {
        public PostDto()
        {
        }

        public int Id { get; set; }

        public Guid GUID { get; set; } = Guid.NewGuid();

        public int PrimaryAuthorId { get; set; }

        public List<AuthorDto> Authors { get; set; } = new List<AuthorDto>();

        [MaxLength(450), Required]
        public string Title { get; set; }

        [Display(Name="Post")]
        public string PostContent { get; set; }

        [MaxLength(1000)]
        public string Excerpt { get; set; }

        public int PostStatusId { get; set; } = (int)PostStatuses.Draft;

        public int CommentStatusId { get; set; } = (int)Enums.CommentStatuses.MemberOnly;

        public int CommentCount { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        public List<string> Tags { get; set; } = new List<string>();
    }
}
