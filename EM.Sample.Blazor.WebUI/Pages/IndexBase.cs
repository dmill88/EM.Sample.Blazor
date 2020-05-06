using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using EM.Sample.Blazor.WebUI.Services;
using EM.Sample.DomainModels.Models;

namespace EM.Sample.Blazor.WebUI.Pages
{
    public class IndexBase : ComponentBase
    {
        [Inject]
        public IBlogQueryService BlogQueryService { get; set; }

        public IEnumerable<BlogListItemDto> BlogListItems { get; set; }

        protected override async Task OnInitializedAsync()
        {
            BlogListItems = await BlogQueryService.GetBlogs(DomainModels.Enums.BlogStatuses.Published);
        }

    }
}
