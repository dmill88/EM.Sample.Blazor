using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using EM.Sample.Blazor.WebUI.Services;
using EM.Sample.DomainModels.Models;
using System.Diagnostics;

namespace EM.Sample.Blazor.WebUI.Pages
{
    public class BlogAdminBase : ComponentBase
    {
        [Inject]
        public IBlogQueryService BlogQueryService { get; set; }

        [Inject] 
        NavigationManager NavigationManager {get; set; }

        public IEnumerable<BlogListItemDto> BlogListItems { get; set; } = null;

        protected override async Task OnInitializedAsync()
        {
            Debug.WriteLine("BlogAdminBase.OnInitializedAsync");
            BlogListItems = await BlogQueryService.GetBlogs(DomainModels.Enums.BlogStatuses.Published);
        }

    }
}
