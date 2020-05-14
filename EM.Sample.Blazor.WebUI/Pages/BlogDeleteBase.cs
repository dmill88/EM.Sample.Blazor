using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using EM.Sample.Blazor.WebUI.Services;
using EM.Sample.DomainModels.Models;

namespace EM.Sample.Blazor.WebUI.Pages
{
    public class BlogDeleteBase : ComponentBase
    {
        [Inject]
        public IBlogQueryService BlogQueryService { get; set; }

        [Inject]
        public IBlogCommandService BlogCommandService { get; set; }

        [Inject]
        public NavigationManager NavigationManager { get; set; }

        [Parameter]
        public int Id { get; set; }

        public BlogDto Blog { get; set; }

        protected override async Task OnInitializedAsync()
        {
            Blog = await BlogQueryService.GetBlog(Id);
        }

        protected async Task DeleteBlog()
        {
            await BlogCommandService.DeleteBlog(Id);
            NavigationManager.NavigateTo("admin");
        }
    }
}
