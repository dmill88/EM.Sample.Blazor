using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using EM.Sample.Blazor.WebUI.Services;
using EM.Sample.DomainModels.Models;
using EM.Sample.DomainModels.Enums;
using System.Diagnostics;
using EM.Data.Helpers;

namespace EM.Sample.Blazor.WebUI.Pages
{
    public class BlogAdminBase : ComponentBase
    {
        [Inject]
        public IBlogQueryService BlogQueryService { get; set; }

        [Inject]
        public IBlogCommandService BlogCommandService { get; set; }

        [Inject] 
        NavigationManager NavigationManager {get; set; }

        public IEnumerable<BlogListItemDto> BlogListItems { get; set; } = null;

        public List<ListItem<int>> BlogStatues { get; } = new List<ListItem<int>>();
        public ListItem<int> SelectedBlogStatus { get; set; } = null;

        protected override async Task OnInitializedAsync()
        {
            Debug.WriteLine("BlogAdminBase.OnInitializedAsync");
            IEnumerable<ListItem<int>> statuses = BlogQueryService.GetBlogStatusList();
            BlogStatues.AddRange(statuses);
            SelectedBlogStatus = BlogStatues.FirstOrDefault(i => i.Value == (int)BlogStatuses.Published);

            await LoadBlogs();
        }


        protected async Task LoadBlogs()
        {
            BlogStatuses status = (BlogStatuses)SelectedBlogStatus.Value;
            Debug.WriteLine($"BlogAdminBase.LoadBlogs {status}");
            BlogListItems = await BlogQueryService.GetBlogs(status);
            //InvokeAsync(StateHasChanged).GetAwaiter().GetResult();
        }

    }
}
