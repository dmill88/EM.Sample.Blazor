using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using EM.Sample.Blazor.WebUI.Services;
using EM.Sample.DomainModels.Models;

namespace EM.Sample.Blazor.WebUI.Pages
{
    public class BlogPostViewBase : ComponentBase
    {
        [Inject]
        public IBlogQueryService BlogQueryService { get; set; }

        [Parameter]
        public int Id { get; set; }

        public BlogPostDto BlogPost { get; set; }

        protected override async Task OnInitializedAsync()
        {
            BlogPost = await BlogQueryService.GetBlogPost(Id);
        }

        public string Authors
        {
            get
            {
                string authors = string.Empty;
                if (BlogPost.Authors.Count > 0)
                {
                    authors = string.Join(", ", BlogPost.Authors.Select(i => i.DisplayName));
                }
                return authors;
            }
        }

        [Parameter] 
        public string PostContent { get; set; }

        //private ElementRef Span;

        //protected override void OnAfterRender()
        //{
        //    Microsoft.AspNetCore.Blazor.Browser.Interop.RegisteredFunction.Invoke("RawHtml", Span, Content);
        //}


    }
}
