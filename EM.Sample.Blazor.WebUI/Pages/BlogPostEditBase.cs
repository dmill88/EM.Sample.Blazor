using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using EM.Sample.Blazor.WebUI.Services;
using EM.Sample.DomainModels.Models;
using EM.Data.Helpers;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using Blazored.TextEditor;

namespace EM.Sample.Blazor.WebUI.Pages
{
    public class BlogPostEditBase: ComponentBase
    {
        [Inject]
        public IBlogQueryService BlogQueryService { get; set; }

        [Inject]
        public IBlogCommandService BlogCommandService { get; set; }

        public string PageTitle { get; set; } = "Edit Blog Post";

        [Parameter]
        public int Id { get; set; } = 0;

        [Parameter]
        public int BlogId { get; set; } = 0;

        public BlogPostDto BlogPost { get; set; } = null;

        public List<AuthorDto> Authors { get; } = new List<AuthorDto>();
        public AuthorDto SelectedAuthor { get; set; } = null;

        public List<ListItem<int>> BlogPostStatues { get; } = new List<ListItem<int>>();
        public ListItem<int> SelectedBlogPostStatus { get; set; } = null;

        public List<ListItem<int>> CommentStatues { get; } = new List<ListItem<int>>();
        public ListItem<int> SelectedCommentStatus { get; set; } = null;

        public bool ShowSaveResult { get; set; }

        public IEnumerable<string> AvailableTags { get; set; } = new List<string>();
        public IEnumerable<string> SelectedTags { get; set; } = new List<string>();

        public BlazoredTextEditor QuillHtml;

        public bool IsDataLoaded { get; set; } = false;

        private bool? successSubmit { get; set; }

        private string validationErrorFromIValidator = string.Empty;

        protected override async Task OnInitializedAsync()
        {
            Debug.WriteLine($"Id {Id}");
            Debug.WriteLine($"BlogId {BlogId}");

            var authors = await BlogQueryService.GetAuthors();
            Authors.AddRange(authors);

            var availableTags = await BlogQueryService.GetTags();
            AvailableTags = availableTags.Select(i => i.Name);

            IEnumerable<ListItem<int>> statuses = BlogQueryService.GetPostStatusesList();
            BlogPostStatues.AddRange(statuses);

            IEnumerable<ListItem<int>> commentStatuses = BlogQueryService.GetCommentStatusesList();
            CommentStatues.AddRange(commentStatuses);

            if (Id > 0)    // Update Post
            {
                BlogPost = await BlogQueryService.GetBlogPost(Id);

                //if (QuillHtml != null)
                //{
                //    await this.QuillHtml.LoadHTMLContent(BlogPost.PostContent);
                //}
                PageTitle = $"Edit {BlogPost.Title}";

                var existingTags = SelectedTags.ToList();
                existingTags.AddRange(BlogPost.Tags);
                SelectedTags = existingTags;

                SelectedAuthor = Authors.FirstOrDefault(i => i.Id == BlogPost.PrimaryAuthorId);
                SelectedBlogPostStatus = BlogPostStatues.FirstOrDefault(i => i.Value == BlogPost.PostStatusId);
                SelectedCommentStatus = CommentStatues.FirstOrDefault(i => i.Value == BlogPost.CommentStatusId);
                IsDataLoaded = true;
            }
            else if (BlogId > 0) // New Post
            {
                BlogPost = await BlogCommandService.AddBlogPost(BlogId);
                PageTitle = $"Add Post";
                var defaultTags = await BlogQueryService.GetBlogTags(BlogId);
                SelectedTags = defaultTags.Select(i => i.Name).ToList();
                SelectedBlogPostStatus = BlogPostStatues.FirstOrDefault(i => i.Value == BlogPost.PostStatusId);
                SelectedCommentStatus = CommentStatues.FirstOrDefault(i => i.Value == BlogPost.CommentStatusId);
                IsDataLoaded = true;
            }

            if (IsDataLoaded)
                StateHasChanged();
        }

        protected async Task HandleValidSubmit()
        {
            IsDataLoaded = false;
            successSubmit = true;
            // This code gets the errors from the class's implementation of the IValidatableObject interface
            if (BlogPost is IValidatableObject)
            {
                ICollection<ValidationResult> validationResults = new List<ValidationResult>();
                successSubmit = Validator.TryValidateObject(BlogPost, new ValidationContext(BlogPost), validationResults);
                if (!successSubmit.Value)
                {
                    validationErrorFromIValidator = validationResults.Select(i => i.ErrorMessage).FirstOrDefault(); // Sample only; need to get all validation errors.
                }
            }

            if (SelectedAuthor != null)
            {
                Debug.WriteLine($"OnValidSubmit SelectedAuthor {SelectedAuthor.FullName}");

                BlogPost.PrimaryAuthorId = SelectedAuthor.Id;
                Debug.WriteLine($"OnValidSubmit BlogPost.PrimaryAuthorId {BlogPost.PrimaryAuthorId}");
            }

            if (SelectedBlogPostStatus != null && SelectedBlogPostStatus.Value > 0)
            {
                BlogPost.PostStatusId = SelectedBlogPostStatus.Value;
            }
            if (SelectedCommentStatus != null && SelectedCommentStatus.Value > 0)
            {
                BlogPost.CommentStatusId = SelectedCommentStatus.Value;
            }

            BlogPost.Tags.Clear();
            BlogPost.Tags.AddRange(SelectedTags);

            BlogPost.PostContent = await this.QuillHtml.GetHTML();

            if (Id == 0)
            {
                BlogPost = await BlogCommandService.AddBlogPost(BlogPost);
                Id = BlogPost.Id;
                await InvokeAsync(StateHasChanged);
            }
            else if (Id > 0)
            {
                BlogPost = await BlogCommandService.UpdateBlogPost(BlogPost);
            }
            PageTitle = BlogPost.Title;
            IsDataLoaded = true;
            ShowSaveResult = successSubmit.Value;
            Debug.WriteLine($"HandleValidSubmit results {successSubmit.Value}");
        }

        protected void HandleInValidSubmit()
        {
            Debug.WriteLine("OnInvalidSubmit. Could be used to provide more detail information to the user.");
        }
    }
}
