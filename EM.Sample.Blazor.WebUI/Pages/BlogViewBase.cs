using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using EM.Sample.Blazor.WebUI.Services;
using EM.Sample.DomainModels.Models;

namespace EM.Sample.Blazor.WebUI.Pages
{
    public class BlogViewBase: ComponentBase
    {
        [Inject]
        public IBlogQueryService BlogQueryService { get; set; }

        [Parameter]
        public int Id { get; set; }

        public BlogDto Blog { get; set; }

        public IEnumerable<PostListItemDto> BlogPosts { get; set; }

        protected override async Task OnInitializedAsync()
        {
            Blog = await BlogQueryService.GetBlog(Id);
            BlogPosts = await BlogQueryService.GetAllBlogPosts(Id);
        }

    }
}
