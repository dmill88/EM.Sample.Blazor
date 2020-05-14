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

namespace EM.Sample.Blazor.WebUI.Pages
{
    public class BlogEditBase : ComponentBase
    {
        [Inject]
        public IBlogQueryService BlogQueryService { get; set; }

        [Inject]
        public IBlogCommandService BlogCommandService { get; set; }

        public string PageTitle { get; set; } = "Edit Blog";

        public string SubmitButtonText { get; set; } = "Update Blog";

        [Parameter]
        public int Id { get; set; }

        public BlogDto Blog { get; set; } = null;

        public bool ShowSaveResult { get; set; }

        public IEnumerable<PostListItemDto> BlogPosts { get; set; } = new List<PostListItemDto>();

        public List<AuthorDto> Authors { get; } = new List<AuthorDto>();
        public AuthorDto SelectedAuthor { get; set; } = null;

        public IEnumerable<string> AvailableTags { get; set; } = new List<string>();
        public IEnumerable<string> SelectedTags { get; set; } = new List<string>();

        //public List<string> testItems { get; set; } = new List<string>() { "hi", "there", "fish", "cat" };
        //public List<string> selectedTestItems = new List<string>() { "fish" };
        //public string selectedTestItem = "fish";

        public List<ListItem<int>> BlogStatues { get; } = new List<ListItem<int>>();
        public ListItem<int> SelectedBlogStatus { get; set; } = null;

        public bool IsDataLoaded { get; set; } = false;

        private bool? successSubmit { get; set; }

        private string validationErrorFromIValidator = string.Empty;

        protected override async Task OnInitializedAsync()
        {
            var authors = await BlogQueryService.GetAuthors();
            Authors.AddRange(authors);

            var availableTags = await BlogQueryService.GetTags();
            AvailableTags = availableTags.Select(i => i.Name);

            IEnumerable<ListItem<int>> statuses = BlogQueryService.GetBlogStatusList();
            BlogStatues.AddRange(statuses);

            if (Id == 0)
            {
                Blog = new BlogDto();
                PageTitle = $"Add Blog";
                SubmitButtonText = "Add Blog";
            }
            else
            {
                Blog = await BlogQueryService.GetBlog(Id);
                PageTitle = $"Edit {Blog.DisplayName}";

                var existingTags = SelectedTags.ToList();
                existingTags.AddRange(Blog.Tags);
                SelectedTags = existingTags;

                SelectedAuthor = Authors.FirstOrDefault(i => i.Id == Blog.PrimaryAuthorId);
                SelectedBlogStatus = BlogStatues.FirstOrDefault(i => i.Value == Blog.BlogStatusId);

                BlogPosts = await BlogQueryService.GetAllBlogPosts(Id);
            }

            IsDataLoaded = true;
        }

        protected async Task HandleValidSubmit()
        {
            IsDataLoaded = false;
            successSubmit = true;
            // This code gets the errors from the class's implementation of the IValidatableObject interface
            if (Blog is IValidatableObject)
            {
                ICollection<ValidationResult> validationResults = new List<ValidationResult>();
                successSubmit = Validator.TryValidateObject(Blog, new ValidationContext(Blog), validationResults);
                if (!successSubmit.Value)
                {
                    validationErrorFromIValidator = validationResults.Select(i => i.ErrorMessage).FirstOrDefault(); // Sample only; need to get all validation errors.
                }
            }
            if (SelectedAuthor != null)
            {
                Debug.WriteLine($"OnValidSubmit SelectedAuthor {SelectedAuthor.FullName}");

                Blog.PrimaryAuthorId = SelectedAuthor.Id;
                Debug.WriteLine($"OnValidSubmit Blog.PrimaryAuthorId {Blog.PrimaryAuthorId}");
            }
            if (SelectedBlogStatus != null && SelectedBlogStatus.Value > 0)
            {
                Blog.BlogStatusId = SelectedBlogStatus.Value;
            }
            
            Blog.Tags.Clear();
            Blog.Tags.AddRange(SelectedTags);

            if (Id == 0)
            {
                Blog = await BlogCommandService.AddBlog(Blog);
                Id = Blog.Id;
                await InvokeAsync(StateHasChanged);
            }
            else
            {
                Blog = await BlogCommandService.UpdateBlog(Blog);
            }
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
